using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    private Vector2 shootInput;

    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private PlayerStats stats;
    public float fireCooldown = 0.2f;

    private float fireTimer;

    public float speed = 5f;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        shootInput = inputActions.Player.Shoot.ReadValue<Vector2>();


        fireTimer -= Time.deltaTime;

        if (shootInput != Vector2.zero && fireTimer <= 0f)
        {
            Shoot(shootInput);
            fireTimer = fireCooldown;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * stats.moveSpeed;
    }

    private void Shoot(Vector2 direction)
    {

        direction.Normalize();

        // Distancia a sacar la bala
        float spawnDistance = 0.5f;

        Vector3 spawnPosition =
       firePoint.position + (Vector3)(direction * spawnDistance);

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );
        bullet.GetComponent<Bullet>().SetDamage(stats.damage);
        bullet.GetComponent<Bullet>().SetSpeed(stats.bulletSpeed);
        bullet.GetComponent<Bullet>().SetRange(stats.range);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
}
