using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Ryan Joshua Smith Claw Player Controller Script 2022
public class ClawPlayerController : MonoBehaviour
{
    [Header("Instantiating Objects")]
    [SerializeField] GameObject[] objectArray;
    [SerializeField] GameObject instantiateTransform;
    private int objectNumber;

    [Header("Movement")]
    [SerializeField] float playerSpeed;

    [Header("Countdown Timer")]
    [SerializeField] float currentTime = 0f;
    [SerializeField] float startTime = 30f;
    [SerializeField] TMP_Text countdownUI;

    [Header("Arm Movement")]
    [SerializeField] Animator closeAnimation;

    //-----------------------------------Start is called once upon creation-------------------------
    private void Start()
    {
        // Starts Spawning Objects
        StartCoroutine(InstantiateObject());

        // Sets To Max
        currentTime = startTime;
    }

    //-----------------------------------Update is called once per frame----------------------------
    private void Update()
    {
        Countdown();

        // Player Movement
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.A) && pos.x > -1.5f) // Magic Numbers for Scene Boundaries
        {
            transform.position += Vector3.left * playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) && pos.x < 1.5f)
        {
            transform.position += Vector3.right * playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W) && pos.y < 1.4f)
        {
            transform.position += Vector3.up * playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) && pos.y > 0.30f)
        {
            transform.position += Vector3.down * playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            closeAnimation.SetBool("Close", true); // Bool, Not Trigger So Holding Key Keeps Closed
        }

        else
        {
            closeAnimation.SetBool("Close", false);
        }
    }

    //-----------------------------------Object Spawner----------------------------
    private IEnumerator InstantiateObject()
    {
        // Picks Random Object Through Correlating Index Number
        objectNumber = Random.Range(0, objectArray.Length);

        // Spawning Object/s At The Correct Location
        Instantiate(objectArray[objectNumber], instantiateTransform.transform.position, Quaternion.identity);

        // Prevents Loads Spawning At Once 
        yield return new WaitForSeconds(2f);

        StartCoroutine(InstantiateObject());
    }

    //-----------------------------------Countdown----------------------------
    private void Countdown()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownUI.text = Mathf.Floor(currentTime).ToString();

        // Restarts Game Once Time Runs Out
        if (currentTime < 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}