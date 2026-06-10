/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Manages UI elements including pickup messages and their display duration.
 */

using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Text element used to display pickup messages to the player.
    /// </summary>
    [SerializeField] private TextMeshProUGUI pickupMessageText;

    /// <summary>
    /// Currently running message coroutine, if any.
    /// </summary>
    private Coroutine messageCoroutine;

    /// <summary>
    /// Displays a pickup message for a set duration then clears it.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="duration">How long to display it in seconds.</param>
    public void ShowPickupMessage(string message, float duration)
    {
        if (pickupMessageText == null) return;

        // If a message is already showing, cancel it first
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(DisplayMessage(message, duration));
    }

    /// <summary>
    /// Coroutine that shows a message then clears it after a delay.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="duration">How long to display it in seconds.</param>
    private IEnumerator DisplayMessage(string message, float duration)
    {
        pickupMessageText.text = message;
        yield return new WaitForSeconds(duration);
        pickupMessageText.text = "";
        messageCoroutine = null;
    }
}