using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BugSpawner : MonoBehaviour {
	public int maxspawn = 4;
	public GameObject bug;
	GameObject b;
	public int bugs;
	public List<BugController> bc;
	int bugnumber = 1;

	PlayerController player;
	MovingWall[] mWalls;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		mWalls = GameObject.FindObjectsOfType<MovingWall> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void spawnBug(){
		if(bugs < maxspawn){
			b = (GameObject)Instantiate (bug, GameObject.FindGameObjectWithTag("spawnpoint").transform.position, Quaternion.identity);
			RoomNotifier.bc.Add(b.GetComponent<BugController>());
			BugController.BugList.Add(b);
			player.NPCs.Add(b);
			PathfindingAgent bugAgent = b.GetComponent<PathfindingAgent>();
			foreach(MovingWall wall in mWalls)
			{
				wall.agents.Add(bugAgent);
			}

			//Debug.Log("Bug Number "  + bugs);
			if(bugs < maxspawn/2){
				b.GetComponent<BugController>().groupNumber = 1;
				BugController.BugGroup1.Add(b);
			}else{
				b.GetComponent<BugController>().groupNumber = 2;
				BugController.BugGroup2.Add(b);
			}
		}
		bugs++;
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			spawnBug();

			if(bugs == maxspawn){
			//	Debug.Log("Notify done spawning");
				bc = GameObject.FindGameObjectsWithTag ("BugController").Select(g => {
					return g.GetComponent<BugController>();
				}).ToList();

				bc.ForEach(b => {b.NotifyBugSpawn();});
			}
		}
	}
}
