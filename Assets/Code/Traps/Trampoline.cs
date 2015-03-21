using UnityEngine;
using System.Collections;

public class Trampoline : Trap {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled) {
			if (other.gameObject.CompareTag("Player")) {
				//pc.isGrounded = false;
				other.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY 
					| RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				other.gameObject.rigidbody.AddForce(Vector3.up * 500f);
				other.gameObject.rigidbody.velocity = new Vector3(0, other.gameObject.rigidbody.velocity.y, 0);
				DisableTrap();
				StartCoroutine("RegainControl");
			}

		}
	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(2f);
	//	pc.isGrounded= true;
		player.rigidbody.constraints = RigidbodyConstraints.None;
	}
}
