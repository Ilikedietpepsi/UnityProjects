using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public bool noObstacle;
    public Vector3 nodePosition;
    public Node parent = null;

    public int g = 9999;
    public int f = 9999;

    public Node(int _x, int _y, bool _noObstacle, Vector3 _nodePosition) {
        x = _x;
        y = _y;
        noObstacle = _noObstacle;
        nodePosition = _nodePosition;
    }
}
