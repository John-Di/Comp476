using UnityEngine;
using System.Collections;

public class SlowingMud : Trap {
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				pc.moveSpeed /= 2.0f;
				Debug.Log("Player Entered TRAP");
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				pc.moveSpeed *= 2.0f;
				Debug.Log("Player Exited TRAP");
			}
		}
	}
}
