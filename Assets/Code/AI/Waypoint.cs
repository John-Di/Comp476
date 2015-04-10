using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	public int id;

	public int GetWaypointID(){
		return this.id;
	}

	public Vector3 GetWaypointPosition(){
		return this.transform.position;
	}
}
