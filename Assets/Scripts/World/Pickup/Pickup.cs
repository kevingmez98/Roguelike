using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
 protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        OnPickup(player);

        Destroy(gameObject);
    }

    protected abstract void OnPickup(PlayerController player);
}
