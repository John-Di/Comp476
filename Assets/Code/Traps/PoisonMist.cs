﻿using UnityEngine;
using System.Collections;

public class PoisonMist : Trap {
	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				Debug.Log("Kill Player Here");
			}
		}
	}
}
