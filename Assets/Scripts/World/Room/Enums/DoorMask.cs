using System;
using UnityEngine;

/*
La idea es que cada dirección tenga un bit distinto, permitiendo que cada
sala tenga todas las combinaciones, por ejemplo, si se buscan salas:

Con puerta arriba y abajo: 0011 que es 3
Con puerta izquierda y arriba: 0101 que es 5
Con puerta arriba, abajo y derecha: 1011 que es 9
Con todas las puertas: 1111 que es 15
*/
[Flags]
public enum DoorMask
{
    None = 0, // 0000
    Up = 1, // 0001
    Down = 2, //0010
    Left = 4,//0100
    Right = 8//1000
}


