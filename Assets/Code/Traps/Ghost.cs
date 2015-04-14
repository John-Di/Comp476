﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ghost : Trap 
{
	public AudioSource scream;
	public int maxSpawn = 4;
	public GameObject bug;
	GameObject b;
	public static int numberOfBugs = 0;
	public List<BugController> bugController;
	public AudioSource bugScreech;

	// Use this for initialization
	void Awake () {
		bugScreech = GameObject.FindGameObjectWithTag ("spawnpoint").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnBug(){
	//	if (numberOfBugs < maxSpawn) {
			b = (GameObject) Instantiate(bug, GameObject.FindGameObjectWithTag("spawnpoint").transform.position, Quaternion.identity);
			RoomNotifier.bc.Add(b.GetComponent<BugController>());
			BugController.BugList.Add(b);
			if(numberOfBugs < maxSpawn/2){
				b.GetComponent<BugController>().groupNumber = 1;
			}else{
				b.GetComponent<BugController>().groupNumber = 2;
			}
		//}

		numberOfBugs ++;
	}
	void OnTriggerEnter(Collider coll)
	{
		if (this.isTrapEnabled) {
			if (coll.tag == "Player") {
					if (!scream.isPlaying) {
						scream.Play ();
						DisableTrap();
					}
				

				if(numberOfBugs < maxSpawn){
					SpawnBug();

					if(numberOfBugs == maxSpawn){
						StartCoroutine("PlayScreech");
					}

					if(numberOfBugs == maxSpawn){
						bugController = GameObject.FindGameObjectsWithTag("BugController").Select(g =>{
							return g.GetComponent<BugController>();
						}).ToList();

						bugController.ForEach(b => {
							b.NotifyBugSpawn();
						});
					}
				}
			}
		}
	}

	IEnumerator PlayScreech(){
		yield return new WaitForSeconds(5f);
		if(!bugScreech.isPlaying){
			bugScreech.Play();
		}
	}
}
