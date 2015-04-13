using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Alignment : FlockingBehaviour 
{
	void Start()
	{
		
	}
	
	public override Vector3 Acceleration
	{
		get
		{
			Vector3 velocitoid = Vector3.zero;
			
			foreach(GameObject neighbour in Neighbours)
			{
				velocitoid += neighbour.GetComponent<FlockingAgent>().Velocity;
			}
			
			velocitoid /= Neighbours.Count;
			
			//Vector3 directionVec = velocitoid - transform.position;
			Vector3 steeringAccel = MaxAcceleration * velocitoid.normalized;
			
			//Debug.DrawRay(transform.position + new Vector3(0f,1f,0f), velocitoid + new Vector3(0f,1f,0f), Color.cyan);
			
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
			}					n = neighbours.Count;	
			
			return neighbours;
		}
	}
}
