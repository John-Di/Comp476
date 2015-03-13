using UnityEngine;
using System.Collections;
using System;

public class Seek : AssignableSteeringBehaviour
{
    void Start()
	{
		targetColor = Color.cyan;		
    }

    public override Vector3 Acceleration
    {
        get
        {			
			Vector3 directionVec = target.transform.position - transform.position;
			Vector3 acceleration = MaxAcceleration * directionVec.normalized;

			return acceleration;
        }
    }
}
