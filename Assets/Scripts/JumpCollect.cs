using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollect : MonoBehaviour
{
	public HeroKnight heroKnight;

	private void OnTriggerEnter2D(Collider2D other)
	{	
		//if the player touches the object
		if (other.gameObject.CompareTag("Jump") && other.gameObject.activeSelf)
		{
			//add a permenate jump boost for the level
			heroKnight.m_jumpForce = heroKnight.m_jumpForce + 3;
			//hide the sprite
			other.gameObject.SetActive(false);
		}
	}
}
