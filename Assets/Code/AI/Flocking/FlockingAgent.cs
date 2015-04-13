using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class FlockingAgent : MonoBehaviour 
{    
	public float MaxVelocity;
	public float MaxAngularVelocity;
	
	public Vector3 Velocity { get; private set; }
	public float AngularVelocity { get; private set; }
	
	List<FlockingBehaviour> behaviours;
	
	void Start()
	{
		behaviours = GetComponents<FlockingBehaviour>().ToList();
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
		foreach(FlockingBehaviour b in behaviours)
		{
			Velocity += b.Acceleration * deltaTime;
		}

		Velocity += GetComponent<AIMovement>().velocity * 0.25f;

		Velocity = (Velocity.magnitude < MaxVelocity) ? Velocity : Velocity.normalized * MaxVelocity;
		
		Velocity = new Vector3(Velocity.x, 0.0f, Velocity.z);		
	}
	
	private void UpdatePosition(float deltaTime)
	{
		transform.position += Velocity * deltaTime;
	}
	
	private void UpdateRotation(float deltaTime)
	{
		if(Velocity.sqrMagnitude > 0f)
			transform.rotation = Quaternion.LookRotation(Velocity.normalized, Vector3.up);
	}
}
