using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	public float maxRangeMouseY = 45f;
	public float turnSpeed = 200f;
	public float moveSpeed = 200f;
	public float jumpForce = 100f;
	public bool isGrounded = true;

	public AudioSource footSteps, beatSound, droneSound;
	public TextMesh fearText;
	public float fearLevel = 0f;
	public float monsterFearTimer = 0f, lightFearTimer = 0f, flashLightTimer = 0, maxFearTimer = 0f;

	Transform head;
	
	float rotationX = 0;
	float rotationY = 0f;

	public List<GameObject> NPCs = new List<GameObject>();

	public int roomNumber = 0;

	public Animator anim;
	public Animator flashLightAnim;

	void Awake()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
		Screen.lockCursor = true;
		head = GameObject.Find("Head").transform;
	}

	void Start()
	{
		PathfindingAgent[] agents = GameObject.FindObjectsOfType<PathfindingAgent> ();
		foreach(PathfindingAgent agent in agents)
			NPCs.Add(agent.gameObject);
	}
	
	void Update()
	{
		if(!anim.GetBool("isDead"))
		{
			Screen.lockCursor = true;
			UpdateRotation();
			UpdateMovement();
			UpdateFear();
		}
		else
		{			
			rigidbody.velocity = Vector3.zero;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
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
		rigidbody.velocity = speed;

		float velMagn = rigidbody.velocity.magnitude;
		if(velMagn > 0.1f && !footSteps.isPlaying && transform.position.y <= 2f)
		{
			footSteps.Play();
			flashLightAnim.SetBool("isMoving",true);
		}
		else if((velMagn <= 0.1f || transform.position.y > 2f) && footSteps.isPlaying)
		{
			footSteps.Stop();
			flashLightAnim.SetBool("isMoving",false);
		}
	}

	void UpdateFear()
	{
		if (fearLevel <= 0f)
			fearLevel = 0f;
		else if(fearLevel >= 1f)
			fearLevel = 1f;

		if(monsterFearTimer >= 0.75f)
			UpdateMonsterFear();
		else
			monsterFearTimer += Time.deltaTime;

		if(lightFearTimer < 1f)
			lightFearTimer += Time.deltaTime;
		if(flashLightTimer < 1f)
			flashLightTimer += Time.deltaTime;

		fearText.text = "Fear: " + floatToRomanNumeral(fearLevel *100);
		UpdateFearSound ();

		if(fearLevel == 1f && maxFearTimer < 10f)
		{
			maxFearTimer += Time.deltaTime;
		}
		else if(fearLevel < 1f && maxFearTimer > 0f)
		{
			maxFearTimer -= Time.deltaTime;
		}
	}

	void UpdateFearSound()
	{
		if(fearLevel >= 0.3f && !beatSound.isPlaying)
		{
			beatSound.Play();
		}
		
		if(fearLevel >= 0.6f && !droneSound.isPlaying)
		{
			droneSound.Play();
		}
	}

	void UpdateMonsterFear()
	{
		foreach(GameObject NPC in NPCs)
		{
			float dist =(NPC.transform.position - transform.position).magnitude;
			if(dist <= 25f && dist >= 1f)
			{
				fearLevel += (0.3f / dist);
				monsterFearTimer = 0f;
			}
		}
	}

	string floatToRomanNumeral(float f)
	{
		if(f<=10f)
			return "I";
		else if(f<=20f)
			return "II";
		else if(f<=30f)
			return "III";
		else if(f<=40f)
			return "IV";
		else if(f<=50f)
			return "V";
		else if(f<=60f)
			return "VI";
		else if(f<=70f)
			return "VII";
		else if(f<=80f)
			return "VIII";
		else if(f<=90f)
			return "IX";
		else
			return "X";
	}

	public void SetRoomNumber(int room)
	{
		roomNumber = room;
	}

	public int GetRoomNumber()
	{
		return roomNumber;
	}
	
	public void Die()
	{
		anim.SetBool("isDead", true);
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.collider.CompareTag("Gerald"))
		{
			Die ();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag ("Fireball"))
		{
			Die ();
		}
	}
}