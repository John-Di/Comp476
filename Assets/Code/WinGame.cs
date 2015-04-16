using UnityEngine;
using System.Collections;

public class WinGame : MonoBehaviour {
	public GameObject exitPositions;

	// Use this for initialization
	void Start () {
		Transform[] exits = exitPositions.GetComponentsInChildren<Transform> ();
		int random = Random.Range (0, exits.Length);
		transform.position = exits [random].position;
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
