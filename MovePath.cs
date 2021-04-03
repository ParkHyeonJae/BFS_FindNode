using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int index;
    public Vector3 position;

    public Node(int index, Vector3 position)
    {
        this.index = index;
        this.position = position;
    }
}

public class MovePath : MonoBehaviour
{
    public List<List<Node>> movePaths = new List<List<Node>>()
    {
        new List<Node>() { new Node(0, Vector3.zero)  },
    };


    public bool ShowNodeButtons = true;
    public bool ShowNodeHandles = true;

    public int destination { get; set; } = 1;
    public int start { get; set; } = 0;
    public List<Node> points = new List<Node>();

    public void StartBFSSearch(System.Action<List<Node>> action)
    {
        BFS(start);

        foreach (var v in points)
        {
            Debug.Log(v.index + "-> ");
        }

        action?.Invoke(points);
    }

    public void AddVertex(Vector3 position)
    {
        movePaths.Add(new List<Node>() { new Node(movePaths.Count, position) });
    }

    public void AttachEdge(Node vertex01,Node vertex02)
    {
        if (vertex01.index == vertex02.index)
            return;
        Debug.Log("Attached");
        movePaths[vertex01.index].Add(vertex02);
        movePaths[vertex02.index].Add(vertex01);

        Debug.Log("=================[vertex01]================");
        Debug.Log($"[vertex01] 추가된 항목 : {vertex01.index}");
        for (int i = 0; i < movePaths[vertex01.index].Count; i++)
            Debug.Log($"[vertex01] 전체 항목 : {movePaths[vertex01.index][i].index}");
        Debug.Log("=================[vertex01]================");

        Debug.Log("=================[vertex02]================");
        Debug.Log($"[vertex02] 추가된 항목 : {vertex02.index}");
        for (int i = 0; i < movePaths[vertex02.index].Count; i++)
            Debug.Log($"[vertex02] 전체 항목 : {movePaths[vertex02.index][i].index}");
        Debug.Log("=================[vertex02]================");
    }

    List<List<Node>> GetVertexs() => movePaths;

    [ContextMenu("ResetPaths")]
    public void ResetPaths()
    {
        movePaths = new List<List<Node>>()
        {
            new List<Node>() { new Node(0, Vector3.zero)  },
        };
        points.Clear();
    }

    [ContextMenu("BFS")]
    public void PrintBFSResult()
    {
        destination = 2;

        BFS(0);

        foreach (var v in points)
        {
            Debug.Log(v.index  + "-> ");
        }
    }

    public void PrintNodeByIndex(int index)
    {
        Debug.Log($"{index} node Info");
        foreach (var v in movePaths[index])
        {
            Debug.Log($"{v.index} : {v.position}");
        }
    }

    
    public void BFS(int start)
    {
        Queue<int> q = new Queue<int>();
        bool[] visited = new bool[movePaths.Count + 1];
        int[] parent = new int[movePaths.Count + 1];
        points.Clear();

        q.Enqueue(start);
        visited[start] = true;
        parent[start] = movePaths[start][0].index;
        Debug.Log("start : " + movePaths[start][0].index);

        while (q.Count > 0)
        {
            int now = q.Dequeue();
            Debug.Log("now: " + now);
            Debug.Log("movePaths[now].Count: " + movePaths[now].Count);
            for (int i = 1; i < movePaths[now].Count; i++)
            {
                Debug.Log($"Process{i} : " + movePaths[now][i].index);
                if (visited[movePaths[now][i].index])
                    continue;
                Debug.Log($"Enqueue{i} : " + movePaths[now][i].index);
                q.Enqueue(movePaths[now][i].index);
                visited[movePaths[now][i].index] = true;
                parent[movePaths[now][i].index] = now;
            }
        }

        Debug.Log("dest : " + destination);

        for (int i = 0; i < parent.Length; i++)
        {
            Debug.Log(i + " : " + parent[i]);
        }

        int dest = destination;
        points.Add(movePaths[destination][0]);
        while (parent[dest] != dest)
        {
            Debug.Log("parent[destination] : " + parent[dest]);
            points.Add(movePaths[parent[dest]][0]);

            dest = parent[dest];
        }
        Debug.Log("end Index : " + movePaths[destination][0].index);
        
        points.Reverse();
    }
}
