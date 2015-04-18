using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public GameObject serialKiller;
	public AudioSource deathSlenderSound;
	public AudioSource warningSound;

	PlayerController player;
	bool soundPlaying = false;
	float timeLimit = 0f;
	float timerDeath = 0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		timeLimit = Random.Range (10f, 30f);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateFearDeath ();
		UpdateFog ();
	}

	void UpdateFearDeath()
	{
		if(player.fearLevel == 1f && !soundPlaying && player.enabled == true)
		{
			warningSound.volume = 1f;
			warningSound.Play();
			soundPlaying = true;
			timerDeath = 0f;
		}
		else if(player.fearLevel <= 0.95f && soundPlaying)
		{
			warningSound.volume = Mathf.Lerp(warningSound.volume, 0f, Time.deltaTime);
			soundPlaying = false;
		}
		if(warningSound.volume < 1f && warningSound.volume > 0f)
			warningSound.volume = Mathf.Lerp(warningSound.volume, 0f, Time.deltaTime);
		if(player.fearLevel > 0.95f) 
			timerDeath += Time.deltaTime;
		if(timerDeath >= timeLimit && player.enabled == true)
		{
			player.enabled = false;
			player.rigidbody.velocity = Vector3.zero;
			player.rigidbody.isKinematic = true;
			deathSlenderSound.Play();
			GameObject slender = (GameObject)Instantiate(serialKiller, player.transform.position + player.transform.forward * 1f, Quaternion.Euler(-90,0,0));
			slender.transform.LookAt(player.transform.position);
			slender.transform.rotation = Quaternion.Euler(-90, slender.transform.rotation.eulerAngles.y, slender.transform.rotation.eulerAngles.z);
			slender.transform.position = new Vector3(slender.transform.position.x, 0f, slender.transform.position.z);
		}
		
		if(player.enabled == false && !deathSlenderSound.isPlaying && !player.anim.GetBool("isDead"))
		{
			player.Die();
		}
	}

	void UpdateFog()
	{
		RenderSettings.fogDensity = Mathf.Lerp (RenderSettings.fogDensity, player.fearLevel / 20, Time.deltaTime);
		if(RenderSettings.fogDensity < 0.01f)
			RenderSettings.fogDensity = 0.01f;
		else if(RenderSettings.fogDensity > 0.5f)
			RenderSettings.fogDensity = 0.5f;

		if(player.fearLevel < 0.2f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.grey, Time.deltaTime);
		else if(player.fearLevel < 0.3f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.grey, Time.deltaTime);
		else if(player.fearLevel < 0.4f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.grey, Time.deltaTime);
		else if(player.fearLevel < 0.5f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.blue, Time.deltaTime);
		else if(player.fearLevel < 0.6f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.blue, Time.deltaTime);
		else if(player.fearLevel < 0.7f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.blue, Time.deltaTime);
		else if(player.fearLevel < 0.8f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.red, Time.deltaTime);
		else if(player.fearLevel < 0.9f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.red, Time.deltaTime);
		else
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.black, Time.deltaTime);
	}
}
