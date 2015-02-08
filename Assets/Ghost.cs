using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour 
{
	public AudioSource scream;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.tag == "Player")
		{
			scream.Play();
		}
	}
}
