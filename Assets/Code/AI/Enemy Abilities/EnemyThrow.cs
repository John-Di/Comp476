using UnityEngine;
using System.Collections;

public class EnemyThrow : MonoBehaviour
{
	GameObject player;
	private SphereCollider col;
	public bool playerInRange = false;
	public bool playerInFieldOfView = false;

	private Vector3 direction;
	public float fieldOfViewAngle = 110f;

	public GameObject projectile;
	public Transform projectilePos;
	private float shotInterval;
	private float resetShotInterval = 3f;

	private AIMovement movement;
	private Animation anim;

	float maxVel;

	// Use this for initialization
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		movement = GetComponent<AIMovement>();
		col = GetComponent<SphereCollider> ();
		anim = GetComponent<Animation> ();
		maxVel = movement.MaxVelocity;

		shotInterval = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (playerInRange)
		{
			foreach (AnimationState state in anim) {
				if(state.name == "idle")
				{
					anim.clip = state.clip;
					anim.Play();
				}
			}
			movement.MaxVelocity = 0.0f;
			direction = (player.transform.position - transform.position);
			Quaternion rotation = Quaternion.LookRotation(direction);
			Quaternion currentRot = transform.localRotation;

			transform.localRotation = Quaternion.Slerp(currentRot, rotation, Time.deltaTime);

			float angle = Vector3.Angle(direction, transform.forward); 

			if(angle < fieldOfViewAngle*0.5f){
				playerInFieldOfView = true;
				//Fire Projectile
				shotInterval -= Time.deltaTime;
				if(shotInterval <= 0){
					shotInterval = resetShotInterval;
					ThrowAttack();
				}
			}
		}
		else
		{
			movement.MaxVelocity = maxVel;
			foreach (AnimationState state in anim) {
				if(state.name == "walk")
				{
					anim.clip = state.clip;
					anim.Play();
				}
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInRange = false;
			playerInFieldOfView = false;
		}
	}

	void ThrowAttack()
	{

		GameObject attack = (GameObject) Instantiate (projectile, transform.GetChild(0).transform.position, Quaternion.identity);
		attack.rigidbody.AddForce((player.transform.position - projectilePos.transform.position).normalized *400f);
	}
}
