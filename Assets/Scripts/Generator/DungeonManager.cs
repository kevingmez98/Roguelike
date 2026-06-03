using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Su trabajo es usar los datos del dungeon generator.
public class DungeonManager : MonoBehaviour
{
    private DungeonGenerator generator;

    [SerializeField]
    private Room combatRoomPrefab;

    [SerializeField]
    private float roomWidth = 20f;

    [SerializeField]
    private float roomHeight = 12f;

    [SerializeField]
    private List<RoomPrefabEntry> roomPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        generator = new DungeonGenerator();

        List<RoomData> rooms =
            generator.GenerateDungeon();

        SpawnRooms(rooms);
    }

    // Update is called once per frame
    private void SpawnRooms(List<RoomData> roomsData)
    {
        foreach (var roomData in roomsData)
        {

            Vector3 position = new Vector3(
                roomData.Position.x * roomWidth,
                roomData.Position.y * roomHeight,
            0);
            roomData.Doors = CalculateDoors(roomData, roomsData);
            Room prefab = GetRoomPrefab(roomData);
            if (prefab == null)
            {
                Debug.LogError(
                    $"No prefab found for {roomData.Type} {roomData.Doors}");
                return;
            }
            Room room = Instantiate(
                prefab,
                position,
                Quaternion.identity);
            room.Data = roomData;

            Debug.Log(
    $"{roomData.Position} - {roomData.Doors}");
        }
    }

    // Calcular direcciones de las puertas del generador. Se podria delegar al generator
    private DoorMask CalculateDoors(
        RoomData room,
        List<RoomData> allRooms)
    {
        // Definir mask por defecto
        DoorMask mask = DoorMask.None;

        // Posición de la room evaluada
        Vector2Int position = room.Position;

        // Verificar si alguna sala se generó en la posición de la evaluada+1 arriba
        // En lugar de any se podria hacer un HashSet<Vector2Int> roomPositions;
        bool hasUp = allRooms.Any(r =>
            r.Position == position + Vector2Int.up);
        // Verificar si alguna sala se generó en la posición de la evaluada+1 abajo
        bool hasDown = allRooms.Any(r =>
            r.Position == position + Vector2Int.down);
        // Verificar si alguna sala se generó en la posición de la evaluada+1 izquierda
        bool hasLeft = allRooms.Any(r =>
            r.Position == position + Vector2Int.left);

        // Verificar si alguna sala se generó en la posición de la evaluada+1 derecha
        bool hasRight = allRooms.Any(r =>
            r.Position == position + Vector2Int.right);

        // Dependiendo de lo encontrado, se van asignando las mask
        if (hasUp)
            mask |= DoorMask.Up;

        if (hasDown)
            mask |= DoorMask.Down;

        if (hasLeft)
            mask |= DoorMask.Left;

        if (hasRight)
            mask |= DoorMask.Right;

        return mask;
    }

    private Room GetRoomPrefab(RoomData roomData)
    {
        Debug.Log(roomData.Type);
        Debug.Log(roomPrefabs[0].Prefab.roomType);
        Debug.Log(roomPrefabs[0].Prefab.SupportedDoors);
        return roomPrefabs.FirstOrDefault(r =>
            r.Prefab.roomType == roomData.Type &&
            r.Prefab.SupportedDoors == roomData.Doors)
            ?.Prefab;
    }
}
