using Rewired;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ryan Joshua Smith Pseudo-random Character Sequence For Type Mini-game Script 2026

public class TypePlayerController : MonoBehaviour
{
    [Header("Score System")]
    private static int playerScore; // Current Score
    [SerializeField] Text scoreText;

    // Easy To Change Point Amount
    [SerializeField] int pointAmount;

    // Highscore
    private static int highScore;
    [SerializeField] Text highScoreText;

    [Header("Rewired")]
    [SerializeField] int playerId; 
    private Player player;
    private string[] rewiredActions = new string[4] { "U", "I", "O", "P" };

    [Header("Player Life Variables")]
    private static int playerLives; // Life Count

    [SerializeField] Text playerLivesText;

    [Header("LeanTween Bar Visual")]
    [SerializeField] GameObject leanTweenBar;
    private int time = 12;

    // Gradually Speeding Up LeanTween
    private int lastChecked = 0;
    private int successCount = 0;

    [Header("Multiplier")]
    private bool multiplierWorking = false;
    private double multiplierValue;
    [SerializeField] Text multiplierText;

    [Header("Core Game Loop")]
    private int[] order;
    private int currentIndexNum = 0;
    [SerializeField] Text sequenceText;

    //-----------------------------------Start is called once upon creation-------------------------
    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("highScore", 0); // Setting the score

        playerLives = 3; // If set above with playerScore game will constantly generate arrows

        playerScore = 0; // Resets score 

        player = ReInput.players.GetPlayer(playerId); // Gets the controls set in Rewired's Manager

        StartTimer();
        CharacterSequence();
        UpdateText();
    }

    //-----------------------------------Update is called once per frame----------------------------
    private void Update()
    {
        if (playerLives == 0)
        {
            SceneManager.LoadScene(0); // Simple restart, once the player has run out of lives
        }

        if (playerScore > highScore)
        {
            highScore = playerScore; // New highscore
            PlayerPrefs.GetInt("highScore");
            PlayerPrefs.GetInt("highScore", highScore); // Gets the score that should be saved
            PlayerPrefs.Save();
        }

        // Displays updated versions of counts 
        highScoreText.text = highScore.ToString();
        scoreText.text = playerScore.ToString();
        playerLivesText.text = playerLives.ToString();

        if (multiplierWorking == true)
        {
            multiplierText.text = multiplierValue.ToString() + "x";
        }

        for (int i = 0; i < rewiredActions.Length; i++)
        {
            if (player.GetButtonDown(rewiredActions[i])) // Detects inputted action
            {
                OnButtonPressed(i);
            }
        }
    }

    //-----------------------------------Create Order of Characters--------------------------
    private void CharacterSequence()
    {
        int orderNum = UnityEngine.Random.Range(1, 5); // Decides amount in sequence
        order = new int[orderNum]; // Simply creates new array that can hold said amount

        List<int> indices = new List<int> { 0, 1, 2, 3 };
        for (int i = 0; i < orderNum; i++) // Runs for Each Key
        {
            int randomIndex = UnityEngine.Random.Range(0, indices.Count); // Picks position
            order[i] = indices[randomIndex]; // 'Stores'
            indices.RemoveAt(randomIndex); // Removes position (avoids overwriting)
        }

        currentIndexNum = 0;
    }

    //-----------------------------------Change the Font and Size--------------------------
    private void UpdateText()
    {
        string text = ""; // Text that gets displayed

        for (int i = 0; i < order.Length; i++) // Loops through each key in sequence again
        {
            string character = rewiredActions[order[i]]; // Getting correlating character

            if (i == currentIndexNum)
            {
                character = "<color=#00FFFF>" + character + "</color>"; // Makes the 'active' button stand out better
            }

            text += character; // Adds to full text
        }

        sequenceText.text = text; // Obviously takes the made sequence and overwrites so can be displayed
    }

    //-----------------------------------Input Checks--------------------------
    private void OnButtonPressed(int index)
    {
        if (index == order[currentIndexNum]) // if the action correlates to that position's button
        {
            multiplierWorking = true;
            multiplierValue += 0.5; // Longer to get a larger score

            currentIndexNum++; // Moves the position to the next in the array

            UpdateText();

            if (currentIndexNum >= order.Length)  // This is what resets everything after all buttons inputted
            {
                successCount++; // Adds 1 to help determine later if can remove from leantween bar time

                SpeedUpTimer();
                ScoreAdd();
                ResetTimer();
                CharacterSequence();
                UpdateText();
            }
        }

        else // Punishment for incorrect Input
        {
            ScoreRemove();
            ResetMultiplier();
        }
    }

    //-----------------------------------Multiplier and Point System--------------------------
    private void ScoreAdd()
    {
        if (multiplierWorking == true)
        {
            playerScore += (int)Math.Round(pointAmount * multiplierValue); // Adding point default amount multiplied by mutliplier count  
        }

        playerScore += pointAmount;
    }

    private void ScoreRemove()
    {
        if (multiplierWorking == true)
        {
            playerScore -= (int)Math.Round(pointAmount * multiplierValue); // Works with multiplier for bigger punishment

        }

        playerScore -= pointAmount;
    }
    private void ResetMultiplier()
    {
        multiplierWorking = false; // This is to let score adding/removing with multiplier know
        multiplierValue = 0;
        multiplierText.text = multiplierValue.ToString() + "x"; // Displays the amount
    }

    private void RemoveLife()
    {
        playerLives--; // Removes one from count

        ResetTimer();
        ResetMultiplier();
        ScoreRemove();
    }

    //-----------------------------------Countdown Bar Visual--------------------------
    private void StartTimer()
    {
        LeanTween.scaleX(leanTweenBar, 0, time).setOnComplete(RemoveLife); // Scales x axis until max which then removes a life
    }

    private void ResetTimer()
    {
        leanTweenBar.transform.localScale = new Vector3(1, 1, 1);
        LeanTween.cancel(leanTweenBar);

        StartTimer(); // Instead of repeating code
    }

    private void SpeedUpTimer()
    {
        if (successCount / 2 > lastChecked / 2) // Every multiple of 3, if more than what it was last time
        {
            if (time >= 2)
            {
                time -= 1; // Makes time, counted up to, less so it's faster
            }

            lastChecked = successCount; // Obvious but just makes the current check into the last check for next time
        }
    }
}