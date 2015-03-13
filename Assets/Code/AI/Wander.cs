using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour
{
	private float timeStuck = 0.0f;	
	public float wanderDistance = 0.02f, wanderRadius = 0.005f, updateRange= 0.015f;
	public float MaxVelocity = 0.1f, MaxAcceleration = 3f;
	public Vector3 target;
	Vector3 direction;

    void Start()
    {
		//targetColor = Color.green;
		Debug.DrawLine(transform.position + new Vector3(0,1,0), transform.position + Velocity +  new Vector3(0,1,0), Color.cyan);
		Debug.DrawLine(transform.position + Velocity + new Vector3(0,1,0), (target != null) ? target: target + new Vector3(0,1,0), Color.green);

	}

    public Vector3 Velocity
    {
        get
		{				
			if((target - transform.position).magnitude <= updateRange || timeStuck <= 0.0)
			{
				target = new Vector3(Random.Range(0f, 50f),0f,Random.Range(0f, 50f));
				//target.position = new Vector3(target.position.x, 0f, target.position.z);
				timeStuck = 8.0f;
			}
			
			timeStuck -= Time.deltaTime;
			
			Vector3 directionVec = target - transform.position;
			Vector3 velocity = MaxVelocity * directionVec.normalized;
			
			return velocity;
        }
	}

	void Update()
	{		
		direction = (target - transform.position);

		UpdateRotation(Time.deltaTime);

		if(Vector3.Angle (Velocity, transform.forward) < 45f || direction.magnitude <= 5f)
		{
			UpdatePosition(Time.deltaTime);
		}
	}

	private void UpdateRotation(float deltaTime)
	{		
		float step = Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, Velocity, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void UpdatePosition(float time)
	{
		if(target == null || direction.magnitude >= 0.5f)
		{
			transform.position += Velocity * time;
		}
		else
		{
			transform.position = target;
		}
	}
}



