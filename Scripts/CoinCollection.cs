using UnityEngine;
using UnityEngine.InputSystem;

public class CoinCollection : MonoBehaviour
{
    // Sound effect played when the coin is collected
    public AudioClip collectSound;

    // When player collects coin +1 score
    public int scoreValue = 1;

    // Tracks whether the player is within interaction range
    public bool playerNear;


    public PlayerScript player;

    // Called when another collider enters the coin's trigger area
    public void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            // Mark player as nearby and store reference to player script
            playerNear = true;
            player = other.GetComponent<PlayerScript>();
        }
    }

    // Called when another collider exits the coin's trigger area
    public void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is the player
        if (other.CompareTag("Player"))
        {
            // Clear player reference and mark player as no longer nearby
            playerNear = false;
            player = null;
        }
    }

    // Runs every frame
    public void Update()
    {
        // Check if player is nearby and presses the E key
        if (playerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Ensure player reference exists before collecting coin
            if (player != null)
            {
                player.CollectCoin(gameObject);
            }
        }
    }

    // Handles the coin collection effects
    public void CollectCoin()
    {
        // Get existing AudioSource component
        AudioSource audio = GetComponent<AudioSource>();

        // Add an AudioSource if one does not exist
        if (audio == null)
            audio = gameObject.AddComponent<AudioSource>();

        // Play collection sound if assigned
        if (collectSound != null)
            audio.PlayOneShot(collectSound);

        // Get the coin's renderer component
        Renderer renderer = GetComponent<Renderer>();

        // Hide the coin visually after collection
        if (renderer != null)
            renderer.enabled = false;

        // Destroy the coin object after 1 second
        Destroy(gameObject, 1f);
    }
}