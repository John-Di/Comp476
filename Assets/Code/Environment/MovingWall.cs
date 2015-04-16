using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWall : MonoBehaviour {
	PlayerController player;
	AudioSource sound;
	MovingObject moving;
	Collider[] colliders;

	public int wallType = 0;//Appearing or disappearing?
	public int fearType = 0;//Need more fear than a threshold or less fear than a threshold?
	public float fearRequired;

	float activationTime = 0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		sound = GetComponent<AudioSource> ();
		moving = GetComponent<MovingObject> ();
		colliders = GetComponents<Collider> ();
		moving.isBlocking = true;
		renderer.enabled = false;
		collider.enabled = false;
		wallType = Random.Range (0, 2);
		fearType = Random.Range (0, 2);
		fearRequired = Random.Range (0.1f, 0.9f);
	}

	// Update is called once per frame
	void Update () {
		if(player.anim.GetBool("isDead"))
			return;

		float dist = Vector3.Distance(transform.position, player.transform.position);
		bool playerSees = !Physics.Linecast (player.transform.position, transform.position, (1 << 8));

		if(dist <= 60f && playerSees)
			UpdateWall();
		else if(renderer.enabled && (dist > 60f || !playerSees))
		{
			renderer.enabled = false;
			collider.enabled = false;
		}
	}

	void UpdateAppearing()
	{
		bool checkFear = ((fearType == 0 && player.fearLevel >= fearRequired) || (fearType == 1 && player.fearLevel < fearRequired));
		if(checkFear && !renderer.enabled)
		{
			if(!sound.isPlaying)
				sound.Play();
			renderer.enabled = true;
			collider.enabled = true;
			activationTime = 0f;
		}
		else if(!checkFear && renderer.enabled)
		{
			renderer.enabled = false;
			collider.enabled = false;
			activationTime = 0f;
		}
	}

	void UpdateDisappearing()
	{
		bool checkFear = (fearType == 0 && player.fearLevel >= fearRequired) || (fearType == 1 && player.fearLevel < fearRequired);
		if(checkFear && renderer.enabled)
		{
			renderer.enabled = false;
			collider.enabled = false;
			activationTime = 0f;
		}
		else if(!checkFear && !renderer.enabled)
		{
			if(!sound.isPlaying)
				sound.Play();
			renderer.enabled = true;
			collider.enabled = true;
			activationTime = 0f;
		}
	}

	void UpdateWall()
	{
//		if(activationTime < 1f)
//		{
//			activationTime += Time.deltaTime;
//			return;
//		}
		switch(wallType)
		{
		case 0://Appearing Wall
			UpdateAppearing();
			break;
		case 1://Disappearing Wall
			UpdateDisappearing();
			break;
		}
		moving.isBlocking = true;
	}
}
