using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    //player and bullet speeds, bullet spawns and prefab
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed = 10f;

    // Reference to Input Action Assets
    public InputAction moveAction;
    public InputAction shootAction;

    private void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
        shootAction.performed += OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        //Spawns bullet at Bulletspawn
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        //Moves bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * bulletSpeed;
        }

        // Destroy bullet after 5 seconds if nothing hit
        Destroy(bullet, 5f);
    }

    private void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
        shootAction.performed -= OnShoot;
    }

    private void Update()
    {
        float moveInput = moveAction.ReadValue<float>();

        // Move object up and down
        Vector3 move = new Vector3(0, moveInput, 0) * speed * Time.deltaTime;
        transform.Translate(move);
    }
}
