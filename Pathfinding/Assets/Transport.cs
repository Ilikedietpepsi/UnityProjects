using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    public GameObject Npc;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("checkForTransport", 2f);
    }

    void checkForTransport() {
        Transform transform = GetComponent<Transform>();
        transform.position = new Vector3(-142f, 60f, -36f);
        if (transform.position.x == -142f && transform.position.y >= 59f && transform.position.z == -36f) {
            Debug.Log("Trans!");
            Invoke("Respawn1", 1f);
            Destroy(gameObject);
            
            
            
        }
    }

    void Respawn1() { 
        Debug.Log("Respawn");
        Instantiate(Npc, new Vector3(-156f, -24f, -76f), Quaternion.identity);
    }

    void Respawn2() {
        Instantiate(Npc, new Vector3(-120f, 16.7f, -3.6f), Quaternion.identity);
    }

    void Respawn3() {
        Instantiate(Npc, new Vector3(-142f, 59f, -36f), Quaternion.identity);
    }

    void Respawn4() {
        Instantiate(Npc, new Vector3(-142f, 59f, -3.6f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
   
    }
}
