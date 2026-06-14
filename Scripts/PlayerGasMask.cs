using UnityEngine;

public class PlayerGasMask : MonoBehaviour
{
    // Tracks whether the player has collected the gas mask
    public bool hasGasMask { get; private set; }

    // Called when the player picks up the gas mask
    public void CollectGasMask()
    {
        // Grant the gas mask to the player
        hasGasMask = true;

        // Find the UI Manager and update the gas mask status display
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.ShowGasMaskStatus(true);
    }

    // Checks whether the player is protected from gas damage
    public bool IsImmuneToGasDamage()
    {
        return hasGasMask;
    }
}