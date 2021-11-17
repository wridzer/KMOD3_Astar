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
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        int HScore = (int)Vector2.Distance(startPos, endPos);
        Node startNode = new Node(startPos, null, 0, HScore);
        open.Add(startNode);
        while(open != null)
        {
            Node currentNode = null;
            foreach(Node n in open)
            {
                if (currentNode == null || n.FScore < currentNode.FScore)
                {
                    currentNode = n;
                } else
                {
                    continue;
                }
                if(currentNode.position == endPos)
                {
                    //found end
                }
            }
            GenerateNodeSuccessors(currentNode, startPos, endPos);
            
        }
        //codes in list??

        return null;
    }

    void GenerateNodeSuccessors(Node currentNode, Vector2Int startPos, Vector2Int endPos)
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
            if (true/*no wall here, also no node*/)
            {
                int HScore = (int)Vector2.Distance(successorPos, endPos);
                int GScore = (int)Vector2.Distance(startPos, successorPos);
                Node successorNode = new Node(startPos, null, GScore, HScore);
            }
        }
        //add them to a list??

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
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
