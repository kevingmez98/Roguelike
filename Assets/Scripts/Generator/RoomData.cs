using UnityEngine;
public class RoomData
{
    public Vector2Int Position;
    public RoomType Type;

    public DoorMask Doors; //Indicar direcciones de puertas

    public string toString()
    {
        string pos = "pos:"+this.Position;
        string doors = "doors:" + this.Doors;
        return pos+doors;
    }
}