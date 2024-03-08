using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    void Fire()
    {
        rb.velocity = Vector2.up * speed;
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