using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// ¿Qué salas existen y dónde están ubicadas?
public class DungeonGenerator
{

    private readonly Vector2Int[] directions =
{
    Vector2Int.up,
    Vector2Int.down,
    Vector2Int.left,
    Vector2Int.right
};
    // Genera el dungeon completo
    public List<RoomData> GenerateDungeon()
    {
        // Lista de salas
        List<RoomData> rooms = GenerateLayout();
        ApplyRules(rooms);
        AssignDoors(rooms);
        return rooms;
    }

    // Generar el layout base del dungeon
    private List<RoomData> GenerateLayout()
    {
        // Lista de salas
        List<RoomData> rooms = new();

        // Listaa con las posiciones de salas ya llenas
        HashSet<Vector2Int> occupiedPositions = new();

        Vector2Int currentPos = Vector2Int.zero;
        // lógica de generación

        // Primero se genera la sala (0,0)
        rooms.Add(new RoomData
        {
            Position = currentPos,
            Type = RoomType.Start
        });

        // Colocar que ya se ocupó la sala (0,0)
        occupiedPositions.Add(currentPos);

        // Se da una cantidad de salas de combate
        int combatRooms = 3;
        int createdRooms = 0;
        // Se eligen las posiciones para cada sala y se agregan a la lista
        while (createdRooms < combatRooms)
        {
            // Elegir una diracción aleatoria para la sala
            Vector2Int nextPos = currentPos + GetRandomDirection();
            if (occupiedPositions.Contains(nextPos))
            {
                continue;
            }


            // Se agrega la data a la lista
            rooms.Add(new RoomData
            {
                Position = nextPos,
                Type = RoomType.Combat
            });

            // Colocar que ya se agrego la sala
            occupiedPositions.Add(nextPos);
            currentPos = nextPos;
            createdRooms++;
        }
        return rooms;
    }

    // Obtener una dirección aleatoria
    private Vector2Int GetRandomDirection()
    {
        return directions[
            Random.Range(0, directions.Length)];
    }

    // Aplicar diferentes reglas
    private void ApplyRules(List<RoomData> rooms)
    {
        AssignBossRoom(rooms);
        AssignTreasureRoom(rooms);
    }

    // Regla para sala de hjefes
    private void AssignBossRoom(List<RoomData> rooms)
    {
        // Buscar una sala candidata
        RoomData room =
    GetExpandableRooms(rooms)
    .OrderByDescending(r =>
        Mathf.Abs(r.Position.x) +
        Mathf.Abs(r.Position.y))
    .FirstOrDefault();

        if (!TryAddSpecialRoom(
               rooms,
               room,
               RoomType.Boss))
        {
            Debug.LogWarning(
                "Couldn't place boss room.");
        }


    }
    // Regla para sala de tesoro
    private void AssignTreasureRoom(List<RoomData> rooms)
    {

        RoomData room =
    GetExpandableRooms(rooms)
    .OrderByDescending(r =>
        Mathf.Abs(r.Position.x) +
        Mathf.Abs(r.Position.y))
    .FirstOrDefault();
        if (!TryAddSpecialRoom(
rooms,
room,
RoomType.Treasure))
        {
            Debug.LogWarning(
                "Couldn't place treasure room.");
        }
    }
    // Añadir una sala especial
    private bool TryAddSpecialRoom(
        List<RoomData> rooms,
        RoomData baseRoom,
        RoomType roomType)
    {
        if (baseRoom == null)
            return false;


        // Verificar direcciones posibles
        foreach (Vector2Int direction in directions)
        {
            Vector2Int candidate =
                baseRoom.Position + direction;

            bool occupied = rooms.Any(r =>
                r.Position == candidate);

            // Si está ocupada seguir buscando
            if (occupied)
                continue;
            // Agregar sala si la posición no está ocupada
            rooms.Add(new RoomData
            {
                Position = candidate,
                Type = roomType
            });

            Debug.Log(
                $"{roomType} room added at {candidate}");

            return true;
        }

        return false;
    }

    /* Helpers */
    // Activar y desactivar puertas para cada sala
    private void AssignDoors(List<RoomData> rooms)
    {
        foreach (RoomData room in rooms)
        {
            room.Doors =
                CalculateDoors(room, rooms);
        }
    }
    // Calcular direcciones de las puertas del generador para una sala
    private DoorMask CalculateDoors(
    RoomData room,
    List<RoomData> rooms)
    {
        // Definir mask por defecto
        DoorMask mask = DoorMask.None;

        // Por cada una de las ddirecciones
        foreach (var direction in directions)
        {
            // Si no hay salas en la posición a la que 
            // apuntaría la nueva posición se continua sin cambiar la mask de esa direccion
            if (!rooms.Any(r =>
           r.Position ==
           room.Position + direction))
            {
                continue;
            }

            // Si se encontró una sala en la posicion actual+direccion se modifica la mask
            if (direction == Vector2Int.up)
                mask |= DoorMask.Up;

            else if (direction == Vector2Int.down)
                mask |= DoorMask.Down;

            else if (direction == Vector2Int.left)
                mask |= DoorMask.Left;

            else if (direction == Vector2Int.right)
                mask |= DoorMask.Right;
        }
        return mask;
    }


    // Traer candidatas a salas expandibles
    private List<RoomData> GetExpandableRooms(
    List<RoomData> rooms)
    {
        return rooms
            .Where(r =>
                r.Type == RoomType.Combat &&
                directions.Any(direction =>
                    !rooms.Any(other =>
                        other.Position ==
                        r.Position + direction)))
            .ToList();
    }

    // Contar conexiones de una sala
    private int CountConnections(
    RoomData room,
    List<RoomData> rooms)
    {
        int connections = 0;
        // Por cada dirección
        foreach (var direction in directions)
        {
            // Si se encuentra una sala en la posición+ddireccion, se cuenta como conexión
            if (rooms.Any(r =>
                r.Position ==
                room.Position + direction))
            {
                connections++;
            }
        }

        return connections;
    }
}

