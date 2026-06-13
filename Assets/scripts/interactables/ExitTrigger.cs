/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Triggers the win condition when the player walks through the exit
 */
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    /// <summary>
    /// Reference to GameManager to trigger the win condition.
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// Finds the GameManager in the scene when the game starts.
    /// </summary>
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
            Debug.LogWarning("ExitTrigger: No GameManager found!");
    }

    /// <summary>
    /// Called when player enters the exit trigger zone.
    /// Triggers win condition if walked through.
    /// </summary>
    /// <param name="other">The collider that entered.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit!");

            if (gameManager != null)
                gameManager.Win();
        }
    }
}