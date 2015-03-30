using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateObjects : MonoBehaviour {
	public List<GameObject> roomContents = new List<GameObject>();
	public List<GameObject> trapList = new List<GameObject>();

	void Start(){

	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			foreach (GameObject content in roomContents) {
				content.SetActive(true);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.CompareTag("Player")){
			foreach (GameObject content in roomContents) {
				content.SetActive(false);
			}
		}
	}
}
