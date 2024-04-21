using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using enemy;
using UnityEngine.SceneManagement;
using Assets.Pixelation.Scripts;

public class sc_Player : MonoBehaviour
{
	//public sc_GameSession_Manager gameSessionManager;

	public int health = 10;

	public float moveSpeed = 10f;
	public float jumpMult = 1f;
	public float shrinkSpeed = 0.01f; // The speed at which the player shrinks
	public float minSize = 0.1f; // The minimum size the player can reach

	public Pixelation shaderPixel;

	public Transform meshToShrink;

	public LayerMask groundMask;

	private Animator anim;
	private float jumpSpeed = 2.5f;
	private float gravity = -9.81f * 4f;
	private float invicibleTime = 1f;
	private float timeLastDmg;
	private sc_Player_Weapon weapon;
	private float moveHorizontal;
	private Vector3 initialScale; // The initial scale of the player
	private Rigidbody rb;
	private float loadTime;
	private float currentLoadTime;
	private float lastDir = 1f;
	private int nbJump = 0;
	private bool isPixelating = false;
	private bool isDead = false;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		weapon = GetComponent<sc_Player_Weapon>();
		anim = GetComponentInChildren<Animator>();
		initialScale = meshToShrink.localScale;
		transform.Rotate(-90f, 90f, 0f);
		loadTime = Time.time;
		currentLoadTime = Time.time;
		timeLastDmg = Time.time;

		Debug.Log("total death: " + sc_GameSession_Manager.instance.playerDeathCount);
	}

	// Update is called once per frame
	void Update()
	{
		if (Physics.CheckSphere(transform.position, 0.1f, groundMask)) {
			nbJump = 0;
		}
		shrinkHandler();
		inputHandler();
	}

	void inputHandler()
	{
		gravity = -9.81f * 2 * transform.localScale.x;

		moveHorizontal = Input.GetAxis("Horizontal");
		if (moveHorizontal > 0.01f || moveHorizontal < -0.01f)
		{
			anim.ResetTrigger("idle");
			anim.SetTrigger("walk");
			Vector3 movement = new Vector3(moveHorizontal * transform.localScale.x * moveSpeed, rb.velocity.y, 0f);
			rb.velocity = movement;

			if (Mathf.Sign(movement.x) > 0f)
			{
				if (lastDir != 1)
				{
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
			lastDir = Mathf.Sign(movement.x);
		}
		else
		{
			anim.ResetTrigger("walk");
			anim.SetTrigger("idle");
		}

		if (Input.GetKeyDown("space") && nbJump < 2)
		{
			anim.ResetTrigger("walk");
			anim.ResetTrigger("idle");
			anim.SetTrigger("jump");
			float jumpHeight = transform.localScale.y * jumpMult;
			float calculatedJumpSpeed = CalculateJumpSpeed(jumpHeight);
			Vector3 jump = new Vector3(rb.velocity.x, calculatedJumpSpeed * jumpSpeed, 0);
			rb.velocity = jump;
			nbJump += 1;
		}

		// Mouse click input (optional)
		if (Input.GetMouseButtonDown(0))
		{
			anim.SetTrigger("attack");
			weapon.Attack();
		}
	}


	void shrinkHandler()
	{
		// Calculate the target scale based on the shrink speed and time
		float t = Mathf.Clamp01(shrinkSpeed * (Time.time - loadTime));
		Vector3 targetScale = Vector3.Lerp(initialScale, Vector3.one * minSize, t);

		// Scale the gravity force based on the player's scale
		float scaleFactor = targetScale.y;

		// Set the player's scale to the target scale
		meshToShrink.localScale = targetScale;
		
		Vector3 scaledGravity = Vector3.up * (gravity / targetScale.y);
		Physics.gravity = new Vector3(0, gravity * 1.5f, 0);
	}

	public void takeDamage()
	{
		if (Time.time - timeLastDmg > invicibleTime && !isDead && !isPixelating)
		{
			health--;
			timeLastDmg = Time.time;

			if (health <= 0)
			{
				isDead = true;
				die();
			}

			isPixelating = true;
			StartCoroutine(takeDamage_PixelShader());
		}
	}

	private void die()
	{
		Debug.Log("The player died");
		sc_GameSession_Manager.instance.playerDeathCount++;
		sc_GameSession_Manager.instance.ReloadScene();
	}

	private IEnumerator takeDamage_PixelShader()
	{
		shaderPixel.BlockCount -= 50;
		yield return new WaitForSeconds(0.5f);
		shaderPixel.BlockCount += 50;
		isPixelating = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collision");
		if (other.gameObject.CompareTag("InstantDie") && !isDead)
		{
			isDead = true;
			die();
		}
		else if (other.gameObject.CompareTag("End"))
		{
			sc_GameSession_Manager.instance.LoadEndingScene();
		}
		else
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
	}

	float CalculateJumpSpeed(float targetJumpHeight)
	{
		// Calculate the jump speed required to reach the target jump height
		return Mathf.Sqrt(-2f * Physics.gravity.y * targetJumpHeight);
	}
}
