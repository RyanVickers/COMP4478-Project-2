using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollect : MonoBehaviour
{
	public HeroKnight heroKnight;
	int timer = 1000;

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if the player touches the object
		if (other.gameObject.CompareTag("Damage") && other.gameObject.activeSelf)
		{
			//add a permanate damage boost for the level
			heroKnight.attackDamage = heroKnight.attackDamage + 40;
			//hide the sprite
			other.gameObject.SetActive(false);
		}
	}
}
