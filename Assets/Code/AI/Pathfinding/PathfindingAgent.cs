﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingAgent : MonoBehaviour {
	public Transform target;
	public float radius = 1f;

	Vector3 curTarget;
	
	public TriangleNode startNode, endNode;
	
	List<TriangleNode> pathList = new List<TriangleNode>();
	List<TriangleNode> openList = new List<TriangleNode>();
	List<TriangleNode> closedList = new List<TriangleNode>();
	public List<Vector3> pathVertices = new List<Vector3> ();
	IComparer<TriangleNode> comparer = new TriangleNode();
	//Keep track of node with the lowest EstimatedDistanceToEnd for unreachable endNodes
	TriangleNode smallestHeuristicNode;

	AIMovement movement;
	CollisionAvoidance avoid;
	
	Vector3 curPos, lastPos;
	bool targetMoved = false;

	MovingObject lastObstacle;
	
	bool hasInit = false;

	Vector3 posX1, posX2;

	// Use this for initialization
	void Start () {
		movement = GetComponent<AIMovement> ();
		avoid = GetComponent<CollisionAvoidance> ();
		movement.target = target.transform.position;
		pathVertices.Add (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		posX1 = transform.position + Quaternion.AngleAxis(90, Vector3.up) * transform.forward;
		posX2 = transform.position + Quaternion.AngleAxis (-90, Vector3.up) * transform.forward;

		bool seenTarget = false;
		//If player can be seen make it the current target
		if(!Physics.Linecast (posX1, target.transform.position, AI_Pathfinding.layoutMask) && !Physics.Linecast (posX2, target.transform.position, AI_Pathfinding.layoutMask))
		{
			curTarget = target.transform.position;
			movement.UpdatePath(curTarget);
			seenTarget = true;
		}
		else if(!hasInit || ObstacleMoved() || TargetMoved())
		{
			hasInit = true;
			try
			{
				UpdatePath();
			}
			catch (System.Exception e) 
			{
				//print (e.GetType ());
				return;
			}
		}
		
		Draw();
		
		//Current target is current node path being seeked
		if(!seenTarget)
			curTarget = movement.target;
		if(avoid.enabled)
			avoid.target = curTarget;

		Debug.DrawLine (posX1, curTarget, Color.red);
		Debug.DrawLine (posX2, curTarget, Color.red);

		RaycastHit hit;
		//Check for collisions with dynamic obstacles (e.g. doors)
		if(Physics.Raycast (transform.position, (curTarget - transform.position), out hit, (curTarget - transform.position).magnitude, AI_Pathfinding.movingMask))
		{
			//If obstacle is not blocking, simply avoid it. Otherwise recalculate path if the current one is not going to the endNode;
			if(!hit.transform.GetComponent<MovingObject>().isBlocking)
			{
				movement.enabled = false;
				avoid.enabled = true;
				return;
			}
			else
			{
				lastObstacle = hit.transform.GetComponent<MovingObject>();
				//Keep track of last blocking obstacle to trigger an update of the path in case the obstacle moves (e.g. doors opens and close)
				try
				{
					UpdatePath();
				}
				catch (System.Exception e) 
				{
					//print (e.GetType ());
					return;
				}
				return;
			}
		}
		
		//Smooth path by cutting unecessary nodes from the path as the agent travels the nodes
		//pathVertices = smoothPath(pathVertices);
		//Update the path to seek
		//if(movement.path != pathVertices)
		//	movement.UpdatePath (pathVertices);
	}
	
	bool ObstacleMoved()
	{
		//Check if last obstacle recorded has moved (e.g. a door that was just closed has opened)
		if(lastObstacle != null)
			return lastObstacle.moveStopped;
		
		return false;
	}
	
	bool TargetMoved()
	{
		//Check if the target has moved
		bool targetMovedStopped = false;
		curPos = target.transform.position;
		if(curPos != lastPos)
		{
			targetMoved = true;
		}
		else if(targetMoved)
		{
			targetMovedStopped = true;
			targetMoved = false;
		}
		lastPos = curPos;
		
		if(!Physics.Linecast(pathVertices[pathVertices.Count - 1], curPos, AI_Pathfinding.layoutMask))
			targetMovedStopped = false;
		
		return targetMovedStopped;
	}
	
	void Draw()
	{
		//Path
		for(int i = 0; i < pathVertices.Count - 1; i++)
		{
			Debug.DrawLine(pathVertices[i], pathVertices[i + 1], Color.green);
		}
	}
	
	public void UpdatePath()
	{
		//Initialize the start and end node to be the triangle area over which the agent is standing
		startNode = GetTriangleNode (transform.position);
		endNode = GetTriangleNode (target.transform.position);
		//Set the triangle node position (actual position in area to seek) to be agent position for start and target position for end.
		startNode.nodePosition = transform.position;
		endNode.nodePosition = target.transform.position;
		//Clear the lists
		openList.Clear ();
		closedList.Clear ();
		pathList.Clear ();
		pathVertices.Clear ();
		//Initialize start node cost and heuristic
		startNode.costSoFar = 0;
		startNode.heuristicValue = (endNode.nodePosition - startNode.nodePosition).magnitude;
		smallestHeuristicNode = startNode;
		//Calculate path
		calculateAStarEuclideanDistance ();
		
		//Convert the pathlist of triangle nodes to a list of Vector3 positions to seek
		for(int i = 0; i < pathList.Count; i++)
		{
			pathVertices.Add(pathList[i].nodePosition);
		}
		
		pathVertices = smoothPath(pathVertices);
		movement.UpdatePath (pathVertices);
	}
	
	TriangleNode GetTriangleNode(Vector3 position)
	{
		position.y = 10;
		Vector3 under = new Vector3 (position.x, -10f, position.z);
		
		RaycastHit hit;
		if(Physics.Raycast (position, (under - position), out hit, (under - position).magnitude, AI_Pathfinding.navigationMask))
		{
			return hit.collider.GetComponentInParent<TriangleNode>();
		}
		
		return startNode;
	}
	
	//AStarEuclideanDistance visit node
	bool visitNode_AStarEuclideanDistance(TriangleNode node) {
		for (int i = 0; i < node.neighborList.Count; i++) {
			TriangleNode nextNode = node.neighborList[i];
			if(nextNode != null)
			{
				if(nextNode != endNode)
				{
					//Position to seek in neighbor node is closest point to target in triangle
					nextNode.nodePosition = nextNode.GetClosestPointInTriangle(node.nodePosition);
					//Check node position is indeed reachable
					if(Physics.Linecast(node.nodePosition, nextNode.nodePosition, AI_Pathfinding.layoutMask))
					{
						nextNode.nodePosition = nextNode.GetClosestPointInTriangle(endNode.nodePosition);
					}
				}
				
				//Check for collisions with moving objects, if a door was closed for example
				RaycastHit hit;
				if(Physics.Raycast (node.nodePosition, (nextNode.nodePosition - node.nodePosition), out hit, (nextNode.nodePosition - node.nodePosition).magnitude, AI_Pathfinding.movingMask))
				{
					//Only avoid node if the obstacle is blocking a path
					if(hit.transform.GetComponent<MovingObject>().isBlocking)
					{
						lastObstacle = hit.transform.GetComponent<MovingObject>();
						continue;
					}
				}
				
				//Distance between current node and neighbor
				float newCostSoFar = node.costSoFar + (nextNode.nodePosition - node.nodePosition).magnitude;
				
				//Check if neighbor is in open list or closed list. If new cost is smaller, update cost
				if(openList.Contains (nextNode) && newCostSoFar < nextNode.costSoFar)
				{
					nextNode.UpdateCostSoFar(newCostSoFar);
				}
				else if(closedList.Contains (nextNode) && newCostSoFar < nextNode.costSoFar)
				{
					nextNode.UpdateCostSoFar(newCostSoFar);
					//Add back the node to open list and remove from closed list since cost was updated
					openList.Add(nextNode);
					closedList.Remove (nextNode);
				}
				
				if (!closedList.Contains (nextNode) && !openList.Contains (nextNode)) {
					nextNode.UpdatePrevNode(node);
					
					nextNode.UpdateCostSoFar(newCostSoFar);
					//Heuristic value is euclidean distance from node to end node
					nextNode.UpdateHeuristic((endNode.nodePosition - nextNode.nodePosition).magnitude);
					
					if(smallestHeuristicNode.heuristicValue > nextNode.heuristicValue)
					{
						smallestHeuristicNode = nextNode;
					}
					
					openList.Add (nextNode);
				}
				
			}
		}
		closedList.Add (node);
		//Check if the openList has more than 1 node, otherwise removing the node will make the list empty (This is for paths where endNode is not reachable, e.g. closed off room)
		if(openList.Count <= 1)
			return false;
		openList.Remove(node);
		
		//Sort the list based on total estimated value of each node
		openList.Sort (comparer);
		return true;
	}
	
	//AStarEuclideanDistance algorithm
	void calculateAStarEuclideanDistance() {
		openList.Add (startNode);
		
		//Loop through the nodes until endNode is found or endNode is in unreacheable closed room
		while (openList[0] != endNode)
		{
			if(!visitNode_AStarEuclideanDistance (openList[0]))
			{
				openList[0] = smallestHeuristicNode;
				print ("unreachable path");
				break;
			}
		}
		
		pathList.Add (openList [0]);
		if(openList[0] == startNode)
			return;
		
		while(true) {
			if (pathList[pathList.Count - 1].prevNode == startNode) {
				pathList.Add (pathList[pathList.Count - 1].prevNode);
				pathList.Reverse ();
				return;
			}
			else {
				pathList.Add (pathList[pathList.Count - 1].prevNode);
			}
		}
	}
	
	public List<Vector3> smoothPath(List<Vector3> inputPath)
	{
		List<Vector3> outputPath = new List<Vector3> ();
		
		int index = 0;
		//Check from the endNode of the inputPath the first visible node
		for(int i = inputPath.Count - 1; i >= 0; i--)
		{
			if(!Physics.Linecast (posX1, inputPath[i], AI_Pathfinding.layoutMask) && !Physics.Linecast (posX2, inputPath[i], AI_Pathfinding.layoutMask))
			{
				RaycastHit hit;
				//Since smoothing happens every frame, check to avoid going to a blocked off path.
				if(Physics.Raycast (transform.position, (inputPath[i] - transform.position), out hit, (inputPath[i] - transform.position).magnitude, AI_Pathfinding.movingMask))
				{
					//Only avoid node if the obstacle is blocking a path
					if(hit.transform.GetComponent<MovingObject>().isBlocking)
					{
						lastObstacle = hit.transform.GetComponent<MovingObject>();
						continue;
					}
				}
				outputPath.Add(inputPath[i]);
				index = i;
				break;
			}
		}
		
		//Happens on some sharp corners
		if(outputPath.Count == 0)
			return inputPath;
		
		for(int i = (index + 2); i <= inputPath.Count - 1; i++)
		{
			if(Physics.Linecast (outputPath[outputPath.Count - 1], inputPath[i], AI_Pathfinding.layoutMask))
			{
				RaycastHit hit;
				//Since smoothing happens every frame, check to avoid going to a blocked off path.
				if(Physics.Raycast (outputPath[outputPath.Count - 1], (inputPath[i] - outputPath[outputPath.Count - 1]), out hit, (inputPath[i] - outputPath[outputPath.Count - 1]).magnitude, AI_Pathfinding.movingMask))
				{
					//Only avoid node if the obstacle is blocking a path
					if(hit.transform.GetComponent<MovingObject>().isBlocking)
					{
						lastObstacle = hit.transform.GetComponent<MovingObject>();
						continue;
					}
				}
				outputPath.Add(inputPath[i - 1]);
			}
		}

		outputPath.Add(inputPath[inputPath.Count - 1]);
		
		return outputPath;
	}

//	void OnCollisionStay(Collision collision) {
//		foreach (ContactPoint contact in collision.contacts) {
//			if(contact.otherCollider.tag != "Wall")
//				continue;
//			Vector3 newPos = rigidbody.position + ((contact.normal.normalized + (movement.target - transform.position).normalized) * 0.5f);
//			newPos.y = transform.position.y;
//			movement.Reposition(newPos);
//			//Debug.DrawRay(contact.point, contact.normal, Color.white,10f);
//		}
//	}
}
