using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class BugController : MonoBehaviour {
	public enum State{Surround, Chase, Rest};
	public State bugState;
	public PlayerController pc;
	public int playerRoomNumber; // change to private later
	public GameObject[] waypointArray;
	public List<GameObject> currentWaypointList;

	public int groupNumber;
	bool firstTime = false; //Delete this

	private const int  maxSwarmCount = 4;
	private int numberOfBugs = 0;

	Transform currentNodePosition;

	public AudioSource bugKill;
	public static bool alreadyPlayed = false;

	public State state{
		get{
			return bugState;
		}
		set{
			ExitState(bugState);
			bugState = value;
			EnterState(bugState);
		}
	}

	// Use this for initialization
	void Awake () {
		Physics.IgnoreLayerCollision (11, 11, true); //Add this until john get's the flocking working

		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		waypointArray = GameObject.FindGameObjectsWithTag ("Waypoint");

		//Set Initial state to Wait
		bugState = State.Chase;
		EnterState (bugState);
	}

	void ExitState(State exitedState){
		switch(exitedState){
			case State.Surround:
				this.GetComponent<AIMovement>().enabled = false;
				this.GetComponent<PathfindingAgent>().enabled = false;
				this.GetComponent<CollisionAvoidance> ().enabled = false;
				ClearCurrentWaypointList();
				//player is in a room, end finding player
				break;
			case State.Chase:
				//end finding player or finding and reaching waypoint
				this.GetComponent<AIMovement>().enabled = false;
				this.GetComponent<PathfindingAgent>().enabled = false;
				this.GetComponent<CollisionAvoidance> ().enabled = false;
				break;
			case State.Rest:
				//NPC Collided with player, end chase
				break;
		}
	}

	void EnterState(State enteredState){
		switch (enteredState) {
//			case State.Wait:
//				//SpawnCheck();
//				WaitCheck();
//				break;
//			case State.FindingPlayer:
//			//find Player
//				break;
			case State.Surround:
				SurroundPlayer();
				break;
			case State.Chase:
				StartChasing();
				break;
			case State.Rest:
				//Player Is Dead LAWL LAWL LAWL
				break;
		}
	}

	/*
	void WaitCheck(){
		if(numberOfBugs < maxSwarmCount){
			this.GetComponent<AIMovement>().enabled = true;
			this.GetComponent<PathfindingAgent>().enabled = true;
			this.GetComponent<PathfindingAgent>().target = wanderPoint.transform;
			this.GetComponent<CollisionAvoidance> ().enabled = true;
		}else if (numberOfBugs == maxSwarmCount) {
			ExitState(bugState);
			bugState = State.FindingPlayer;
			EnterState(bugState);
		} 
	}*/
	

	void StartChasing(){
		this.GetComponent<AIMovement>().enabled = true;
		this.GetComponent<PathfindingAgent>().enabled = true;
		this.GetComponent<PathfindingAgent>().target = GameObject.FindGameObjectWithTag("Player").transform;
		this.GetComponent<CollisionAvoidance> ().enabled = true;
	}

	void SurroundPlayer(){
		//change State to FindPlayer
		int random = Random.Range (0, currentWaypointList.Count);
		currentNodePosition = currentWaypointList [random].transform;
		if(this.groupNumber == 1){
			this.GetComponent<AIMovement>().enabled = true;
			this.GetComponent<PathfindingAgent>().enabled = true;
			this.GetComponent<PathfindingAgent>().target = currentNodePosition;
			this.GetComponent<CollisionAvoidance> ().enabled = true;
		}

		if(this.groupNumber == 2){
			ExitState(bugState);
			bugState = State.Chase;
			EnterState(bugState);
		}
	}
	
	private void ClearCurrentWaypointList(){
		currentWaypointList.Clear ();
	}

	public void PlayerChangedRooms(int roomNumber){
		if (bugState == State.Surround || bugState == State.Chase) {
			if (playerRoomNumber != roomNumber) {
				playerRoomNumber = roomNumber;
				ClearCurrentWaypointList ();

				for (int i = 0; i < waypointArray.Length; i++) {
					int waypointID = waypointArray [i].GetComponent<Waypoint> ().GetWaypointID ();
					if (waypointID == playerRoomNumber) {
						currentWaypointList.Add (waypointArray [i]);
					}
				}


				if (playerRoomNumber == 1 || playerRoomNumber == 2) { //Player is in Hallway
					ExitState(bugState);
					bugState = State.Chase;
					EnterState(bugState);
				} else {
					ExitState(bugState);
					bugState = State.Surround;
					EnterState(bugState);
				}
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player")) {
			other.GetComponent<PlayerController>().Die();
			ExitState(bugState);
			bugState = State.Rest;
			EnterState(bugState);

			if(!alreadyPlayed){
				bugKill.Play();
			}
		}

		if (currentNodePosition != null && bugState == State.Surround) {
			if (other.gameObject.Equals (currentNodePosition.gameObject)) {
				ExitState (bugState);
				bugState = State.Chase;
				EnterState (bugState);
			}
		}
	}
}
