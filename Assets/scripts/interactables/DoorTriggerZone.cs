/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: A stationary trigger zone attached to a door.
 *              Closes the door when the player walks away from it.
 */

using System.Collections;
using UnityEngine;

public class DoorTriggerZone : MonoBehaviour
{
    /// <summary>
    /// Reference to the Door script on the parent door object.
    /// </summary>
    [SerializeField] private Door door;

    /// <summary>
    /// How long to wait before closing the door after the player exits.
    /// </summary>
    [SerializeField] private float closeDelay = 1f;

    /// <summary>
    /// Called when an object exits the trigger zone.
    /// Closes the door after a short delay if the player walks away.
    /// </summary>
    /// <param name="other">The collider that exited.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DelayedClose());
        }
    }

    /// <summary>
    /// Waits briefly then tells the door to close.
    /// </summary>
    private IEnumerator DelayedClose()
    {
        yield return new WaitForSeconds(closeDelay);
        door.CloseFromTrigger();
    }
}