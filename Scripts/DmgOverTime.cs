using UnityEngine;

public class DmgOverTime : MonoBehaviour
{
    // Time interval between each damage tick
    [Tooltip("How often the player takes damage while standing in the trigger.")]
    public float damageInterval = 1f;

    // Tracks elapsed time between damage applications
    public float damageTimer;

    // Reference to the player currently taking damage
    public PlayerDmg playerTakingDamage;

    // Reference to the UI Manager
    public UIManager uiManager;

    // Called when the script instance is being loaded
    public void Awake()
    {
        // Find and store the UI Manager in the scene
        uiManager = FindObjectOfType<UIManager>();
    }

    // Called when a collider enters the trigger area
    public void OnTriggerEnter(Collider other)
    {
        StartDamage(other);
    }

    // Called every frame while a collider remains in the trigger area
    public void OnTriggerStay(Collider other)
    {
        ContinueDamage(other);
    }

    // Called when a collider exits the trigger area
    public void OnTriggerExit(Collider other)
    {
        // Stop tracking damage if the exiting object is the current player
        if (FindPlayer(other) == playerTakingDamage)
            playerTakingDamage = null;
    }

    // Called when a collision first occurs
    public void OnCollisionEnter(Collision collision)
    {
        StartDamage(collision.collider);
    }

    // Called every frame while a collision continues
    public void OnCollisionStay(Collision collision)
    {
        ContinueDamage(collision.collider);
    }

    // Called when a collision ends
    public void OnCollisionExit(Collision collision)
    {
        // Stop tracking damage if the exiting object is the current player
        if (FindPlayer(collision.collider) == playerTakingDamage)
            playerTakingDamage = null;
    }

    // Starts the damage process when a player enters the area
    public void StartDamage(Collider other)
    {
        var player = FindPlayer(other);

        // Check if the collider belongs to a player
        if (player != null)
        {
            playerTakingDamage = player;

            // Reset the damage timer
            damageTimer = 0f;

            // Apply immediate damage
            ApplyDamage();
        }
    }

    // Continues applying damage while the player remains in the area
    public void ContinueDamage(Collider other)
    {
        // Exit if no player is currently taking damage
        if (playerTakingDamage == null)
            return;

        // Exit if the collider does not belong to the tracked player
        if (FindPlayer(other) != playerTakingDamage)
            return;

        // Increase timer based on elapsed frame time
        damageTimer += Time.deltaTime;

        // Apply damage when the interval is reached
        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;
            ApplyDamage();
        }
    }

    // Attempts to find a PlayerDmg component on the collider or its parent
    public PlayerDmg FindPlayer(Collider other)
    {
        if (other != null)
        {
            // Check the collider itself
            var direct = other.GetComponent<PlayerDmg>();
            if (direct != null)
                return direct;

            // Check parent objects
            var parent = other.GetComponentInParent<PlayerDmg>();
            if (parent != null)
                return parent;
        }

        // No player component found
        return null;
    }

    // Applies damage to the tracked player
    public void ApplyDamage()
    {
        // Exit if no player is being tracked
        if (playerTakingDamage == null)
            return;

        // Check if the player has gas mask protection
        var gasMask = playerTakingDamage.GetComponent<PlayerGasMask>();
        if (gasMask != null && gasMask.IsImmuneToGasDamage())
            return;

        // Damage the player
        playerTakingDamage.TakeDamage();

        // Update the health display in the UI
        if (uiManager != null)
            uiManager.UpdateHealth(playerTakingDamage.health);
    }
}