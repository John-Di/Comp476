using UnityEngine;
using System.Collections;

public class MonaLisa : MonoBehaviour {
	public Material monaLisaScary;
	public GameObject monaLisa;

	PlayerController player;
	AudioSource jumpSound;
	bool jumpscareActivated = false;

	// Use this for initialization
	void Start () {
		jumpSound = GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player") && !jumpscareActivated && !jumpSound.isPlaying) 
		{
			jumpscareActivated = true;
			jumpSound.Play();
			player.fearLevel += 0.2f;
			monaLisa.renderer.material = monaLisaScary;
		}
	}
}
