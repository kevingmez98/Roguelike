using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int healAmount = 1;

    protected override void OnPickup(PlayerController player)
    {
        player.Heal(healAmount);
    }
}
