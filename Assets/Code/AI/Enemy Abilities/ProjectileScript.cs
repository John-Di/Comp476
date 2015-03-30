using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	float bulletSpeed = 2f;
	float bulletLifeTime = 10f;
	// Use this for initialization
	void Start () {
		rigidbody.AddForce (transform.up * bulletSpeed);
		Destroy (gameObject, bulletLifeTime);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
