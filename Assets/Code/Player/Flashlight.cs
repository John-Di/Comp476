using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour 
{
	public AudioSource click;
	public float MAX_INTENSITY;
	public Light flashlight;

	// Use this for initialization
	void Start () 
	{
		MAX_INTENSITY = flashlight.intensity;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, 0.5f)) 
		{
			transform.position = transform.parent.position + transform.parent.forward * (3.25f - 1.5f * hit.distance);
		}/*
		else
		{
			transform.position = transform.parent.position + transform.parent.forward * 3.25f;
		}*/

		if(Input.GetButtonDown("Fire1"))
		{
			flashlight.intensity += MAX_INTENSITY;			
			flashlight.intensity %= 2*MAX_INTENSITY;
			click.Play();
		}
	}
}
