using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Player_Health : MonoBehaviour
{
	public int health = 10;

	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();    
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void takeDamage()
	{
		health--;

		if (health <= 0)
		{
			Debug.Log("IM DEEAAAAAD");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("THAT AN ENEMY");
			sc_Enemy_AI enemy = other.gameObject.GetComponent<sc_Enemy_AI>();
			if (enemy.isInoffensive())
				Destroy(other.gameObject);
		}
		else
			Debug.Log("Friend");
	}
}
