using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public GameObject tree;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTrees();
    }



    void GenerateTrees()
    {
        float x = 0f;
        float z = 0f;
        for (int i = 0; i < 30; i++)
        {
            x = Random.Range(-39f, 64f);
            z = Random.Range(-100f, -20.2f);
            Vector3 pos = new Vector3(x, 40.623f, z);
            Instantiate(tree, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
