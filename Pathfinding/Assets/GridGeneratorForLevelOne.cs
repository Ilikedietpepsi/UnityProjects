using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneratorForLevelOne : MonoBehaviour
{
    public static Node[,] grid;
    int rows, cols;
    public Vector2 gridSize;
    public float nodeSize;
    public LayerMask ObstacleMask;
    public LayerMask WaitingAreaMask;
    public GameObject player;

    


    public void SetGrid() {
        grid = new Node[18,23];
        Vector3 start = new Vector3(-55,3,-9) - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int x = 0; x < 18; x++) {
            for (int y = 0; y < 23; y++) {
                Vector3 p = start + Vector3.right * (x * nodeSize + (float)nodeSize / 2f) + Vector3.forward * (y * nodeSize + (float)nodeSize /2f);
                bool empty = true;
                if (Physics.CheckSphere(p, nodeSize-0.2f,ObstacleMask)) {
                    empty = false;
                }

                // if (Physics.CheckSphere(p, nodeSize-0.2f,WaitingAreaMask)) {
                //     empty = false;
                // }

                grid[x,y] = new Node(x, y, empty, p);

            }
        }
    }

    public Node WhichNode(Vector3 p) {
        
        float x = (p.x + 55f +gridSize.x/2) / gridSize.x;
        float y = (p.z + 9f + gridSize.y/2) / gridSize.y;

        if (x<0 || x>1 || y<0 || y>1) {
            return null;
        }
        int xPos = Mathf.RoundToInt(17 * x);
        int yPos = Mathf.RoundToInt(22 * y);

        return grid[xPos, yPos];

    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(new Vector3(-55,0,-9), new Vector3(gridSize.x,1,gridSize.y));
        if (grid != null) {
         
            foreach (Node n in grid)
            {
                if (n.noObstacle) {
                    Gizmos.color = Color.green;
                }
                else if (!n.noObstacle){
                    Gizmos.color = Color.red;
                }

                if (n.x == 0 && n.y == 22) {
                    Gizmos.color = Color.black;
                }
             
                Gizmos.DrawCube(n.nodePosition, Vector3.one * (nodeSize-.1f));
            }
        }
    }
}
