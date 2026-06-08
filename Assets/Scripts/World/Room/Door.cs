using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door State")]
    [SerializeField] private Collider2D col;

    [SerializeField] private DoorDirection direction;
    public DoorDirection Direction => direction;

    [SerializeField] private Collider2D blockCollider;

    [Header("Room Connection")]
    [SerializeField] private Transform spawnPoint;

    public Transform SpawnPoint => spawnPoint;

    // Habitación a la que pertenece
    private Room room;


    private void Awake()
    {
        room = GetComponentInParent<Room>();
    }
    public void Open()
    {
        blockCollider.enabled = false;
    }

    public void Close()
    {
        blockCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player"))
            return;

        Debug.Log(direction);
        room.GoToRoom(direction);
    }
}
