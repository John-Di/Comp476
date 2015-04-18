using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 personalLastSighting;

	private GameObject player;
	private Vector3 previousSighting;
	private SphereCollider col;
	public  Vector3 direction;

	void Awake(){
		col = GetComponent<SphereCollider> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update(){

	//	playerInSight = LineOfSight ();
		Debug.DrawRay(transform.position, direction, Color.red);
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == player) {
			//Debug.Log("Player IN RADIUS");
			//By default player is not in sight
		

			//Vector from enemy to player
			direction = player.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			//angle between forward and where player is is half angle of view
			if(angle < fieldOfViewAngle * 0.5f){
				RaycastHit hit;

				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius)){
					//Player In Sight
					playerInSight = true;
					//Debug.Log("Player IN ANGLE OF VIEW");
				}
			}else{
				playerInSight = false;
			}
		}
	}

	void OnTriggerExit(Collider other){
		//player leaves radius of view
		if (other.gameObject == player) {
			//player not in sight
			playerInSight = false;
		}
	}
	
	/*bool LineOfSight(){
		float angle = Vector3.Angle(direction, transform.forward);

		if(angle < fieldOfViewAngle * 0.5f){
			RaycastHit hit;

			if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius)){
				Debug.DrawRay(transform.position, direction.normalized, Color.red);
				if(hit.collider.gameObject == player){
					//Player In Sight
					//Do Something
					Debug.Log("PlayerIsInSight");
					return true;
				}
			}
		}
		return false;
	}*/
}
