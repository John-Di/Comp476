using UnityEngine;
using System.Collections;

public class Magnetic : Trap {

	Transform magnetField;
	float radius;
	float force = 135f;
	SphereCollider sphereCol; 
	Vector3 direction;
	bool inProximity;
	// Use this for initialization

	void Start () {
		magnetField = gameObject.GetComponent<Transform> ();
		inProximity = false;
		EnableTrap ();
		sphereCol = gameObject.GetComponent<SphereCollider> ();
		radius = sphereCol.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (inProximity) {
			Vector3 attractField = magnetField.position - player.transform.position;
			float index = (radius - attractField.magnitude) / radius;
			player.rigidbody.AddForce (force * attractField * index);
		} 

		direction = (transform.position - player.transform.position) - new Vector3(0, 0.5f, 0);
		Quaternion rotation = Quaternion.LookRotation(direction);
		Quaternion currentRot = transform.localRotation;
		
		transform.localRotation = Quaternion.Slerp(currentRot, rotation, Time.deltaTime * 5.0f);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.Equals(player)) {
			inProximity = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.Equals(player)) {
			inProximity = false;
		}
	}
}
