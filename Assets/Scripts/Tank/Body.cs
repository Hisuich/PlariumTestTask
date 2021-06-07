using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Body", menuName = "Body", order = 1)]
public class Body : TankPart
{
    [SerializeField]
    private int durability;

    [SerializeField]
    private int speed;

    public Body()
    {
        partType = PartType.Body;
    }

    public int Speed
    { 
        get { return speed; }
    }

    public int Durability
    { 
        get { return durability; }
    }

    public void TakeDamage(Bullet bullet)
    {

    }

}
