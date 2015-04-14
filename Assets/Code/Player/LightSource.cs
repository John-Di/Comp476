using UnityEngine;
using System.Collections;

public class LightSource : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player"))
		{
			PlayerController pc = other.GetComponent<PlayerController>();
			if(pc.lightFearTimer >= 0.5f)
			{
				pc.fearLevel -= 0.1f;
				pc.lightFearTimer = 0f;
			}
		}
	}
}
