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
    private bool isAggr = false;

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
            isAggr = true;
        }
        else
        {
            InoffensiveBehavior();
			isAggr = false;
		}
    }

    private void AggressiveBehavior()
    {
		Vector3 direction = (player.position - transform.position).normalized;
		rb.velocity = direction * aggressiveSpeed;
	}

    private void InoffensiveBehavior()
    {
		Vector3 direction = (transform.position - player.position).normalized;
		rb.velocity = direction * inoffensiveSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (isAggr)
        {
		    if (other.gameObject.CompareTag("Player")) {
			    // COLLISION WITH THE PLAYER
			    sc_Player playerhp = other.gameObject.GetComponent<sc_Player>();
                playerhp.takeDamage();
            }
        }
	}

    public bool isInoffensive()
    {
        return !isAggr;
    }

    public bool isAggressive()
    {
        return isAggr;
    }
}
