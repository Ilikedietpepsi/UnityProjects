using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPF
{
    Node[,] grid0;
    Node[,] grid1;
    Node[,] grid2;


    public AStarPF() {
        grid0 = GridGeneratorForPlaneOne.grid;
        grid1 = GridGeneratorForLevelOne.grid;
        grid2 = GridGeneratorForLevelTwo.grid;
        
    }


    public List<Node> FindPath(Vector3 start, Vector3 dest) {
        if (start.y == 3f && dest.y == 3f) {
            if (start.x >= -13f && dest.x <= -36f) {
                return PlaneOneToLevelOne(start, dest);
            }

            else if (start.x >= -13f && dest.x >= -13f) {
                return PlaneOneToPlaneOne(start, dest);
            }

            else if (start.x <= -36f && dest.x <= -36f) {
                return LevelOneToLevelOne(start, dest);
            }

            else if (start.x <= -36f && dest.x >= -13f) {
                return LevelOneToPlaneOne(start, dest);
            }

            else if (start.x < -13f && start.x > -36f && dest.x <= -36f) {
                return BridgeToLevelOne(start, dest);
            }

            else if (start.x < -13f && start.x > -36f && dest.x >= -13f) {
                return BridgeToPlaneOne(start, dest);
            }
        }

        else if (start.y == 3f && dest.y >= 21.5f) {
            if (start.x <= -36f && dest.x <= -36f) {
                return LevelOneToLevelTwo(start, dest);
            }

            else if (start.x >= -13f && dest.x <= -36) {
                return PlaneOneToLevelTwo(start, dest);
            }
        }

        else if (start.y >= 21.5f && dest.y == 3f) {
            if (start.x <= -36f && dest.x <= -36f) {
                return LevelTwoToLevelOne(start, dest);
            }

            else if (start.x <= -36f && dest.x >= -13) {
                return LevelTwoToPlaneOne(start, dest);
            }
        }

        else if (start.y >= 21.5f && dest.y >= 21.5f) {
            return LevelTwoToLevelTwo(start, dest);
        }

        else if (start.y > 3f && start.y < 21.5 && dest.x >= -13.5f && dest.y == 3f) {
            return SlopeToPlaneOne(start, dest);
        }

        else if (start.y > 3f && start.y < 21.5 && dest.y >= 21.5f) {
            return SlopeToLevelTwo(start, dest);
        }

      

        return null;
    }

    Node FindMinF(List<Node> list) {
        Node min = list[0];
        foreach (Node node in list)
        {
            if (node.f < min.f) {
                min = node;
            }
        }

        return min;
    }

    List<Node> GetNb(Node node, Node[,] grid) {
        List<Node> nb = new List<Node>();

        for (int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++) {
                if (i==0 && j==0) {
                    continue;
                }

                else {
                    if (node.x + i >= 0 && node.x + i < grid.GetLength(0) && node.y + j >= 0 && node.y + j < grid.GetLength(1)) {
                        
                        nb.Add(grid[node.x + i, node.y + j]); 
                        
                        
                    }
                }
            }
            
        }

        return nb;


    }

    List<Node> GetPath(Node s, Node t) {
        List<Node> path = new List<Node>();
        Node cur = t;
        while (cur != s) {
            path.Add(cur);
            cur = cur.parent;
           
        }
        path.Reverse();
        return path;
        
  
    }

    int GetDistance(Node n1, Node n2) {
        int x = Mathf.Abs(n1.x - n2.x);
        int y = Mathf.Abs(n1.y - n2.y);

        if (x > y) {
            return 14 * y + 10 * (x-y);
        }

        else {
            return 14 * x + 10 * (y-x);
        }
    }

    public Node WhichNodeOnPlaneOne(Vector3 p) {
        Vector2 gridSize = new Vector2(26, 46);
        
        float x = (p.x + gridSize.x/2) / gridSize.x;
        float y = (p.z + 9f + gridSize.y/2) / gridSize.y;

        int xPos = Mathf.RoundToInt(12 * x);
        int yPos = Mathf.RoundToInt(22 * y);
    

        return grid0[xPos, yPos];
    }

    public Node WhichNodeOnLevelOne(Vector3 p) {
        Vector2 gridSize = new Vector2(36, 46);
        
        float x = (p.x + 55f + gridSize.x/2) / gridSize.x;
        float y = (p.z + 9f + gridSize.y/2) / gridSize.y;

        int xPos = Mathf.RoundToInt(17 * x);
        int yPos = Mathf.RoundToInt(22 * y);
        
     

        return grid1[xPos, yPos];
    }

    public Node WhichNodeOnLevelTwo(Vector3 p) {
        Vector2 gridSize = new Vector2(36, 46);
        
        float x = (p.x + 68f + gridSize.x/2) / gridSize.x;
        float y = (p.z + 9f + gridSize.y/2) / gridSize.y;

        int xPos = Mathf.RoundToInt(17 * x);
        int yPos = Mathf.RoundToInt(22 * y);

        return grid2[xPos, yPos];
    }

    void InitializeGrid0() {
        for (int i = 0; i < 13; i++) {
            for (int j = 0; j < 23; j++) {
                grid0[i,j].parent = null;
                grid0[i,j].g = 9999;
                grid0[i,j].f = 9999;
            }
        }
    }

    void InitializeGrid1() {
        for (int i = 0; i < 18; i++) {
            for (int j = 0; j < 23; j++) {
                grid1[i,j].parent = null;
                grid1[i,j].g = 9999;
                grid1[i,j].f = 9999;
            }
        }
    }

    void InitializeGrid2() {
        for (int i = 0; i < 18; i++) {
            for (int j = 0; j < 23; j++) {
                grid2[i,j].parent = null;
                grid2[i,j].g = 9999;
                grid2[i,j].f = 9999;
            }
        }
    }


    public List<Node> PlaneOneToPlaneOne(Vector3 start, Vector3 dest) {
        Node s = WhichNodeOnPlaneOne(start);
        Node t = WhichNodeOnPlaneOne(dest);

        List<Node> fringe = new List<Node>(); 
        InitializeGrid0();

        s.g = 0;
        s.f = GetDistance(s, t);
        fringe.Add(s);

        while (fringe.Count > 0) {
            Node cur = FindMinF(fringe);
            fringe.Remove(cur);
           
            if (cur == t) {
                return GetPath(s, cur);
            }
            List<Node> nbs = GetNb(cur, grid0);
            foreach (Node nb in nbs)
            {
                if (!nb.noObstacle) {
                    continue;
                }

                int d = cur.g + GetDistance(cur, nb);
                if (d < nb.g) {
                    if (!fringe.Contains(nb)) {
                        fringe.Add(nb);
                    }
                    nb.g = d;
                    nb.f = nb.g + GetDistance(nb, t);
                    nb.parent = cur;
                }
            }


        }

        return null;
    }
    public List<Node> LevelOneToLevelOne(Vector3 start, Vector3 dest) {
        Node s = WhichNodeOnLevelOne(start);
        Node t = WhichNodeOnLevelOne(dest);

        List<Node> fringe = new List<Node>(); 
        InitializeGrid1();

        s.g = 0;
        s.f = GetDistance(s, t);
        fringe.Add(s);

        while (fringe.Count > 0) {
            Node cur = FindMinF(fringe);
            fringe.Remove(cur);
            
            if (cur == t) {
                return GetPath(s, cur);
            }
            List<Node> nbs = GetNb(cur, grid1);
            foreach (Node nb in nbs)
            {
                if (!nb.noObstacle) {
                    continue;
                }

                int d = cur.g + GetDistance(cur, nb);
                if (d < nb.g) {
                    if (!fringe.Contains(nb)) {
                        fringe.Add(nb);
                    }
                    nb.g = d;
                    nb.f = nb.g + GetDistance(nb, t);
                    nb.parent = cur;
                }
            }


        }
        return null;

    }
    public List<Node> LevelTwoToLevelTwo(Vector3 start, Vector3 dest) {
        Node s = WhichNodeOnLevelTwo(start);
        Node t = WhichNodeOnLevelTwo(dest);

        List<Node> fringe = new List<Node>(); 
        InitializeGrid2();
        s.g = 0;
        s.f = GetDistance(s, t);
        fringe.Add(s);

        while (fringe.Count > 0) {
            Node cur = FindMinF(fringe);
            fringe.Remove(cur);
           
            if (cur == t) {
                return GetPath(s, cur);
              
            }

            foreach (Node nb in GetNb(cur, grid2))
            {
                if (!nb.noObstacle) {
                    continue;
                }

                int d = cur.g + GetDistance(cur, nb);
                if (d < nb.g) {
                    if (!fringe.Contains(nb)) {
                        fringe.Add(nb);
                    }
                    nb.g = d;
                    nb.f = nb.g + GetDistance(nb, t);
                    nb.parent = cur;
                }
            }


        }
        return null;

    }
    public List<Node> PlaneOneToLevelOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = PlaneOneToPlaneOne(start, new Vector3(-13f, 3f ,-9f));
        Node newStart = WhichNodeOnLevelOne(new Vector3(-38f, 3f, -9f));
   
        List<Node> path2 = LevelOneToLevelOne(newStart.nodePosition, dest);

        List<Node> path3 = new List<Node>();
        if (path1==null || path2 == null) {
            return null;
        }
        float x = -15f;
        while (x>-38f) {
            Node node = new Node(0,0,true,new Vector3(x,3f,-9f));
            x-=2f;
            path3.Add(node);
        }
        
        foreach (Node node in path3)
        {
            path1.Add(node);
        }
        foreach (Node node in path2)
        {
            path1.Add(node);
        }

        return path1;
        
    }

    public List<Node> LevelOneToPlaneOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = LevelOneToLevelOne(start, new Vector3(-38f, 3f ,-9f));
        List<Node> path2 = PlaneOneToPlaneOne(new Vector3(-13f, 3f, -9f), dest);
        List<Node> path3 = new List<Node>();
        float x = -36f;
        if (path1==null || path2 == null) {
            return null;
        }
        while (x<-13f) {
            Node node = new Node(0,0,true,new Vector3(x,3f,-9f));
            x+=2f;
            path3.Add(node);
        }
        
        foreach (Node node in path3)
        {
            path1.Add(node);
        }
        foreach (Node node in path2)
        {
            path1.Add(node);
        }

        return path1;
    }
   

    

    public List<Node> LevelOneToLevelTwo(Vector3 start, Vector3 dest) {

        List<Node> path = new List<Node>();
        if (start.z < -9f) {
            path = LevelOneToLevelOne(start, new Vector3(-59f, 3f, -21f));
        }

        else {
            path = LevelOneToLevelOne(start, new Vector3(-59f, 3f, 3f));
        }
        return path;
    }

    public List<Node> LevelTwoToLevelOne(Vector3 start, Vector3 dest) {
       
        List<Node> path = new List<Node>();
        if (start.z < -9f) {
            path = LevelTwoToLevelTwo(start, new Vector3(-73f, 21.5f, -21f));
        }

        else {
            path = LevelTwoToLevelTwo(start, new Vector3(-73f, 21.5f, 3f));
        }
        return path;
    }

    public List<Node> PlaneOneToLevelTwo(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        List<Node> path3 = new List<Node>();
        if (start.z <= -9f) {
            path1 = PlaneOneToPlaneOne(start, new Vector3(-13f, 3f ,-23f));
            Node newStart = WhichNodeOnLevelTwo(new Vector3(-50f, 21.5f, -23));
            path2 = LevelTwoToLevelTwo(newStart.nodePosition, dest);
            if (path1==null || path2 == null) {
                return null;
            }
            float x = -15f;
            while(x>-50f) {
                Node node = new Node(0,0,true,new Vector3(x, (x + 11f) * (-18f / 37f) + 3f, -23f));
                path3.Add(node);
                x-=2f;
            }
        }

        else {
            path1 = PlaneOneToPlaneOne(start, new Vector3(-13f, 3f ,6f));
            Node newStart = WhichNodeOnLevelTwo(new Vector3(-50f, 21.5f, 6f));
            path2 = LevelTwoToLevelTwo(newStart.nodePosition, dest);
            float x = -15f;
            if (path1==null || path2 == null) {
                return null;
            }
            while(x>-50f) {
                Node node = new Node(0,0,true,new Vector3(x, (x + 11f) * (-18f / 37f) + 3f, 6f));
                path3.Add(node);
                x-=2f;
            }
        }
        
        foreach (Node node in path3)
        {
            path1.Add(node);
        }
        foreach (Node node in path2)
        {
            path1.Add(node);
        }
        return path1;
    }

    public List<Node> LevelTwoToPlaneOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        List<Node> path3 = new List<Node>();

        if (start.z <= -9f) {
            path1 = LevelTwoToLevelTwo(start, new Vector3(-50f, 21.5f, -23f));
            Node newStart = WhichNodeOnPlaneOne(new Vector3(-13f, 3f ,-23f));
            path2 = PlaneOneToPlaneOne(newStart.nodePosition, dest);
            float x = -48f;
            while(x<-13f) {
                Node node = new Node(0,0,true,new Vector3(x, (x + 11f) * (-18f / 37f) + 3f, -23f));
                path3.Add(node);
                x+=2f;
            }
        }

        else {
            path1 = LevelTwoToLevelTwo(start, new Vector3(-50f, 21.5f, 6f));
            Node newStart = WhichNodeOnPlaneOne(new Vector3(-13f, 3f ,6f));
            path2 = PlaneOneToPlaneOne(newStart.nodePosition, dest);
            float x = -48f;
            while(x<-13f) {
                Node node = new Node(0,0,true,new Vector3(x, (x + 11f) * (-18f / 37f) + 3f, 6f));
                path3.Add(node);
                x+=2f;
            }
        }
        if (path1==null || path2 == null) {
            return null;
        }
        foreach (Node node in path3)
        {
            path1.Add(node);
        }

        foreach (Node node in path2)
        {
            path1.Add(node);
        }
        return path1;
    }

    public List<Node> SlopeToPlaneOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        float startX = start.x;
        while (startX < -13f) {
            float z;
            if(start.z >-9f) {
                z = 6f;
            }
            else {
                z = -23f;
            }
            Node node = new Node(0,0,true,new Vector3(startX, (startX + 11f) * (-18f / 37f) + 3f, z));
            path1.Add(node);
            startX+=2f;
        }
        if (start.z <= -9f) {
            
            path2 = PlaneOneToPlaneOne(new Vector3(-13f, 3f ,-23f), dest);
            if (path2 == null) {
                return null;
            }
            foreach (Node node in path2)
            {
                path1.Add(node);
            }
         

            return path1;

        }

        else {
    
            path2 = PlaneOneToPlaneOne(new Vector3(-13f, 3f ,6f), dest);
            if (path2 == null) {
                return null;
            }
            foreach (Node node in path2)
            {
                path1.Add(node);
            }
            
            return path1;
        }

        

  
    }

    public List<Node> SlopeToLevelTwo(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        float startX = start.x;
        while (startX > -50f) {
            float z;
            if(start.z >-9f) {
                z = 6f;
            }
            else {
                z = -23f;
            }
            Node node = new Node(0,0,true,new Vector3(startX, (startX + 11f) * (-18f / 37f) + 3f, z));
            path1.Add(node);
            startX-=2f;
        }
        if (start.z <= -9f) {
         
            path2 = LevelTwoToLevelTwo(new Vector3(-50f, 21.5f, -23), dest);
        }

        else {
            
            path2 = LevelTwoToLevelTwo(new Vector3(-50f, 21.5f, 6), dest);
        }
        if (path2 == null) {
            return null;
        }
        foreach (Node n in path2)
        {
            path1.Add(n);
        }
        
        return path1;
    }

    public List<Node> BridgeToPlaneOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        float startX = start.x;
        while (startX < -13f) {
            Node node = new Node(0,0,true,new Vector3(startX, 3f, -9f));
            path1.Add(node);
            startX+=2f;
        }

        path2 = PlaneOneToPlaneOne(new Vector3(-13f, 3f, -9f), dest);
        if (path2 == null) {
            return null;
        }
        foreach (Node n in path2)
        {
            path1.Add(n);
        }

        return path1;
    }

    public List<Node> BridgeToLevelOne(Vector3 start, Vector3 dest) {
        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        float startX = start.x;
        while (startX > -36f) {
            Node node = new Node(0,0,true,new Vector3(startX, 3f, -9f));
            path1.Add(node);
            startX-=2f;
        }

        path2 = LevelOneToLevelOne(new Vector3(-36f, 3f, -9f), dest);
        if (path2 == null) {
            return null;
        }
        foreach (Node n in path2)
        {
            path1.Add(n);
        }

        return path1;
    }

    


}
