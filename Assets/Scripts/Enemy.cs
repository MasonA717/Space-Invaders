using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public GameObject bulletPrefab;
    public GameManager gameManager; // Reference to the GameManager script
    public AudioSource enemyAudioSource;
    public AudioClip enemyShotSound, enemyDeathSound;

    public int remainingEnemies;
    private bool canFire = true, isDeathSoundPlaying = false;
    
    private void Awake()
    {
        // Try to find an existing AudioSource component
        enemyAudioSource = GetComponent<AudioSource>();

        // If AudioSource component doesn't exist, create and configure it
        if (enemyAudioSource == null)
        {
            enemyAudioSource = gameObject.AddComponent<AudioSource>();
            enemyAudioSource.playOnAwake = false;
        }

        // Load audio clips
        enemyShotSound = Resources.Load<AudioClip>("Enemy Shot");
        enemyDeathSound = Resources.Load<AudioClip>("Enemy Death");
    }
    
    void Start()
    {
        remainingEnemies = GetTotalEnemyCount();
        StartCoroutine(PeriodicBulletFire());
    }
    
    void Update()
    {
        // Check if the death sound effect is playing
        if (!isDeathSoundPlaying)
        {
            MoveTowardsPlayer();
        }
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
        
        enemyAudioSource.clip = enemyShotSound;
        enemyAudioSource.Play();
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
                // Play the death sound effect
                enemyAudioSource.clip = enemyDeathSound;
                enemyAudioSource.Play();
                
                isDeathSoundPlaying = true; // Set the flag to indicate that the death sound effect is playing
                StartCoroutine(WaitForDeathSound()); // Wait for the death sound effect to finish playing
                
                gameManager.IncrementScore(); // Increment the score based on the score multiplier
                gameManager.UpdateScoreMultiplier(1); // Increase the score multiplier
                
                // Adjust enemy speed based on remaining enemies.
                if (remainingEnemies <= GetTotalEnemyCount())
                {
                    // Speed up enemies.
                    float speedAdjustment = 1.5f; // 50% increase
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                    {
                        enemy.moveSpeed *= speedAdjustment;
                    }
                } else if (remainingEnemies == 0)
                {
                    GameManager.Instance.EndGame(true); // Game victory logic
                }
            }
        }
        else if (other.CompareTag("Barrier"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
    
    IEnumerator WaitForDeathSound()
    {
        yield return new WaitForSeconds(enemyDeathSound.length); // Wait for the duration of the death sound effect
        Destroy(gameObject); // Destroy the enemy GameObject
        isDeathSoundPlaying = false; // Reset the flag after the death sound effect finishes playing
    }
}