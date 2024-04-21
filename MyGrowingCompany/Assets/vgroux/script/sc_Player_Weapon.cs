using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using enemy;
public class sc_Player_Weapon : MonoBehaviour
{

	public float attackDuration = 1f;

	private float timeSinceLastAttack = 0f;
	private bool canDmg = false;
	private Animator anim;
	// Start is called before the first frame update
	void Start()
	{
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		timeSinceLastAttack += Time.deltaTime;
		if (timeSinceLastAttack > attackDuration)
		{
			canDmg = false;
			Debug.Log("end attack");
		}
	}

	public void Attack()
	{
		timeSinceLastAttack = 0;
		canDmg = true;
		Debug.Log("start attack");
	}

	public void StopAttack()
	{
		canDmg = false;
		Debug.Log("end attack");
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (canDmg)
		{
			if (collision.gameObject.CompareTag("Enemy"))
			{
				collision.gameObject.GetComponent<sc_Enemy_AI_abstract>().takeDamage();
				canDmg = false;
				Debug.Log("Hit the enemy");
			}
		}
	}
}
