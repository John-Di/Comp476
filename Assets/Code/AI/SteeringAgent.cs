using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class SteeringAgent : MonoBehaviour
{
    public float MaxVelocity;
    public float MaxAngularVelocity;

    public Vector3 Velocity { get; private set; }
    public float AngularVelocity { get; private set; }

	List<SteeringBehaviour> behaviours;
    
    void Start()
    {
		behaviours = GetComponents<SteeringBehaviour>().ToList();
    }
    
    void FixedUpdate()
    {
        UpdateVelocities(Time.deltaTime);

        UpdatePosition(Time.deltaTime);
        UpdateRotation(Time.deltaTime);
    }

    public void ResetVelocities()
    {
        Velocity = Vector3.zero;
        AngularVelocity = 0f;
    }

    private void UpdateVelocities(float deltaTime)
    {
		bool halt = false;

		foreach(SteeringBehaviour b in behaviours)
		{
			if(!halt && b.HasTarget)
			{
				//Velocity += b.Acceleration * deltaTime;
				halt = b.HaltTranslation;
				Debug.Log(b.Acceleration);
			}
		}

		if(halt)
		{
			Velocity = Vector3.zero;
		}
		else
		{				
			//Velocity = new Vector3(Velocity.x, 0.0f, Velocity.z);		
			//Velocity = (Velocity.magnitude < MaxVelocity) ? Velocity : Velocity.normalized * MaxVelocity;
		}

    }

    private void UpdatePosition(float deltaTime)
    {
		transform.position += Velocity * deltaTime;
    }

    private void UpdateRotation(float deltaTime)
    {
        if(Velocity.sqrMagnitude > 0f)
		{
            transform.rotation = Quaternion.LookRotation(Velocity.normalized, Vector3.up);
		}
    }
}
