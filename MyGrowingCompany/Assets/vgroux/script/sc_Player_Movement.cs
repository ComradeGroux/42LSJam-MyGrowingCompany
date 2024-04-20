using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Player_Movement : MonoBehaviour
{
	public float moveSpeed = 10f;
	public float jumpSpeed = 4f;

	private float initJumpHeight = 4f;
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		// Movement input
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (moveHorizontal != 0 )
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
		}
	}

	float CalculateJumpSpeed(float targetJumpHeight)
	{
		// Calculate the jump speed required to reach the target jump height
		return Mathf.Sqrt(-2f * Physics.gravity.y * targetJumpHeight);
	}
}
