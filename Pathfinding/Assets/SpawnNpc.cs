using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNpc : MonoBehaviour
{

    public int NumberToSpawn;
    public GameObject Npc;
    private List<Vector3> positions = new List<Vector3>();
    float maxX, minX, maxZ, minZ;
    int count = 0;
    float distance = 4.5f;
    public static int pathing;
    public static int repath;

    public static int abandon;
    public static double totalTime;
    float startTime;
    void Start() {
        startTime = 0f;
    }

    public void SpawnNpcs()
    {
        Spawn();
        
    }

    public void Spawn() {
    
        for (int i=0; i < NumberToSpawn * 5; i++) {
        
            float f = Random.Range(0f,60f);
            if (f<=10f) {
                SpawnNpcOnPlane();
                if (count == NumberToSpawn) {
                    break;
                }
            }

            else if (f<=20f) {
                SpawnNpcOnSlopeOne();
                if (count == NumberToSpawn) {
                    break;
                }
            }

            else if (f<=30f) {
                SpawnNpcOnSlopeTwo();
                if (count == NumberToSpawn) {
                    break;
                }
            }

            else if (f<=40f) {
                SpawnNpcOnBridge();
                if (count == NumberToSpawn) {
                    break;
                }
            }

            else if (f<=50f) {
                SpawnNpcOnLevelOne();
                if (count == NumberToSpawn) {
                    break;
                }
            }

            else {
                SpawnNpcOnLevelTwo();
                if (count == NumberToSpawn) {
                    break;
                }
            }
        }

        foreach (Vector3 p in positions)
        {
           
            
            GameObject NewNpc = Npc;
            Color NewColor = new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f));
 
            MeshRenderer gameObjectRenderer = NewNpc.GetComponent<MeshRenderer>();
 
            Material newMaterial = new Material(Shader.Find("Standard"));
 
            newMaterial.color = NewColor;
            newMaterial.SetFloat("_Glossiness", 1f);;
            gameObjectRenderer.material = newMaterial;
            Instantiate(NewNpc, p, Quaternion.identity);
            
        }
    }

    public void SpawnNpcOnPlane() {
        maxX = 12f;
        minX = -13f;
        maxZ = 13f;
        minZ = -30f;
        Vector3 p = new Vector3(Random.Range(minX, maxX), 3f, Random.Range(minZ, maxZ));
        if (IsOccupiedOnPlane(p)) {
            positions.Add(p);
        
            count++;
        }
    }

    public void SpawnNpcOnSlopeOne() {
        maxX = -13f;
        minX = -50f;
        float x = Random.Range(minX, maxX);
        float y = (x + 11f) * (-18f / 37f) + 3f;
        Vector3 p = new Vector3(x, y, 6f);
        if (IsOccupiedOnPlane(p)) {
            positions.Add(p);
      
            count++;
        }

    }

    public void SpawnNpcOnSlopeTwo() {
        maxX = -13f;
        minX = -49f;
        float x = Random.Range(minX, maxX);
        float y = (x + 11f) * (-18f / 37f) + 3f;
        Vector3 p = new Vector3(x, y, -23f);
        if (IsOccupiedOnPlane(p)) {
            positions.Add(p);
          
            count++;
        }

    }

    public void SpawnNpcOnBridge() {
        maxX = -13f;
        minX = -34f;
        float x = Random.Range(minX, maxX);
        Vector3 p = new Vector3(x, 3f, -9f);
        if (IsOccupiedOnPlane(p)) {
            positions.Add(p);

            count++;
        }
    }

    public void SpawnNpcOnLevelOne() {
        maxX = -39f;
        minX = -69f;
        maxZ = 13f;
        minZ = -30f;
        Vector3 p = new Vector3(Random.Range(minX, maxX), 3f, Random.Range(minZ, maxZ));
        if (IsOccupiedOnLevelOne(p)) {
            positions.Add(p);
          
            count++;
        }
    }

    public void SpawnNpcOnLevelTwo() {
        maxX = -52f;
        minX = -83f;
        maxZ = 13f;
        minZ = -30f;
        Vector3 p = new Vector3(Random.Range(minX, maxX), 21.5f, Random.Range(minZ, maxZ));
        if (IsOccupiedOnLevelTwo(p)) {
            positions.Add(p);
     
            count++;
        }
    }

    public bool IsOccupiedOnPlane(Vector3 p) {
        foreach (Vector3 position in SpawnObs.positionsOnPlane)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        foreach (Vector3 position in positions)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }
        return true;
    }

    public  bool IsOccupiedOnLevelOne(Vector3 p) {
        Vector3 transport1 = new Vector3(-59f, 3f, -21f);
        Vector3 transport2 = new Vector3(-59f, 3f, 3f);
        foreach (Vector3 position in SpawnObs.positionsOnLevelOne)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        foreach (Vector3 position in positions)
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
        return true;
    }

    public bool IsOccupiedOnLevelTwo(Vector3 p) {
        Vector3 transport1 = new Vector3(-73f, 21f, -21f);
        Vector3 transport2 = new Vector3(-73f, 21f, 3f);
        foreach (Vector3 position in SpawnObs.positionsOnLevelTwo)
        {
            if ((position-p).sqrMagnitude <= distance * distance) {
                return false;
            }
        }

        foreach (Vector3 position in positions)
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
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 120f) {
       
            Debug.Log(pathing);
            Debug.Log(repath);
            Debug.Log(totalTime/pathing);
            Debug.Log(abandon);
        }
    }
}
