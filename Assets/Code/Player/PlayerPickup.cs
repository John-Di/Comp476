using UnityEngine;
using System.Collections;

public class PlayerPickup : MonoBehaviour
{
	public Transform hand;
	GameObject mainCamera;
	
	bool carrying = false;
	float distance = 2.5f;
	float dropForce = 150f;
	
	// Use this for initialization
	void Start ()
	{
		mainCamera = GameObject.FindWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(carrying)
		{
			DropObject ();
		}
		else
		{
			PickupObject ();
		}
	}
	
	void PickupObject()
	{
		if(Input.GetKey (KeyCode.Mouse1))
		{
			// Get middle of screen
			int screenX = Screen.width/2;
			int screenY = Screen.height/2;
			
			Ray ray = mainCamera.camera.ScreenPointToRay (new Vector3(screenX, screenY));
			RaycastHit hit;
			if(Physics.Raycast (ray, out hit, distance))
			{
				if(hit.collider.tag == "Pickupable")
				{
					carrying = true;
					//hit.transform.position = hand.position;
					hit.transform.SetParent (hand);
					hit.rigidbody.isKinematic = true;
				}
			}
		}
	}
	
	void DropObject()
	{
		if(Input.GetKeyUp (KeyCode.Mouse1))
		{
			carrying = false;
			hand.GetChild (0).rigidbody.isKinematic = false;
			hand.GetChild (0).rigidbody.AddForce(hand.forward * dropForce);
			hand.GetChild (0).SetParent(null);
		}
	}
}
