using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Enemy_AI : MonoBehaviour
{
    public Transform player;
	public float aggressiveSpeed = 9f;
	public float inoffensiveSpeed = 4f;
	public float detectionRadius = 5f;

	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		if (player.localScale.x < transform.localScale.x) {
			
            AggressiveBehavior();
        }
        else
        {
            InoffensiveBehavior();
        }
    }

    private void AggressiveBehavior()
    {
		// Move towards the player
		Vector3 direction = (player.position - transform.position).normalized;
		rb.velocity = direction * aggressiveSpeed;

	}

    private void InoffensiveBehavior()
    {
		Vector3 direction = (player.position + transform.position).normalized;
		rb.velocity = direction * inoffensiveSpeed;

	}
}
