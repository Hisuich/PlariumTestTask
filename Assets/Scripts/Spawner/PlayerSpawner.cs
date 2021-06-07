using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{

    [SerializeField]
    private GameObject hero;

    [SerializeField]
    private List<Tank> tanks;

    private bool isHeroSpawn;

    private TankManager tankManager;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TankManager>();
        isHeroSpawn = true;
    }

    override public GameObject Spawn(Vector3 position)
    {
        GameObject tankObject;
        if (isHeroSpawn)
        {
            tankObject = Instantiate(hero);
            isHeroSpawn = false;
        }
        else
        {
            tankObject = Instantiate(tanks[Random.Range(0,tanks.Count)].gameObject);
        }

        Tank newTank = tankObject.GetComponent<Tank>();
        tankObject.transform.position = position;
        Gun gun;
        Body body;
        Tower tower;
        tankManager.SetRandomTank(out gun, out body, out tower);
        newTank.SetBody(body);
        newTank.SetGun(gun);
        newTank.SetTower(tower);
        

        return tankObject;
    }
}
