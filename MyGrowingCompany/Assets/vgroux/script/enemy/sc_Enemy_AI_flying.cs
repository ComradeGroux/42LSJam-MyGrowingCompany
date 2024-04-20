using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemy
{
	public class sc_Enemy_Volant : sc_Enemy_AI_abstract
	{
		protected override void AggressiveBehavior()
		{
			Vector3 direction = (player.position - transform.position).normalized;
			rb.velocity = direction * aggressiveSpeed;
		}
		override protected void InoffensiveBehavior()
		{
			Vector3 direction = (transform.position - player.position).normalized;
			rb.velocity = direction * inoffensiveSpeed;
		}
	}
}
