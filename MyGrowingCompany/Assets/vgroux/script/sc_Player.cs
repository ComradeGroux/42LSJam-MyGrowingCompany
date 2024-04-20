using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using enemy;
using UnityEngine.SceneManagement;
using Assets.Pixelation.Scripts;

public class sc_Player : MonoBehaviour
{
	public int health = 10;
	public float moveMaxSpeed = 200f;
	public float runAccelAmount = 5f;
	public float runDeccelAmount = 5f;


	public float moveSpeed = 10f;
	public float jumpSpeed = 4f;
	public float shrinkSpeed = 0.025f; // The speed at which the player shrinks
	public float minSize = 0.1f; // The minimum size the player can reach
	public float gravity = -9.81f * 4f;

	public Pixelation shaderPixel;
	public float initJumpHeight = 4f;

	public float timeLastDmg;
	public float invicibleTime = 1f;

	private sc_Player_Weapon weapon;
	private float moveHorizontal;
	private Vector3 initialScale; // The initial scale of the player
	private Rigidbody rb;
	private float loadTime;
	private float lastDir = 1f;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		weapon = GetComponent<sc_Player_Weapon>();
		initialScale = transform.localScale;
		transform.Rotate(-90f, 90f, 0f);
		loadTime = Time.time;
		timeLastDmg = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		shrinkHandler();
		inputHandler();
	}

	private void FixedUpdate()
	{
		
	}


	void inputHandler()
	{
		gravity = -9.81f * 2 * transform.localScale.x;

		moveHorizontal = Input.GetAxis("Horizontal");
		if (moveHorizontal > 0.01f || moveHorizontal < -0.01f)
		{
			Vector3 movement = new Vector3(moveHorizontal * transform.localScale.x, rb.velocity.y, 0f);
			rb.velocity = movement;

			if (Mathf.Sign(movement.x) > 0f) {
				if (lastDir != 1) { 
					transform.Rotate(0f, 0f, -180f);
				}
			}
			else
			{
				if (lastDir == 1)
				{
					transform.Rotate(0f, 0f, 180f);
				}
			}
			lastDir = Mathf.Sign(moveHorizontal);
			//movement.y = movement.x;
			//movement.x = 0;
			//movement.z = -90f;
		}

		if (Input.GetKeyDown("space"))
		{
			float jumpHeight = initJumpHeight;
			Vector3 jump = new Vector3(rb.velocity.x, CalculateJumpSpeed(jumpHeight * transform.localScale.y) * jumpSpeed, 0);
			rb.velocity = jump;
		}

		// Mouse click input (optional)
		if (Input.GetMouseButtonDown(0))
		{
			weapon.Attack();
		}
	}

	public void StopAttack()
	{
		weapon.StopAttack();
	}

	void shrinkHandler()
	{
		// Calculate the target scale based on the shrink speed and time
		float t = Mathf.Clamp01(shrinkSpeed * (Time.time - loadTime));
		Vector3 targetScale = Vector3.Lerp(initialScale, Vector3.one * minSize, t);

		// Scale the gravity force based on the player's scale
		float scaleFactor = targetScale.y;

		// Set the player's scale to the target scale
		transform.localScale = targetScale;
		
		Vector3 scaledGravity = Vector3.up * (gravity / targetScale.y);
		Physics.gravity = new Vector3(0, gravity * 1.5f, 0);

		// Apply the scaled gravity force to the Rigidbody
		//rb.AddForce(scaledGravity, ForceMode.Acceleration);
	}
	void attack()
	{
		// TRIGGER ANIMATION

	}

	public void takeDamage()
	{
		if (Time.time - timeLastDmg > invicibleTime)
		{
			health--;

			Debug.Log("Player hitted\tcurrent hp: " + health);
			StartCoroutine(takeDamage_PixelShader());
			timeLastDmg = Time.time;
		}
	}

	private IEnumerator takeDamage_PixelShader()
	{
		shaderPixel.BlockCount -= 50;
		if (health <= 0)
		{
			Debug.Log("IM DEEAAAAAD");
			//transform.rotation = Quaternion.LookRotation(new Vector3(-90, 0, rb.velocity.x));
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		yield return new WaitForSeconds(0.5f);
		shaderPixel.BlockCount += 50;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (Time.time - timeLastDmg > invicibleTime)
		{
			if (other.gameObject.CompareTag("Enemy"))
			{
				sc_Enemy_AI_abstract enemy = other.gameObject.GetComponent<sc_Enemy_AI_abstract>();
				if (enemy.isInoffensive())
				{
					//Destroy(other.gameObject);
				}
				else
				{
					takeDamage();
				}
			}
			timeLastDmg = Time.time;
		}
	}

	float CalculateJumpSpeed(float targetJumpHeight)
	{
		// Calculate the jump speed required to reach the target jump height
		return Mathf.Sqrt(-2f * Physics.gravity.y * targetJumpHeight);
	}

}
