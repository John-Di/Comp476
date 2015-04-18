using UnityEngine;
using System.Collections;

public class GeraldBehaviour : MonoBehaviour
{
	GameObject[] geraldWaypoints;
	PathfindingAgent pa;
	int randomTarget;
	PlayerController player;

	public int roomNumber;
	
	void Awake ()
	{
		pa = GetComponent<PathfindingAgent>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		geraldWaypoints = GameObject.FindGameObjectsWithTag ("GeraldWaypoint");
		NewWaypoint ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckLocation ();
		CheckPlayer ();
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
		float dist = new Vector3 (pa.target.position.x - transform.position.x, 0, pa.target.position.z - transform.position.z).magnitude;
		if(dist <= 0.2f)
		{
			//Debug.Log ("Reached target");
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

	void CheckPlayer()
	{
		// Permanently sets the player as target if in the same room and not in hallways
		if(player.GetRoomNumber () != 1 && player.GetRoomNumber () != 2)
		{
			if(player.GetRoomNumber () == this.GetRoomNumber ())
			{
				SetPlayerAsTarget ();
				return;
			}
		}
	}

	void SetPlayerAsTarget()
	{
		pa.target = player.transform;
	}
}
