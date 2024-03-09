using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;
    private bool isEnemyBullet = false; // Flag to indicate whether the bullet is fired by an enemy

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    // Method to set the bullet as an enemy bullet
    public void SetEnemyBullet()
    {
        isEnemyBullet = true;
    }

    void Fire()
    {
        // Set the velocity based on the direction
        rb.velocity = (isEnemyBullet ? Vector2.down : Vector2.up) * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the bullet upon collision with a barrier or an enemy
        if (other.CompareTag("Barrier") || other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Destroy the bullet if it exits the vertical dimension of the canvas
        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}