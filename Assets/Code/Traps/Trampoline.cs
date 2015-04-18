using UnityEngine;
using System.Collections;

public class Trampoline : Trap {
	public bool trapActive;

	void Start()
	{
		fearValue = 0.075f;
	}

	void Update(){
		trapActive = isTrapEnabled;
	}
	
	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled) {
			if (other.gameObject.CompareTag("Player")) {
				pc.fearLevel += fearValue;
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
		player.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		yield return new WaitForSeconds (5f);
		EnableTrap ();
	}
}
