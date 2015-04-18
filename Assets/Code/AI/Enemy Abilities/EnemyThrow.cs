using UnityEngine;
using System.Collections;

public class EnemyThrow : MonoBehaviour
{
	GameObject player;
	PlayerController pc;
	private SphereCollider col;
	public bool playerInRange = false;
	public bool playerInFieldOfView = false;

	private Vector3 direction;
	public float fieldOfViewAngle = 110f;

	public GameObject projectile;
	public Transform projectilePos;
	private float shotInterval;
	private float resetShotInterval = 1.5f;

	private AIMovement movement;
	private Animation anim;

	float maxVel;

	// Use this for initialization
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		pc = player.GetComponent<PlayerController> ();
		movement = GetComponent<AIMovement>();
		col = GetComponent<SphereCollider> ();
		anim = GetComponent<Animation> ();
		maxVel = movement.MaxVelocity;

		shotInterval = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!pc.anim.GetBool("isDead"))
		{
			float dist = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z).magnitude;
			if(dist <= 30f)
			{
				playerInRange = true;
			}
			else
			{
				playerInRange = false;
				playerInFieldOfView = false;
			}
		}
		if (playerInRange)
		{
			foreach (AnimationState state in anim) {
				if(state.name == "attack")
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

	void ThrowAttack()
	{
		GameObject attack = (GameObject) Instantiate (projectile, transform.GetChild(0).transform.position, Quaternion.identity);
		attack.rigidbody.AddForce((player.transform.position - projectilePos.transform.position).normalized *400f);
	}

//	void OnTriggerStay(Collider other)
//	{
//		if (other.gameObject == player)
//		{
//			playerInRange = true;
//		}
//	}
//
//	void OnTriggerExit(Collider other)
//	{
//		if (other.gameObject == player)
//		{
//			playerInRange = false;
//			playerInFieldOfView = false;
//		}
//	}
}
