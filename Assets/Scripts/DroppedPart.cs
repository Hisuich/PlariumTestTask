using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DroppedPart : MonoBehaviour
{
    private TankPart part;

    public TankPart Part
    { 
        set { part = value; }
        get { return part; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Tank")
        {
            Destroy(gameObject);
        }
    }

}
