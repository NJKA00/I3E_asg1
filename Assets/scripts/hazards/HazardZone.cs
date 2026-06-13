/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Deals damage over time to the player when they are inside the hazard zone.
 *              Plays a sound effect on each damage tick.
 */
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    /// <summary>
    /// The name of the hazard.
    /// </summary>
    [SerializeField] private string hazardName = "Hazard";

    /// <summary>
    /// Amount of damage dealt per tick
    /// </summary>
    [SerializeField] private float damagePerTick = 15f;

    /// <summary>
    /// How many seconds between each tick
    /// </summary>
    [SerializeField] private float tickRate = 1f;

    /// <summary>
    /// Audio clip played on each damage tick
    /// </summary>
    [SerializeField] private AudioClip hazardSFX;

    /// <summary>
    /// Timer to track time between ticks.
    /// </summary>
    private float tickTimer = 0f;

    /// <summary>
    /// Whether the player is currently inside the hazard zone
    /// </summary>
    private bool playerInZone = false;

    /// <summary>
    /// Reference to player's health script.
    /// </summary>
    private PlayerHealth playerHealth;

    /// <summary>
    /// Called every frame. Deals damage and plays SFX on each tick.
    /// </summary>
    void Update()
    {
        if (playerInZone && playerHealth != null)
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickRate)
            {
                playerHealth.TakeDamage(damagePerTick);

                if (hazardSFX != null)
                    AudioSource.PlayClipAtPoint(hazardSFX, transform.position);

                tickTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Called when player enters the hazard zone trigger.
    /// </summary>
    /// <param name="other">The collider that entered.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth == null)
                Debug.LogWarning("Player entered " + hazardName + " but has no PlayerHealth!");

            playerInZone = true;
            tickTimer = tickRate;
            Debug.Log("Player entered " + hazardName + " zone!");
        }
    }

    /// <summary>
    /// Called when player exits the hazard zone trigger.
    /// </summary>
    /// <param name="other">The collider that exited.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            playerHealth = null;
            Debug.Log("Player exited " + hazardName + " zone!");
        }
    }
}       