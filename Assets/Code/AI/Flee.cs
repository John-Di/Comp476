using UnityEngine;
using System.Collections;
using System;

public class Flee : AssignableSteeringBehaviour
{
    void Start()
    {
		targetColor = Color.blue;
	}
	
    public override Vector3 Acceleration
    {
        get
        {			
			Vector3 directionVec = transform.position - target.position;
			Vector3 steeringAccel = MaxAcceleration * (directionVec / directionVec.magnitude);

			return steeringAccel;
        }
    }
}
