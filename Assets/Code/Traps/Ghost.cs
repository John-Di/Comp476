using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ghost : Trap {
	public AudioSource scream;

	// Use this for initialization
	void Start () {
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
			}
		}
	}
}
