using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Separation : FlockingBehaviour 
{
	void Start()
	{
		
	}
	
	public override Vector3 Acceleration
	{
		get
		{
			Vector3 centroid = Vector3.zero;
			
			foreach(GameObject neighbour in Neighbours)
			{
				centroid += neighbour.transform.position;
			}			
			
			centroid /= Neighbours.Count;
			
			Vector3 directionVec = transform.position - centroid;
			Vector3 steeringAccel = MaxAcceleration * directionVec.normalized;
			
			
			return steeringAccel;
		}
	}
	
	public override List<GameObject> Neighbours
	{
		get
		{			
			List<GameObject> neighbours = new List<GameObject>();
			
			foreach(GameObject bug in GameObject.FindGameObjectsWithTag(tag))
			{
				if((bug.transform.position - transform.position).magnitude <= radius)
				{
					neighbours.Add(bug);
				}
			}			n = neighbours.Count;			
			
			return neighbours;
		}
	}
}
