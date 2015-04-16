using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	float loadTime = 3f;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.GetComponent<PlayerController>().anim.GetBool("isDead"))
		{
			loadTime -= Time.deltaTime;
			
			if(loadTime <= 0f)
			{
				Screen.lockCursor = false;
				Application.LoadLevel ("Menu");
			}
		}
	}
}
