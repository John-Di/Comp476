using UnityEngine;
using System.Collections;

public class SnapTrap : Trap {

	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				other.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				StartCoroutine("RegainControl");
			}
		}
	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(1.25f);
		//	pc.isGrounded= true;
		player.rigidbody.constraints = RigidbodyConstraints.None;
		DisableTrap ();
	}
}
