using UnityEngine;
public class StraightMovement : BulletMovement
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Move()
    {
        rb.linearVelocity =
            bullet.direction * bullet.speed;
    }
}