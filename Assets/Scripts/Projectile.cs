using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100;
    private Rigidbody2D rb;

    private void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Apply a forward force to move the bullet
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // Cast a ray in the forward direction of the bullet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, speed * Time.deltaTime);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // Damage

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
