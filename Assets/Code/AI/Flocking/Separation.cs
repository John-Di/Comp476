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

			List<GameObject> enemies = new List<GameObject>();


			if(GameObject.Find("Oogie") != null)
			{
				enemies.Add(GameObject.Find("Oogie"));
			}

			if(GameObject.Find("Mr. Gerald") != null)
			{
				enemies.Add(GameObject.Find("Mr. Gerald"));
			}

			foreach(GameObject bug in GameObject.FindGameObjectsWithTag(tag))
			{
				enemies.Add(bug);
			}

			foreach(GameObject e in enemies)
			{
				if((e.transform.position - transform.position).magnitude <= radius)
				{
					neighbours.Add(e);
				}
			}			n = neighbours.Count;			
			
			return neighbours;
		}
	}
}
