using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// ¿Qué salas existen y dónde están ubicadas?
public class DungeonGenerator
{
    public List<RoomData> GenerateDungeon()
    {
        // Lista de salas
        List<RoomData> rooms = GenerateLayout();
        ApplyRules(rooms);
        AssignDoors(rooms);
        return rooms;
    }

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

        // Se agrega la sala final
        Vector2Int bossPos;
        Debug.Log("current: ");
        Debug.Log(currentPos);
        do
        {
            bossPos = currentPos + GetRandomDirection();
        }
        while (occupiedPositions.Contains(bossPos));
        rooms.Add(new RoomData
        {
            Position = bossPos,
            Type = RoomType.Boss
        });
        return rooms;
    }

    // Aplicar diferentes reglas
    private void ApplyRules(List<RoomData> rooms)
    {
        AssignBossRoom(rooms);
    }

    // Regla para sala de hjefes
    private void AssignBossRoom(List<RoomData> rooms)
    {
        // Buscar hojas
        List<RoomData> leaves =
      GetLeafRooms(rooms);

        // Buscar la hoja mas lejana
        RoomData leaf =
         leaves
         .OrderByDescending(r =>
             Mathf.Abs(r.Position.x) +
             Mathf.Abs(r.Position.y))
         .FirstOrDefault();

        if (leaf == null)
        {
            Debug.Log("No hojas disponibles");
            return;
        }
        /* Se asigna a una dirección de la hoja*/
        // Todas las direcciones
        Vector2Int[] directions =
            {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        // Por cada direccion
        foreach (Vector2Int direction in directions)
        {
            // Tomar una posición candidata
            Vector2Int candidate =
                leaf.Position + direction;

            // Verificar si la posición está ocupada
            bool occupied =
                rooms.Any(r =>
                    r.Position == candidate);

            // Si no está ocupada se agrega la habitación del jefe
            if (!occupied)
            {
                rooms.Add(new RoomData
                {
                    Position = candidate,
                    Type = RoomType.Boss
                });

                Debug.Log(
                    $"Boss room added at {candidate}");

                return;
            }
        }

    }
    // Regla para sala de tesoro
    private void AssignTreasureRoom(List<RoomData> rooms)
    {

    }

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
    private Vector2Int GetRandomDirection()
    {
        Vector2Int[] directions =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        return directions[
            Random.Range(0, directions.Length)];
    }

    // Traer todas las hojas
    private List<RoomData> GetLeafRooms(
        List<RoomData> rooms)
    {
        // Buscar salas combate que solo tienen 1 vecino
        return rooms
            .Where(r =>
                CountConnections(r, rooms) == 1 &&
                r.Type == RoomType.Combat)
            .ToList();
    }

    // Contar conexiones de una sala
    private int CountConnections(
    RoomData room,
    List<RoomData> rooms)
    {
        int connections = 0;

        Vector2Int position = room.Position;

        if (rooms.Any(r =>
            r.Position == position + Vector2Int.up))
        {
            connections++;
        }

        if (rooms.Any(r =>
            r.Position == position + Vector2Int.down))
        {
            connections++;
        }

        if (rooms.Any(r =>
            r.Position == position + Vector2Int.left))
        {
            connections++;
        }

        if (rooms.Any(r =>
            r.Position == position + Vector2Int.right))
        {
            connections++;
        }

        return connections;
    }
}

