using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int maxHealth = 5;
    private int health;

    public Material[] healthColors; // Array to store different colors for each health value
    private Renderer meshRenderer;

    private void Start()
    {
        health = maxHealth;
        meshRenderer = GetComponent<Renderer>();
        UpdateBarrierColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Decrease barrier health
            health--;

            // Destroy the bullet
            Destroy(other.gameObject);

            // Check if health is zero and destroy the barrier
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                // Update barrier color when health changes
                UpdateBarrierColor();
            }
        }
    }

    private void UpdateBarrierColor()
    {
        // Ensure health is within valid range
        int clampedHealth = Mathf.Clamp(health, 1, healthColors.Length);

        // Set the color based on health
        meshRenderer.material = healthColors[clampedHealth - 1];
    }
}