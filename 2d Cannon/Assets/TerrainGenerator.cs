using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private LineRenderer terrain;
    public int amplitude = 30;
    static public int width;
    static public int count = 100;
    
    private float interval;
    static public float height;
    List<LineRenderer> noises = new List<LineRenderer>(); 
    // Start is called before the first frame update
    void Start()
    {
        width = 3;
        terrain = GetComponent<LineRenderer>();
        terrain.positionCount = 36865;
        //First part 8192
        
        interval = (float)width / 8192;
        for (int i = 0; i <=8192; i++) {
            terrain.SetPosition(i, new Vector2(i*interval, 0));
        }
        
        //Uphill 8192
        System.Random rand = new System.Random();
        height = (float)rand.NextDouble() * 4f + 1.5f;
        interval = 2f / 8192;
        float k = height / 2f;
        float b = 0 - k * 3f;
        for (int i = 8192; i <=16384; i++) {
            terrain.SetPosition(i, new Vector2(3+(i-8192)*interval, (3+(i-8192)*interval)*k +b));
        }
        
        //Mountain top 4096
        interval = 1f / 4096;
        for (int i = 16384; i <=20480; i++) {
            terrain.SetPosition(i, new Vector2(5+(i-16384)*interval, height));
        }

        //Downhill 8192
        interval = 2f / 8192;
        k *= -1f;
        b = height - k * 6f;
        for (int i = 20480; i <=28672; i++) {
            terrain.SetPosition(i, new Vector2(6+(i-20480)*interval, (6+(i-20480)*interval)*k +b));
        }
        
        //last part
        interval = 6f / 8192;
        for (int i = 28672; i <=36864; i++) {
            terrain.SetPosition(i, new Vector2(8f+(i-28672)*interval, 0));
        }

        terrain = midpoint(terrain, 0, 8192);
        terrain = midpoint(terrain, 8192, 16384);
        terrain = midpoint(terrain, 16384, 20480);
        terrain = midpoint(terrain, 20480, 28672);
        terrain = midpoint(terrain, 28672, 36864);
        
    }

    LineRenderer midpoint(LineRenderer terrain, int leftIndex, int rightIndex) {
        int mid = (rightIndex + leftIndex) / 2;
        if (rightIndex - leftIndex <= 1) {
            return terrain;
        }
        System.Random rand = new System.Random();
        float height = (float)rand.NextDouble() * 0.1f - 0.05f;
        Vector2 cur = terrain.GetPosition(mid);
        terrain.SetPosition(mid, new Vector2(cur.x, cur.y + height));
        terrain = lineup(terrain, leftIndex, rightIndex, mid);
        terrain = midpoint(terrain, leftIndex, mid);
        terrain = midpoint(terrain, mid, rightIndex);
        return terrain;


    }

    LineRenderer lineup(LineRenderer terrain, int left, int right, int mid) {
        float k1;
        float k2;
        float b1;
        float b2;
        Vector2 leftPos = terrain.GetPosition(left);
        Vector2 midPos = terrain.GetPosition(mid);
        Vector2 rightPos = terrain.GetPosition(right);
        k1 = (float) (midPos.y - leftPos.y) / (midPos.x - leftPos.x);
        b1 = (float) leftPos.y - k1 * leftPos.x;
 

        k2 = (float) (midPos.y - rightPos.y) / (midPos.x - rightPos.x) * 1f;
        b2 = (float) rightPos.y - k2 * rightPos.x;
  
        for (int i = left; i < mid; i++) {
            Vector2 cur = terrain.GetPosition(i);
            terrain.SetPosition(i, new Vector2(cur.x, cur.x * k1 + b1));
        }

        for (int i = mid; i < right; i++) {
            Vector2 cur = terrain.GetPosition(i);
            terrain.SetPosition(i, new Vector2(cur.x, cur.x * k2 + b2));
        }

        return terrain;

    }

    


  
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
