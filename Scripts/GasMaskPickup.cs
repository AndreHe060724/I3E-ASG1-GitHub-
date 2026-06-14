using UnityEngine;
using UnityEngine.InputSystem;

public class GasMaskPickup : MonoBehaviour
{
    // Tracks whether the player is within pickup range
    public bool playerNear;

    // Reference to the player's gas mask component
    public PlayerGasMask playerGasMask;

    // Called when another collider enters the trigger area
    public void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            // Mark the player as nearby and store the PlayerGasMask reference
            playerNear = true;
            playerGasMask = other.GetComponent<PlayerGasMask>();
        }
    }

    // Called when another collider exits the trigger area
    public void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is the player
        if (other.CompareTag("Player"))
        {
            // Clear player reference and mark player as no longer nearby
            playerNear = false;
            playerGasMask = null;
        }
    }

    // Runs every frame
    public void Update()
    {
        // Exit if the player is not nearby or the gas mask component is missing
        if (!playerNear || playerGasMask == null)
            return;

        // Check if the E key was pressed
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Give the gas mask to the player
            playerGasMask.CollectGasMask();

            // Remove the pickup object from the scene
            Destroy(gameObject);
        }
    }
}