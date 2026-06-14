using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // UI text used to display the current coin count
    public TMP_Text coinText;

    // Reference to the UI Manager
    public UIManager uiManager;

    // Tracks the total number of coins collected
    public int totalCoins = 0;

    // Called when the player enters a trigger collider
    public void OnTriggerEnter(Collider other) 
    {
        // Attempt to collect the collided object if it is a coin
        CollectCoin(other.gameObject);
    }

    // Called when the menu input action is triggered
    void OnMenu(InputValue value)
    {
        // Open or close the game menu
        uiManager.ToggleMenu();
    }

    // Called when the CharacterController collides with another object
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Attempt to collect the collided object if it is a coin
        CollectCoin(hit.gameObject);

        // Check if the collided object is an enemy
        if (hit.gameObject.CompareTag("Enemy"))
        {

        }
    }

    // Handles coin collection logic
    public void CollectCoin(GameObject other)
    {
        // Exit if the object is not tagged as a coin
        if (!other.CompareTag("Coin"))
            return;

        // Try to get the CoinCollection component
        CoinCollection coinPickup = other.GetComponent<CoinCollection>();

        if (coinPickup != null)
        {
            // Use the coin's own collection method
            coinPickup.CollectCoin();
        }
        else
        {
            // Play the object's audio clip if available
            AudioSource audio = other.GetComponent<AudioSource>();
            if (audio != null && audio.clip != null)
                audio.Play();

            // Hide the coin visually
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = false;

            // Destroy the coin after a short delay
            Destroy(other, 1f);
        }

        // Increase the player's coin count
        totalCoins++;

        // Update the coin counter text
        if (coinText != null)
            coinText.text = "Coins: " + totalCoins;

        // Update the score display in the UI
        if (uiManager != null)
        {
            uiManager.UpdateScore(totalCoins);
        }

        // Display score update in the console
        Debug.Log("Score updated: " + totalCoins);

        // Display coin count in the console
        Debug.Log("Coins collected: " + totalCoins);

        // Check if all required coins have been collected
        if(totalCoins == 10)
        {
            // Show the win screen
            uiManager.ShowWinner();

            // Display victory message in the console
            Debug.Log("Congratulations! You've collected all the coins!");
        }
    }
}