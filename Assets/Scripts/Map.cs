using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private GameObject Empty; //0,3

    [SerializeField]
    private GameObject PlayerBase; //1

    [SerializeField]
    private GameObject EnemyBase; //2

    [SerializeField]
    private GameObject horizontalWall; //5

    [SerializeField]
    private GameObject verticalWall; //6

    [SerializeField]
    private GameObject crossWall; //7

    private MapGenerator generator;

    private List<List<int>> mapNodes;

    private float tileSize;
    private int[] baseMap;

    private void Awake()
    {
        generator = new MapGenerator();

        tileSize = Empty.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        baseMap = generator.GetMap();
        int mapSize = generator.GetMapSize();
        mapNodes = generator.GetMapNodes();
        for (int i = 0; i < mapSize;i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                GameObject emptyTile = Instantiate(Empty, transform);
                emptyTile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                if (baseMap[i * mapSize + j] == 1)
                {
                    GameObject tile = Instantiate(PlayerBase, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i * mapSize + j] == 2)
                {
                    GameObject tile = Instantiate(EnemyBase, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i * mapSize + j] == 3)
                {
                    GameObject tile = Instantiate(Empty, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i*mapSize+j] == 4)
                {
                    GameObject tile = Instantiate(crossWall, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i * mapSize + j] == 5)
                {
                    GameObject tile = Instantiate(horizontalWall, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i * mapSize + j] == 6)
                {
                    GameObject tile = Instantiate(verticalWall, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
                if (baseMap[i * mapSize + j] == 7)
                {
                    GameObject tile = Instantiate(crossWall, transform);
                    tile.transform.position = new Vector2(transform.position.x + tileSize * j, transform.position.y + tileSize * i);
                }
            }
        }
    }

    public Vector2 GetPositionByMapIndex(int index)
    {
        int x = index % generator.GetMapSize();
        int y = index / generator.GetMapSize();

        Vector2 position = new Vector2(x * tileSize, tileSize * y);
        return position;
    }


    public Stack<Vector2> GetPath(Vector2 from, Vector2 to, int offset = 0)
    {
        Vector2 mapPosition = transform.position;
        Vector2 fromMapPosition = from - mapPosition;
        Vector2 toMapPosition = to - mapPosition;

        int fromIndex = (int)(fromMapPosition.y / tileSize) * generator.GetMapSize() + (int)Mathf.Round(fromMapPosition.x / tileSize);
        int toIndex = (int)(toMapPosition.y / tileSize) * generator.GetMapSize() + (int)Mathf.Round(toMapPosition.x / tileSize);

        Debug.Log("Tile size " + tileSize);
        Debug.Log("From " + fromIndex + " fromMapPosition: " + GetPositionByMapIndex(fromIndex));
        Debug.Log("To " + toIndex);

        Dictionary<int, int> passedTiles = new Dictionary<int, int>();

        Dictionary<int, int> path = new Dictionary<int, int>();

        HashSet<int> visited = new HashSet<int>();

        path.Add(fromIndex,0);

        bool pathFound = false;

        while (!pathFound)
        {
            int currentIndex = -1;
            int minValue = int.MaxValue;

            foreach (var key in path.Keys)
            {
                if (visited.Contains(key)) continue;
                if (path[key] < minValue)
                {
                    currentIndex = key;
                    minValue = path[key];
                    break;
                }
            }
            if (currentIndex == -1)
            {
                Debug.Log("Path is not found");
                return new Stack<Vector2>();
            }

            visited.Add(currentIndex);

            List<int> mapNode = mapNodes[generator.GetGraphIndexByMapIndex(currentIndex, mapNodes)];

            Vector2 fromPosition = GetPositionByMapIndex(currentIndex);

            for (int i = 1; i < mapNode.Count;i++)
            {
                if (mapNode[i] == toIndex)
                {
                    passedTiles.Add(mapNode[i], mapNode[0]);
                    pathFound = true;
                    break;
                }

                if (baseMap[mapNode[i]] == 1 || baseMap[mapNode[i]] == 2) continue;
                
                Vector2 toPosition = GetPositionByMapIndex(mapNode[i]);

                int value = path[mapNode[0]] + (int)(Mathf.Abs(toPosition.x - fromPosition.x) + Mathf.Abs(toPosition.y - fromPosition.y));

                if (passedTiles.ContainsKey(mapNode[i]))
                {
                    if (path[passedTiles[mapNode[i]]] > value)
                    {
                        path[passedTiles[mapNode[i]]] = value;
                        passedTiles[mapNode[i]] = mapNode[0];
                    }
                }
                else
                {
                    passedTiles.Add(mapNode[i], mapNode[0]);
                }

                if (path.ContainsKey(mapNode[i]))
                    path[mapNode[i]] = value;
                else
                    path.Add(mapNode[i], value);
            }
            
        }

        Stack<Vector2> pathTo = new Stack<Vector2>();
        if (!passedTiles.ContainsKey(toIndex)) return pathTo;
        pathTo.Push(GetPositionByMapIndex(toIndex));
        int indexToPush = passedTiles[toIndex];
        while (indexToPush != fromIndex)
        {
            pathTo.Push(GetPositionByMapIndex(indexToPush));
            indexToPush = passedTiles[indexToPush];
        }
    
        return pathTo;
    }
    public float GetMapSize()
    {
        int mapSize = generator.GetMapSize();
        return mapSize * tileSize;
    }

    public float GetTileSize()
    {
        return tileSize;
    }

}
