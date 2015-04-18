using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour {
	public bool isBlocking = false;
	public bool moveStopped = false;

	bool hasMoved = false;

	float radius;

	Vector3 curPos, lastPos;

	// Use this for initialization
	void Start () {
		radius = transform.localScale.x / 2;
	}

	// Update is called once per frame
	void Update () {
		UpdateHasMoved ();

		//Only update the blocking condition if moved
		if(moveStopped)
			UpdateIsBlocking();
	}

	public void SetMoved(bool b)
	{
		moveStopped = b;
	}

	void UpdateHasMoved()
	{
		//Check if last obstacle recorded has moved (e.g. a door that was just closed has opened)
		bool obstacleMovedStopped = false;
		curPos = transform.position;
		if(curPos != lastPos)
		{
			hasMoved = true;
		}
		else if(hasMoved)
		{
			obstacleMovedStopped = true;
			hasMoved = false;
		}
		lastPos = curPos;
		
		moveStopped = obstacleMovedStopped;
	}

	public void UpdateIsBlocking()
	{
		List<Vector3> colliderDirections = new List<Vector3> ();
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, AI_Pathfinding.layoutMask);
		//First check what colliders are hitting based on the radius
		for(int i = 0; i < hitColliders.Length; i++)
		{
			//Then for each collider check if the collider bounds intersects
			if(collider.bounds.Intersects(hitColliders[i].collider.bounds))
			{
				//Store the orientation of each collider
				Vector3 vHit1 = hitColliders[i].transform.position + (hitColliders[i].transform.localScale.x / 2) * (Quaternion.AngleAxis(90, Vector3.up) * hitColliders[i].transform.forward);
				Vector3 vHit2 = hitColliders[i].transform.position + (hitColliders[i].transform.localScale.x / 2) * (Quaternion.AngleAxis(-90, Vector3.up) * hitColliders[i].transform.forward);
				//Debug.DrawRay(vHit1, (vHit2 - vHit1), Color.green, 5f);
				Vector3 orient = (vHit1 - vHit2).normalized;
				colliderDirections.Add(orient);
			}
		}

		int count = 0;
		for(int i = 0; i < colliderDirections.Count; i++)
		{
			for(int j = 0; j < colliderDirections.Count; j++)
			{
				if(i == j)
					continue;
				//For each intersecting collider, check if any 2 colliders were parallel. This is to confirm if colliders could be a corridor for example.
				if(Vector3.Angle(colliderDirections[i], colliderDirections[j]) <= 10)
					count++;
			}
		}

		if(count >= 2)
			isBlocking = true;
		else
			isBlocking = false;
	}
}
