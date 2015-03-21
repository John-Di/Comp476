using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Pathfinding : MonoBehaviour {
	public TriangleNode node;

	public static List<TriangleNode> nodeList = new List<TriangleNode>();

	public static LayerMask layoutMask = 1 << 8;
	public static LayerMask navigationMask = 1 << 9;
	public static LayerMask movingMask = 1 << 10;

	Transform navigationNodes;

	// Use this for initialization
	void Start () {
		navigationNodes = GameObject.FindGameObjectWithTag("NavigationNodes").transform;

		NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();

		//Loop through all triangles generated from the navmesh and add them to a list of triangle nodes
		for(int i = 0; i < triangles.indices.Length; i += 3)
		{
			Vector3 v1 = triangles.vertices[triangles.indices[i]];
			Vector3 v2 = triangles.vertices[triangles.indices[i + 1]];
			Vector3 v3 = triangles.vertices[triangles.indices[i + 2]];

			Vector3 centroid = (v1 + v2 + v3) / 3;

			//Instantiate the triangle node at the centroid position
			TriangleNode tNode = (TriangleNode)Instantiate(node, centroid, Quaternion.identity);
			//Set parent (for clean hierarchy in inspector)
			tNode.transform.parent = navigationNodes;
			tNode.transform.position = new Vector3(tNode.transform.position.x, 0, tNode.transform.position.z);
			tNode.SetVertices(v1, v2 ,v3);
			//Create the mesh used for collision checking to assign triangle nodes to positions.
			tNode.CreateMesh();

			nodeList.Add(tNode);
		}

		//Generate neighbors for each triangle node
		for (int i = 0; i < nodeList.Count; i++)
		{
			GenerateNeighbors(nodeList[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void GenerateNeighbors(TriangleNode tNode)
	{
		for (int i = 0; i < nodeList.Count; i++)
		{
			if(nodeList[i] == tNode)
				continue;

			//Nodes are neighbors if they share a vertex
			if(tNode.SharesEdge(nodeList[i]))
			{
				tNode.neighborList.Add(nodeList[i]);
			}
		}
	}
}
