using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    //player and bullet speeds, bullet spawns and prefabs
    [SerializeField] private float speed = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] private GameObject explosionPrefab;

    
    [SerializeField] private float fireRate = 0.25f;

    [SerializeField] private AudioClip shootClip;
    private AudioSource audioSource;

    // Input Action Assets
    public InputAction moveAction;
    public InputAction shootAction;
    public InputAction restartAction;

    private bool isShooting = false;   // track if button is held
    private float nextShotTime = 0f;     

    private void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
        restartAction.Enable();

        shootAction.performed += ctx => isShooting = true;
        shootAction.canceled += ctx => isShooting = false;
    }

    private void FireBullet()
    {
        //Spawns bullet at Bulletspawn
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        audioSource.PlayOneShot(shootClip);

        //Moves bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * bulletSpeed;
        }


        //spawm explosion when shot
        GameObject flash = Instantiate(explosionPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Destroy(flash, 0.5f); // destroy after animation finishes

        // Destroy bullet after 5 seconds if nothing hit
        Destroy(bullet, 5f);
    }

    private void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
        restartAction.Disable();

    }

    private void Update()
    {
        float moveInput = moveAction.ReadValue<float>();

        // Move up and down
        Vector3 move = new Vector3(0, moveInput, 0) * speed * Time.deltaTime;
        transform.Translate(move);

        // Shooting with delay
        if (isShooting && Time.time >= nextShotTime)
        {
            FireBullet();
            nextShotTime = Time.time + fireRate;
        }

        if (restartAction.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Start()
    {
      audioSource = GetComponent<AudioSource>();
    }
}
