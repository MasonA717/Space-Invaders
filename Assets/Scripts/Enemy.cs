using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;
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
        // Move the enemy horizontally
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // Check if the enemy is below the player's Y coordinate
        if (transform.position.y < -4f)
        {
            // Destroy the player and lose a life
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            // DestroyPlayer();
        }

        // Check if the enemy has reached the west or east barrier
        if ((transform.position.x < -8f && moveSpeed < 0) || (transform.position.x > 8f && moveSpeed > 0))
        {
            // Move down by 1 unit
            transform.Translate(Vector2.down * 1f);
            // Reverse the horizontal direction
            moveSpeed = -moveSpeed;
        }
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
        bulletPosition.y -= 1f; // Position the bullet below the enemy.
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
        return 7; // Replace with the actual count.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();

            // Check if the bullet is not fired by an enemy
            if (!bullet.isEnemyBullet)
            {
                // Handle player bullet collision with enemies.
                Destroy(other.gameObject); // Destroy the bullet.
                Destroy(gameObject); // Destroy the enemy.
                remainingEnemies--;

                // Adjust enemy speed based on remaining enemies.
                if (remainingEnemies <= GetTotalEnemyCount())
                {
                    // Speed up enemies.
                    float speedAdjustment = 1.5f; // 50% increase
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                    {
                        enemy.moveSpeed *= speedAdjustment;
                    }
                }
            }
        }
        else if (other.CompareTag("Barrier"))
        {
            // Handle player bullet collision with barricades.
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            // Handle collision with the player (enemy bullet hitting the player).
            Destroy(other.gameObject);
        }
    }
}
