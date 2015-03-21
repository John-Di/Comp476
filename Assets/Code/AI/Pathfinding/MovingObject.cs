using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour {
	public bool isBlocking = false;

	float radius;

	Vector3 curPos, lastPos;

	// Use this for initialization
	void Start () {
		radius = transform.localScale.x / 2;
	}

	// Update is called once per frame
	void Update () {
		curPos = transform.position;
		if(curPos != lastPos)
		{
			//Only update the blocking condition if moved
			UpdateIsBlocking();
		}
		lastPos = curPos;
	}

	void UpdateIsBlocking()
	{
		List<Vector3> colliderDirections = new List<Vector3> ();
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, AI_Pathfinding.layoutMask);
		//First check what colliders are hitting based on the radius
		for(int i = 0; i < hitColliders.Length; i++)
		{
			//Then for each collider check if the renderer bounds intersects
			if(renderer.bounds.Intersects(hitColliders[i].renderer.bounds))
			{
				//Store the orientation of each collider
				Vector3 vHit1 = hitColliders[i].transform.position + (hitColliders[i].transform.localScale.x / 2) * (Quaternion.AngleAxis(90, Vector3.up) * hitColliders[i].transform.forward);
				Vector3 vHit2 = hitColliders[i].transform.position + (hitColliders[i].transform.localScale.x / 2) * (Quaternion.AngleAxis(-90, Vector3.up) * hitColliders[i].transform.forward);
				Debug.DrawRay(vHit1, (vHit2 - vHit1), Color.green, 1f);
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
