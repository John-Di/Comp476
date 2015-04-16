using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public GameObject serialKiller;
	public AudioSource deathSound;

	PlayerController player;
	bool soundPlaying = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
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
			deathSound.volume = 1f;
			deathSound.Play();
			soundPlaying = true;
		}
		else if(player.fearLevel <= 0.95f && soundPlaying)
		{
			deathSound.volume = Mathf.Lerp(deathSound.volume, 0f, Time.deltaTime);
			soundPlaying = false;
		}
		if(deathSound.volume < 1f && deathSound.volume > 0f)
			deathSound.volume = Mathf.Lerp(deathSound.volume, 0f, Time.deltaTime);

		
		if(deathSound.time >= 10f && deathSound.volume == 1f && player.enabled == true)
		{
			player.enabled = false;
			player.rigidbody.velocity = Vector3.zero;
			player.rigidbody.isKinematic = true;
			GameObject slender = (GameObject)Instantiate(serialKiller, player.transform.position + player.transform.forward * 1f, Quaternion.Euler(-90,0,0));
			slender.transform.LookAt(player.transform.position);
			slender.transform.rotation = Quaternion.Euler(-90, slender.transform.rotation.eulerAngles.y, slender.transform.rotation.eulerAngles.z);
			slender.transform.position = new Vector3(slender.transform.position.x, -0.75f, slender.transform.position.z);
		}
		else if(deathSound.time >= 10f && deathSound.volume < 1f)
			deathSound.volume = 0f;
		
		if(player.enabled == false && !deathSound.isPlaying && !player.anim.GetBool("isDead"))
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
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.red, Time.deltaTime);
		else if(player.fearLevel < 0.8f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.red, Time.deltaTime);
		else if(player.fearLevel < 0.9f)
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.black, Time.deltaTime);
		else
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.black, Time.deltaTime);
	}
}
