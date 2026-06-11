/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Handles player interaction using Raycasting from the center of the screen.
 *              Detects interactable objects within range and notifies them when the player
 *              looks at them or looks away.
 */

using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    /// <summary>
    /// The maximum distance the player can interact with objects.
    /// </summary>
    [SerializeField] private float interactRange = 2.5f;

    /// <summary>
    /// The camera used to cast the interaction ray (first-person camera).
    /// </summary>
    [SerializeField] private Camera playerCamera;

    /// <summary>
    /// Reference to the player's inventory script.
    /// </summary>
    private PlayerInventory playerInventory;

    /// <summary>
    /// The currently targeted KeycardCollectible, if any.
    /// </summary>
    private KeycardCollectible currentKeycard;

    /// <summary>
    /// The currently targeted Door, if any.
    /// </summary>
    private Door currentDoor;

    /// <summary>
    /// The currently targeted GunCollectible, if any.
    /// </summary>
    private GunCollectible currentGun;

    /// <summary>
    /// Called when the script is first initialised.
    /// </summary>
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerInventory == null)
            Debug.LogWarning("PlayerInteract: No PlayerInventory found on this GameObject!");
    }

    /// <summary>
    /// Called every frame. Casts a ray from the center of the screen
    /// and checks for interactable objects within range.
    /// </summary>
    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // Check for keycard
            KeycardCollectible keycard = hit.collider.GetComponentInParent<KeycardCollectible>();
            if (keycard != null)
            {
                ClearDoorTarget();
                ClearGunTarget();
                if (currentKeycard != keycard)
                {
                    ClearKeycardTarget();
                    currentKeycard = keycard;
                    currentKeycard.OnRaycastEnter(playerInventory);
                }
                return;
            }

            // Check for door
            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null)
            {
                ClearKeycardTarget();
                ClearGunTarget();
                if (currentDoor != door)
                {
                    ClearDoorTarget();
                    currentDoor = door;
                    currentDoor.OnRaycastEnter(playerInventory);
                }
                return;
            }

            // Check for gun collectible
            GunCollectible gun = hit.collider.GetComponentInParent<GunCollectible>();
            if (gun != null)
            {
                ClearKeycardTarget();
                ClearDoorTarget();
                if (currentGun != gun)
                {
                    ClearGunTarget();
                    currentGun = gun;
                    currentGun.OnRaycastEnter();
                }
                return;
            }

            // Hit something else
            ClearAllTargets();
        }
        else
        {
            ClearAllTargets();
        }
    }

    /// <summary>
    /// Clears the current keycard target and hides its prompt.
    /// </summary>
    private void ClearKeycardTarget()
    {
        if (currentKeycard != null)
        {
            currentKeycard.OnRaycastExit();
            currentKeycard = null;
        }
    }

    /// <summary>
    /// Clears the current door target and hides its prompt.
    /// </summary>
    private void ClearDoorTarget()
    {
        if (currentDoor != null)
        {
            currentDoor.OnRaycastExit();
            currentDoor = null;
        }
    }

    /// <summary>
    /// Clears the current gun target and hides its prompt.
    /// </summary>
    private void ClearGunTarget()
    {
        if (currentGun != null)
        {
            currentGun.OnRaycastExit();
            currentGun = null;
        }
    }

    /// <summary>
    /// Clears all current targets.
    /// </summary>
    private void ClearAllTargets()
    {
        ClearKeycardTarget();
        ClearDoorTarget();
        ClearGunTarget();
    }
}