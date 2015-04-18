using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour 
{
	public AudioSource click;
	public float MAX_INTENSITY;
	public Light flashlight;

	PlayerController player;

	float batteryLife = 15f;
	float lightBattery;

	// Use this for initialization
	void Start () 
	{
		MAX_INTENSITY = flashlight.intensity;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		lightBattery = batteryLife;
		flashlight.intensity = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
		{
			if(lightBattery > 0f)
			{
				flashlight.intensity += MAX_INTENSITY;			
				flashlight.intensity %= 2*MAX_INTENSITY;
			}
			click.Play();
		}

		if(flashlight.intensity == MAX_INTENSITY && lightBattery >= 0f)
		{
			lightBattery -= Time.deltaTime;
		}
		else if(flashlight.intensity == 0 && lightBattery < batteryLife)
		{
			lightBattery += Time.deltaTime * 2f;
		}

		if(lightBattery <= 0f && flashlight.intensity != 0)
		{
			lightBattery = 0f;
			flashlight.intensity = 0;
			click.Play();
		}

		if(player.flashLightTimer >= 0.75f)
		{
			if(flashlight.intensity == 0)
			{
				player.fearLevel += 0.025f;
			}
			else
			{
				player.fearLevel -= 0.04f;
			}
			player.flashLightTimer = 0f;
		}
	}
}
