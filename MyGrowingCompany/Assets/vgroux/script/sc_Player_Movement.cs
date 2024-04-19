using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Player_Movement : MonoBehaviour
{
	public float moveSpeed = 5f;

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
		Vector3 movement = new Vector3(moveHorizontal, 0f, 0f) * moveSpeed;
		rb.velocity = movement;

		// Mouse click input (optional)
		if (Input.GetMouseButtonDown(0))
		{
			// Handle left mouse button click here
			Debug.Log("RIGHT CLICKED");
		}
	}
}
