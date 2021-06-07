using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{ 
    Base,
    Player
}

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Tank tank;

    private Map map;

    private bool isTargetLocked;

    private GameObject target;

    [SerializeField]
    private TargetType targetType;

    private double updatePathTime;

    private Stack<Vector2> path;

    private Vector2 curDirection;
    private Vector2 curDestination;

    private bool isMoving;

    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        updatePathTime = Time.time;
        isMoving = false;
        path = new Stack<Vector2>();
       // path = map.GetPath(transform.position, target.transform.position);  
    }

    private void LockTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetType == TargetType.Player ? "Tank" : targetType == TargetType.Base ? "Base" : "Tank");

        if (targetType == TargetType.Player)
            foreach (GameObject tankObject in targets)
            {
                Tank tank = tankObject.GetComponent<Tank>();

                if (tank.TankTeam == Team.Player)
                {
                    target = tankObject;
                    isTargetLocked = true;
                    tank.onDestroy += () => { isTargetLocked = false; };
                    break;
                }
            }

        if (targetType == TargetType.Base)
        {
            foreach (GameObject baseObject in targets)
            {
                Base enemyBase = baseObject.GetComponent<Base>();

                if (enemyBase.BaseTeam == Team.Player)
                {
                    target = baseObject;
                    isTargetLocked = true;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (!isTargetLocked)
            LockTarget();

        if (isTargetLocked && target != null)
        {
            if (Time.time - updatePathTime > 2)
            {
                path = map.GetPath(transform.position, target.transform.position);
                updatePathTime = Time.time;
            }
        }

        if (CanAttack())
        {
            tank.Attack();
            tank.Stop();
        }
        else
            Move();

    }

    private bool CanAttack()
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 position = transform.position;
        Vector2 direction = (curDestination - position).normalized;
        direction.x = Mathf.Round(direction.x);
        direction.y = Mathf.Round(direction.y);
        Debug.DrawRay(tank.BulletSpawnPosition, direction, Color.black, tank.Range);
        RaycastHit2D hit = Physics2D.Raycast(tank.BulletSpawnPosition, direction, tank.Range);

        Debug.Log("Hitten " + hit.collider);
        if (hit.collider == null) return false;
        if (hit.collider.gameObject == target)
        {
            return true;
        }
        else
        {
            return false;

        }

    }

    private void Move()
    {
        Vector2 position = transform.position;
        Vector2 mapOffset = transform.position;

        //Debug.Log(path.Pop());

        // transform.position = path.Pop();

        if (!isMoving && path.Count > 0)
        {
            curDestination = path.Pop();
            isMoving = true;
        }
        else if (isMoving)
        {
            Vector2 direction = (curDestination - position).normalized;
            tank.Move(direction);
            if ((position - curDestination).magnitude < 0.1f)
            {
                transform.position = curDestination;
                if (path.Count > 0)
                    curDestination = path.Pop();
                else
                    isMoving = false;
                    
            }
        }
    }
}
