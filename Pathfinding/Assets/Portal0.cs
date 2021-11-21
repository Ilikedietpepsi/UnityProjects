using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal0 : MonoBehaviour
{
    public static int waitingNum;
    public static Vector3 position;
    public static List<GameObject> waitList;

    // Start is called before the first frame update
    void Start()
    {
        waitingNum = 0;
        position = new Vector3(-73f, 21.5f, -21);
        waitList = new List<GameObject>();
    }

    


    // Update is called once per frame
    void Update()
    {
   
    }
}
