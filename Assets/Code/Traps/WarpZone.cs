using UnityEngine;
using System.Collections;

public class WarpZone : Trap {

	public GameObject entrance;
	public GameObject exit;

	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled) {
			if (other.gameObject == player) {
				player.transform.position = new Vector3(exit.transform.position.x, player.transform.position.y, exit.transform.position.z);
				DisableTrap();
			}
		}
	}
}
