using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// 
    private Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
    private List<Node> successorNodes = new List<Node>();
    private List<Node> open = new List<Node>();

    //Directions
    Vector2Int left = new Vector2Int(-1, 0);
    Vector2Int right = new Vector2Int(1, 0);
    Vector2Int up = new Vector2Int(0, 1);
    Vector2Int down = new Vector2Int(0, -1);

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        //Check if begin and end are not the same
        if(startPos == endPos) { return new List<Vector2Int>(); }
        //Counter to end infinite loop
        int numberOfOperations = 0;
        //Generate grid if neccessary
        CreateGrid(grid);


        //Create start node
        float HScore = Vector2.Distance(startPos, endPos);
        Node startNode = new Node(startPos, null, 0, HScore);
        open.Add(startNode);
        //Main loop to find end
        Node currentNode = startNode;
        while (open.Count > 0)
        {
            //Check if not in infinite loop
            if (numberOfOperations > 10000) { Debug.Log("Could not find path"); return null; }

            //Get new current node
            currentNode = GetLowestFScore();

            //Check if end
            if (currentNode.position == endPos)
            {
                allNodes.Clear();
                successorNodes.Clear();
                open.Clear();
                return Backtrack(currentNode);
            }

            //Genarate new nodes
            successorNodes.Clear();
            GenerateNodeSuccessors(currentNode, grid);

            open.Remove(currentNode);
            foreach (Node n in successorNodes)
            {
                // Create the values
                float newGScore = currentNode.GScore + Vector2.Distance(n.position, currentNode.position);
                float newHScore = Vector2.Distance(n.position, endPos);
                if (newGScore < n.GScore)
                {
                    // This path to neighbor is better than any previous one. Record it!
                    n.parent = currentNode;
                    n.GScore = newGScore;
                    n.HScore = newHScore;
                    if (!open.Contains(n))
                    {
                        open.Add(n);
                    }
                }
            }
            numberOfOperations++;
        }
        Debug.LogError("Openlist empty before path was found");
        return null;
    }

    private void CreateGrid(Cell[,] grid)
    {
        foreach(Cell cell in grid)
        {
            allNodes.Add(cell.gridPosition, new Node(cell.gridPosition, null, float.PositiveInfinity, 0));
        }
    }

    private Node GetLowestFScore()
    {
        Node bestNode = null;

        foreach (Node n in open)
        {
            if (bestNode == null || n.FScore < bestNode.FScore)
            {
                bestNode = n;
            }
        }
        Debug.Log("FScore: " + bestNode.FScore + " Position: " + bestNode.position);

        return bestNode;
    }

    private List<Vector2Int> Backtrack(Node currentNode)
    {
        List<Vector2Int> directions = new List<Vector2Int>();

        Node thisNode = currentNode;

        while(thisNode.parent != null)
        {
            directions.Add(thisNode.position);
            thisNode = thisNode.parent;
        }
        directions.Add(thisNode.position);

        directions.Reverse();

        return directions;
    }

    private void GenerateNodeSuccessors(Node currentNode, Cell[,] grid)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        neighbours.Add(currentNode.position + up);
        neighbours.Add(currentNode.position + left);
        neighbours.Add(currentNode.position + down);
        neighbours.Add(currentNode.position + right);

        foreach (Vector2Int pos in neighbours)
        {
            if(allNodes.ContainsKey(pos))
            {
                if(CheckWall(currentNode, pos, grid))
                {
                    Node childNode = allNodes[pos];
                    successorNodes.Add(childNode);
                }

            }
        }
        
    }

    private bool CheckWall(Node _current, Vector2Int _neighbour, Cell[,] grid)
    {
        //get direction
        Vector2Int dir = _neighbour - _current.position;
        //if dir left check this wall left, other wall right
        if(dir == left)
        {
            if (grid[_current.position.x, _current.position.y].HasWall(Wall.LEFT) || grid[_neighbour.x, _neighbour.y].HasWall(Wall.RIGHT))
            {
                return false;
            }
        }
        //if dir up check this wall up, other wall down
        if (dir == up)
        {
            if (grid[_current.position.x, _current.position.y].HasWall(Wall.UP) || grid[_neighbour.x, _neighbour.y].HasWall(Wall.DOWN))
            {
                return false;
            }
        }
        //if dir right check this wall right, other wall left
        if (dir == right)
        {
            if (grid[_current.position.x, _current.position.y].HasWall(Wall.RIGHT) || grid[_neighbour.x, _neighbour.y].HasWall(Wall.LEFT))
            {
                return false;
            }
        }
        //if dir down check this wall down, other wall up
        if (dir == down)
        {
            if (grid[_current.position.x, _current.position.y].HasWall(Wall.DOWN) || grid[_neighbour.x, _neighbour.y].HasWall(Wall.UP))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, float GScore, float HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
