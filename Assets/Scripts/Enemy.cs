using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 3;      
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float hitStunDuration = 0.5f;

    private bool isStunned = false;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private int points = 100;

    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;
    private AudioSource audioSource;

    private void Update()
    {
        if (!isStunned)
        {
            // Move left 
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (transform.position.x < -7.5)
        {
            playerHealth.loseALife();

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        health -= damage;
        audioSource.PlayOneShot(hitClip);

        StartCoroutine(HitStun());

        if (health <= 0)
        {
            audioSource.PlayOneShot(dieClip);
            Die();
        }
    }
    private IEnumerator HitStun()
    {
        isStunned = true;            // stop moving
        yield return new WaitForSeconds(hitStunDuration);
        isStunned = false;           // resume movement
    }

    private void Die()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 0.5f);
        }

        if (playerHealth != null)
        {
            playerHealth.AddScore(points);
        }

        Destroy(gameObject); // remove enemy
    }
    
    //Destroy the bullet when hitting an enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();

            // Destroy the bullet on impact
            Destroy(collision.gameObject);
        }
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
        }

        audioSource = GetComponent<AudioSource>();
    }

}
