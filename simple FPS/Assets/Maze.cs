using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell {
        public bool visited;
        public GameObject up; 
        public GameObject left; 
        public GameObject right; 
        public GameObject down; 
    }
    public GameObject wall;
    public float wallLength = 2.5f;
    public int xSize = 6;
    public int ySize = 15; 

   
    public Cell[] cells;
    public int currentCell = 0;
    private int cellsNum;
    private int visitedCells;
    private bool startSignal = false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backTrack;
    private int currentNumberOfRows = 0;
    
    private List<GameObject> walls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //Create a 6*15 grid
        CreatWalls();
       
    }

    void CreatWalls() {
        
        Vector3 startPosition = new Vector3((-xSize/2)+wallLength/2, 0f, (-ySize/2)+wallLength/2);
        Vector3 myPos = startPosition;
        GameObject tempWall;

        //Creating X-axis
        for (int i = 0; i < ySize; i++) {
            for (int j = 0; j <= xSize; j++) {
                myPos = new Vector3(startPosition.x + (j*wallLength)-wallLength/2, 30.0f, startPosition.z + (i*wallLength)-wallLength/2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                
                walls.Add(tempWall);
                tempWall.SetActive(false); //First hide the walls
            }
        }

        //Creating Y-axis
        for (int i = 0; i <= ySize; i++) {
            for (int j = 0; j < xSize; j++) {
                myPos = new Vector3(startPosition.x + (j*wallLength), 30.0f, startPosition.z + (i*wallLength)-wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
          
                walls.Add(tempWall);
                tempWall.SetActive(false); //First hide the walls
            }
        }
        cells = new Cell[ySize*xSize];

        CreateCells();
    }

    void CreateCells() {
        //keeping track of the cells we visited
        lastCells = new List<int>(); 
        lastCells.Clear();
        
        int row = 0;
        int firstSouthWall = (xSize+1) * ySize;
        int firstNorthWall = firstSouthWall + xSize;
      

        //Asign walls to cell
        for (int i = 0; i < cells.Length;i++) {
            if (i % xSize == 0 && i != 0) {
                row++;
            }

            cells[i] = new Cell();
            cells[i].left = walls[i + row];
            cells[i].right = walls[i + 1 + row];
            cells[i].up = walls[i + firstNorthWall];
            cells[i].down = walls[i + firstSouthWall];
        }
        cellsNum = xSize * ySize;
        createMaze();
    }

    void createMaze() {
        while (visitedCells < cellsNum) {
            if (startSignal) {
                findNeighbour();
                //if the neighbor is not visited yet, break the wall 
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true) {
                    BreakWall(currentCell, currentNeighbour);
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);//keeping track of the cells we visited, in case we need to back track
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0) {
                        backTrack = lastCells.Count - 1; 
                    }
                }
            }

            //First iteration
            else {
                currentCell = Random.Range(0,cellsNum);
                cells[currentCell].visited = true;
                visitedCells++;
                startSignal = true;
            }
        }

    }

    void BreakWall(int neighbour, int current) {
        //If the neighbour is above the current cell
        if (neighbour-current==xSize) {
            Destroy(cells[current].up);
        }
        //If the neighbour is to the right of the current cell
        else if (neighbour-current==1) {
            Destroy(cells[current].right);
        }
        
        //If the neighbour is beneath the current cell
        else if (neighbour-current==-xSize) {
            Destroy(cells[current].down);
        }

        //If the neighbour is to the left of the current cell
        else if (neighbour-current==-1) {
            Destroy(cells[current].left);
        }
    }

    void findNeighbour() {
        int count = 0;
        int[] neighbours = new int[4];
        
        
        //adding right cell
        if (currentCell + 1 < cellsNum && (currentCell + 1) % xSize != 0) {
            if (cells[currentCell+1].visited == false) {
                neighbours[count] = currentCell+1;
                count++;
            }
        }

        //adding left cell
        if (currentCell - 1 >= 0 && currentCell % xSize  != 0) {
            if (cells[currentCell-1].visited == false) {
                neighbours[count] = currentCell-1;
                count++;
            }
        }

        //adding upper cell
        if (currentCell + xSize < cellsNum) {
            if (cells[currentCell+xSize].visited == false) {
                neighbours[count] = currentCell+xSize;
                count++;
            }
        }

        //adding lower cell
        if (currentCell - xSize >= 0) {
            if (cells[currentCell-xSize].visited == false) {
                neighbours[count] = currentCell-xSize;
                count++;
            }
        }

        //If a neighbor is found, return a random neighbor
        if (count != 0) {
            int theChosenNeighbour = Random.Range(0,count);
            currentNeighbour = neighbours[theChosenNeighbour];
        }

        //If all neighbors were all visited or no neighbor was found, then backtrack
        else {
            if (backTrack > 0) {
                currentCell = lastCells[backTrack];
                backTrack--;
            }
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Bullet.treesDestroyed > currentNumberOfRows && currentNumberOfRows < 15) {
            //reveal one more row of the matrix, when another tree is destroyed 
           for (int i = currentNumberOfRows*6; i < (currentNumberOfRows+1)*6; i++){
               if (cells[i].up != null) {
                   cells[i].up.SetActive(true);
               }
               if (cells[i].down != null) {
                   cells[i].down.SetActive(true);
               }
               if (cells[i].right != null) {
                   cells[i].right.SetActive(true);
               }
               if (cells[i].left != null) {
                   cells[i].left.SetActive(true);
               }
               

              
               
           }
           currentNumberOfRows++;

           
        }
        
    }
}
