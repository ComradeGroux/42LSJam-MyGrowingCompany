using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Player : MonoBehaviour
{
	public int health = 10;
	public float moveSpeed = 10f;
	public float jumpSpeed = 4f;
	public float shrinkSpeed = 0.025f; // The speed at which the player shrinks
	public float minSize = 0.1f; // The minimum size the player can reach
	
	private float gravity = 9.81f * 4f;
	private float initJumpHeight = 2f;
	private Vector3 initialScale; // The initial scale of the player
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody>();
		initialScale = transform.localScale;
	}

	// Update is called once per frame
	void Update()
    {
		shrinkHandler();
		inputHandler();
	}

	void inputHandler()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (moveHorizontal != 0)
		{
			Vector3 movement = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, 0f);
			rb.velocity = movement;
		}

		if (Input.GetKeyDown("space"))
		{
			float jumpHeight = initJumpHeight * transform.localScale.y;
			//Debug.Log("jumpHeight = " +  jumpHeight);
			rb.velocity = new Vector3(rb.velocity.x, CalculateJumpSpeed(jumpHeight) * jumpSpeed, rb.velocity.z);
		}

		// Mouse click input (optional)
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("RIGHT CLICKED");
			attack();
		}
	}

	void shrinkHandler()
	{
		// Calculate the target scale based on the shrink speed and time
		float t = Mathf.Clamp01(shrinkSpeed * Time.time);
		Vector3 targetScale = Vector3.Lerp(initialScale, Vector3.one * minSize, t);

		// Set the player's scale to the target scale
		transform.localScale = targetScale;

		// Scale the gravity force based on the player's scale
		float scaleFactor = Mathf.Max(targetScale.x, targetScale.y);
		Vector3 scaledGravity = Vector3.down * (scaleFactor / gravity) * 10f;

		Debug.Log(scaledGravity);
		// Apply the scaled gravity force to the Rigidbody
		rb.AddForce(scaledGravity, ForceMode.Acceleration);
	}

	public void takeDamage()
	{
		health--;

		if (health <= 0)
		{
			Debug.Log("IM DEEAAAAAD");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("THAT AN ENEMY");
			sc_Enemy_AI enemy = other.gameObject.GetComponent<sc_Enemy_AI>();
			if (enemy.isInoffensive())
				Destroy(other.gameObject);
		}
		else
			Debug.Log("Friend");
	}

	float CalculateJumpSpeed(float targetJumpHeight)
	{
		// Calculate the jump speed required to reach the target jump height
		return Mathf.Sqrt(-2f * Physics.gravity.y * targetJumpHeight);
	}

	void attack()
	{
		// TRIGGER ANIMATION

	}
}