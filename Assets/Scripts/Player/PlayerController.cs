using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    private Vector2 shootInput;

    private Rigidbody2D rb;


    private PlayerStats stats;

    private bool isInvulnerable;

    public float invulnerabilityTime = 1f;
    public float fireCooldown = 0.2f;

    public GameObject bulletPrefab;

    private float fireTimer;

    public Transform firePoint;

    public float currentHealth;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        currentHealth = stats.maxHealth;
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

    // Rutina para cambiar el estado de invulnerabilidad cuando termine el tiempo
    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false;
    }
    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        currentHealth -= damage;

        StartCoroutine(InvulnerabilityCoroutine());

        if (currentHealth <= 0)
        {
            Debug.Log("Muelto");
        }
    }

    public void Heal(int healAmount)
    {
        var totalHealth = currentHealth + healAmount;
        if (totalHealth > stats.maxHealth)
        {
            currentHealth = stats.maxHealth;
        }
        currentHealth = totalHealth;
    }

    public void AddCoins(int amount)
    {
        stats.coins += amount; 
    }
}
