using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Collider2D collider;

    [SerializeField]
    private Rigidbody2D rigidbody;

    private float speed = 5;

    private int damage;

    public int Damage
    {
        get { return damage; }
    }

    private int range;

    private Vector2 origin;

    public void SetStats(int damage, int range)
    {
        this.damage = damage;
        this.range = range;
    }

    public void SetBullet(Vector2 origin, Vector2 direction)
    {
        transform.position = origin;
        this.origin = origin;

        rigidbody.velocity = direction * speed;
        
    }

    private void Update()
    {
        Vector2 position = transform.position;

        if ((position - origin).magnitude >= range*1.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
