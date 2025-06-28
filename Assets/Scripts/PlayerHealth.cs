using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private float immuneTime = 0.5f;
    private float lastHitTime = -Mathf.Infinity;
    public TextMeshProUGUI healthText;
    private GameOverManager gameOverManager;

    public void Start()
    {
        gameOverManager = FindFirstObjectByType<GameOverManager>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (Time.time - lastHitTime < immuneTime) return;

        lastHitTime = Time.time;
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            gameOverManager.ShowGameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }
}
