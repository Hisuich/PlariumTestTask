using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    private int[] baseMap;
    private int size;

    private List<List<int>> graph;

    public MapGenerator()
    {
        size = 39;
        graph = new List<List<int>>();
        baseMap = new int[size * size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == 1 || i == size - 2)
                {
                    //Define cell for spawn point
                    if (j == (size / 2 - 1) || j == (size / 2 + 1))
                    {
                        baseMap[size * i + j] = 3;
                    }

                    //Define cell for TeamBase
                    else if (j == (size / 2))
                    {
                        if (i == 1) baseMap[size * i + j] = 1;
                        if (i == size - 2) baseMap[size * i + j] = 2;
                    }

                    else
                    {
                        baseMap[size * i + j] = 0;
                    }

                }
            }
        }

        // Map threshold is always wall
        for (int i = 0; i < size;i++)
        {
            for (int j = 0; j < size;j++)
            {
                int mapIndex = i * size + j;
                if (baseMap[mapIndex] != 0) continue;
                
                // threshold is always wall
                if (i == 0 || j == 0 || i == size - 1 || j == size - 1) baseMap[i * size + j] = 4;
                // cells near treshold must be always free field;
                //else if (i == 1 || j == 1 || i == size - 2 || j == size - 2) baseMap[i * size + j] = 0;
                else if ((i) % 2 == 0) baseMap[i * size + j] = 4;
                else if ((j) % 2 == 1)
                {
                    graph.Add(new List<int>() { i * size + j });

                    //Find Neighbors
                    if (j - 2 > 0)
                    {
                        int nextIndex = i * size + j - 2;
                        if (GetGraphIndexByMapIndex(nextIndex, graph) != -1)
                        {
                            graph[graph.Count - 1].Add(nextIndex);
                            graph[GetGraphIndexByMapIndex(nextIndex, graph)].Add(i * size + j);
                        }
                    }
                    if (i - 2 > 0)
                    {
                        int nextIndex = (i-2) * size + j;
                        if (GetGraphIndexByMapIndex(nextIndex, graph) != -1)
                        {
                            graph[graph.Count - 1].Add(nextIndex);
                            graph[GetGraphIndexByMapIndex(nextIndex, graph)].Add(i * size + j);
                        }
                    }
                }
                else baseMap[i * size + j] = 4;
            }
        }

        
        /*
        string str = "";
        for (int i = 0; i < graph.Count;i++)
        {
            for (int j = 0; j < graph[i].Count;j++)
            {
                str += graph[i][j] + " ";
            }
            str += "\n";
        }*/
        GenerateLabyrinth();
        for (int i = 0; i < 6;i++)
            DecreaseDeadEnds();
        SetWall();

    }

    // Check walls for it's neightbors to define wall type - horizontal, vertical or cross;
    private void SetWall()
    {
        for (int i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                int curIndex = i * size + j;

                if (baseMap[curIndex] == 4)
                {
                    // 5 - horizontal
                    // 6 - vertical
                    // 7 - cross


                    bool horizontal = false;
                    bool vertical = false;

                    if (IsWall(baseMap[curIndex - 1]) || IsWall(baseMap[curIndex + 1])) horizontal = true;
                    if (IsWall(baseMap[curIndex - size]) || IsWall(baseMap[curIndex + size])) vertical = true;

                    if (horizontal && vertical) baseMap[curIndex] = 7;
                    else if (horizontal) baseMap[curIndex] = 5;
                    else if (vertical) baseMap[curIndex] = 6;
                }
            }
        }
    }

    public List<List<int>> GetMapNodes()
    {
        List<List<int>> mapNodes = new List<List<int>>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int mapIndex = i * size + j;
                if (!IsWall(baseMap[mapIndex]))
                {
                    mapNodes.Add(new List<int>() { mapIndex });

                    if (i-1 >= 0)
                    {
                        int nextIndex = (i - 1) * size + j;

                        if (!IsWall(baseMap[nextIndex]) && GetGraphIndexByMapIndex(nextIndex, mapNodes) != -1)
                        {
                            mapNodes[mapNodes.Count-1].Add(nextIndex);
                            mapNodes[GetGraphIndexByMapIndex(nextIndex,mapNodes)].Add(mapIndex);
                        }
                    }

                    if (j-1 >= 0)
                    {
                        int nextIndex = i * size + j-1;
                        if (!IsWall(baseMap[nextIndex]) && GetGraphIndexByMapIndex(nextIndex, mapNodes) != -1)
                        {
                            mapNodes[mapNodes.Count-1].Add(nextIndex);
                            mapNodes[GetGraphIndexByMapIndex(nextIndex, mapNodes)].Add(mapIndex);
                        }
                    }
                }
            }
        }

        return mapNodes;
    }

    private bool IsWall(int id)
    {
        return id >= 4 && id <= 7; 
    }

    private void DecreaseDeadEnds()
    {
        for (int i = 1; i < size-1; i++)
        {
            for (int j = 1; j < size-1; j++)
            {
                int curIndex = i * size + j;

                if (baseMap[curIndex] == 4)
                {
                    // Check if there more then one connection to shis wall
                    // If yes - do nothing
                    // Otherwise - remove wall

                    int neightborNumber = 0;

                    if (baseMap[curIndex - 1] == 4) neightborNumber++;
                    if (baseMap[curIndex + 1] == 4) neightborNumber++;
                    if (baseMap[curIndex - size] == 4) neightborNumber++;
                    if (baseMap[curIndex + size] == 4) neightborNumber++;

                    if (neightborNumber <= 1) baseMap[curIndex] = 0;
                }
            }
        }
    }

    private void GenerateLabyrinth()
    {
        // We mark mapIndexes that we passed
        List<int> visited = new List<int>();

        // We add graph index that we want to visit if we reach dead end
        Stack<int> toVisit = new Stack<int>();

        int j = Random.Range(0, graph.Count);
        int node = graph[j][0];
        toVisit.Push(node);
        visited.Add(graph[j][0]);

        while (toVisit.Count > 0)
        {
            node = graph[j][0];

            int next = Random.Range(1, graph[j].Count);

            bool found = true;
            
            if (visited.Contains(graph[j][next]))
            {
                found = false;
                for (int i = 1; i < graph[j].Count;i++)
                {
                    if (!visited.Contains(graph[j][i]))
                    {
                        next = i;
                        found = true;
                        break;
                    }
                }
            }
            if (found)
            {
                int nextNode = graph[j][next];
                toVisit.Push(nextNode);
                visited.Add(nextNode);

                j = GetGraphIndexByMapIndex(nextNode,graph);

                // Get direction and add pass
                int direction = nextNode - node;
                int pass = node + direction / 2;
                baseMap[pass] = 0;
            }
            else
            {
                j = GetGraphIndexByMapIndex(toVisit.Pop(),graph);
            }
        }
    }

    public int GetGraphIndexByMapIndex(int mapIndex, List<List<int>> graph)
    {
        for (int i = 0; i < graph.Count;i++)
        {
            if (graph[i][0] == mapIndex) return i;
        }
        return -1;
    }

    public int[] GetMap()
    {
        return baseMap;
    }

    public int GetMapSize()
    {
        return size;
    }
}
