using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public AudioSource playerAudioSource;
    public AudioClip playerShotSound, playerDeathSound;
    private int lives = 3;   
    public float movementSpeed = 5f;
	private bool canFire = true, isDeathSoundPlaying = false;
    
    private void Awake()
    {
        // Try to find an existing AudioSource component
        playerAudioSource = GetComponent<AudioSource>();

        // If AudioSource component doesn't exist, create and configure it
        if (playerAudioSource == null)
        {
            playerAudioSource = gameObject.AddComponent<AudioSource>();
            playerAudioSource.playOnAwake = false;
        }

        // Load audio clips
        playerShotSound = Resources.Load<AudioClip>("Player Shot");
        playerDeathSound = Resources.Load<AudioClip>("Player Death");
    }
    
    // Update is called once per frame
    void Update()
    {
        // Check if the death sound effect is playing
        if (!isDeathSoundPlaying)
        {
            MovePlayer();
            ShootBullet();
        }
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontalInput * movementSpeed * Time.deltaTime);
    }
	
    void ShootBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            Vector3 bulletPosition = transform.position;
        	bulletPosition.y += 1f; // Position the bullet above the player.
        	GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            
            playerAudioSource.clip = playerShotSound;
            playerAudioSource.Play();

            // Set canFire to false, preventing the player from firing until this bullet is destroyed
            canFire = false;

            // Start a coroutine to wait for the bullet to be destroyed
            StartCoroutine(WaitForBulletDestroy(bullet));
        }
    }

    IEnumerator WaitForBulletDestroy(GameObject bullet)
    {
        // Wait until the bullet is destroyed
        yield return new WaitUntil(() => bullet == null);

        // Set canFire back to true, allowing the player to fire again
        canFire = true;
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (bullet.isEnemyBullet)
            {
                // Handle collision with enemy bullets
                DestroyPlayer();
            }
        }
    }
    
    public void DestroyPlayer()
    {
        lives--;
        
        // Play the death sound effect
        playerAudioSource.clip = playerDeathSound;
        playerAudioSource.Play();
        
        isDeathSoundPlaying = true; // Set the flag to indicate that the death sound effect is playing
        StartCoroutine(WaitForDeathSound()); // Wait for the death sound effect to finish playing
        
        if (lives <= 0)
        {
            GameManager.Instance.EndGame(false); // Game over logic
        }
    }
    
    IEnumerator WaitForDeathSound()
    {
        yield return new WaitForSeconds(playerDeathSound.length); // Wait for the duration of the death sound effect
        GameManager.Instance.ResetScene(); // Reset the scene as long as the player has lives remaining
        Destroy(gameObject); // Destroy the enemy GameObject
        isDeathSoundPlaying = false; // Reset the flag after the death sound effect finishes playing
    }
}