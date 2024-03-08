using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Handle bullet collision with enemy
            Destroy(other.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy the enemy
			Debug.Log("Ouch!");
        }
	}
}