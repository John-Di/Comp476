using UnityEngine;
using System.Collections;

public class OogieKill : MonoBehaviour {
	public AudioSource yell;
	bool firstTimePlaying = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<PlayerController>().Die();
			this.gameObject.GetComponent<PathfindingAgent>().enabled = false;
			this.gameObject.GetComponent<AIMovement>().enabled = false;
			this.gameObject.GetComponent<CollisionAvoidance>().enabled = false;
			if(firstTimePlaying){
				yell.Play();
				firstTimePlaying = false;
			}
		}
	}
}
