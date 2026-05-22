using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    public float damage = 1f;

    private Rigidbody2D rb;
    private Vector2 direction;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        rb.linearVelocity = direction * speed;
        //  transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si la colision se hizo con un enemigo
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Debug.Log("Pega");
            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
