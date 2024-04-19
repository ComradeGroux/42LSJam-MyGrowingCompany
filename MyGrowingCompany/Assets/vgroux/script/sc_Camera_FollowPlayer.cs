using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Camera_FollowPlayer : MonoBehaviour
{
	public Transform player; // The player GameObject's Transform
	public Vector3 offset; // The offset between the camera and the player
	public float initialFOV; // The initial field of view of the camera
	public float minFOV; // The minimum field of view the camera can reach

	private Camera playerCamera;

	private void Start()
	{
		playerCamera = GetComponent<Camera>();
	}

	private void LateUpdate()
	{
		// Calculate the scale factor based on the player's current scale
		float scaleFactor = Mathf.Max(player.localScale.x, player.localScale.y);

		// Adjust the camera's position to maintain the same distance from the player
		Vector3 scaledOffset = offset * scaleFactor;
		transform.position = player.position + scaledOffset;

		// Calculate the new field of view based on the scale factor
		float newFOV = Mathf.Lerp(initialFOV, minFOV, 1 - scaleFactor);

		// Set the camera's field of view
		playerCamera.fieldOfView = newFOV;
	}
}
