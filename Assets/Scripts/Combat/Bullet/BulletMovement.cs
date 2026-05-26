using UnityEngine;

public abstract class BulletMovement : MonoBehaviour
{
    protected Bullet bullet;

    public virtual void Initialize(Bullet bullet)
    {
        this.bullet = bullet;
    }

    public abstract void Move();
}
