using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public int groupSpeedUpThreshold = 10;
    public GameObject bulletPrefab;

    private int remainingEnemies;
    private bool canFire = true;

    void Start()
    {
        remainingEnemies = GetTotalEnemyCount();
        StartCoroutine(PeriodicBulletFire());
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        // Implement sideways and downward movement towards the player.
        // Use Transform.Translate or Rigidbody2D.velocity.
    }

    IEnumerator PeriodicBulletFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f)); // Adjust the interval as needed.
            if (canFire)
                ShootBullet();
        }
    }

    void ShootBullet()
    {
        // Instantiate bullets from enemies and move them downwards.
        Vector3 bulletPosition = transform.position;
        bulletPosition.y -= 1f; // Adjust this value as needed to position the bullet below the enemy.
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetEnemyBullet(); // Set the bullet as an enemy bullet.
        StartCoroutine(DelayedBulletReset(bullet));
    }

    IEnumerator DelayedBulletReset(GameObject bullet)
    {
        canFire = false;
        yield return new WaitUntil(() => bullet == null);
        canFire = true;
    }

    int GetTotalEnemyCount()
    {
        // Implement a method to count the total number of enemies in the scene.
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
            }	
        }
        else if (other.CompareTag("Barricade"))
        {
            // Handle player bullet collision with barricades.
        }
    }
}
