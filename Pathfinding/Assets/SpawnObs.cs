using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObs : MonoBehaviour
{
    public GameObject Obs1;
    public GameObject Obs2;
    public GameObject Obs3;
    public GameObject Obs4;
    public GameObject Obs5;
    private GameObject[] Obs = new GameObject[5];
    static public List<Vector3> positionsOnPlane = new List<Vector3>();
    static public List<Vector3> positionsOnLevelOne = new List<Vector3>();
    static public List<Vector3> positionsOnLevelTwo = new List<Vector3>();
    public float distanceBetweenObjects;
    private float maxX;
    private float minX;
    private float maxZ;
    private float minZ;
    int count = 0;
    public SpawnNpc spawnNpc;
    public GridGeneratorForPlaneOne grid0;
    public GridGeneratorForLevelOne grid1;
    public GridGeneratorForLevelTwo grid2;

    public GameObject s;
    public GameObject t;
    // Start is called before the first frame update
    void Awake()
    {
        Obs[0] = Obs1;
        Obs[1] = Obs2;
        Obs[2] = Obs3;
        Obs[3] = Obs4;
        Obs[4] = Obs5;
        Spawn();
        grid0.SetGrid(); 
        grid1.SetGrid();  
        grid2.SetGrid();
        spawnNpc.SpawnNpcs();

        
        
    }

    void Spawn() {
        maxX = 10f;
        minX = -8;
        maxZ = 10f;
        minZ = -29f;
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 p = new Vector3(x, 1f, z);
        positionsOnPlane.Add(p);

        p = new Vector3(-45f, 1f, -2);
        positionsOnLevelOne.Add(p);

        p = new Vector3(-57f, 21f, -22);
        positionsOnLevelTwo.Add(p);

        
        for (int j = 0;j<20;j++ ){
            float i = Random.Range(0,30);
            if (i <= 10f) {
                maxX = 10f;
                minX = -8;
                maxZ = 10f;
                minZ = -29f;
                SpawnOnPlane();
                if (count==7) {
                    break;
                }
            } 
            else if (i <= 20f) {
                maxX = -41f;
                minX = -60;
                maxZ = 11f;
                minZ = -28f;
        
                SpawnOnLevelOne();
                if (count==7) {
                    break;
                }
            
            }

            else if (i <= 30f) {
                maxX = -56f;
                minX = -79;
                maxZ = 11f;
                minZ = -27f;

                SpawnOnLevelTwo();
                if (count==7) {
                    break;
                }
            }
       
        }
        foreach (Vector3 position in positionsOnPlane)
        {
            GameObject ObsToSpawn = chooseRandom();
            Instantiate(ObsToSpawn, position, Quaternion.identity);
        }
        foreach (Vector3 position in positionsOnLevelOne)
        {
            GameObject ObsToSpawn = chooseRandom();
            Instantiate(ObsToSpawn, position, Quaternion.identity);
        }
        foreach (Vector3 position in positionsOnLevelTwo)
        {
            GameObject ObsToSpawn = chooseRandom();
            Instantiate(ObsToSpawn, position, Quaternion.identity);
        }
     
            
            
        
    }

    void SpawnOnPlane() {
        
      
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 p = new Vector3(x, 1f, z);
        int j = 0;
        while(j<=positionsOnPlane.Count-1){
                 
            if ((p-positionsOnPlane[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects){
                if (j==positionsOnPlane.Count-1){
                    positionsOnPlane.Add(p);
                    count++;
              
                }
                j++;
            }
            else {
                return;
            }
        }    
  
        

    }

    void SpawnOnLevelOne() {
        Vector3 transport1 = new Vector3(-59f, 3f, -21f);
        Vector3 transport2 = new Vector3(-59f, 3f, 3f);
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 p = new Vector3(x, 1f, z);
        int j = 0;
        while(j<=positionsOnLevelOne.Count-1){
                 
            if ((p-positionsOnLevelOne[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects && (p-transport1).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects &&(p-transport2).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects){
                if (j==positionsOnLevelOne.Count-1){
                    positionsOnLevelOne.Add(p);
                    count++;
                  
                }
                j++;
            }
            else {
                
                return;
            }
        }    
       
        

    }

    void SpawnOnLevelTwo() {
        Vector3 transport1 = new Vector3(-73f, 20f, -21f);
        Vector3 transport2 = new Vector3(-73f, 20f, 3f);
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 p = new Vector3(x, 21f, z);
        int j = 0;
        while(j<=positionsOnLevelTwo.Count-1){
                 
            if ((p-positionsOnLevelTwo[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects && (p-transport1).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects &&(p-transport2).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects){
                if (j==positionsOnLevelTwo.Count-1){
                    positionsOnLevelTwo.Add(p);
                    count++;
                   
                }
                j++;
            }
            else {
                
                return;
            }
        }    

        
        

    }

    GameObject chooseRandom() {
        float rand = Random.Range(0,50);
        if (rand < 10f) {
            return Obs[0];
        }

        else if (rand < 20f) {
            return Obs[1];
        }

        else if (rand < 30f) {
            return Obs[2];
        }

        else if (rand < 40f) {
            return Obs[3];
        }

        else{
            return Obs[4];
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
