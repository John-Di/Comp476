using UnityEngine;
using System.Collections;

public abstract class AssignableSteeringBehaviour : SteeringBehaviour 
{
	protected void AssignTarget(Transform t)
	{
		target = t;
	}
}
