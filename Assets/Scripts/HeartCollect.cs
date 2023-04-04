using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class HeartCollect : MonoBehaviour
{
	public HeroKnight heroKnight;
	//sounds
	[SerializeField] AudioSource effectSource;
	[SerializeField] AudioClip heartClip;
	public Image healthBar;

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if the player touches the object
		if (other.gameObject.CompareTag("Heart") && other.gameObject.activeSelf)
		{
			//play the sounds
			effectSource.clip = heartClip;
			effectSource.volume = 0.15f;
			effectSource.Play();

			//give 50 health to the player
			heroKnight.currentHealth = heroKnight.currentHealth += 50;
			//if health over 100
			if (heroKnight.currentHealth > 100)
			{
				//set health to 100
				heroKnight.currentHealth = 100;
			}
			healthBar.fillAmount = heroKnight.currentHealth/100f;
			//hide sprite
			other.gameObject.SetActive(false);
		}
	}
}
