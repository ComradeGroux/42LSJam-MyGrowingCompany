using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace enemy
{

	public abstract class sc_Enemy_AI_abstract : MonoBehaviour
	{
		public Transform player;
		public float aggressiveSpeed = 50f;
		public float inoffensiveSpeed = 25f;
		public float detectionRadius = 5f;

		public int hp = 2;

		protected Rigidbody rb;
		protected bool isAggr = false;

		// Start is called before the first frame update
		protected virtual void Start()
		{
			rb = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		protected void Update()
		{
			if (player.localScale.x < transform.localScale.x + 2f)
			{

				AggressiveBehavior();
				isAggr = true;
			}
			else
			{
				InoffensiveBehavior();
				isAggr = false;
			}
		}

		protected virtual void AggressiveBehavior()
		{
			Vector3 direction = (player.position - transform.position).normalized;
			rb.velocity = direction * aggressiveSpeed;
		}

		protected virtual void InoffensiveBehavior()
		{
			Vector3 direction = (transform.position - player.position).normalized;
			rb.velocity = direction * inoffensiveSpeed;
		}

		protected void OnTriggerEnter(Collider other)
		{
			if (isAggr)
			{
				if (other.gameObject.CompareTag("Player"))
				{
					// COLLISION WITH THE PLAYER
					sc_Player player = other.gameObject.GetComponentInParent<sc_Player>();
					if (player != null)
					{
						player.takeDamage();
					}
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

		public void takeDamage()
		{
			hp--;
			Debug.Log("Enemy hitted");

			if (hp <= 0)
			{
				Debug.Log("THE ENEMY IS DEAAAAAAAAAD");
				Destroy(this.gameObject);
			}
		}
	}
}