using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Base : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPoint_1;

    [SerializeField]
    private GameObject spawnPoint_2;

    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private int maxTank;

    private int tanksLeft;

    [SerializeField]
    private Team team;

    public Team BaseTeam
    {
        get { return team; }
    }

    [SerializeField]
    private int maxDurability;

    private int durability;

    private int tankToSpawn = 2;

    private int rand = 0;
    void Start()
    {
        tanksLeft = maxTank;
        durability = maxDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (tankToSpawn > 0 && tanksLeft > 0)
        {
            GameObject spawnedTank = spawner.Spawn(rand == 0 ? spawnPoint_1.transform.position : spawnPoint_2.transform.position);
            tankToSpawn--;
            tanksLeft--;
            spawnedTank.GetComponent<Tank>().onDestroy += () => { tankToSpawn++; };
            rand = (rand + 1) % 2;
        }

        if (durability <= 0)
            Destroyed();
    }
    private void TakeDamage(Bullet bullet)
    {
        durability -= bullet.Damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
     if (collision.collider.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.collider.gameObject.GetComponent<Bullet>());
        }
    }

    private void EndGame()
    {
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenu>().Activate();
    }

    private void Destroyed()
    {
        EndGame();
        Destroy(gameObject);
    }
}
