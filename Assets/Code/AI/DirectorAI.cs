using UnityEngine;
using System.Collections;

public class DirectorAI : MonoBehaviour {
	

	GameObject[] NPCs;

	// Use this for initialization
	void Start () {
		PathfindingAgent[] agents = GameObject.FindObjectsOfType<PathfindingAgent> ();
		NPCs = new GameObject[agents.Length];
		for(int i = 0; i < agents.Length; i++)
			NPCs[i] = agents[i].gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
