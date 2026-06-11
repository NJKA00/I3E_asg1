    /*
    * Author: Kayden
    * Date: 09/06/2026
    * Description: Manages the player's health, taking damage, and death.
    */

    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;    


    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// The player's maximum health.
        /// </summary>
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private Slider healthBar;
        /// <summary>
        /// Text displaying the player's current health value.
        /// </summary>
        [SerializeField] private TextMeshProUGUI healthText;

        /// <summary>
        /// The player's current health.
        /// </summary>
        private float currentHealth;

        /// <summary>
        /// Reference to the GameManager.
        /// </summary>
        //private GameManager gameManager;

        /// <summary>
        /// Called when the script is first initialised.
        /// </summary>
        void Start()
        {
            currentHealth = maxHealth;
        // gameManager = FindObjectOfType<GameManager>();
            healthBar.value = currentHealth;
            healthText.text = "HP: " + Mathf.RoundToInt(currentHealth);
        }

        /// <summary>
        /// Applies damage to the player.
        /// </summary>
        /// <param name="amount">Amount of damage to deal.</param>
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            Debug.Log("Player health: " + currentHealth);
            healthBar.value = currentHealth;
            healthText.text = "HP: " + Mathf.RoundToInt(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Returns the player's current health.
        /// </summary>
        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        /// <summary>
        /// Returns the player's maximum health.
        /// </summary>
        public float GetMaxHealth()
        {
            return maxHealth;
        }

        /// <summary>
        /// Handles player death.
        /// </summary>
        private void Die()
        {
            Debug.Log("Player has died!");
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            gameManager.GameOver();
        }
    }   