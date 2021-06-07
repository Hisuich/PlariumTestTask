using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Spawner : MonoBehaviour
{

    abstract public GameObject Spawn(Vector3 position);
}
