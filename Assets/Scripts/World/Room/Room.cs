using UnityEngine;
using System.Collections.Generic;
public class Room : MonoBehaviour
{
    [Header("References")]
    public List<Door> doors;
    public List<Enemy> enemies;

    public RoomType roomType;

    public Transform cameraTarget;
    public Transform CameraTarget => cameraTarget;
    public RoomData Data;

    [SerializeField]
    private DoorMask supportedDoors;
    public DoorMask SupportedDoors => supportedDoors;

    private RoomState roomState;


    [SerializeField]
    private Transform rewardSpawnPoint;

    [SerializeField]
    private GameObject[] rewardPrefabs;

    private void Awake()
    {
        RegisterEnemies();
        roomState = RoomState.Unvisited;
    }

    private void Start()
    {
        if (roomType != RoomType.Start)
            return;

        Camera.main.transform.position = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            Camera.main.transform.position.z
        );

        OnPlayerEntered();
    }
    private void RegisterEnemies()
    {
        enemies.Clear();

        Enemy[] roomEnemies = GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in roomEnemies)
        {
            enemies.Add(enemy);

            enemy.SetRoom(this);
        }

    }
    public void OnPlayerEntered()
    {

        switch (roomState)
        {
            case RoomState.Unvisited:
                roomState = RoomState.Active;
                if (roomType == RoomType.Start)
                {
                    OpenDoors();
                    break;
                }
                CloseDoors();
                ActivateEnemies();
                Debug.Log("Combat started");

                break;

            case RoomState.Active:

                CloseDoors();

                break;

            case RoomState.Cleared:

                break;
        }

    }

    public void EnemyDied(Enemy enemy)
    {
        enemies.Remove(enemy);

        Debug.Log("Enemy defeated");

        CheckCompletion();
    }

    private void CompleteRoom()
    {
        roomState = RoomState.Cleared;

        OpenDoors();

        SpawnReward();

        Debug.Log("Room completed");
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.Close();
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
    }
    private void ActivateEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    private void CheckCompletion()
    {
        if (enemies.Count == 0)
        {
            CompleteRoom();
        }
    }

    private void SpawnReward()
    {
        int index = Random.Range(0, rewardPrefabs.Length);

        Instantiate(
            rewardPrefabs[index],
            rewardSpawnPoint.position,
            Quaternion.identity
        );
    }
}
