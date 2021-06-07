using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun", order = 3)]
public class Gun : TankPart
{
    [SerializeField]
    private Bullet bullet;

    [SerializeField]
    private int damage;

    [SerializeField]
    private int range;

    [SerializeField]
    private float cooldown;

    private float cooldownTimer = 0;

    public int Range
    { 
    get { return range; }
    }

    [SerializeField]
    private int bulletCount;

    public Gun()
    {
        partType = PartType.Gun;
    }

    public void Attack(Vector2 origin, Vector2 direction)
    {
        if (Time.time - cooldownTimer > cooldown)
        {
            float offset = 0.2f;
            float startOffset = (bulletCount - 1) * (offset/2);
            for (int i = 0; i < bulletCount;i++)
            {
                Vector2 originOffset = new Vector2((-startOffset + offset * i)*direction.y, (-startOffset + offset * i)*direction.x);
                Bullet newBullet = Instantiate(bullet.gameObject).GetComponent<Bullet>();
                newBullet.SetStats(damage, range);
                newBullet.SetBullet(origin + originOffset, direction);

            }
            cooldownTimer = Time.time;
        }
    }
}
