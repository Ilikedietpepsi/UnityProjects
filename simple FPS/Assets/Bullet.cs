using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static int treesDestroyed = 0;
    public static bool hasBulletInScene = false;

    void OnTriggerEnter(Collider other)
    {
        //If the bullet hits a tree
        if (other.gameObject.transform.tag == "Tree")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            hasBulletInScene = false;
            treesDestroyed++;
        }

        //If the bullet hit anything else
        else
        {
            Destroy(this.gameObject);
            hasBulletInScene = false;
        }


    }



}
