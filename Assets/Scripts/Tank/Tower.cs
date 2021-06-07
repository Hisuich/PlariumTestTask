using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tower", menuName = "Tower", order = 2)]
public class Tower : TankPart
{
    [SerializeField]
    private int precision;

    public Tower()
    {
        partType = PartType.Tower;
    }
    public Vector2 Aim(Vector3 direction)
    {
        int rand = Random.Range(0, 101);

        if (rand < precision)
        {
            return direction;
        }
        else
        {
            direction.x = Mathf.Sign(direction.x);
            direction.y = Mathf.Sign(direction.y);
        }

        return direction.normalized;
    }
}
