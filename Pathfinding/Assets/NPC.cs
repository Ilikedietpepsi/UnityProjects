using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Vector3 destination = Vector3.zero;
    private Vector3 transportDestination = Vector3.zero;
    float distance = 4.5f;
    public float speed;
    float clock;
    int curNode = 0;
    static Vector3 curNodePos;
    AStarPF astar;
    RRTPF rrt;
    Node[] path;
    Vector3 startPosition;
    public LayerMask npcMask;
    public LayerMask waitingAreaMask;
    public bool AStar;
    public bool RRT;
    private bool wantToTransport = false;
    private int blocking= 0;

    int timer = 0;


 

    void Start() {
        path = new Node[0];
        astar = new AStarPF();
        rrt = new RRTPF(npcMask);
  
        Move();
    }
    void Move() {
        // setDestination();
        // while (!checkDestination(destination)) {
        //     setDestination();
        // }
        GeneratePath();
             
    }

    bool checkDestination(Vector3 p) {
        Vector3 transport1 = new Vector3(-59f, 3f, -21f);
        Vector3 transport2 = new Vector3(-59f, 3f, 3f);
        Vector3 transport3 = new Vector3(-73f, 21f, -21f);
        Vector3 transport4 = new Vector3(-73f, 21f, 3f);
        foreach (Vector3 position in SpawnObs.positionsOnPlane)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        foreach (Vector3 position in SpawnObs.positionsOnLevelOne)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        foreach (Vector3 position in SpawnObs.positionsOnLevelTwo)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        if ((transport1-p).sqrMagnitude <= distance * distance) {
            return false;
        }

        else if ((transport2-p).sqrMagnitude <= distance * distance) {
            return false;
        }

        else if ((transport3-p).sqrMagnitude <= distance * distance) {
            return false;
        }

        else if ((transport4-p).sqrMagnitude <= distance * distance) {
            return false;
        }

        return true;

    }

    void setDestination() {
        float y;
        if (Random.Range(0, 10) <=5 ) {
            y = 3f;
        }
        else {
            y = 21.5f;
        }

        if (y == 3f) {
            if (Random.Range(0, 10) <=5 ) {
                destination = new Vector3(Random.Range(-69f, -36f), 3f, Random.Range(-30f, 13f));
            }
            else {
                destination = new Vector3(Random.Range(-13f, 12f), 3f, Random.Range(-30f, 13f));
            }
        }

        else {
            destination = new Vector3(Random.Range(-83f, -50f), 21.5f, Random.Range(-30f, 13f));
        }
        
    }
    void ToPortal0() {
        Node node0 = astar.WhichNodeOnLevelTwo(new Vector3(-73f, 21.5f, -16f));
        if (node0.noObstacle && !Physics.CheckSphere(node0.nodePosition, 1.2f, npcMask)) {
            transform.position = node0.nodePosition;
            return;
        }
        Node node1 = astar.WhichNodeOnLevelTwo(new Vector3(-68f, 21.5f, -21f));
        if (node1.noObstacle && !Physics.CheckSphere(node1.nodePosition, 1.2f, npcMask)) {
            transform.position = node1.nodePosition;
            return;
        }
        Node node2 = astar.WhichNodeOnLevelTwo(new Vector3(-78f, 21.5f, -21f));
        if (node2.noObstacle && !Physics.CheckSphere(node2.nodePosition, 1.2f, npcMask)) {
            transform.position = node2.nodePosition;
            return;
        }
        Node node3 = astar.WhichNodeOnLevelTwo(new Vector3(-73f, 21.5f, -26f));
        if (node3.noObstacle && !Physics.CheckSphere(node0.nodePosition, 1.2f, npcMask)) {
            transform.position = node3.nodePosition;
            return;
        }
    }

    void ToPortal1() {
        Node node0 = astar.WhichNodeOnLevelTwo(new Vector3(-73f, 21.5f, -2f));
        if (node0.noObstacle && !Physics.CheckSphere(node0.nodePosition, 1.2f, npcMask)) {
            transform.position = node0.nodePosition;
            return;
        }
        Node node1 = astar.WhichNodeOnLevelTwo(new Vector3(-73f, 21.5f, 8f));
        if (node1.noObstacle && !Physics.CheckSphere(node1.nodePosition, 1.2f, npcMask)) {
            transform.position = node1.nodePosition;
            return;
        }
        Node node2 = astar.WhichNodeOnLevelTwo(new Vector3(-68f, 21.5f, 3f));
        if (node2.noObstacle && !Physics.CheckSphere(node2.nodePosition, 1.2f, npcMask)) {
            transform.position = node2.nodePosition;
            return;
        }
        Node node3 = astar.WhichNodeOnLevelTwo(new Vector3(-79f, 21.5f, 3f));
        if (node3.noObstacle && !Physics.CheckSphere(node3.nodePosition, 1.2f, npcMask)) {
            transform.position = node3.nodePosition;
            return;
        }
    }

    void ToPortal2() {
        Node node0 = astar.WhichNodeOnLevelOne(new Vector3(-56f, 3f, -27f));
    
        if (node0.noObstacle && !Physics.CheckSphere(node0.nodePosition, 1.2f, npcMask)) {
            transform.position = node0.nodePosition;
           
            return;
        }
        Node node1 = astar.WhichNodeOnLevelOne(new Vector3(-56f, 3f, -15f));
        if (node1.noObstacle && !Physics.CheckSphere(node1.nodePosition, 1.2f, npcMask)) {
            transform.position = node1.nodePosition;
            return;
        }
        Node node2 = astar.WhichNodeOnLevelOne(new Vector3(-52f, 3f, -21f));
        if (node2.noObstacle && !Physics.CheckSphere(node2.nodePosition, 1.2f, npcMask)) {
            transform.position = node2.nodePosition;
            return;
        }
        Node node3 = astar.WhichNodeOnLevelOne(new Vector3(-64f, 3f, -21f));
        if (node3.noObstacle && !Physics.CheckSphere(node3.nodePosition, 1.2f, npcMask)) {
            transform.position = node3.nodePosition;
            return;
        }
    }

    void ToPortal3() {
        Node node0 = astar.WhichNodeOnLevelOne(new Vector3(-65f, 3f, 3f));
        if (node0.noObstacle && !Physics.CheckSphere(node0.nodePosition, 1.2f, npcMask)) {
            transform.position = node0.nodePosition;
            return;
        }
        Node node1 = astar.WhichNodeOnLevelOne(new Vector3(-53f, 3f, 3f));
        if (node1.noObstacle && !Physics.CheckSphere(node1.nodePosition, 1.2f, npcMask)) {
            transform.position = node1.nodePosition;
            return;
        }
        Node node2 = astar.WhichNodeOnLevelOne(new Vector3(-59f, 3f, 9f));
        if (node2.noObstacle && !Physics.CheckSphere(node2.nodePosition, 1.2f, npcMask)) {
            transform.position = node2.nodePosition;
            return;
        }
        Node node3 = astar.WhichNodeOnLevelOne(new Vector3(-59f, 3f, -2f));
        if (node3.noObstacle && !Physics.CheckSphere(node3.nodePosition, 1.2f, npcMask)) {
            transform.position = node3.nodePosition;
            return;
        }
    }

    void CheckPortal() {
        
        if (Physics.CheckSphere(transform.position, 1f, waitingAreaMask) && wantToTransport) {//If NPC is at the waiting area and wants to transport
        
        
            if (transform.position.z < -9f && transform.position.y == 21.5f) {
               
               
                gameObject.SetActive(false);
             
                    
                ToPortal2();
                gameObject.SetActive(true);
                GeneratePath();
                return;

                    
            }

            else if (transform.position.z >= -9f && transform.position.y == 21.5f) {
             
               
                gameObject.SetActive(false);
             
                    
                ToPortal3();
                gameObject.SetActive(true);
                GeneratePath();
                return;
            }

            else if (transform.position.z < -9f && transform.position.y == 3f) {
                gameObject.SetActive(false);
             
                    
                ToPortal0();
                gameObject.SetActive(true);
                GeneratePath();
                return;
            }

            else {
                gameObject.SetActive(false);
                ToPortal1();
                gameObject.SetActive(true);
                GeneratePath();
                return;
            }
        }
    }

    void GeneratePath() {
        SpawnNpc.pathing++;
        clock = 0;
        curNode = 0;
        if (destination == Vector3.zero) {
            setDestination();
            while(!checkDestination(destination)) {
                setDestination();
            }
        }
        else {
            if (!wantToTransport) {
                SpawnNpc.repath++;
            }
            
        }

        
        
        if ((transform.position.y == 3f && transform.position.x <= -36f && destination.y == 21.5f) ||  (transform.position.y == 21.5f && destination.y == 3f && destination.x <= -36f) ) {
            
            wantToTransport = true; //If NPC wants to go from level one to level two(or the other way around), set the wantToTransport flag to true;
        }
        else {
            wantToTransport = false;
        }
        List<Node> p = new List<Node>();
        if (AStar) {
            float a = Time.realtimeSinceStartup;
   
            p = astar.FindPath(transform.position, destination);
       
            SpawnNpc.totalTime += (Time.realtimeSinceStartup-a);
        }
        else if(RRT) {
            rrt.tree = new List<Node>();
            double a = Time.realtimeSinceStartup;
            p = rrt.FindPath(transform.position, destination);
           
            SpawnNpc.totalTime += (Time.realtimeSinceStartup-a);
        }
        if (p!=null) {
    
            startPosition = transform.position;
            path = p.ToArray();
        }

        else {
            path = new Node[0];
        }
        
        
    }

    void CheckBlocking() {
        gameObject.layer = 9;
        if (Physics.CheckSphere(path[curNode].nodePosition, 1f, npcMask)) {
            SpawnNpc.abandon++;
            
            blocking++;
            // if (curNode > 0) {
            //     transform.position = Vector3.Lerp(startPosition, path[curNode-1].nodePosition, clock*3);
            // }

            if (blocking > 2) {
                destination = Vector3.zero;
            }
            
            gameObject.layer = 8;
            GeneratePath();  
            
            
             
        }

        else {
            blocking = 0;
        }
    

    
        gameObject.layer = 8;
    }

    void RePath() {
        if (timer == 20) {
            timer = 0;
            destination = Vector3.zero;
            GeneratePath();
        }
    }

    void MoveToNextNode() {
        if (transform.position != path[curNode].nodePosition) {
            transform.position = Vector3.Lerp(startPosition, path[curNode].nodePosition, clock);
            // transform.position += (path[curNode].nodePosition - transform.position) * Time.deltaTime * speed;
                
        }

        else {
            curNode++;
            if (curNode >= path.Length) {
                curNode = 0;
                path = new Node[0];
              
                destination = Vector3.zero;
   
              
                
            }
            else {
                clock = 0;
                startPosition = transform.position;
            }
                    
                
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        
        if (path.Length != 0) {
            clock += Time.deltaTime * speed;
            CheckBlocking();
            CheckPortal();
            if (path.Length != 0) {
                MoveToNextNode();

            }
                
        }

        else {
            timer++;
            RePath();
        }
    }
}
