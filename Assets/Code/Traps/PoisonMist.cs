using UnityEngine;
using System.Collections;

public class PoisonMist : Trap 
{
	void OnTriggerEnter(Collider other)
	{
		if (this.isTrapEnabled == true) 
		{
			if (other.gameObject == player) 
			{
				PlayerController p = other.GetComponent<PlayerController>();
				p.Die();
			}
		}
	}
}
