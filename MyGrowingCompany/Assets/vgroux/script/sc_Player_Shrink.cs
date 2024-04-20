using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
	public float shrinkSpeed = 0.1f; // The speed at which the player shrinks
	public float minSize = 0.1f; // The minimum size the player can reach
	public float gravity = 9.81f * 2f;
	
	private Rigidbody rb;
	private Vector3 initialScale; // The initial scale of the player

	private void Start()
	{
		initialScale = transform.localScale;
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// Calculate the target scale based on the shrink speed and time
		float t = Mathf.Clamp01(shrinkSpeed * Time.time);
		Vector3 targetScale = Vector3.Lerp(initialScale, Vector3.one * minSize, t);

		// Set the player's scale to the target scale
		transform.localScale = targetScale;

		// Scale the gravity force based on the player's scale
		float scaleFactor = Mathf.Max(targetScale.x, targetScale.y);
		Vector3 scaledGravity = Vector3.up * (scaleFactor / gravity) * 10f;

		// Apply the scaled gravity force to the Rigidbody
		rb.AddForce(scaledGravity, ForceMode.Acceleration);
	}
}
