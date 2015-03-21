using UnityEngine;
using System.Collections;

public class CollisionAvoidance : MonoBehaviour {
	public Vector3 target;

	float repelForce = 50f;
	
	float speed;

	AIMovement movement;
	MovingObject obstacle;
	PathfindingAgent agent;

	// Use this for initialization
	void Start () {
		movement = GetComponent<AIMovement> ();
		agent = GetComponent<PathfindingAgent> ();
		speed = movement.MaxVelocity;

		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = (target - transform.position).normalized;

		//Is the target reachable? i.e. the obstacle was avoided
		if(!Physics.Linecast(transform.position, target, AI_Pathfinding.movingMask) || (obstacle && obstacle.isBlocking))
		{
			movement.enabled = true;
			enabled = false;
			return;
		}

		RaycastHit hit;
		Ray ray = new Ray(transform.position, transform.forward);
		//Get normal of obstacle and add it to the direction
		if(Physics.Raycast(ray, out hit, Vector3.Distance(target, transform.position), AI_Pathfinding.movingMask))
		{
			obstacle = hit.transform.GetComponent<MovingObject>();

			direction += hit.normal * repelForce;
		}

		RaycastHit hitLayout;
		//If avoiding on the wrong side of the obstacle (e.g. one side of the obstacle is on the wall), rotate to avoid on other side
		if(Physics.Raycast(ray, out hitLayout, 1, AI_Pathfinding.layoutMask))
		{
			transform.rotation = Quaternion.LookRotation( (hitLayout.point + hitLayout.normal) - transform.position );
			//transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( (hitLayout.point + hitLayout.normal) - transform.position ), Time.deltaTime );
			//If avoiding the wall makes the agent lose sight of the target he was seeking, need to recompute the path
			if(Physics.Linecast(transform.position, target, AI_Pathfinding.layoutMask))
			{
				agent.UpdatePath();
				movement.enabled = true;
				enabled = false;
			}
			return;
		}
		
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime);
		transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
