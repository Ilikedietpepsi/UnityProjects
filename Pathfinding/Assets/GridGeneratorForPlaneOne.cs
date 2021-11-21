using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneratorForPlaneOne : MonoBehaviour
{
    public static Node[,] grid;
    int rows, cols;
    public Vector2 gridSize;
    public float nodeSize;
    public LayerMask ObstacleMask;


    
    // AStarPF astar = new AStarPF();

    


    public void SetGrid() {
        grid = new Node[13,23];
        Vector3 start = new Vector3(0,3,-9) - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int x = 0; x < 13; x++) {
            for (int y = 0; y < 23; y++) {
                Vector3 p = start + Vector3.right * (x * nodeSize + (float)nodeSize / 2f) + Vector3.forward * (y * nodeSize + (float)nodeSize /2f);
                bool empty = true;
                if (Physics.CheckSphere(p, nodeSize-0.2f ,ObstacleMask)) {
                    empty = false;
                }

                grid[x,y] = new Node(x, y, empty, p);

            }
        }
    }

    public Node WhichNode(Vector3 p) {
        
        float x = (p.x + gridSize.x/2) / gridSize.x;
        float y = (p.z + 9f + gridSize.y/2) / gridSize.y;
        if (x<0 || x>1 || y<0 || y>1) {
            return null;
        }

        int xPos = Mathf.RoundToInt(12 * x);
        int yPos = Mathf.RoundToInt(22 * y);
        

        return grid[xPos, yPos];

    }


}
