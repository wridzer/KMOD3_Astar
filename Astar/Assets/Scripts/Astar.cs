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
    private List<Node> allNodes = new List<Node>();
    private List<Node> successorNodes = new List<Node>();
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        //Check if begin and end are not the same
        if(startPos == endPos) { return new List<Vector2Int>(); }
        //Counter to end infinite loop
        int numberOfOperations = 0;
        //Create start node
        float HScore = Vector2.Distance(startPos, endPos);
        Node startNode = new Node(startPos, null, 0, HScore);
        open.Add(startNode);
        //Main loop to find end
        Node currentNode = null;
        while (open != null)
        {
            //Check if not in infinite loop
            if (numberOfOperations > 10000) { Debug.Log("Could not find path"); return null; }
            //Get new current node
            Node oldCurrentNode = currentNode;
            currentNode = GetLowestFScore();
            if (oldCurrentNode != currentNode)
            {
                open.Remove(currentNode);
                closed.Add(currentNode);
            }
            //Check if end
            if (currentNode.position == endPos)
            {
                closed.Clear();
                allNodes.Clear();
                successorNodes.Clear();
                open.Clear();
                return Backtrack(currentNode);
            }
            //Genarate new nodes
            successorNodes.Clear();
            GenerateNodeSuccessors(currentNode);
            foreach (Node n in successorNodes)
            {
                if (closed.Contains(n))
                {
                    continue;
                }
                // Create the values
                n.GScore = currentNode.GScore + Vector2.Distance(n.position, currentNode.position);
                n.HScore = Vector2.Distance(n.position, endPos);
                // Child is already in openList
                if (open.Contains(n))
                {
                    if (n.FScore > currentNode.FScore)
                    {
                        continue;
                    }
                }
                // Add the child to the openList
                n.parent = currentNode;
                open.Add(n);
            }
            numberOfOperations++;
        }
        return null;
    }

    private Node GetLowestFScore()
    {
        Node bestNode = null;

        foreach (Node n in open)
        {
            if (bestNode == null || n.FScore < bestNode.FScore)
            {
                bestNode = n;
                Debug.Log("FScore: " + bestNode.FScore + " Position: " + bestNode.position);
            }
        }
        Debug.Log("sex");

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

    private void GenerateNodeSuccessors(Node currentNode)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        Vector2Int successorPos = currentNode.position + new Vector2Int(-1, 1);
        Vector2Int successorPos1 = currentNode.position + new Vector2Int(0, 1);
        Vector2Int successorPos2 = currentNode.position + new Vector2Int(1, 1);
        Vector2Int successorPos3 = currentNode.position + new Vector2Int(-1, 0);
        Vector2Int successorPos4 = currentNode.position + new Vector2Int(1, 0);
        Vector2Int successorPos5 = currentNode.position + new Vector2Int(-1, -1);
        Vector2Int successorPos6 = currentNode.position + new Vector2Int(0, -1);
        Vector2Int successorPos7 = currentNode.position + new Vector2Int(1, -1);

        neighbours.Add(successorPos);
        neighbours.Add(successorPos1);
        neighbours.Add(successorPos2);
        neighbours.Add(successorPos3);
        neighbours.Add(successorPos4);
        neighbours.Add(successorPos5);
        neighbours.Add(successorPos6);
        neighbours.Add(successorPos7);

        for (int i = 0; i < neighbours.Count; i++)
        {
            bool notCreate = false;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(currentNode.position.x, 0.5f, currentNode.position.y), new Vector3(neighbours[i].x, 0.5f, neighbours[i].y), out hit))
            {
                Gizmos.DrawLine(new Vector3(currentNode.position.x, 0.5f, currentNode.position.y), new Vector3(neighbours[i].x, 0.5f, neighbours[i].y));
                notCreate = true;
            }
            foreach(Node n in allNodes)
            {
                if (n.position == neighbours[i])
                {
                    notCreate = true;
                    successorNodes.Add(n);
                }
            }
            if (notCreate)
            {
                neighbours.Remove(neighbours[i]);
            } else
            {
                Node successorNode = new Node(neighbours[i], null, 0, 0);
                allNodes.Add(successorNode);
                successorNodes.Add(successorNode);
                neighbours.Remove(neighbours[i]);
            }
        }        
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
