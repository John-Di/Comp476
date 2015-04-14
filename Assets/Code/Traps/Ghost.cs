using UnityEngine;
using System.Collections;

public class Ghost : Trap 
{
	public AudioSource scream;

	// Use this for initialization
	void Start () {
		fearValue = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll)
	{
		if (this.isTrapEnabled) {
			if (coll.tag == "Player") {
				if (!scream.isPlaying) {
					coll.gameObject.GetComponent<PlayerController>().fearLevel += fearValue;
					scream.Play ();
					DisableTrap();
				}
			}
		}
	}
}
