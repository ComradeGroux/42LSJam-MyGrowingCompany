using enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemy
{
	public class sc_Enemy_AI_grounded : sc_Enemy_AI_abstract
	{
		protected override void AggressiveBehavior()
		{
			Vector3 direction = (player.position - transform.position).normalized;
			direction.y = 0;
			rb.velocity = direction * aggressiveSpeed;
		}
		override protected void InoffensiveBehavior()
		{
			Vector3 direction = (transform.position - player.position).normalized;
			direction.y = 0;
			rb.velocity = direction * inoffensiveSpeed;
		}
	}
}
