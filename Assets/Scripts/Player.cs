using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    private int lives;
	public int initialLives = 3;    
    public float movementSpeed = 5f;
	private bool canFire = true;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ShootBullet();
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

	public void DestroyPlayer()
    {
        lives--;

        if (lives <= 0)
        {
            // Game over logic
            GameManager.Instance.EndGame();
        }
        else
        {
            // Reset player position or other respawn logic
            // Example: transform.position = startPosition;
        }
    }
	
    void OnTriggerEnter2D(Collider2D other)
{
    Bullet bullet = other.GetComponent<Bullet>();
    if (bullet != null)
    {
        if (bullet.isEnemyBullet)
        {
            // Handle collision with enemy bullets (destroy player, reduce health, or other logic).
            Destroy(gameObject); // Example: destroy the player.
        }

        // Always destroy the bullet on collision.
        Destroy(other.gameObject);
		DestroyPlayer();
    }
}

}