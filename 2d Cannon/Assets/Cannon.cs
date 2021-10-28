using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    public Transform ball;
    public LineRenderer barrel;
    private float length;
    private Vector2 barrel1;
    static public float firePower = 50f;
    static public List<Transform> aliveBullets = new List<Transform>();
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Firepower: " + firePower;
        barrel1 = barrel.GetPosition(1);
        Vector2 barrel0 = barrel.GetPosition(0);
        length = Mathf.Sqrt( (barrel1.y - barrel0.y) * (barrel1.y - barrel0.y) +  (barrel1.x - barrel0.x) * (barrel1.x - barrel0.x));

    }

    void Fire() {
        Transform bullet = Instantiate(ball, barrel.GetPosition(0), Quaternion.identity);
        aliveBullets.Add(bullet);
        Vector2 direction = (barrel.GetPosition(0) - barrel.GetPosition(1)).normalized;
        bullet.GetComponent<Bullet>().Setup(direction);
    }


    void Moveup() {
        Vector2 cur = barrel.GetPosition(0);
        if (cur.x >= 13.8) {
            barrel.SetPosition(0, new Vector2(barrel1.x, barrel1.y + length));
            return;
        }
        float newY = cur.y + 0.1f;
        float newX = Mathf.Sqrt(length * length -( newY - barrel1.y) *  ( newY - barrel1.y)) * -1f + barrel1.x;
       
        barrel.SetPosition(0, new Vector2(newX, newY));

    }

    void Movedown() {
        Vector2 cur = barrel.GetPosition(0);
        if (cur.x <= 12.88645) {
            barrel.SetPosition(0, new Vector2(barrel1.x - length, barrel1.y ));
            return;
        }
        float newY = cur.y - 0.1f;
        float newX = Mathf.Sqrt(length * length -( newY - barrel1.y) *  ( newY - barrel1.y)) * -1f + barrel1.x;
     
        barrel.SetPosition(0, new Vector2(newX, newY));

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("space"))                                                      // Getting a key press from the user. 
        {
            Fire();
        }
        if (Input.GetKeyUp("up")) {
            Moveup();
        }

        if (Input.GetKeyUp("down")) {
            Movedown();
        }

        if (Input.GetKeyUp("left")) {
            if (firePower >=0) {
                firePower = firePower - 5f;
                text.text = "firepower: " + firePower;
            }
        }

        if (Input.GetKeyUp("right")) {
            firePower = firePower + 5f;
            text.text = "Firepower: " + firePower;
        }

        
    }
}
