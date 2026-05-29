using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door State")]
    [SerializeField] private Collider2D col;

    [SerializeField] private DoorDirection direction;

    [SerializeField] private Collider2D blockCollider;

    [Header("Room Connection")]
    [SerializeField] private Room targetRoom;

    [SerializeField] private Transform spawnPoint;

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

        if (targetRoom == null || spawnPoint == null)
        {
            Debug.LogWarning($"Door {name} is missing references.");
            return;
        }

        other.transform.position = spawnPoint.position;

        Camera.main.transform.position = new Vector3(
            targetRoom.CameraTarget.position.x,
            targetRoom.CameraTarget.position.y,
            Camera.main.transform.position.z
        );

        targetRoom.OnPlayerEntered();
    }
}
