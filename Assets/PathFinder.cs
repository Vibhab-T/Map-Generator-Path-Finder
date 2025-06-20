using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{

    public GameObject vehiclePrefab;
    public RoadNode goalNode;
    public RoadNode startNode; 

    private GameObject vehicle;

    private List<RoadNode> path;
    private int currentIndex = 0;

    private bool isMoving = false;
    private bool destinationReached = false;


    private GameObject spawn(GameObject vehiclePrefab, Vector3 position)
    {
      
        GameObject vehicle = Instantiate(vehiclePrefab, position, Quaternion.identity, transform);
        return vehicle;
    }
    public void SetGoalNode(RoadNode node)
    {
        Debug.Log("Goal Node: ");
        Debug.Log(node.Position);
        goalNode = node;
        
        if (startNode != null && goalNode != null && vehicle != null)
        {
            Debug.Log("Path Started");
            findPathAndStartMoving();
        }
    }

    public void SetStartNode(RoadNode node)
    {
        Debug.Log("Start Node: ");
        Debug.Log(node.Position);
        startNode = node;
        vehicle = spawn(vehiclePrefab, startNode.Position);
    }
    private void startTravel()
    {
        isMoving = true;
    }

    private void findPathAndStartMoving()
    {
        path = findPath(startNode, goalNode);
        if(path.Count > 1)
        {
            currentIndex = 1;
            isMoving = true;
        }
    }

    private List<RoadNode> findPath(RoadNode start, RoadNode goal)
    {
        //using simple breadth-first-search BFS
        var queue = new Queue<RoadNode>();
        var cameFrom = new Dictionary<RoadNode, RoadNode>();
        queue.Enqueue(start);
        cameFrom[start] = null;

        while(queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach(var neighbor in current.neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        List<RoadNode> path = new List<RoadNode>();
        if (!cameFrom.ContainsKey(goal))
        {
            return path;
        }

        var node = goal;
        while(node != null)
        {
            path.Add(node);
            node = cameFrom.ContainsKey(node) ? cameFrom[node] : null;
        }
        path.Reverse();
        return path;
    }

    private void Update()
    {
        if (isMoving && path != null && currentIndex <path.Count && vehicle != null)
        {
            Vector3 targetPosition = path[currentIndex].Position;

            vehicle.transform.position = Vector3.MoveTowards(vehicle.transform.position, targetPosition, 50 * Time.deltaTime);

            if (vehicle.transform.position == targetPosition)
            {
                currentIndex++;
                if (currentIndex >= path.Count)
                {
                    isMoving = false;
                    Debug.Log("Destination Reached");
                }
            } 
        }
    }



}
