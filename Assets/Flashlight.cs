using UnityEngine;
using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
		{
			gameObject.GetComponent<Light>().intensity++;			
			gameObject.GetComponent<Light>().intensity %= 2;
		}
	}
	
}
