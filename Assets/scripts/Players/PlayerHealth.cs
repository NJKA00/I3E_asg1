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
        /// Set player's max health
        /// </summary>
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private Slider healthBar;
        /// <summary>
        /// Displays the text (HP:xxx)
        /// </summary>
        [SerializeField] private TextMeshProUGUI healthText;

        /// <summary>
        /// player's current health
        /// </summary>
        private float currentHealth;

        /// <summary>
        /// Reference to the GameManager.
        /// </summary>
        

        /// <summary>
        /// Called when the script is first initialised to get player's health info
        /// </summary>
        void Start()
        {
            currentHealth = maxHealth;
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
        /// show the player's current health
        /// </summary>
        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        /// <summary>
        /// show the player's maximum health
        /// </summary>
        public float GetMaxHealth()
        {
            return maxHealth;
        }

        /// <summary>
        /// Plays when chatacyer dies
        /// </summary>
        private void Die()
        {
            Debug.Log("Player has died!");
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            gameManager.GameOver();
        }
    }   