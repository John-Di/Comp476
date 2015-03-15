using UnityEngine;
using System.Collections;
using System;

public class Arrive : AssignableSteeringBehaviour
{
    public float slowRadius;
    public float arriveRadius;

    void Start()
	{
		targetColor = Color.magenta;
	}


    public override Vector3 Acceleration
    {
        get
        {			
			Vector3 directionVec = target.transform.position - transform.position;
			Vector3 acceleration = MaxAcceleration * directionVec.normalized;

			return (directionVec.magnitude <= slowRadius) ? -acceleration : acceleration;

        }
    }

    public override bool HaltTranslation
	{
        get
		{
			return ((target.transform.position - transform.position).magnitude <= arriveRadius);
        }
    }
}
