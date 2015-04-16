using UnityEngine;
using System.Collections;

public class CursorLock : MonoBehaviour {
	public bool screenLock = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = screenLock;
	}
}
