using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRTPF
{
    Node[,] grid0;
    Node[,] grid1;
    Node[,] grid2;
    Node sourceNode;
    public List<Node> tree = new List<Node>();
    LayerMask npc;


    public RRTPF(LayerMask _npc) {
        grid0 = GridGeneratorForPlaneOne.grid;
        grid1 = GridGeneratorForLevelOne.grid;
        grid2 = GridGeneratorForLevelTwo.grid;
        npc = _npc;
        

        
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

        else if (start.y > 3f && start.y < 21.5 && dest.y == 21.5f) {
            return SlopeToLevelTwo(start, dest);
        }

        return null;

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

    void CheckAndAddPointOnPlaneOne(Vector3 newNode) {
        Node newOne = WhichNodeOnPlaneOne(newNode);
    
        float shortest = 99999f;
        float dist;
        bool added = false;
        if (!newOne.noObstacle) {
            return;
        }
        foreach (Node old in tree)
        {
            if (old == newOne) {
                break;
            }

            if (old.nodePosition.x < -13f || old.nodePosition.y != 3f) {
                continue;
            }
            RaycastHit hit;
            if (ValidBetweenPointsOnPlaneOne(old.nodePosition, newNode)) {
                added = true;
                dist = (newNode - old.nodePosition).sqrMagnitude;
                if (dist < shortest) {
                    shortest = dist;
                    newOne.parent = old;
                }

            }
        }
        if (added) {
            tree.Add(newOne);
        
        }

        return;
    }

    bool ValidBetweenPointsOnPlaneOne(Vector3 node, Vector3 newNode) {
        int i=0;
        Vector3 step = (newNode - node).normalized;
        while((newNode-node).sqrMagnitude>8f) {
            i++;
            Node n = WhichNodeOnPlaneOne(node);
            if (!n.noObstacle) {
                return false;
            }
            node += step;
            if (i>100) {
                Debug.Log("175");
                return true;
            }
        }

        Node dest = WhichNodeOnPlaneOne(newNode);
        if (!dest.noObstacle) {
            return false;
        }
        
        return true;
    }

    void CheckAndAddPointOnLevelOne(Vector3 newNode) {
        Node newOne = WhichNodeOnLevelOne(newNode);
        float shortest = 99999f;
        float dist;
        bool added = false;
        if (!newOne.noObstacle) {
            return;
        }
        foreach (Node old in tree)
        {
            if (old == newOne) {
                break;
            }
            if (old.nodePosition.x > -36f || old.nodePosition.y != 3f) {
                continue;
            }
            RaycastHit hit;
            if (ValidBetweenPointsOnLevelOne(old.nodePosition, newNode)) {
                dist = (newNode - old.nodePosition).sqrMagnitude;
                added = true;
                if (dist < shortest) {
                    shortest = dist;
                    newOne.parent = old;
            
                }

            }
        }

        if (added) {
            tree.Add(newOne);
        }

        return;
    }

    bool ValidBetweenPointsOnLevelOne(Vector3 node, Vector3 newNode) {
        Vector3 step = (newNode - node).normalized;
        int i=0;
        while((newNode-node).sqrMagnitude>8f) {
            i++;
            Node n = WhichNodeOnLevelOne(node);
            if (!n.noObstacle) {
                return false;
            }
            node += step;
            if (i>100) {
                Debug.Log("233");
                return true;
            } 
        }

        Node dest = WhichNodeOnLevelOne(newNode);
        if (!dest.noObstacle) {
            return false;
        }
        
      
        
        return true;
    }

    void CheckAndAddPointOnLevelTwo(Vector3 newNode) {
        Node newOne = WhichNodeOnLevelTwo(newNode);
        float shortest = 99999f;
        float dist;
        bool added = false;
        if (!newOne.noObstacle) {
            return;
        }
        foreach (Node old in tree)
        {
            if (old == newOne) {
                break;
            }
            if (old.nodePosition.y != 21.5f) {
                continue;
            }
            RaycastHit hit;
            if (ValidBetweenPointsOnLevelTwo(old.nodePosition, newNode)) {
                dist = (newNode - old.nodePosition).sqrMagnitude;
                added = true;
                if (dist < shortest) {
                    shortest = dist;
                    newOne.parent = old;
                    
                }

            }
        }
        if (added) {
            tree.Add(newOne);
        }

        return;
    }

    bool ValidBetweenPointsOnLevelTwo(Vector3 node, Vector3 newNode) {
        Vector3 step = (newNode - node).normalized;
        int i = 0;
        while((newNode-node).sqrMagnitude>8f) {
            i++;
            Node n = WhichNodeOnLevelTwo(node);
            if (!n.noObstacle) {
                return false;
            }
            node += step;
            if (i>100) {
                Debug.Log("292");
                return true;
            }
        }

        Node dest = WhichNodeOnLevelTwo(newNode);
        if (!dest.noObstacle) {
            return false;
        }
        
      
        
        return true;
    }

    List<Node> GetPath(Node s, Node t) {
        int j=0;
        List<Node> path = new List<Node>();
   
        Node cur = t;
        while (cur != s) {
            j++;
            path.Add(cur);

            cur = cur.parent;
            if (j>200) {
            
                Debug.Log("306");
                return null;
            }
           
        }
        path.Add(s);
        path.Reverse();
        Node[] roadmap = path.ToArray();
        path = new List<Node>();
        for (int i = 0; i<roadmap.Length-1; i++) {
            j = 0;
            Node curNode = roadmap[i];
            Node nextNode = roadmap[i+1];
            Vector3 current = curNode.nodePosition;
            Vector3 next = nextNode.nodePosition;
            path.Add(curNode);
            if ((next-current).sqrMagnitude >=8f) {
            
                Vector3 step = 2*((next - current).normalized);
                current +=step;
                while((next-current).sqrMagnitude >=8f) {
                    j++;
                    Node newNode = new Node(0,0,true,current);
                    path.Add(newNode);
                    current += step;
                    if (j>100) {
                      
                        Debug.Log("350");
                        return null;
                    }
                   
                } 
                


            }

            path.Add(nextNode);
            
        }
        
        return path;
        
  
    }

    Vector3 GeneratePointOnPlaneOne() {
        float x = Random.Range(-13f, 12f);
        float z = Random.Range(-30f, 13f);
        return new Vector3(x, 3f, z);
    }

    Vector3 GeneratePointOnLevelOne() {
        float x = Random.Range(-69f, -37f);
        float z = Random.Range(-30f, 13f);
        return new Vector3(x, 3f, z);
    }

    Vector3 GeneratePointOnLevelTwo() {
        float x = Random.Range(-83f, -50f);
        float z = Random.Range(-30f, 13f);
        return new Vector3(x, 21.5f, z);
    }

    List<Node> PlaneOneToPlaneOne(Vector3 start, Vector3 dest) {
        Node startNode  = WhichNodeOnPlaneOne(start);
        Node destNode = WhichNodeOnPlaneOne(dest);
        Vector3 newPoint = Vector3.zero;
        tree.Add(startNode);
        int i=0;
        
        while (!tree.Contains(destNode)) {
            i++;
            newPoint = GeneratePointOnPlaneOne();

            CheckAndAddPointOnPlaneOne(newPoint);
            CheckAndAddPointOnPlaneOne(dest);
            if (i>50) {
                // Debug.Log(start);
                // Debug.Log(dest);
                // Debug.Log("403");
                return null;
            }
        }

        return GetPath(startNode, destNode);


    }
    List<Node> LevelOneToLevelOne(Vector3 start, Vector3 dest) {
       
        Node startNode = WhichNodeOnLevelOne(start);
        Node destNode = WhichNodeOnLevelOne(dest);
        Vector3 newPoint = Vector3.zero;
        tree.Add(startNode);
        int i = 0;
        while (!tree.Contains(destNode)) {
            i++;
            newPoint = GeneratePointOnLevelOne();

            CheckAndAddPointOnLevelOne(newPoint);
            CheckAndAddPointOnLevelOne(dest);
            if (i>50) {
                // Debug.Log(start);
                // Debug.Log(dest);
                // Debug.Log("428");
                return null;
            }
        }

        return GetPath(startNode, destNode);
    }

    List<Node> LevelTwoToLevelTwo(Vector3 start, Vector3 dest) {
       
        Node startNode = WhichNodeOnLevelTwo(start);
        Node destNode = WhichNodeOnLevelTwo(dest);
        Vector3 newPoint = Vector3.zero;
        tree.Add(startNode);
        int i = 0;
        while (!tree.Contains(destNode)) {
            i++;
            newPoint = GeneratePointOnLevelTwo();

            CheckAndAddPointOnLevelTwo(newPoint);
            CheckAndAddPointOnLevelTwo(dest);
            if (i>50) {
                // Debug.Log("446");
                return null;
            }
        }

        return GetPath(startNode, destNode);
    }
    List<Node> PlaneOneToLevelOne(Vector3 start, Vector3 dest) {
       
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

    List<Node> LevelOneToPlaneOne(Vector3 start, Vector3 dest) {
       
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

    

    

    List<Node> PlaneOneToLevelTwo(Vector3 start, Vector3 dest) {
       
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

    List<Node> LevelTwoToPlaneOne(Vector3 start, Vector3 dest) {
       
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

    List<Node> LevelOneToLevelTwo(Vector3 start, Vector3 dest) {

        List<Node> path = new List<Node>();
        if (start.z < -9f) {
            path = LevelOneToLevelOne(start, new Vector3(-59f, 3f, -21f));
        }

        else {
            path = LevelOneToLevelOne(start, new Vector3(-59f, 3f, 3f));
        }
        return path;
    }

    List<Node> LevelTwoToLevelOne(Vector3 start, Vector3 dest) {
       
        List<Node> path = new List<Node>();
        if (start.z < -9f) {
            path = LevelTwoToLevelTwo(start, new Vector3(-73f, 21.5f, -21f));
        }

        else {
            path = LevelTwoToLevelTwo(start, new Vector3(-73f, 21.5f, 3f));
        }
        return path;
    }

    List<Node> BridgeToLevelOne(Vector3 start, Vector3 dest) {
       
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

    List<Node> BridgeToPlaneOne(Vector3 start, Vector3 dest) {
       
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

    List<Node> SlopeToPlaneOne(Vector3 start, Vector3 dest) {
       
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

    List<Node> SlopeToLevelTwo(Vector3 start, Vector3 dest) {
       
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










}
