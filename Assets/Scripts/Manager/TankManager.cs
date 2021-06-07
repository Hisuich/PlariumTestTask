using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    private List<Gun> guns;
    private List<Body> bodies;
    private List<Tower> towers;

    [SerializeField]
    private Gun minGun;

    [SerializeField]
    private Body minBody;

    [SerializeField]
    private Tower minTower;

    private void Start()
    {
        LoadGuns();
        LoadBodies();
        LoadTowers();
    }

    private void LoadGuns()
    {
        guns = new List<Gun>();
        guns.AddRange(Resources.LoadAll<Gun>("Tank/Gun"));
    }

    private void LoadBodies()
    {
        bodies = new List<Body>();
        bodies.AddRange(Resources.LoadAll<Body>("Tank/Body"));
    }

    private void LoadTowers()
    {
        towers = new List<Tower>();
        towers.AddRange(Resources.LoadAll<Tower>("Tank/Tower"));
    }

    public void SetRandomTank(out Gun gun, out Body body, out Tower tower)
    {
        gun = Instantiate(guns[Random.Range(0, guns.Count)]);
        body = Instantiate(bodies[Random.Range(0, bodies.Count)]);
        tower = Instantiate(towers[Random.Range(0, towers.Count)]);
    }

    public void SetMinTank(out Gun gun, out Body body, out Tower tower)
    {
        gun = Instantiate(minGun);
        body = Instantiate(minBody);
        tower = Instantiate(minTower);
    }
}
