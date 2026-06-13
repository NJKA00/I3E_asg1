/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Represents a keycard collectible that the player can pick up
 *              using Raycasting and the Interact button (E).
 *              Displays a prompt when the player looks at it.
 */

using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class KeycardCollectible : MonoBehaviour
{
    /// <summary>
    /// The ID of the keycard.
    /// Must match the requiredKeycardID on the corresponding Door.
    /// </summary>
    [SerializeField] private string keycardID = "Red";

    /// <summary>
    /// Display name shown in UI prompts like ( "Red Keycard").
    /// </summary>
    [SerializeField] private string keycardName = "Red Keycard";

    /// <summary>
    /// UI Text element showing "Press E to collect [name]".
    /// </summary>
    [SerializeField] private TextMeshProUGUI interactPromptText;

    /// <summary>
    /// Reference to the UIManager for displaying pickup messages.
    /// </summary>
    [SerializeField] private UIManager uiManager;

    /// <summary>
    /// How long the pickup message stays on screen in seconds.
    /// </summary>
    [SerializeField] private float messageDuration = 5f;

    /// <summary>
    /// Whether the player is currently looking at this keycard within interact range.
    /// </summary>
    private bool isInRange = false;

    /// <summary>
    /// The input action used to interact with objects. Configurable in the Inspector.
    /// </summary>
    [SerializeField] private InputAction interactAction = new InputAction(type: InputActionType.Button);

    /// <summary>
    /// Reference to the PlayerInventory to register the collected keycard.
    /// </summary>
    private PlayerInventory playerInventory;

    /// <summary>
    /// Audio clip played when a keycard is collected
    /// </summary>
    [SerializeField] private AudioClip collectSFX;

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
    /// Called every frame, handles interact input when character presses E to collect item
    /// </summary>
    void Update()
    {
        if (isInRange && interactAction.WasPressedThisFrame())
        {
            Collect();
        }
    }

    /// <summary>
    /// Called by PlayerInteract when players looks at object using raycast
    /// Shows the interact prompt.
    /// </summary>
    /// <param name="inventory">The player's inventory script.</param>
    public void OnRaycastEnter(PlayerInventory inventory)
    {
        playerInventory = inventory;
        isInRange = true;

        if (interactPromptText != null)
            interactPromptText.text = "Press E to collect " + keycardName;
    }

    /// <summary>
    /// Called by PlayerInteract when stops looking at object using Raycast
    /// Hides the interact prompt.
    /// </summary>
    public void OnRaycastExit()
    {
        isInRange = false;
        playerInventory = null;

        if (interactPromptText != null)
            interactPromptText.text = "";
    }

    /// <summary>
    /// Handles the collection of keycards
    /// Registers it with the player's inventory and shows pickup message
    /// plays collection SFX and destroys the card
    /// </summary>
    private void Collect()
    {
        if (playerInventory == null) return;

        playerInventory.AddKeycard(keycardID);

        if (interactPromptText != null)
            interactPromptText.text = "";

        if (uiManager != null)
            uiManager.ShowPickupMessage(keycardName + " collected!", messageDuration);
        else
            Debug.LogWarning("KeycardCollectible: No UIManager assigned on " + keycardName);

        Debug.Log(keycardName + " collected!");

        if (collectSFX != null)
            AudioSource.PlayClipAtPoint(collectSFX, transform.position);

        Destroy(gameObject);
    }

    /// <summary>
    /// Returns the keycard ID string.
    /// </summary>
    /// <returns>The keycard ID (e.g. "Red", "Orange").</returns>
    public string GetKeycardID()
    {
        return keycardID;
    }
}