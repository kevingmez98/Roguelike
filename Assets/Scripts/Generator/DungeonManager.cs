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

    [SerializeField]
    private Transform player;
    // Lista de room instanciadas
    private Dictionary<Vector2Int, Room> spawnedRooms =
    new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        generator = new DungeonGenerator();

        List<RoomData> rooms =
            generator.GenerateDungeon();

        foreach (RoomData roomData in rooms)
        {
            Debug.Log(roomData.toString());
        }
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
            // Configurar las puertas de la room paara activar las necesarias
            room.ConfigureDoors(
    roomData.Doors);

            // Pasar referencia al dungeon manager
            room.Initialize(this);

            Debug.Log(
    $"{roomData.Position} - {roomData.Doors}");

            // Agregar sala a la lista
            spawnedRooms.Add(
            roomData.Position,
            room);
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
        return roomPrefabs.FirstOrDefault(r =>
            r.Prefab.roomType == roomData.Type)
            ?.Prefab;
    }


    public void ChangeRoom(Room currentRoom, DoorDirection direction)
    {
        // Dar un valor para moverse en la coordenada segun el enum y la dirección dada
        Vector2Int offset = direction switch
        {
            DoorDirection.Top => Vector2Int.up,
            DoorDirection.Bottom => Vector2Int.down,
            DoorDirection.Left => Vector2Int.left,
            DoorDirection.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
        Vector2Int targetPos =
               currentRoom.Data.Position + offset;

        if (spawnedRooms.TryGetValue(
                targetPos,
                out Room targetRoom))
        {
            Debug.Log(
                $"Moving to {targetRoom.Data.Position}");

            // Obtener dirección de la puerta de la sala destino usando el opuesto de la sala actual
            DoorDirection entranceDirection = GetOpposite(direction);

            // Obtener punto de spawn de la puerta destino
            Door entranceDoor = targetRoom.GetDoor(entranceDirection);
            Transform spawn = entranceDoor.SpawnPoint;

            // Mover camara
            Camera.main.transform.position = new Vector3(
                targetRoom.CameraTarget.position.x,
                targetRoom.CameraTarget.position.y,
                Camera.main.transform.position.z);

            // Mover jugador y activar sala
            player.transform.position =
                spawn.position;
            targetRoom.OnPlayerEntered();
        }
        else
        {
            Debug.LogError(
                $"No room found at {targetPos}");
        }
    }
    // Traer la dirección opuesta a una dirección
    private DoorDirection GetOpposite(
    DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.Top => DoorDirection.Bottom,
            DoorDirection.Bottom => DoorDirection.Top,
            DoorDirection.Left => DoorDirection.Right,
            DoorDirection.Right => DoorDirection.Left,
            _ => direction
        };
    }
}
