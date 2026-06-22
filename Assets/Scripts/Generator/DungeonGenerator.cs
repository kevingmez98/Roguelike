using UnityEngine;
using System.Collections.Generic;

// ¿Qué salas existen y dónde están ubicadas?
public class DungeonGenerator
{
    public List<RoomData> GenerateDungeon()
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
}

