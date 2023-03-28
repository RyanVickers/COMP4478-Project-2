using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCollect : MonoBehaviour {
  private int score = 0;
  public TextMeshProUGUI scoreText;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Coin") && other.gameObject.activeSelf) {
      score++;
      scoreText.SetText("Score: " + score);
      other.gameObject.SetActive(false);
    }
  }
}