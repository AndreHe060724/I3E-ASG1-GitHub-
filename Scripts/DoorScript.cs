using UnityEngine;
using UnityEngine.InputSystem;

public class DoorScript : MonoBehaviour
{
    // Distance within which the player can interact with the door
    [Tooltip("How close the player must be to open the door with E.")]
    public float interactionRange = 3f;

    // Reference to the door's Animator component
    public Animator animator;

    // Tracks whether the door is currently open or closed
    public bool isOpen = false;

    // Indicates whether the Animator contains an "IsOpen" parameter
    public bool hasOpenParameter;

    // Called when the script instance is being loaded
    public void Awake()
    {
        // Try to get the Animator attached to this object
        animator = GetComponent<Animator>();

        // If not found, search for an Animator in child objects
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Check whether the Animator contains the "IsOpen" boolean parameter
        hasOpenParameter = animator != null && AnimatorHasBoolParameter(animator, "IsOpen");
    }

    // Allows other scripts to interact with the door
    public int Interact()
    {
        // Toggle the door's open/closed state
        ToggleDoor();
        return 0;
    }

    // Runs every frame
    public void Update()
    {
        // Exit if no keyboard is detected or E was not pressed this frame
        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        // Open or close the door if the player is nearby
        if (IsPlayerNear())
            ToggleDoor();
    }

    // Checks if the player is within interaction range
    public bool IsPlayerNear()
    {
        // Find all colliders within the interaction radius
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        // Loop through detected colliders
        foreach (var hit in hits)
        {
            // Return true if a player object is found
            if (hit.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
                return true;
        }

        // No player found within range
        return false;
    }

    // Checks whether the Animator contains a specific boolean parameter
    public bool AnimatorHasBoolParameter(Animator targetAnimator, string parameterName)
    {
        // Return false if the Animator is missing
        if (targetAnimator == null)
            return false;

        // Loop through all Animator parameters
        foreach (var parameter in targetAnimator.parameters)
        {
            // Return true if the parameter name matches
            if (parameter.name == parameterName)
                return true;
        }

        // Parameter was not found
        return false;
    }

    // Opens or closes the door
    public void ToggleDoor()
    {
        // Switch between open and closed states
        isOpen = !isOpen;

        // Use the "IsOpen" boolean parameter if available
        if (animator != null && hasOpenParameter)
        {
            animator.SetBool("IsOpen", isOpen);
            return;
        }

        // Otherwise, trigger the "Open" animation
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }
}