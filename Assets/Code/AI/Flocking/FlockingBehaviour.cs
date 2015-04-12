using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(FlockingAgent))]
public abstract class FlockingBehaviour : MonoBehaviour
{
	public float MaxAcceleration;
	public float MaxAngularAcceleration;
	public float radius = 50f;
	public int n;
	
	public virtual Vector3 Acceleration
	{
		get
		{
			return Vector3.zero;
		}
	}
	
	public virtual float AngularAcceleration
	{
		get
		{
			return 0f;
		}
	}
	
	public virtual bool HaltTranslation
	{
		get
		{
			return false;
		}
	}
	
	public virtual bool HaltRotation
	{
		get
		{
			return false;
		}
	}
	
	public virtual List<GameObject> Neighbours
	{
		get
		{
			return null;
		}
	}
}
