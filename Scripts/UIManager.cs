using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Displays the player's score
    public TMP_Text ScoreText;

    // Displays the player's health
    public TMP_Text HealthText;

    // Displays the gas mask status
    public TMP_Text GasMaskText;

    // Displays the number of collectibles remaining
    public TMP_Text CollectiblesLeftText;

    // Reference to the pause/menu panel
    public GameObject MenuPanel;

    // Reference to the game over panel
    public GameObject GameOverPanel;

    // Reference to the winner panel
    public GameObject WinnerPanel;

    // Name of the main menu scene
    public string mainMenuSceneName = "MainMenu";

    // Tracks whether the menu is currently open
    public bool isMenuOpen;

    // Called when the script instance is being loaded
    public void Awake()
    {
        // Attempt to find the Game Over panel if not assigned
        if (GameOverPanel == null)
            GameOverPanel = GameObject.Find("GameOverPanel");

        // Use the MenuPanel as a fallback if GameOverPanel cannot be found
        if (GameOverPanel == null)
            GameOverPanel = MenuPanel;
    }

    // Called before the first frame update
    public void Start()
    {
        // Attempt to find the HealthText object if not assigned
        if (HealthText == null)
            HealthText = GameObject.Find("HealthText")?.GetComponent<TMP_Text>();

        // Find the player and initialize the health display
        var player = FindObjectOfType<PlayerDmg>();
        if (player != null)
            UpdateHealth(player.health);
    }

    // Updates the score displayed on the UI
    public void UpdateScore(int score)
    {
        if (ScoreText != null)
            ScoreText.text = score.ToString();

        Debug.Log("Score updated: " + score);
    }

    // Updates the player's health display
    public void UpdateHealth(int health)
    {
        if (HealthText != null)
            HealthText.text = "Health: " + health;
    }

    // Updates the gas mask status display
    public void ShowGasMaskStatus(bool hasMask)
    {
        if (GasMaskText != null)
            GasMaskText.text = hasMask ? "Gas Mask Equipped" : "Find the Gas Mask";
    }

    // Opens or closes the menu panel
    public void ToggleMenu()
    {
        // Toggle menu state
        isMenuOpen = !isMenuOpen;

        // Show or hide the menu panel
        if (MenuPanel != null)
            MenuPanel.SetActive(isMenuOpen);

        // Update cursor visibility and lock state
        Cursor.visible = MenuPanel.activeSelf;
        Cursor.lockState = MenuPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Reloads the current scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Displays the game over screen
    public void ShowGameOver()
    {
        // Hide the menu panel if it is open
        if (MenuPanel != null)
            MenuPanel.SetActive(false);

        // Attempt to find the Game Over panel if not assigned
        if (GameOverPanel == null)
            GameOverPanel = GameObject.Find("GameOverPanel");

        // Use the MenuPanel as a fallback if GameOverPanel cannot be found
        if (GameOverPanel == null)
            GameOverPanel = MenuPanel;

        // Show the Game Over panel
        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        // Unlock and show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Loads the main menu scene
    public void QuitToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("Main menu scene name is not set. Set it in the UIManager Inspector.");
        }
    }

    // Displays the winner screen
    public void ShowWinner()
    {
        // Hide the menu panel if it is open
        if (MenuPanel != null)
            MenuPanel.SetActive(false);

        // Attempt to find the Winner panel if not assigned
        if (WinnerPanel == null)
            WinnerPanel = GameObject.Find("WinnerPanel");

        // Use the MenuPanel as a fallback if WinnerPanel cannot be found
        if (WinnerPanel == null)
            WinnerPanel = MenuPanel;

        // Show the Winner panel
        if (WinnerPanel != null)
            WinnerPanel.SetActive(true);

        // Unlock and show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}