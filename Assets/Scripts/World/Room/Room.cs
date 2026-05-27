using UnityEngine;
using System.Collections.Generic;
public class Room : MonoBehaviour
{
    [Header("References")]
    public List<Door> doors;
    public List<Enemy> enemies;

    private bool roomCompleted = false;
    private bool playerInside = false;


    private void Awake()
    {
        RegisterEnemies();
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
        if (playerInside)
            return;

        playerInside = true;

        if (roomCompleted)
            return;

        CloseDoors();

        ActivateEnemies();

        Debug.Log("Combat started");
    }

    public void EnemyDied(Enemy enemy)
    {
        enemies.Remove(enemy);

        Debug.Log("Enemy defeated");

        if (enemies.Count <= 0)
        {
            CompleteRoom();
        }
    }

    private void CompleteRoom()
    {
        roomCompleted = true;

        OpenDoors();

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
}
