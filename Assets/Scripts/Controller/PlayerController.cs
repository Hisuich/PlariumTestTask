using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tank tank;

    private void Start()
    {
        Camera.main.GetComponent<CameraObject>().SetPlayer(gameObject);
        tank.onDestroy += EndGame;
    }
    private void Move()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }

        tank.Move(direction);
    }

    private void Attack()
    {
        tank.Attack();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            tank.SetAdditionalPart();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            tank.DestroyAdditionalPart();
        }

    }

    private void EndGame()
    {
        GameObject.FindGameObjectWithTag("Main Menu").GetComponent<MainMenu>().Activate();
    }
}
