/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Controls a sliding door that opens when the player interacts with it
 *              using Raycasting and the Interact button (E), only with the
 *              correct keycard. Uses an Animator to play open and close animations.
 *              The door closes when the player walks away. SFX is implemented for movement of door
 */

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    /// <summary>
    /// The keycard ID required to open this door
    /// Must match the keycardID on the corresponding KeycardCollectible.
    /// </summary>
    [SerializeField] private string requiredKeycardID = "Red";

    /// <summary>
    /// UI Text element showing interact prompt ("Press E to open door")
    /// </summary>
    [SerializeField] private TextMeshProUGUI interactPromptText;

    /// <summary>
    /// Reference to the UIManager for displaying feedback messages
    /// </summary>
    [SerializeField] private UIManager uiManager;

    /// <summary>
    /// The input action used to interact with the door which is configurable in inspector
    /// </summary>
    [SerializeField] private InputAction interactAction = new InputAction(type: InputActionType.Button);

    /// <summary>
    /// Audio clip played when the door opens
    /// </summary>
    [SerializeField] private AudioClip openSFX;

    /// <summary>
    /// Audio clip played when the door closes
    /// </summary>
    [SerializeField] private AudioClip closeSFX;

    /// <summary>
    /// Used for doors that automatically open
    /// </summary>
    [SerializeField] private bool autoOpen = false;

    /// <summary>
    /// Reference to the Animator component on each door.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Audio source component playing door sounds.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Whether the door is currently open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Whether the player is currently looking at the door within interact range.
    /// </summary>
    private bool isInRange = false;

    /// <summary>
    /// Whether the player has successfully unlocked this door.
    /// </summary>
    private bool isUnlocked = false;

    /// <summary>
    /// Reference to the player's inventory to check for keycards.
    /// </summary>
    private PlayerInventory playerInventory;

    /// <summary>
    /// Called when the game starts.
    /// Gets the Animator and AudioSource components on this GameObject.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
            Debug.LogWarning("Door: No Animator component found on " + gameObject.name);
        if (audioSource == null)
            Debug.LogWarning("Door: No AudioSource component found on " + gameObject.name);
    }

    /// <summary>
    /// Called when the object becomes enabled. Activates the interact input action.
    /// </summary>
    void OnEnable()
    {
        interactAction.Enable();
    }

    /// <summary>
    /// Called when the object becomes disabled. Deactivates the interact input action.
    /// </summary>
    void OnDisable()
    {
        interactAction.Disable();
    }

    /// <summary>
    /// Called every frame. Handles interact input.
    /// </summary>
    void Update()
    {
        if (isInRange && interactAction.WasPressedThisFrame())
        {
            TryOpen();
        }
    }

    /// <summary>
    /// Called by PlayerInteract when the player looks at the door with raycast
    /// Shows the appropriate interact prompt
    /// </summary>
    /// <param name="inventory">The player's inventory script.</param>
    public void OnRaycastEnter(PlayerInventory inventory)
    {
        playerInventory = inventory;
        isInRange = true;

        if (interactPromptText == null) return;

        if (isUnlocked)
            interactPromptText.text = isOpen ? "Press E to close door" : "Press E to open door";
        else
            interactPromptText.text = "Requires " + requiredKeycardID + " Keycard";
    }

    /// <summary>
    /// Called by PlayerInteract when the player's stops looking at the door with raycast
    /// Hides the interact prompt
    /// </summary>
    public void OnRaycastExit()
    {
        isInRange = false;
        playerInventory = null;

        if (interactPromptText != null)
            interactPromptText.text = "";
    }

    /// <summary>
    /// Called by DoorTriggerBridge when player enters trigger zone.
    /// Auto opens the door if autoOpen is enabled.
    /// </summary>
    public void PlayerEntered()
    {
        if (autoOpen)
        {
            isUnlocked = true;
            isOpen = true;  
            animator.SetBool("isOpen", true);

            if (audioSource != null && openSFX != null)
                audioSource.PlayOneShot(openSFX);

            Debug.Log(gameObject.name + " auto opened!");
        }
    }

    /// <summary>
    /// Attempts to open or close the door if the player has the required keycard.
    /// </summary>
    private void TryOpen()
    {
        if (playerInventory == null) return;

        if (playerInventory.HasKeycard(requiredKeycardID))
        {
            isUnlocked = true;
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);

            if (audioSource != null)
            {
                if (isOpen && openSFX != null)
                    audioSource.PlayOneShot(openSFX);
                else if (!isOpen && closeSFX != null)
                    audioSource.PlayOneShot(closeSFX);
            }

            if (interactPromptText != null)
                interactPromptText.text = isOpen ? "Press E to close door" : "Press E to open door";

            Debug.Log(gameObject.name + (isOpen ? " opened!" : " closed!"));
        }
        else
        {
            if (uiManager != null)
                uiManager.ShowPickupMessage("You need the " + requiredKeycardID + " Keycard!", 2f);

            Debug.Log("Missing keycard: " + requiredKeycardID);
        }
    }

    /// <summary>
    /// Called by the DoorTriggerZone when the player walks away.
    /// Closes the door if it is currently open.
    /// </summary>
    public void CloseFromTrigger()
    {
        if (isUnlocked && isOpen)
        {
            isOpen = false;
            animator.SetBool("isOpen", false);

            if (audioSource != null && closeSFX != null)
                audioSource.PlayOneShot(closeSFX);

            Debug.Log("Door closed after player walked away.");
        }
    }
}