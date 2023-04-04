using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCollect : MonoBehaviour {
    private int score = 0;
    public TextMeshProUGUI scoreText;

    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioClip coinClip;

    private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Coin") && other.gameObject.activeSelf) {
          effectSource.clip = coinClip;
          effectSource.volume = 0.07f;
          effectSource.Play();


          score++;
          scoreText.SetText(score.ToString());
          other.gameObject.SetActive(false);
    }
  }
}