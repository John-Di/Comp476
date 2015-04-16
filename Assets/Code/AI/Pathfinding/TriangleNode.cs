using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriangleNode : MonoBehaviour, IComparer<TriangleNode> {
	//Fields
	public List<TriangleNode> neighborList = new List<TriangleNode>();
	
	public float costSoFar, heuristicValue, totalEstimatedValue;
	
	public TriangleNode prevNode;

	public int pathLengh = 0;

	public Vector3 [] vertices = new Vector3[3];

	public Vector3 nodePosition;
	
	// Use this for initialization
	void Start ()
	{
		nodePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void draw(Color c)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			for (int j = 0; j < vertices.Length; j++)
			{
				Debug.DrawLine(vertices[i], vertices[j], c, 20f);
			}
		}

		for (int i = 0; i < neighborList.Count; i++)
		{
			Debug.DrawLine(nodePosition, neighborList[i].nodePosition, Color.green, 20f);
		}
	}

	public void SetVertices(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		vertices[0] = v1;
		vertices[1] = v2;
		vertices[2] = v3;
	}

	public void CreateMesh()
	{
		Mesh mesh = new Mesh();
		GetComponentInChildren<MeshCollider> ().sharedMesh = mesh;
		mesh.vertices = vertices;
		mesh.triangles = new int[] {0 , 1, 2};

		//Offset the collider child by the transform position
		transform.GetChild (0).transform.position -= transform.position;
	}

	public bool SharesEdge(TriangleNode tNode)
	{
		int count = 0;
		for(int i = 0; i < vertices.Length; i++)
		{
			for(int j = 0; j < tNode.vertices.Length; j++)
			{
				if(vertices[i] == tNode.vertices[j])
					count++;
			}
		}
		if(count >= 2)
			return true;
		
		return false;
	}

	public Vector3 GetClosestPointInTriangle(Vector3 target)
	{
		float distance;

		Vector3 point = ClosestPointOnLine (vertices [0], vertices [1], target);
		distance = (target - point).magnitude;

		Vector3 point2 = ClosestPointOnLine (vertices [0], vertices [2], target);
		if((target - point2).magnitude < distance)
		{
			distance = (target - point2).magnitude;
			point = point2;
		}

		Vector3 point3 = ClosestPointOnLine (vertices [1], vertices [2], target);
		if((target - point3).magnitude < distance)
			point = point3;

		return point;
	}
	
	Vector3 ClosestPointOnLine(Vector3 vA,  Vector3 vB, Vector3 vPoint)
	{
		Vector3 vVector1 = vPoint - vA;
		Vector3 vVector2 = (vB - vA).normalized;
		
		float d = Vector3.Distance(vA, vB);
		float t = Vector3.Dot(vVector2, vVector1);
		
		if (t <= 0)
			return vA;
		
		if (t >= d)
			return vB;
		
		Vector3 vVector3 = vVector2 * t;
		
		Vector3 vClosestPoint = vA + vVector3;
		
		return vClosestPoint;
	}

	public void UpdatePrevNode(TriangleNode prev)
	{
		prevNode = prev;
		pathLengh = prev.pathLengh + 1;
	}
	
	public void UpdateCostSoFar(float newCost)
	{
		costSoFar = newCost;
		totalEstimatedValue = costSoFar + heuristicValue;
	}
	
	public void UpdateHeuristic(float newHeuristic)
	{
		heuristicValue = newHeuristic;
		totalEstimatedValue = costSoFar + heuristicValue;
	}
	
	public int Compare(TriangleNode x, TriangleNode y)
	{
		int compareResult = x.totalEstimatedValue.CompareTo (y.totalEstimatedValue);
		if (compareResult == 0) {
			return (x.heuristicValue.CompareTo (y.heuristicValue));
		}
		return compareResult;
	}
}
