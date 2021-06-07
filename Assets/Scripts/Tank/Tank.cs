using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
Player,
Enemy
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private Tower tower;

    [SerializeField]
    private Body body;

    [SerializeField]
    private Gun gun;

    [SerializeField]
    private SpriteRenderer bodySprite;

    [SerializeField]
    private SpriteRenderer towerSprite;

    [SerializeField]
    private SpriteRenderer gunSprite;

    [SerializeField]
    private GameObject droppedPart;

    [SerializeField]
    private TankPart additionalPart;

    public TankPart AdditionalPart
    { 
        get { return additionalPart; }
    }

    [SerializeField]
    private Team team;

    public int Range
    { 
    get { return gun.Range; }
    }


    public Team TankTeam
    {
        get { return team; }
    }

    private Rigidbody2D rigidbody;

    [SerializeField]
    private GameObject bulletSpawn;

    public Vector2 BulletSpawnPosition
    { 
        get { return bulletSpawn.transform.position;}
    }

    public Action onDestroy;

    private Vector2 lastDirection;

    private int durability;

    public int Durability
    { 
        get { return durability; }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        SetupSprites();

        MapGenerator generator = new MapGenerator();
    }

    public void SetAdditionalPart()
    {
        if (additionalPart != null)
        {
            if (additionalPart.type == PartType.Body)
            {
                SetBody((Body)additionalPart);
            }
            else if (additionalPart.type == PartType.Gun)
            {
                SetGun((Gun)additionalPart);
            }
            else if (additionalPart.type == PartType.Tower)
            {
                SetTower((Tower)additionalPart);
            }
            additionalPart = null;
        }
    }

    public void DestroyAdditionalPart()
    {
        additionalPart = null;
    }

    public void Stop()
    {
        rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (durability <= 0)
            Destroyed();
    }
    private void SetupSprites()
    {
        if (team == Team.Player)
        {
            bodySprite.sprite = body.PlayerSprite;
            towerSprite.sprite = tower.PlayerSprite;
            gunSprite.sprite = gun.PlayerSprite;
        }
        if (team == Team.Enemy)
        {
            bodySprite.sprite = body.EnemySprite;
            towerSprite.sprite = tower.EnemySprite;
            gunSprite.sprite = gun.EnemySprite;
        }
    }

    public void SetBody(Body body)
    {
        this.body = body;
        SetupSprites();

        durability = body.Durability;
        
    }

    public void SetGun(Gun gun)
    {
        this.gun = gun;
        SetupSprites();
    }

    public void SetTower(Tower tower)
    {
        this.tower = tower;
        SetupSprites();
    }

    public void Attack()
    {
        gun.Attack(bulletSpawn.transform.position, tower.Aim(lastDirection));
    }

    public void Move(Vector2 direction)
    {

        rigidbody.velocity = direction * body.Speed;

        if (direction != Vector2.zero)
        {
            lastDirection = direction;
            float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
        }
    }
    
    public void TakeDamage(Bullet bullet)
    {
        durability -= bullet.Damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.collider.gameObject.GetComponent<Bullet>());
        }
        else if (collision.collider.gameObject.tag == "Part")
        {
            additionalPart = collision.collider.gameObject.GetComponent<DroppedPart>().Part;
        }
    }

    private void LeftTankPart()
    {
        int rand = UnityEngine.Random.Range(0, 3);
        GameObject dropped = Instantiate(this.droppedPart);
        dropped.transform.position = transform.position;

        if (rand == 0)
        {
            dropped.GetComponent<DroppedPart>().Part = Instantiate(body);
            dropped.GetComponent<SpriteRenderer>().sprite = dropped.GetComponent<DroppedPart>().Part.GetSprite(team);
        }
        else if (rand == 1)
        {
            dropped.GetComponent<DroppedPart>().Part = Instantiate(gun);
            dropped.GetComponent<SpriteRenderer>().sprite = dropped.GetComponent<DroppedPart>().Part.GetSprite(team);
        }
        else if (rand == 2)
        {
            dropped.GetComponent<DroppedPart>().Part = Instantiate(tower);
            dropped.GetComponent<SpriteRenderer>().sprite = dropped.GetComponent<DroppedPart>().Part.GetSprite(team);
        }
    }

    private void Destroyed()
    {
        onDestroy?.Invoke();
        LeftTankPart();
        Destroy(gameObject);
    }

}
