using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private EnemyStats stats;

    private Vector2 direction;

    private float currentHealth;

    private Rigidbody2D rb;

    private Transform player;

    private Room currentRoom;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        currentHealth = stats.maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void FixedUpdate()
    {
        direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * stats.moveSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Metodo para hacer daño
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player =
            collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(stats.contactDamage);
        }
    }

    public void SetRoom(Room room)
    {
        currentRoom = room;
    }

    private void Die()
    {
        if (currentRoom != null)
        {
            currentRoom.EnemyDied(this);
        }
        Destroy(gameObject);
    }
}
