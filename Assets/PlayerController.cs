using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float maxRangeMouseY = 45f;
	public float turnSpeed = 200f;
	public float moveSpeed = 200f;
	public float jumpForce = 100f;
	
	Transform head;
	
	float rotationX = 0;
	float rotationY = 180f;
	
	void Awake()
	{
		Screen.lockCursor = true;
		head = transform.Find("Head").transform;
	}
	
	void Update()
	{
		UpdateRotation();
		UpdateMovement();
	}
	
	void UpdateRotation()
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");
		
		rotationY += mouseX * turnSpeed * Time.deltaTime;
		rotationX -= mouseY * turnSpeed * Time.deltaTime;
		rotationX = Mathf.Clamp(rotationX, -maxRangeMouseY, maxRangeMouseY);
		transform.rotation = Quaternion.Euler(new Vector3(0 , rotationY, 0));
		head.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
	}
	
	void UpdateMovement()
	{
		float axisX = Input.GetAxis("Horizontal");
		float axisZ = Input.GetAxis("Vertical");
		
		var speed = new Vector3(0, rigidbody.velocity.y, 0);
		speed += Quaternion.Euler(0, rotationY, 0) * Vector3.right * axisX * moveSpeed * Time.deltaTime;
		speed += Quaternion.Euler(0, rotationY, 0) * Vector3.forward * axisZ * moveSpeed * Time.deltaTime;
		rigidbody.velocity = (speed);
	}
}

