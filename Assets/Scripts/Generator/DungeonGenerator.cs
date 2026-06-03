using UnityEngine;
using System.Collections.Generic;

// ¿Qué salas existen y dónde están ubicadas?
public class DungeonGenerator
{
    public List<RoomData> GenerateDungeon()
    {
        List<RoomData> rooms = new();

        // lógica de generación
        rooms.Add(new RoomData
        {
            Position = new Vector2Int(0, 0),
            Type = RoomType.Start
        });

        rooms.Add(new RoomData
        {
            Position = new Vector2Int(0, 1),
            Type = RoomType.Combat
        });

        rooms.Add(new RoomData
        {
            Position = new Vector2Int(0, 2),
            Type = RoomType.Boss
        });

                rooms.Add(new RoomData
        {
            Position = new Vector2Int(1, 2),
            Type = RoomType.Boss
        });

        return rooms;
    }
}
