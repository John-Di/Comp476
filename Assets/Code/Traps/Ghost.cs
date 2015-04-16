using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ghost : Trap {
	public AudioSource scream;
	MovingWall[] mWalls;

	// Use this for initialization
	void Start () {
		mWalls = GameObject.FindObjectsOfType<MovingWall> ();
		scream = GetComponent<AudioSource> ();

		fearValue = 0.15f;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (this.isTrapEnabled) {
			if (coll.tag == "Player") {
					if (!scream.isPlaying) {
						scream.Play ();
						DisableTrap();
					pc.fearLevel += fearValue;
					}
				

	/*			if(numberOfBugs < maxSpawn){
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
				}*/
			}
		}
	}
}
