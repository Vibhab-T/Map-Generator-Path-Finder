using Unity.VisualScripting;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    public int width=10;
    public int height=10;
    public Material roadMat;
    public float spacing = 1.1f;
    public Material notRoadMat;
    public GameObject tilePrefab;

    [Range(0, 1)]
    public float verticalRoadProbability = 0.3f;

    private RoadNode goalNode;
    private RoadNode startNode;
    private RoadNode[,] nodeGrid;

    private void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        nodeGrid = new RoadNode[width, height];

        //create tiles and nodes
        for(int i =0; i<width; i++)
        {
            for(int j=0; j<height; j++){

                Vector3 pos = new(i* spacing, 0, j * spacing);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tileScript tileScript = tile.GetComponent<tileScript>();

                bool isRoad = false;

                if (j % 2 == 0)
                {
                    isRoad = true;
                }else
                {
                    isRoad = (i % 2 == 1 && Random.value < verticalRoadProbability);
                }
                if (isRoad)
                {
                    tileScript.makeRoad(roadMat);
                    nodeGrid[i, j] = tile.AddComponent<RoadNode>();
                }
                else
                {
                    tileScript.makeRoad(notRoadMat);
                }
            }
        }
        //link neighbours 
        for (int i = 0; i <width; i++)
        {
            for(int j = 0; j<height; j++)
            {
                RoadNode node = nodeGrid[i, j];
                if (node == null) continue;

                //up neighbour
                if (j+1 < height && nodeGrid[i, j+1] != null)
                {
                    node.neighbors.Add(nodeGrid[i, j + 1]);
                }

                //down neighbour
                if (j -1 >= 0 && nodeGrid[i, j - 1] != null)
                {
                    node.neighbors.Add(nodeGrid[i, j-1]);
                }

                //left neighbour
                if (i - 1 >= 0 && nodeGrid[i-1, j] != null)
                {
                    node.neighbors.Add(nodeGrid[i-1, j]);
                }

                //right neihbour
                if(i+1<width && nodeGrid[i+1, j] != null)
                {
                    node.neighbors.Add(nodeGrid[i + 1, j]);
                }
            }
        }

        //select random start and goal nodes
        RoadNode[] nodes = FindObjectsOfType<RoadNode>();
        if (nodes.Length > 1)
        {
            int goalIndex;
            int startIndex = Random.Range(0, nodes.Length);
            do
            {
                goalIndex = Random.Range(0, nodes.Length);
            } while (goalIndex == startIndex);
            
            startNode = nodes[startIndex];
            goalNode = nodes[goalIndex];
        }
        var Pathfinder = FindAnyObjectByType<PathFinder>();
        Pathfinder.SetStartNode(startNode);
        Pathfinder.SetGoalNode(goalNode);   
    }

}

