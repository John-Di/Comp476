﻿using UnityEngine;
using System.Collections;

public class EnemyThrow : MonoBehaviour {
	GameObject player;
	private SphereCollider col;
	public bool playerInRange = false;
	public bool playerInFieldOfView = false;

	private Vector3 direction;
	public float fieldOfViewAngle = 110f;

	public GameObject projectile;
	private float shotInterval;
	private float resetShotInterval = 1.5f;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		col = GetComponent<SphereCollider> ();

		shotInterval = resetShotInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInRange) {
			direction = (player.transform.position - transform.position);
			Quaternion rotation = Quaternion.LookRotation(direction);
			Quaternion currentRot = transform.localRotation;

			transform.localRotation = Quaternion.Slerp(currentRot, rotation, Time.deltaTime);

			float angle = Vector3.Angle(direction, transform.forward); 

			if(angle < fieldOfViewAngle*0.5f){
				playerInFieldOfView = true;
				//Fire Projectile
				shotInterval -= Time.deltaTime;
				if(shotInterval < 0){
					shotInterval = resetShotInterval;
					ThrowAttack();
				}
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == player) {
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject == player) {
			playerInRange = false;
			playerInFieldOfView = false;
		}
	}

	void ThrowAttack(){
		Instantiate (projectile, transform.position, Quaternion.identity);
	}
}