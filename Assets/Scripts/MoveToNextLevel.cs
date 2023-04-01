using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    public GameObject[] coinsInLevel; // holds the array of coins in level
    public int targetScore; //uses coins in level to generate a target score
    public TextMeshProUGUI scoreText; //holds text obj
    // Start is called before the first frame update
    void Start()
    {
        coinsInLevel = GameObject.FindGameObjectsWithTag("Coin");//creates array of coins in level 
        targetScore = coinsInLevel.Length;//find the amount of coins in level
        Debug.Log("Amount of Coins in Level: " + targetScore);
    }
    // Update is called once per frame
    void Update()
    {
        TargetScoreAchieved();
    }

    public void TargetScoreAchieved() {
        if(scoreText.text == "Score: " + targetScore) { //if the score is equal to target score(amount of coins)
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1); //go to next scene
        }
    }
}
