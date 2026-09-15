using Rewired;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ryan Joshua Smith Turn-based combat Gameplay for Dungeon Game Script 2025
public class DungeonPlayerController : MonoBehaviour
{
    [Header("Rewired")]
    private Player player;

    [Header("Health")]
    // Health integer variable, ui bar & healing with amount
    private int minimumHealth = 10; // because instantly killing them would be too harsh
    private int maximumHealth = 100;
    private int currentHealth;
    [SerializeField] Slider healthSlider;

    [Header("Potion")]
    private int minimumPotion = 0;
    private int maximumPotion = 14; // Limit amount player can have at once
    private int currentPotion = 0;
    [SerializeField] Text potionAmountText;

    [Header("Screen Parents")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject scoreScreen;

    [Header("Informing Player")]
    [SerializeField] Text updateTextHealth;

    [Header("Score Screen")]
    private int defeatedCounter;
    private float elapsedTime = 0f;
    private bool timerRunning;
    [SerializeField] Text defeatedText;
    [SerializeField] Text timerText;
    [SerializeField] GameObject scoreScreenDeathTitle; //circumstantial/ responsive title
    [SerializeField] GameObject scoreScreenAliveTitle;

    [Header("Enemy")]
    private int enemyCurrentHealth;
    private int currentDamage;
    public GameObject enemy;
    [SerializeField] Slider enemyHealthBar;
    [SerializeField] Text enemyText;
    [SerializeField] Animator enemyHitAnimation;
    [SerializeField] Animator playerHitAnimation;

    private void Awake()
    {
        currentHealth = 100;

        timerRunning = true;

        player = ReInput.players.GetPlayer(0); // Gets Specific Profile's Controls
    }

    //-----------------------------------Start is called once upon creation-------------------------
    private void Start()
    {
        UpdateHealthBar();

        currentPotion = Random.Range(minimumPotion, maximumPotion);
        UpdatePotionAmountText();

        enemyCurrentHealth = Random.Range(minimumHealth, maximumHealth);

        enemyHealthBar.value = enemyCurrentHealth;

        enemy.SetActive(true);

    }

    //-----------------------------------Update is called once per frame----------------------------
    private void Update()
    {
        UpdateHealthBar();
        UpdatePotionAmountText();
        PlayerDeath();

        if (player.GetButtonDown("End")) // This is set as the E key for now, to Win
        {
            if (!scoreScreen.activeInHierarchy)
            {
                playerHUD.SetActive(false); // Hides hud

                scoreScreen.SetActive(true);

                scoreScreenAliveTitle.SetActive(true); // Changes title, default is off for both titles

                StartCoroutine(Reset()); // Begins the game again so player doesn't have to refresh page
            }
        }

        if (timerRunning == true)
        {
            elapsedTime += Time.deltaTime; // Just a normal timer for the scorescreen until it's made false
        }

        if (scoreScreen.activeInHierarchy) // Score Screen stops time count and displays
        {
            timerRunning = false; // Stops Timer

            if (timerRunning == false)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}"; // Displays minutes and seconds instead of just say the time in only seconds, appears like normal time
            }

            return;
        }
    }

    //-----------------------------------Healing-------------------------
    public void PlayerHeal()
    {
        if (currentPotion > minimumPotion && currentHealth < maximumHealth)
        {
            int healAmount = Random.Range(1, 20);
            currentHealth = Mathf.Min(currentHealth + healAmount, maximumHealth);
            currentPotion -= 1;
            enemyText.text = "+" + healAmount.ToString() + " Vitality";

            UpdateHealthBar();
            UpdatePotionAmountText();
        }

        if (enemy.activeInHierarchy)
        {
            StartCoroutine(EnemyAttack());
        }
    }


    //-----------------------------------Attacking----------------------------
    public void PlayerAttack()
    {
        if (!enemy.activeInHierarchy)
        {
            return; // Stops player from attacking if an enemy is not on screen
        }

        else if (enemy.activeInHierarchy)
        {
            enemyText.gameObject.SetActive(true);
            currentDamage = Random.Range(2, 18);
            enemyCurrentHealth -= currentDamage;
            enemyText.text = "Thou Attacked For " + currentDamage.ToString() + " Damage";
            enemyHealthBar.value = enemyCurrentHealth;

            EnemyDeath();
            enemyHitAnimation.SetTrigger("Hit"); // Small Animation to visually represent Attack
        }

        StartCoroutine(EnemyAttack());
    }

    //-----------------------------------Player's Death----------------------------
    private void PlayerDeath()
    {
        if (currentHealth <= 0)
        {
            playerHUD.SetActive(false); // Hides hud

            scoreScreen.SetActive(true);

            scoreScreenDeathTitle.SetActive(true); // Changes title, default is off for both titles

            StartCoroutine(Reset());
        }
    }

    //-----------------------------------Enemy Health----------------------------
    private void EnemyDeath() // Hides sprite to visually represent defeating enemy
    {
        if (enemyCurrentHealth <= 0)
        {
            enemy.SetActive(false);
            enemyText.gameObject.SetActive(true);
            defeatedCounter += 1; // Counter for the score screen
            defeatedText.text = defeatedCounter.ToString();

            currentPotion += Random.Range(1, 6); // Rewards player with potion/s

            enemyText.text = "Enemy Slain";

            StartCoroutine(Pause());
        }
    }

    //-----------------------------------Enemy Attack----------------------------
    private IEnumerator EnemyAttack()
    {
        if (enemy.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.5f);

            currentDamage = Random.Range(2, 18); // Picks a number in-between for the damage amount 

            currentHealth -= currentDamage; // Simply takes the damage amount off the player's health counter

            updateTextHealth.text = currentDamage.ToString() + " Damage taken"; // Informs player how much damage they've taken

            playerHitAnimation.SetTrigger("Hit"); // Small Animation to visually represent Attack
        }
    }

    //-----------------------------------Keeps Health and Potion Correctly Displayed-------------------------
    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
    }

    private void UpdatePotionAmountText()
    {
        potionAmountText.text = currentPotion.ToString(); // int to string
    }

    //-----------------------------------Delay-------------------------
    private IEnumerator Pause() // Shows a new enemy after the player defeats one
    {
        yield return new WaitForSeconds(2);
        enemy.SetActive(true);
        enemyCurrentHealth = Random.Range(minimumHealth, maximumHealth);

        enemyHealthBar.value = enemyCurrentHealth;
    }
    private IEnumerator Reset() // Waits, so the player can read their stats, before restarting
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}