using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BugSpawner : MonoBehaviour {
	public int maxspawn;
	GameObject[] spawnPoints;
	public GameObject bug;
	GameObject b;
	public List<BugController> bc;

	// Use this for initialization
	void Awake () {
		spawnPoints = GameObject.FindGameObjectsWithTag ("spawnpoint");
		maxspawn = spawnPoints.Count();
		spawnBug ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void spawnBug(){

		for (int i=0; i < spawnPoints.Length; i++) {
			b = (GameObject)Instantiate(bug, spawnPoints[i].transform.position,Quaternion.identity);
			RoomNotifier.bc.Add(b.GetComponent<BugController>());

			if(i < (spawnPoints.Length/2) ){
				b.GetComponent<BugController>().groupNumber =1;
				b.GetComponent<BugController>().PlayerChangedRooms(44);
			}else{
				b.GetComponent<BugController>().groupNumber =2;
				b.GetComponent<BugController>().PlayerChangedRooms(44);
			}
		}

		//RoomNotifier.bc.ForEach(s => {s.PlayerChangedRooms(44);}); //44 us player starting room
		/*
		if(bugs < maxspawn){
			b = (GameObject)Instantiate (bug, GameObject.FindGameObjectWithTag("spawnpoint").transform.position, Quaternion.identity);
			RoomNotifier.bc.Add(b.GetComponent<BugController>());
			BugController.BugList.Add(b);
			player.NPCs.Add(b);

			//Debug.Log("Bug Number "  + bugs);
			if(bugs < maxspawn/2){
				b.GetComponent<BugController>().groupNumber = 1;
			}else{
				b.GetComponent<BugController>().groupNumber = 2;
			}
		}
		bugs++;*/
	}

	IEnumerator WaitForBugs(){
		yield return new WaitForSeconds(1f);
		spawnBug ();
	}
}
