using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 10f;

    public float damage = 1f;

    public float range = 5f;


    [HideInInspector]
    public Vector2 direction;
    private Rigidbody2D rb;
    

    private Vector2 startPosition;

    private BulletMovement movement;

    private void Awake()
    {
        movement = GetComponent<BulletMovement>();
    }

    private void Start()
    {
        movement.Initialize(this);

        float lifetime = range / speed;

        Destroy(gameObject, lifetime);
    }


    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        movement.Move();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si la colision se hizo con un elemento que puede recibir daño
        IDamageable damageable =
            collision.GetComponent<IDamageable>();


        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

           Destroy(gameObject);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

        public void SetRange(float range)
    {
        this.range = range;
    }
}
