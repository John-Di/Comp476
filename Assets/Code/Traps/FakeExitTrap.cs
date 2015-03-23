using UnityEngine;
using System.Collections;

public class FakeExitTrap : Trap {
	public GameObject enemy;
	public float yrotation;
	public AudioSource laughter;

	void Start(){
		laughter = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			//Play Sinister Laugh
			if(!laughter.isPlaying){
				laughter.Play();
			}

			Instantiate(enemy, gameObject.transform.position, Quaternion.Euler(player.transform.rotation.x, yrotation, player.transform.rotation.z));
			DisableTrap();
			Destroy(gameObject, 3.5f);
		}
	}
}
