using UnityEngine;
using System.Collections;

public class TimedDeath : MonoBehaviour {
	public float lifeTime;

	float timer = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(timer >= lifeTime)
			Destroy(gameObject);
		else
			timer += Time.deltaTime;
	}
}
