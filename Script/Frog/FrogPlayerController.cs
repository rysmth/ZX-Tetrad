using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ryan Joshua Smith Frog Player controller script for Frog micro-game 2026
public class FrogPlayerController : MonoBehaviour
{
    [Header("Visual Indicators/ Heads-Up Display")]
    [SerializeField] TMP_Text playerPointsText;
    [SerializeField] GameObject[] lifeSprites;
    [SerializeField] TMP_Text winText;

    [Header("Player Variables")]
    private int playerPoints;
    private int playerLives;

    [Header("Determine Player Position")]
    [SerializeField] GameObject[] potentialPositions;
    private int currentPosition;

    private void Start()
    {
        // Hard-coded due to only have two spaces for the heart sprites
        playerLives = 2;

        // Sets starting position and actually moves the player there to begin
        currentPosition = 3;
        gameObject.transform.position = potentialPositions[currentPosition].transform.position;

    }

    //-----------------------------------Player Movement-------------------------
    private void FixedUpdate()
    {
        // Top Left is 1, Top Right is 2, Bottom Left is 3 and Bottom Right is 4. There are so many conditions.

        if (Input.GetKey(KeyCode.W))
        {
            if (currentPosition == 2)
            {
                currentPosition = 0;
            }

            else if (currentPosition == 3)
            {
                currentPosition = 1;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (currentPosition == 0)
            {
                currentPosition = 2;
            }

            else if (currentPosition == 1)
            {
                currentPosition = 3;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (currentPosition == 1)
            {
                currentPosition = 0;
            }

            else if (currentPosition == 3)
            {
                currentPosition = 2;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (currentPosition == 0)
            {
                currentPosition = 1;
            }

            else if (currentPosition == 2)
            {
                currentPosition = 3;
            }
        }

        // Moves the player to the position
        gameObject.transform.position = potentialPositions[currentPosition].transform.position;

        // If the player has more than or exactly the hardcoded 15 points (due to static HUD Text) then the script knows to trigger the win state
        if (playerPoints >= 12)
        {
            StartCoroutine(WinState());
        }
    }

    //-----------------------------------Collision Behaviour-------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Simple tag check to determine what actions to take
        if (collision.gameObject.CompareTag("Insect"))
        {
            // Adds a point and displays this
            playerPoints += 1;
            playerPointsText.text = playerPoints.ToString() + "/12";

            // Removes the colliding object from scene to give the illusion the frog ate it
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Bird"))
        {
            // Checks if the player has enough lives to continue estentially
            if (playerLives == 2)
            {
                lifeSprites[1].SetActive(false);
            }

            else if (playerLives <= 1)
            {
                lifeSprites[0].SetActive(false);
                SceneManager.LoadScene(0);
            }

            // Removes a life regardless
            playerLives -= 1;

            // Removes the prefab from scene
            Destroy(collision.gameObject);
        }
    }

    //-----------------------------------Actions for if the player wins-------------------------
    private IEnumerator WinState()
    {
        // Lets the player aware visually
        winText.text = "You have won!!!";

        // Prevents the game from continuing as the player could be killed and cause complications
        Time.timeScale = 0;

        // Lets the player take it in for a few seconds if they did not notice
        yield return new WaitForSecondsRealtime(3);

        // Lets the game continue as normal, so the player can play again without it being paused on the next run
        Time.timeScale = 1;

        // Sends the game back to the title screen
        SceneManager.LoadScene(0);
    }
}