/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Tracks the player's collected keycards and collectible parts.
 */

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// List of keycard IDs the player currently holds.
    /// </summary>
    private List<string> collectedKeycards = new List<string>();

    /// <summary>
    /// Adds a keycard to the player's inventory.
    /// </summary>
    /// <param name="keycardID">The ID of the keycard to add.</param>
    public void AddKeycard(string keycardID)
    {
        if (!collectedKeycards.Contains(keycardID))
        {
            collectedKeycards.Add(keycardID);
            Debug.Log("Keycard added to inventory: " + keycardID);
        }
    }

    /// <summary>
    /// Checks whether the player holds a specific keycard.
    /// </summary>
    /// <param name="keycardID">The ID to check for.</param>
    /// <returns>True if the player has the keycard, false otherwise.</returns>
    public bool HasKeycard(string keycardID)
    {
        return collectedKeycards.Contains(keycardID);
    }
}