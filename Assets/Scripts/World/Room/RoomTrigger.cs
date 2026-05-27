using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Room room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.OnPlayerEntered();
        }
    }
}