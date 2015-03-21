using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingAgent : MonoBehaviour {
	public Transform target;
	
	Vector3 curTarget;

	TriangleNode startNode, endNode;

	List<TriangleNode> pathList = new List<TriangleNode>();
	List<TriangleNode> openList = new List<TriangleNode>();
	List<TriangleNode> closedList = new List<TriangleNode>();
	List<Vector3> pathVertices = new List<Vector3> ();
	IComparer<TriangleNode> comparer = new TriangleNode();
	//Keep track of node with the lowest EstimatedDistanceToEnd for unreachable endNodes
	TriangleNode smallestHeuristicNode;

	AIMovement movement;
	CollisionAvoidance avoid;
	
	Vector3 curPos, lastPos, lastObstacleCurPos, lastObstacleLastPos;
	bool targetMoved = false, lastObstacleMoved = false;

	MovingObject lastObstacle;

	bool hasInit = false;

	// Use this for initialization
	void Start () {
		movement = GetComponent<AIMovement> ();
		avoid = GetComponent<CollisionAvoidance> ();
		movement.target = target.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if(!hasInit || ObjectMoved())
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
		curTarget = movement.target;
		//If target is reachable make it the current target
		if(!Physics.Linecast(transform.position, target.transform.position, AI_Pathfinding.layoutMask))
			curTarget = target.transform.position;

		if(avoid.enabled)
		{
			avoid.target = curTarget;
		}

		RaycastHit hit;
		//Check for collisions with dynamic obstacles (e.g. doors)
		if(Physics.Raycast (transform.position, (curTarget - transform.position), out hit, (curTarget - transform.position).magnitude, AI_Pathfinding.movingMask))
		{
			lastObstacle = hit.transform.GetComponent<MovingObject>();
			//If obstacle is not blocking, simply avoid it. Otherwise recalculate path if the current one is not going to the endNode;
			if(!lastObstacle.isBlocking)
			{
				movement.enabled = false;
				avoid.enabled = true;
				return;
			}
			else if(pathVertices[pathVertices.Count - 1] != endNode.nodePosition)
			{
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
		pathVertices = smoothPath(pathVertices);
		//Update the path to seek
		if(movement.path != pathVertices)
			movement.UpdatePath (pathVertices);
	}

	bool ObjectMoved()
	{
		//Check if last obstacle recorded has moved (e.g. a door that was just closed has opened)
		bool obstacleMovedStopped = false;
		if(lastObstacle != null)
		{
			lastObstacleCurPos = lastObstacle.transform.position;
			if(lastObstacleCurPos != lastObstacleLastPos)
			{
				lastObstacleMoved = true;
			}
			else if(lastObstacleMoved)
			{
				obstacleMovedStopped = true;
				lastObstacleMoved = false;
			}
			lastObstacleLastPos = lastObstacleCurPos;
		}

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

		return (obstacleMovedStopped || targetMovedStopped);
	}

	void Draw()
	{
		//Path
		for(int i = 0; i < pathVertices.Count - 1; i++)
		{
			Debug.DrawLine(pathVertices[i], pathVertices[i + 1], Color.green);
		}

		//Fill
//		foreach(TriangleNode node in openList)
//		{
//			node.draw(Color.yellow);
//		}
//
//		foreach(TriangleNode node in closedList)
//		{
//			node.draw(Color.yellow);
//		}
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
		position.y = 0;

		//Check which triangle node the agent is standing on
		Collider[] hitColliders = Physics.OverlapSphere(position, 1f, AI_Pathfinding.navigationMask);
		if(hitColliders.Length == 0)
			hitColliders = Physics.OverlapSphere(position, 2f, AI_Pathfinding.navigationMask);
		for(int i = 0; i < hitColliders.Length; i++)
		{
			TriangleNode n = hitColliders[i].GetComponentInParent<TriangleNode> ();
			if(n != null)
				return n;
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
					nextNode.nodePosition = nextNode.GetClosestPointInTriangle(endNode.nodePosition);
					//Check node position is indeed reachable
					if(Physics.Linecast(node.nodePosition, nextNode.nodePosition, AI_Pathfinding.layoutMask))
					{
						nextNode.nodePosition = nextNode.GetClosestPointInTriangle(node.nodePosition);
					}
				}

				//Check for collisions with moving objects, if a door was closed for example
				RaycastHit hit;
				if(Physics.Raycast (node.nodePosition, (nextNode.nodePosition - node.nodePosition), out hit, (nextNode.nodePosition - node.nodePosition).magnitude, AI_Pathfinding.movingMask))
				{
					lastObstacle = hit.transform.GetComponent<MovingObject>();
					//Only avoid node if the obstacle is blocking a path
					if(lastObstacle.isBlocking)
						continue;
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

	List<Vector3> smoothPath(List<Vector3> inputPath)
	{
		//If there is only one node in the path, nothing to smooth
		if(inputPath.Count <= 1)
			return inputPath;

		List<Vector3> outputPath = new List<Vector3> ();

		int index = 0;
		//Check from the endNode of the inputPath the first visible node
		for(int i = inputPath.Count - 1; i >= 0; i--)
		{
			if(!Physics.Linecast (transform.position, inputPath[i], AI_Pathfinding.layoutMask))
			{
				RaycastHit hit;
				//Since smoothing happens every frame, check to avoid going to a blocked off path.
				if(Physics.Raycast (transform.position, (inputPath[i] - transform.position), out hit, (inputPath[i] - transform.position).magnitude, AI_Pathfinding.movingMask))
				{
					lastObstacle = hit.transform.GetComponent<MovingObject>();
					//Only avoid node if the obstacle is blocking a path
					if(lastObstacle.isBlocking)
						continue;
				}
				outputPath.Add(inputPath[i]);
				index = i;
				break;
			}
		}

		if(outputPath.Count == 0)
			return inputPath;

		//From the first visible node, further add to the path only the necessary nodes to access endNode
		for(int i = (index + 2); i < inputPath.Count; i++)
		{
			if(i > inputPath.Count - 1)
				break;
			if(Physics.Linecast (outputPath[outputPath.Count - 1], inputPath[i], AI_Pathfinding.layoutMask))
			{
				outputPath.Add(inputPath[i - 1]);
			}
		}

		//Add end node
		outputPath.Add (inputPath[inputPath.Count - 1]);
		
		return outputPath;
	}
}
