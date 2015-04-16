using UnityEngine;
using System.Collections;

public class OogieDecision : MonoBehaviour {
	static float  timer; 
	static float respawnTimer;
	public static GameObject oogie;
	PathfindingAgent oogieAgent;
	bool isOogieActive = false;
	public GameObject oogiePrefab;
	public AudioSource oogieAudio;
	GameObject[] waypoints;
	int randomPosition;
	PlayerController player;

	// Use this for initialization
	void Start () {
		timer = Random.Range (45, 60);
		respawnTimer = Random.Range (20, 30);
		oogie = GameObject.FindGameObjectWithTag ("Oogie");
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		randomPosition = Random.Range(0, waypoints.Length);
		oogieAudio = GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		oogie = (GameObject)Instantiate (oogiePrefab, waypoints[randomPosition].transform.position, Quaternion.identity);
		oogie.transform.position = new Vector3(oogie.transform.position.x, 0.5f, oogie.transform.position.z);
		oogie.GetComponent<PathfindingAgent> ().target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

	//	Debug.Log ("TIME LEFT ALIVE: " + timer + " Time UNTIL RESPAWN " + respawnTimer);
		if (timer > 0) {
			//chase Player
			timer -= Time.deltaTime;
		} else { //timer is <=0 
			if(player.NPCs.Contains(oogie))
				player.NPCs.Remove(oogie);
			if(oogie)
				oogie.GetComponent<MonsterBehavior>().Death();
			oogie = null;
			respawnTimer -= Time.deltaTime;
			if(respawnTimer > 0){

			}else{
				RespawnOogie();
			}
		}
	}

	void RespawnOogie(){
		//resetTimers
		timer = Random.Range (45, 60);
		respawnTimer = Random.Range (20, 30);

		//Debug.Log ("Respawned Oogie "  + "Timer: " + timer + ", RespawnTimer: " + respawnTimer);
		//Move him to a waypoint
		randomPosition = Random.Range(0, waypoints.Length);
		if (!oogieAudio.isPlaying) {
			oogieAudio.Play();
			player.fearLevel += 0.05f;
		}
		oogie = (GameObject)Instantiate (oogiePrefab, waypoints[randomPosition].transform.position, Quaternion.identity);
		oogie.transform.position = new Vector3(oogie.transform.position.x, 0.5f, oogie.transform.position.z);
		oogieAgent = oogie.GetComponent<PathfindingAgent> ();
		oogieAgent.target = GameObject.FindGameObjectWithTag ("Player").transform;
		if(!player.NPCs.Contains(oogie))
			player.NPCs.Add(oogie);
	}
}
