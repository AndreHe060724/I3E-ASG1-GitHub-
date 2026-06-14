using UnityEngine;

public class PlayerDmg : MonoBehaviour
{
    // Player's current health
    public int health = 100;

    // Amount of damage taken when damaged
    public int damageAmount = 10;

    // Tag used to identify objects that can damage the player
    [SerializeField] public string damageTag = "Enemy";

    // Sound effect played when the player dies
    [SerializeField] public AudioClip deathSound;

    // Reference to the UI Manager
    public UIManager uiManager;

    // Reduces the player's health
    public void TakeDamage()
    {
        // Subtract damage from current health
        health -= damageAmount;

        // Display current health in the console
        Debug.Log("Player took damage. Current health: " + health);

        // Check if health has reached zero or below
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    // Called when the script instance is being loaded
    public void Awake()
    {
        // Find and store a reference to the UI Manager in the scene
        uiManager = FindObjectOfType<UIManager>();
    }

    // Called when another collider enters this object's trigger collider
    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a damage source
        if (IsDamageSource(other))
        {
            KillPlayer();
        }
    }

    // Called when the CharacterController hits another collider
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object is a damage source
        if (IsDamageSource(hit.collider))
        {
            KillPlayer();
        }
    }

    // Checks whether the collider is tagged as a damage source
    public bool IsDamageSource(Collider other)
    {
        return other != null && other.CompareTag(damageTag);
    }

    // Immediately sets health to zero and triggers death
    public void KillPlayer()
    {
        health = 0;
        Die();
    }

    // Handles player death logic
    public void Die()
    {
        // Display death message in the console
        Debug.Log("Player has died.");

        // Play death sound if assigned
        if (deathSound != null)
        {
            // Get existing AudioSource component
            AudioSource audio = GetComponent<AudioSource>();

            // Add an AudioSource if one does not exist
            if (audio == null)
                audio = gameObject.AddComponent<AudioSource>();

            // Play the death sound effect
            audio.PlayOneShot(deathSound);
        }

        // Update UI to show the game over screen
        if (uiManager != null)
        {
            uiManager.MenuPanel.SetActive(false);
            uiManager.ShowGameOver();
        }

        // Delay destruction until the death sound finishes playing
        float destroyDelay = deathSound != null ? deathSound.length : 0.2f;

        // Remove the player object from the scene
        Destroy(gameObject, destroyDelay);
    }
}