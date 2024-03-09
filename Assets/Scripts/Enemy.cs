using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public int groupSpeedUpThreshold = 10;
    public GameObject bulletPrefab;
    
    private int remainingEnemies;
    private float timeUntilNextBullet;

    void Start()
    {
        remainingEnemies = GetTotalEnemyCount();
        timeUntilNextBullet = Random.Range(0f, 5f); // Initial random countdown.
        StartCoroutine(PeriodicBulletFire());
    }

    void Update()
    {
        MoveTowardsPlayer();
        HandleEnemyBulletFiring();
    }

    void MoveTowardsPlayer()
    {
        // Implement sideways and downward movement towards the player.
        // Use Transform.Translate or Rigidbody2D.velocity.
    }

    void HandleEnemyBulletFiring()
    {
        timeUntilNextBullet -= Time.deltaTime;

        if (timeUntilNextBullet <= 0)
        {
            ShootBullet();
            timeUntilNextBullet = Random.Range(0f, 5f); // Set a new random countdown.
        }
    }

    void ShootBullet()
    {
        // Instantiate bullets from enemies and move them downwards.
    	Vector3 bulletPosition = transform.position;
    	bulletPosition.y -= 1f; // Adjust this value as needed to position the bullet below the enemy.
    	GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
    	bullet.GetComponent<Bullet>().SetEnemyBullet(); // Set the bullet as an enemy bullet.
    	// Check for collisions with the player and destroy the player upon collision.
    }

    IEnumerator PeriodicBulletFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f)); // Adjust the interval as needed.
            ShootBullet();
        }
    }

    int GetTotalEnemyCount()
    {
        // Implement a method to count the total number of enemies in the scene.
        // You can use GameObject.FindObjectsOfType to find all enemy objects.
        return 0; // Replace with the actual count.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Handle player bullet collision with enemies.
            Destroy(other.gameObject); // Destroy the bullet.
            Destroy(gameObject); // Destroy the enemy.
            remainingEnemies--;

            // Adjust enemy speed based on remaining enemies.
            if (remainingEnemies <= groupSpeedUpThreshold)
            {
                // Speed up enemies.
                // Implement your logic here.
            }
        }
        else if (other.CompareTag("Barricade"))
        {
            // Handle player bullet collision with barricades.
            // You may need to implement barricade hit counters and destruction logic.
        }
    }
}