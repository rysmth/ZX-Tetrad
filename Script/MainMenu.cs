using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Ryan Joshua Smith Main Menu For Compilation Game Script 2026

public class MainMenu : MonoBehaviour
{
    [Header("Menu Controls")]
    [SerializeField] InputAction menuUpInputAction;
    [SerializeField] InputAction menuDownInputAction;
    [SerializeField] InputAction selectInputAction;

    [Header("Menu Position")]
    private int menuPosition;
    [SerializeField] int startPosition;

    [Header("Menu Visual Indicator")]
    [SerializeField] TMP_Text[] menuText;
    [SerializeField] TMP_Text activeText;
    private string activeTextUnaltered;
    [SerializeField] Color activeTextColor;
    [SerializeField] Color inactiveTextColor;

    private void Awake()
    {
        menuPosition = startPosition;
        UpdateText();
        VisualIndicator();

        menuUpInputAction.Enable();
        menuDownInputAction.Enable();
        selectInputAction.Enable();
    }

    private void Update()
    {
        Move();

        if (selectInputAction.triggered && menuPosition == 0)
        {
            SceneManager.LoadScene(1);
        }

        else if (selectInputAction.triggered && menuPosition == 1)
        {
            SceneManager.LoadScene(2);
        }

        else if (selectInputAction.triggered && menuPosition == 2)
        {
            SceneManager.LoadScene(3);
        }

        else if (selectInputAction.triggered && menuPosition == 3)
        {
            SceneManager.LoadScene(4);
        }
    }

    private void Move()
    {
        if (menuUpInputAction.triggered)
        {
            ResetVisualIndicator();
            menuPosition = (menuPosition - 1 + menuText.Length) % menuText.Length;
            UpdateText();
            VisualIndicator();
        }

        else if (menuDownInputAction.triggered)
        {
            ResetVisualIndicator();
            menuPosition = (menuPosition + 1) % menuText.Length;
            UpdateText();
            VisualIndicator();
        }
    }

    private void UpdateText()
    {
        activeText = menuText[menuPosition];
        activeTextUnaltered = activeText.text;
    }

    private void VisualIndicator()
    {
        activeText.text = "> " + activeText.text;
        activeText.color = activeTextColor;
    }

    private void ResetVisualIndicator()
    {
        activeText.text = activeTextUnaltered;
        activeText.color = inactiveTextColor;
    }
}