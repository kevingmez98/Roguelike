using UnityEngine;

public class CoinPickup : Pickup
{
    [SerializeField] private int amount = 1;

    protected override void OnPickup(PlayerController player)
    {
        player.AddCoins(amount);
    }
}
