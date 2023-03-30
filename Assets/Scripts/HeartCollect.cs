using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HeartCollect : MonoBehaviour
{
	public HeroKnight heroKnight;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Heart") && other.gameObject.activeSelf)
		{
			heroKnight.currentHealth = heroKnight.currentHealth += 50;
			if (heroKnight.currentHealth > 100)
			{
				heroKnight.currentHealth = 100;
			}
			other.gameObject.SetActive(false);
		}
	}
}
