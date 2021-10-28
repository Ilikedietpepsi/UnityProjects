using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RisingAir : MonoBehaviour
{
    public Text text;
    public static int airSpeed;
    private int count;
   
    private int maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 4;
        airSpeed = Random.Range(0, maxSpeed);
        text.text = "RisingAir0 speed: " + airSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if (count % 200 == 0) {
            airSpeed = Random.Range(0, maxSpeed);
            Debug.Log(airSpeed);
            text.text = (string) "RisingAir0 speed: " + airSpeed;
        }
    }
}
