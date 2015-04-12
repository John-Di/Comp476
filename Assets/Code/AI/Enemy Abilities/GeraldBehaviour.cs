using UnityEngine;
using System.Collections;

public class GeraldBehaviour : MonoBehaviour
{
	GameObject[] geraldWaypoints;
	PathfindingAgent pa;
	int randomTarget;
	Vector3 geraldZinWaypointSpace;

	// Use this for initialization
	void Awake ()
	{
		pa = GetComponent<PathfindingAgent>();
	}

	void Start()
	{
		geraldWaypoints = GameObject.FindGameObjectsWithTag ("GeraldWaypoint");
		NewWaypoint ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckLocation ();
	}

	void NewWaypoint()
	{
		randomTarget = Random.Range (0, geraldWaypoints.Length);
		pa.target = geraldWaypoints[randomTarget].transform;
	}

	void CheckLocation()
	{
		if(Vector3.Distance (pa.target.position, transform.position) <= 0.1f)
		{
			Debug.Log ("Reached target");
			NewWaypoint ();
		}
	}
}
