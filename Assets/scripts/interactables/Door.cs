/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Controls a sliding door that opens when the player interacts with it
 *              using Raycasting and the Interact button (E), provided they have the
 *              correct keycard. Uses an Animator to play open and close animations.
 *              The door closes when the player looks away.
 */

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Door : MonoBehaviour
{
    /// <summary>
    /// The keycard ID required to open this door (e.g. "Red", "Orange").
    /// Must match the keycardID on the corresponding KeycardCollectible.
    /// </summary>
    [SerializeField] private string requiredKeycardID = "Red";

    /// <summary>
    /// UI Text element showing interact prompt (e.g. "Press E to open door").
    /// </summary>
    [SerializeField] private TextMeshProUGUI interactPromptText;

    /// <summary>
    /// Reference to the UIManager for displaying feedback messages.
    /// </summary>
    [SerializeField] private UIManager uiManager;

    /// <summary>
    /// The input action used to interact with the door. Configurable in the Inspector.
    /// </summary>
    [SerializeField] private InputAction interactAction = new InputAction(type: InputActionType.Button);

    /// <summary>
    /// Reference to the Animator component on this door.
    /// </summary>
    private Animator animator;

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
    /// Called when the script is first initialised.
    /// Gets the Animator component on this GameObject.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("Door: No Animator component found on " + gameObject.name);
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
    /// Called by PlayerInteract when the player's raycast hits this door.
    /// Shows the appropriate interact prompt.
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
    /// Called by PlayerInteract when the player's raycast stops hitting this door.
    /// Hides the interact prompt and closes the door.
    /// </summary>
   public void OnRaycastExit()
{
    isInRange = false;
    playerInventory = null;

    if (interactPromptText != null)
        interactPromptText.text = "";
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
            Debug.Log("Door closed after player walked away.");
        }
    }
}