using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWall : MonoBehaviour {
	PlayerController player;
	AudioSource sound;
	MovingObject moving;
	Collider[] colliders;
	public List<PathfindingAgent> agents;

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
		PathfindingAgent[] agentsArray = GameObject.FindObjectsOfType<PathfindingAgent> ();
		foreach(PathfindingAgent agent in agentsArray)
			agents.Add(agent);
		renderer.enabled = false;
		collider.enabled = false;
		wallType = Random.Range (0, 2);
		fearType = Random.Range (0, 2);
		fearRequired = Random.Range (0.1f, 0.9f);
	}

//	void SetupWall()
//	{
//		switch(wallType)
//		{
//		case 0://Appearing Wall --> set it to disappeared
//			renderer.enabled = false;
//			collider.enabled = false;
//			moving.SetMoved(true);
//			break;
//		case 1://Disappearing Wall --> set it to appeared
//			renderer.enabled = true;
//			collider.enabled = true;
//			moving.SetMoved(true);
//			break;
//		}
//	}

	// Update is called once per frame
	void Update () {
		float dist = -1f;
		if(!player.anim.GetBool("isDead"))
			dist = Vector3.Distance(transform.position, player.transform.position);

		if(dist != -1f && dist <= 40f)
			UpdateWall();
		else if(dist != -1f && dist > 40f && renderer.enabled)
		{
			renderer.enabled = false;
			collider.enabled = false;
			foreach(PathfindingAgent agent in agents)
				agent.UpdatePath();
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
			foreach(PathfindingAgent agent in agents)
				agent.UpdatePath();
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
			if(!sound.isPlaying)
				sound.Play();
			renderer.enabled = false;
			collider.enabled = false;
			activationTime = 0f;
		}
		else if(!checkFear && !renderer.enabled)
		{
			renderer.enabled = true;
			collider.enabled = true;
			foreach(PathfindingAgent agent in agents)
				agent.UpdatePath();
			activationTime = 0f;
		}
	}

	void UpdateWall()
	{
		if(activationTime < 3f)
		{
			activationTime += Time.deltaTime;
			return;
		}
		switch(wallType)
		{
		case 0://Appearing Wall
			UpdateAppearing();
			break;
		case 1://Disappearing Wall
			UpdateDisappearing();
			break;
		}
	}
}
