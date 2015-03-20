using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	public bool isTrapEnabled = false;

	void EnableTrap(){
		isTrapEnabled = true;
	}

	void DisableTrap(){
		isTrapEnabled = false;
	}
}
