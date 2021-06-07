using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    private List<Tank> tanks;

    private TankManager tankManager;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TankManager>();
    }
    public override GameObject Spawn(Vector3 position)
    {
        GameObject tankObject = Instantiate(tanks[Random.Range(0, tanks.Count)].gameObject);
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
