using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderSpawner : MonoBehaviour 
{
	public GameObject spiderPrefab;
	public static int count = 0;
	public List<GameObject> spiders;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(count == 5)
		{

		}
	}

	public void Spawn()
	{
		GameObject spider = (GameObject) Instantiate(spiderPrefab, transform.position, Quaternion.identity);
	}
}
