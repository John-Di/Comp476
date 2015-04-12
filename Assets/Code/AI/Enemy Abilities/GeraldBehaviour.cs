using UnityEngine;
using System.Collections;

public class GeraldBehaviour : MonoBehaviour
{
	GameObject[] geraldWaypoints;
	PathfindingAgent pa;
	int randomTarget;

	public int roomNumber;
	
	void Awake ()
	{
		pa = GetComponent<PathfindingAgent>();
		geraldWaypoints = GameObject.FindGameObjectsWithTag ("GeraldWaypoint");
		NewWaypoint ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckLocation ();
	}

	// Get a new waypoint
	void NewWaypoint()
	{
		randomTarget = Random.Range (0, geraldWaypoints.Length);
		pa.target = geraldWaypoints[randomTarget].transform;
	}

	// Selects new target when it reaches the current target
	void CheckLocation()
	{
		if(Vector3.Distance (pa.target.position, transform.position) <= 0.1f)
		{
			Debug.Log ("Reached target");
			NewWaypoint ();
		}
	}

	public void SetRoomNumber(int room)
	{
		roomNumber = room;
	}
	
	public int GetRoomNumber()
	{
		return roomNumber;
	}
}
