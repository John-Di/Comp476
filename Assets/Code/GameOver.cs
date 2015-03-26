using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	float loadTime = 3f;

	PlayerController player;

	// Use this for initialization
	void Start () 
	{
		player = GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(player.anim.GetBool("isDead"))
		{
			loadTime -= Time.deltaTime;

			if(loadTime <= 0f)
			{
				Application.LoadLevel ("Game Over");
			}
		}
	}
}
