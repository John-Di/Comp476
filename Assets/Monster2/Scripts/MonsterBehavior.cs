using UnityEngine;
using System.Collections;

public class MonsterBehavior : MonoBehaviour {
	public AudioSource walkSound;
	public AudioSource runSound;
	public AudioSource growlSound;
	public GameObject fadeOut;

	PathfindingAgent agent;
	Animator anim;
	float distance;
	float growlTime = 0f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<PathfindingAgent> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		distance = new Vector3((agent.target.transform.position.x - transform.position.x), 0f, (agent.target.transform.position.z - transform.position.z)).magnitude;
		anim.SetFloat ("distance", distance);

		UpdateSound ();
	}

	void UpdateSound()
	{
		if(distance <= 20f && !walkSound.isPlaying)
			walkSound.Play();
		else if(distance > 20f && !runSound.isPlaying)
			runSound.Play();

		if(growlTime >= 10f && !growlSound.isPlaying)
		{
			growlSound.Play();
			growlTime = 0f;
		}
		else
			growlTime += Time.deltaTime;
	}

	public void Death()
	{
		GameObject particles = (GameObject)Instantiate(fadeOut, transform.position, Quaternion.identity);
		particles.transform.position = new Vector3(particles.transform.position.x, particles.transform.position.y + 5f, particles.transform.position.z);
		Destroy (gameObject);
	}
}
