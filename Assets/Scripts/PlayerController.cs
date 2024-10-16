using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player params")]
    public float speed = 4;
    [HideInInspector] public bool interacting;
    [HideInInspector] public bool atBase = true;

    [Header("Weapon params")]
    public GameObject weapon;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletPrefab;

    Vector3 currentVelocity;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        HandleWeapon();
    }

    private void FixedUpdate()
    {
        if (!interacting)
        {
            // Move the player using Rigidbody2D
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical) * speed;
            rb.velocity = movement;
    
            // Make the camera follow the player
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), ref currentVelocity, 0.25f);
        }
    }

    private void LateUpdate() {
        // Make the camera follow the player
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), ref currentVelocity, 0.25f);
    }

    public void HandleWeapon()
    {
        // Rotate the weapon
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(weapon.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Shoot
        if (!atBase && Input.GetMouseButtonDown(0))
        {
            // Flash muzzle
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, weapon.transform.Find("Gun").Find("Barrel").Find("Muzzle").position, weapon.transform.rotation);
            Destroy(muzzleFlash, 0.1f);

            // Spawn bullet
            Instantiate(bulletPrefab, weapon.transform.Find("Gun").Find("Muzzle").position, weapon.transform.rotation);
        }
    }
}
