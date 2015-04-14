using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	//for testing traps are true
	protected bool isTrapEnabled = true;
	
	protected GameObject player;
	protected PlayerController pc;

	protected float fearValue;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		pc = player.GetComponent<PlayerController> ();
	}

	protected void EnableTrap(){
		isTrapEnabled = true;
	}

	protected void DisableTrap(){
		isTrapEnabled = false;
	}

	protected void EnableGameTrap(){
		gameObject.SetActive(true);
	}
	
	protected void DisableGameTrap(){
		gameObject.SetActive(false);
	}
}
