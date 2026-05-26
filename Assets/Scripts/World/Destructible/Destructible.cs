using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
