using UnityEngine;
using System.Collections;

public class SnapTrap : Trap {
	Vector3 direction;

	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				other.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				StartCoroutine("RegainControl");
			}
		}
	}

	void Update(){
		//Rotate To face player, only in the Y Axis
		direction = (transform.position - player.transform.position);
		direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction);
		Quaternion currentRot = transform.localRotation;
		
		transform.localRotation = Quaternion.Slerp(currentRot, rotation, Time.deltaTime * 5.0f);

	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(1.25f);
		player.rigidbody.constraints = RigidbodyConstraints.None;
		DisableTrap ();
		DisableGameTrap ();
	}
}
