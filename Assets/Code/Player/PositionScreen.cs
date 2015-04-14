using UnityEngine;
using System.Collections;

public class PositionScreen : MonoBehaviour {
	public Vector3 screenPosition;

	float width, height;

	// Use this for initialization
	void Start () {
		transform.position = Camera.main.ViewportToWorldPoint(screenPosition);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.ViewportToWorldPoint(screenPosition);
	}
}
