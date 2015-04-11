using UnityEngine;
using System.Collections;

public class FakeExitTrap : Trap {
	public GameObject enemyPrefab;
	public GameObject enemy;
	public float yrotation;
	public AudioSource laughter;
	private GameObject oogieController;

	void Start(){
		laughter = gameObject.GetComponent<AudioSource> ();
	}

	void FixedUpdate(){
		//enemy = GameObject.FindGameObjectWithTag ("Oogie");
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			//Play Sinister Laugh
			if(!laughter.isPlaying){
				laughter.Play();
			}
			//enemy = GameObject.FindGameObjectWithTag("Oogie");
			if(enemy == null){
				//enemy = (GameObject) Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.Euler(player.transform.rotation.x, yrotation, player.transform.rotation.z));
				//enemy.GetComponent<PathfindingAgent>().target = GameObject.Find("Player").transform;

				//oogieController.GetComponent<OogieDecision>().enabled = true;

				Destroy (GameObject.FindGameObjectWithTag("OogieController"));
				oogieController = (GameObject) Resources.Load("OogieController");
				Instantiate(oogieController, Vector3.zero, Quaternion.identity);
				enemy = GameObject.FindGameObjectWithTag("Oogie");
				StartCoroutine("MoveNPCToLocation");
				//enemy.transform.position = Vector3.zero;
			}else{
				enemy.transform.position = this.transform.position;
			}

			DisableTrap();
			Destroy(gameObject, 3.5f);
		}
	}

	IEnumerator MoveNPCToLocation(){
		yield return new WaitForSeconds(0.1f);
		enemy.transform.position = this.gameObject.transform.position;
	}
}
