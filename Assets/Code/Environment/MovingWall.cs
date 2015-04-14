using UnityEngine;
using System.Collections;

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
		wallType = Random.Range (0, 2);
		fearType = Random.Range (0, 2);
		fearRequired = Random.Range (0.1f, 0.9f);

		SetupWall ();
	}

	void SetupWall()
	{
		switch(wallType)
		{
		case 0://Appearing Wall --> set it to disappeared
			renderer.enabled = false;
			foreach(Collider c in colliders) {
				c.enabled = false;
			}
			moving.SetMoved(true);
			break;
		case 1://Disappearing Wall --> set it to appeared
			renderer.enabled = true;
			foreach(Collider c in colliders) {
				c.enabled = true;
			}
			moving.SetMoved(true);
			break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void UpdateAppearing()
	{
		bool checkFear = ((fearType == 0 && player.fearLevel >= fearRequired) || (fearType == 1 && player.fearLevel < fearRequired));
		if(checkFear && !renderer.enabled)
		{
			if(!sound.isPlaying)
				sound.Play();
			renderer.enabled = true;
			foreach(Collider c in colliders) {
				c.enabled = true;
			}
			moving.SetMoved(true);
			activationTime = 0f;
		}
		else if(!checkFear && renderer.enabled)
		{
			renderer.enabled = false;
			foreach(Collider c in colliders) {
				c.enabled = false;
			}
			moving.SetMoved(true);
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
			foreach(Collider c in colliders) {
				c.enabled = false;
			}
			moving.SetMoved(true);
			activationTime = 0f;
		}
		else if(!checkFear && !renderer.enabled)
		{
			renderer.enabled = true;
			foreach(Collider c in colliders) {
				c.enabled = true;
			}
			moving.SetMoved(true);
			activationTime = 0f;
		}
	}

	void UpdateWall()
	{
		if(activationTime < 5f)
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

	void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player"))
		{
			UpdateWall();
		}
	}
}
