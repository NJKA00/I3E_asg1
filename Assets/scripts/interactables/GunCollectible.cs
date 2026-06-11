/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Gun collectible
 */
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 
public class GunCollectible : MonoBehaviour
{   
    /// <summary>
    /// The score value of each gun.
    /// </summary>
    [SerializeField] private float gunScore = 10f;
    /// <summary>
    /// The name of each gun collectible.
    /// </summary>
    [SerializeField] private string gunName = "Kyrie Elysion";
    /// <summary>
    /// text box shown on ui screen 
    /// </summary>
    [SerializeField] private TextMeshProUGUI interactPromptText;
    /// <summary>
    /// Control text and duration on screen
    /// </summary>
    [SerializeField] private UIManager uiManager;
    /// <summary>
    /// Assign keybind to collect the collectible
    /// </summary>
    [SerializeField] private InputAction interactAction = new InputAction(type: InputActionType.Button);    
    /// <summary>
    /// How long message lasts for
    /// </summary>
    [SerializeField] private float messageDuration = 5f;

    /// <summary>
    /// Whether the player is currently looking at this collectible within interact range.
    /// </summary>
    private bool isInRange = false;
    /// <summary>
    /// help to store data on collectibles
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// find the object which has the gamemanager on it, and is called when game starts
    /// </summary>
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
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
    /// check if player is in range and if can press e to collect
    /// </summary>
    void Update()
    {
        if (isInRange && interactAction.WasPressedThisFrame())
        {
            Collect();
        }
    }
    /// <summary>
    /// Called by PlayerInteract when the player's raycast hits this object.
    /// Shows the interact prompt.
    /// </summary>
    public void OnRaycastEnter()
    {
        
        isInRange = true;

        if (interactPromptText != null)
            interactPromptText.text = "Press E to collect " + gunName;
    }
    /// <summary>
    /// Called by PlayerInteract when the player's raycast stops hitting this object.
    /// Hides the interact prompt.
    /// </summary>
    public void OnRaycastExit()
    {
        isInRange = false;
        

        if (interactPromptText != null)
            interactPromptText.text = "";
    }
    /// <summary>
    /// Handles the collection of gun collectibles.
    /// Registers the amount of points incremented under gamemanager and destroys this object.
    /// </summary>
    private void Collect()
    {
        if (gameManager != null)
        {
        gameManager.AddScore(gunScore);
        }
        else
        {
            Debug.LogWarning("GunCollectible: No GameManager found!");
        }
        if (interactPromptText != null)
            interactPromptText.text = "";

        if (uiManager != null)
            uiManager.ShowPickupMessage(gunName + " collected!", messageDuration);
        else
            Debug.LogWarning("Guncollectible: No UIManager assigned on " + gunName);

        Debug.Log(gunName + " collected!");
        Destroy(gameObject);
    }
}
