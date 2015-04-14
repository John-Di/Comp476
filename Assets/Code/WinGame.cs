using UnityEngine;
using System.Collections;

public class WinGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			AutoFade.LoadLevel ("Win Screen", 2, 1, Color.white);
		}
	}
}
