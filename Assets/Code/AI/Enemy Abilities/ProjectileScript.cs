using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
	float bulletSpeed = 2f;
	float bulletLifeTime = 5f;

	// Use this for initialization
	void Start ()
	{
		rigidbody.AddForce (transform.up * bulletSpeed);
		Destroy (gameObject, bulletLifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag ("Player") || other.CompareTag ("Wall"))
		{
			Destroy (gameObject);
		}
	}
}
