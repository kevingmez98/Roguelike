using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Collider2D col;

    [SerializeField] private DoorDirection direction;

    public void Open()
    {
        col.enabled = false;
    }

    public void Close()
    {
        col.enabled = true;
    }
}
