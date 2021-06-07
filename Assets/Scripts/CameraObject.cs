using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    private float mapWidth;
    private float mapHeight;

    private Vector2 widthTreshold;
    private Vector2 heightTreshold;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Map map;

    [SerializeField]
    private Camera camera;

    private void Start()
    {
        mapWidth = map.GetMapSize();
        mapHeight = map.GetMapSize();
        float size = camera.orthographicSize;

        float tileSize = map.GetTileSize();

        widthTreshold = new Vector2(map.transform.position.x + tileSize * (size-1), map.transform.position.x + mapWidth - tileSize * (size));
        heightTreshold = new Vector2(map.transform.position.y + tileSize * ((size / 2)), map.transform.position.y + mapHeight - tileSize * ((size / 2)));
    }

    private void Update()
    {
        if (player != null)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector2 playerPosition = player.transform.position;
        Vector3 cameraPosition = playerPosition;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, widthTreshold.x, widthTreshold.y);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, heightTreshold.x, heightTreshold.y);
        cameraPosition.z = -10;

        transform.position = cameraPosition;

    }

    public void SetPlayer(GameObject player)
    {
        this.player = player; 
    }
}
