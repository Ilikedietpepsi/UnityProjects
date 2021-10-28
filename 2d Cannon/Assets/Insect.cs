using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    public GameObject point;
    public LineRenderer body;
    private Point[] positions;
    private GameObject[] points = new GameObject[11];
    private Vector2 center;
    private int insectsNum;
    private float wingLength = 0.4f;
    private float tipLength = 0.18f;
    private Vector2 velocity;

    public struct Point{
        public Vector2 posNow;
        public Vector2 posOld;

        public Point(Vector2 pos) {
            posNow = pos;
            posOld = pos;
        }
    }

    void Start() {
        
        Spawn();
    }

    void Spawn() {
        
        float x = Random.Range(0.4f,3f);
        float y;
        float k = TerrainGenerator.height / 2;
        float b = 0 - 2f * k;
        float angle1;
        float angle2;
        float centerLineSlope;
        
        positions = new Point[11];
        center = new Vector2(x, Random.Range(0.4f, 6f));
        positions[0] = new Point(center);
     
        x = Random.Range(center.x, center.x + wingLength);
        y = Mathf.Sqrt(wingLength * wingLength - (x - center.x) * (x - center.x));
        positions[1] = new Point(new Vector2(x, center.y + y));
        for (int i = 2; i < 5; i++) {
            angle1 = Mathf.Atan((positions[i-1].posNow.y - center.y) / (positions[i-1].posNow.x - center.x));
            angle2 = angle1 - 3.14159265f / 9;
            x = wingLength * Mathf.Cos(angle2);
            y = wingLength * Mathf.Sin(angle2);
            positions[i] = new Point(new Vector2(center.x + x, center.y + y));
        }

        angle1 = Mathf.Atan((positions[1].posNow.y - center.y) / (positions[1].posNow.x - center.x));
        angle2 = angle1 + (3.14159265f * 40f / 180f);
        x = tipLength * Mathf.Cos(angle2);
        y = tipLength * Mathf.Sin(angle2);
        positions[5] = new Point(new Vector2(center.x + x, center.y + y));
    
        centerLineSlope = (float)Mathf.Tan(angle1 + 3.14159265f / 3);
        b = (float)positions[0].posNow.y - centerLineSlope * positions[0].posNow.x;
        float perp = -1f / centerLineSlope;
        ////Get the reflection
        for (int i = 1; i < 6; i++) {
            float b1 = (float)positions[i].posNow.y - perp * positions[i].posNow.x;
            float midX = (b1 - b) / (centerLineSlope - perp);
            float midY = midX * perp + b1;
            positions[i+5] = new Point(new Vector2(2f * midX - positions[i].posNow.x, 2f * midY - positions[i].posNow.y));
       
        }
        ////
       


    }
   

    void Draw() {
        if (checkForOut()) {
           
            Spawn();
        }

        else {
            Vector3[] linePositions = new Vector3[19];
            linePositions[0] = (Vector3)positions[10].posNow;
            linePositions[1] = (Vector3)positions[0].posNow;
            linePositions[2] = (Vector3)positions[4].posNow;
            linePositions[3] = (Vector3)positions[3].posNow;
            linePositions[4] = (Vector3)positions[2].posNow;
            linePositions[5] = (Vector3)positions[1].posNow;
            linePositions[6] = (Vector3)positions[0].posNow;
            linePositions[7] = (Vector3)positions[9].posNow;
            linePositions[8] = (Vector3)positions[8].posNow;
            linePositions[9] = (Vector3)positions[7].posNow;
            linePositions[10] = (Vector3)positions[6].posNow;
            linePositions[11] = (Vector3)positions[0].posNow;
            linePositions[12] = (Vector3)positions[3].posNow;
            linePositions[13] = (Vector3)positions[2].posNow;
            linePositions[14] = (Vector3)positions[0].posNow;
            linePositions[15] = (Vector3)positions[8].posNow;
            linePositions[16] = (Vector3)positions[7].posNow;
            linePositions[17] = (Vector3)positions[0].posNow;
            linePositions[18] = (Vector3)positions[5].posNow;
            body.positionCount = 19;
            body.SetPositions(linePositions);
            // for(int i=0; i<11; i++) {
            //     if (points[i] != null) {
            //         Destroy(points[i]);
            //     }
            //     points[i] = Instantiate(point, positions[i].posNow, Quaternion.identity);
            
            // }
        }
        
        
        
        
        

    }

    bool checkForOut() {
        for (int i = 0; i < 11; i++) {
            
            if (positions[i].posNow.x > 12f) {
                return true;
            }

            

            else if (positions[i].posNow.y > 10f) {
             
                return true;
            }

            else if (positions[i].posNow.x >9f) {
                return true;
            }
        }
        return false;
    }

    void Move() {
        velocity = new Vector2(Random.Range(-0.005f, 0.005f), Random.Range(-0.002f,0.003f));
        Vector2 v = Vector2.zero;
        bool collide = false;

     


        
        foreach (Transform bullet in Cannon.aliveBullets)
        {
            for (int i = 0; i < 11; i++) {
                Point p = positions[i];
                if (bullet == null) {
                    continue;
                }
                float centerX = bullet.position.x;
                float centerY = bullet.position.y;
                float distSq = (float)(p.posNow.x - centerX) * (p.posNow.x - centerX) + (p.posNow.y - centerY) * (p.posNow.y - centerY);
                // If contact with a ball
                if (distSq <= 0.15f) {
                    collide = true;
                    v = new Vector2((p.posNow.x - centerX), (p.posNow.y - centerY));
                    // v = (new Vector2((p.posNow.x - centerX), (p.posNow.y - centerY))).normalized;
                    // Debug.Log(v.x);
                    // p.posNow = new Vector2(centerX + 0.35f* v.x, centerY + 0.35f * v.y);
                    p.posNow += 3f * v * Time.fixedDeltaTime;
                    positions[i] = p;
                    
                }
            }
                
            
        }
           
        for (int i=0;i<11;i++) {
            Point p = positions[i];
            if (p.posNow.x >=0.5f && p.posNow.x<=1f) {
                v = new Vector2(0f, RisingAir.airSpeed);
                p.posNow += 0.1f * v * Time.deltaTime;
                positions[i] = p;
                break;

            }

            if (p.posNow.x >=1.5f && p.posNow.x<=2f) {
                v = new Vector2(0f, RisingAir1.airSpeed);
                p.posNow += 0.1f * v * Time.deltaTime;
                positions[i] = p;
                break;
            }
            if (p.posNow.x >=2.5f && p.posNow.x<=3f) {
                v = new Vector2(0f, RisingAir2.airSpeed);
                p.posNow += 0.1f * v * Time.deltaTime;
                positions[i] = p;
                break;

            }
            
        }
       
    
        
        for (int i = 0; i < 11; i++) {
          
            Point p = positions[i];
            Vector2 displacement = p.posNow - p.posOld;
            p.posOld = p.posNow;
            p.posNow += displacement;
            p.posNow += velocity * Time.fixedDeltaTime; 
             
           
            if (p.posNow.x < 0f) {
                p.posNow.x = 0;
            }

            if (p.posNow.y < 0f) {
                p.posNow.y = 0f;
            }
            if (p.posNow.x >3f && p.posNow.x < 5f && p.posNow.y < (p.posNow.x-3f)*(TerrainGenerator.height / 2f)) {
                p.posNow.y = (p.posNow.x-3f)*(TerrainGenerator.height / 2f);
                p.posNow.x -=0.01f;
            }
            
            positions[i] = p;
        }
        
        

        Constraint();

        
        // for(int i = 0; i < 11; i++) {
        //     for (int j =0; j<11; j++) {
        //         ApplyConstraint(i , j);
        //         ApplyConstraint(j , i);
        //     }
        //    Constraint();    
    //    }

    

    }

    void Constraint() {
        
       

        ApplyConstraint(5,10);
        ApplyConstraint(0,5);
        ApplyConstraint(0,10);

        for(int j=1;j<10;j++) {
            ApplyConstraint(5,j);
            ApplyConstraint(5,j+1);
            ApplyConstraint(10,j);
            ApplyConstraint(10,j+1);
            ApplyConstraint(j,j+1);
        }

   
        ApplyConstraint(1,6);
        
        // for (int i = 1; i < 11; i++) {
        //     ApplyConstraint(0,i);
        // }
        // ApplyConstraint(1,2);
        // ApplyConstraint(1,3);
        // ApplyConstraint(1,4);
       

        // ApplyConstraint(6,7);
        // ApplyConstraint(6,8);
        // ApplyConstraint(6,9);

        
     
        // if (collidePos != -1) {
        //     for (int i = 0; i< 11; i++) {
        //         ApplyConstraint(collidePos, i);
        //         ApplyConstraint(i, collidePos);
        //     }
        // }

        // for(int i = 0; i < 11; i++) {
        //     for (int j =0; j<11; j++) {
        //         ApplyConstraint(i , j);
        //         ApplyConstraint(j , i);
        //     }
        // }
        
        // Constraint Body to tip
      
     
        // for (int i = 1; i < 11; i++) {
        //     ApplyConstraint(0, i);
        //     if (i==5 || i==10) {

        //         Point center = positions[0];
        //         Point nb = positions[i];
        //         float dist = (center.posNow - nb.posNow).magnitude;
        //         float error = Mathf.Abs(dist - tipLength);

        //         Vector2 dir = (center.posNow - nb.posNow).normalized;
                
        //         Vector2 change = dir * error;

        //         if (dist > tipLength) {
        //             center.posNow -= change * 0.5f;
        //             positions[0] = center;
        //             nb.posNow += change * 0.5f;
        //             positions[i] = nb;
        //         }

        //         else {
        //             center.posNow += change * 0.5f;
        //             positions[0] = center;
        //             nb.posNow -= change * 0.5f;
        //             positions[i] = nb;
        //         }
        //     }

        //     else {
        //         Point center = positions[0];
        //         Point nb = positions[i];
        //         float dist = (center.posNow - nb.posNow).magnitude;
        //         float error = Mathf.Abs(dist - wingLength);
        //         Vector2 dir = (100 * (center.posNow - nb.posNow)).normalized;
        //         Vector2 change = dir * error;
        //         if (dist > wingLength) {
        //             center.posNow -= change * 0.5f;
        //             positions[0] = center;
        //             nb.posNow += change * 0.5f;
        //             positions[i] = nb;
        //         }

        //         else {
        //             center.posNow += change * 0.5f;
        //             positions[0] = center;
        //             nb.posNow -= change * 0.5f;
        //             positions[i] = nb;
        //         }
        //     }
        // }

        // //Constraint tip to tip
        //  for (int i = 1; i < 11; i++) {
        //      for (int j =1; j < 11 ;j++) {
        //          ApplyConstraint(i, j);
        //          ApplyConstraint(j, i);
        //      }
             
        //     Point tip = positions[i];
        //     Point nb;
        //     float dist;
        //     float angle;
        //     float c;
        //     float error;
        //     Vector2 dir;
        //     Vector2 change;
        //     if (i==5 || i==10) {
        //         int nbPos;
        //         if (i==10) {
        //             nb = positions[5];
        //             nbPos = 5;
        //         }
        //         nb = positions[10];
        //         nbPos = 10;
        //         dist = (tip.posNow - positions[nbPos].posNow).magnitude;
        //         angle = (40f/180f) * 3.14159265f;
        //         c = CosLaw(angle, tipLength, tipLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100 * (tip.posNow - positions[nbPos].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[nbPos] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[nbPos] = nb;
        //         }
        //         int start = 1;
        //         int end = 5;
        //         if (i==10) {
        //             start = 6;
        //             end = 9;
        //         }
        //         for(int j = start; j < end; j++) {
        //             nb = positions[j];
        //             dist = (tip.posNow - positions[j].posNow).magnitude;
        //             angle = ((40f + (j-1) * 20)/180f) * 3.14159265f;
        //             c = CosLaw(angle, tipLength, wingLength);
        //             error = Mathf.Abs(dist - c);
        //             dir = (100 * (tip.posNow - positions[j].posNow)).normalized;
        //             change = dir * error;
        //             if (dist > c) {
        //                 tip.posNow -= change * 0.5f;
        //                 positions[i] = tip;
        //                 nb.posNow += change * 0.5f;
        //                 positions[j] = nb;
        //             }

        //             else {
        //                 tip.posNow += change * 0.5f;
        //                 positions[i] = tip;
        //                 nb.posNow -= change * 0.5f;
        //                 positions[j] = nb;
        //             }
        //         }
        //     }

        //     else if (i==1 || i==6) {
        //         nb = positions[i+4];
        //         dist = (tip.posNow - positions[i+4].posNow).magnitude;
        //         angle = (40f/180f) * 3.14159265f;
        //         c = CosLaw(angle, wingLength, tipLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100 *(tip.posNow - positions[i+4].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[i+4] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[i+4] = nb;
        //         }
        //     }

        //     else if (i<=4){
        //         nb = positions[i-1];
        //         dist = (tip.posNow - positions[i-1].posNow).magnitude;
        //         angle = (20f/180f) * 3.14159265f;
        //         c = CosLaw(angle, wingLength, wingLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100*(tip.posNow - positions[i-1].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[i-1] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[i-1] = nb;
        //         }

        //         nb = positions[i+5];
        //         dist = (tip.posNow - positions[i+5].posNow).magnitude;
        //         angle = ((120f+(i-1)*20f*2f) /180f) * 3.14159265f;
        //         c = CosLaw(angle, wingLength, wingLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100*(tip.posNow - positions[i+5].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[i+5] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[i+5] = nb;
        //         }
        //     }

        //     else {
        //         nb = positions[i-1];
        //         dist = (tip.posNow - positions[i-1].posNow).magnitude;
        //         angle = (20f/180f) * 3.14159265f;
        //         c = CosLaw(angle, wingLength, wingLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100*(tip.posNow - positions[i-1].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[i-1] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[i-1] = nb;
        //         }

        //         nb = positions[i-5];
        //         dist = (tip.posNow - positions[i-5].posNow).magnitude;
        //         angle = ((120f+(i-6)*20f*2f) /180f) * 3.14159265f;
        //         c = CosLaw(angle, wingLength, wingLength);
        //         error = Mathf.Abs(dist - c);
        //         dir = (100*(tip.posNow - positions[i-5].posNow)).normalized;
        //         change = dir * error;
        //         if (dist > c) {
        //             tip.posNow -= change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow += change * 0.5f;
        //             positions[i-5] = nb;
        //         }

        //         else {
        //             tip.posNow += change * 0.5f;
        //             positions[i] = tip;
        //             nb.posNow -= change * 0.5f;
        //             positions[i-5] = nb;
        //         }
        //     }

   

            
            
        //  }
    }

    void ApplyConstraint(int aPos, int nbPos) {
        float c = 0;
        Point a;
        Point nb;
        float dist;
        float error;
        if (aPos == nbPos) {
            return;
        }

        else {
            a = positions[aPos];
            nb = positions[nbPos];
            dist = (a.posNow - nb.posNow).magnitude;
            if (aPos == 0) {
                if(nbPos == 5 || nbPos == 10) {
                    c = tipLength;
         
                }

                else {
                    c = wingLength;
                }
            }

            else if (aPos == 5) {
                if (nbPos == 10) {
                    float angle = (40/180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, tipLength);
                }
                else if (nbPos == 0) {
                    c = tipLength;
                }

                else if (nbPos <= 4){
                    float angle = ((40f + (nbPos - 1) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else {
                    float angle = ((80f + (nbPos - 6) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }
            }

            else if (aPos == 10) {
                if (nbPos == 5) {
                    float angle = (40/180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, tipLength);
                }
                else if (nbPos == 0) {
                    c = tipLength;
                }

                else if (nbPos <= 4){
                    float angle = ((80f + (nbPos - 1) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else {
                    float angle = ((40f + (nbPos - 6) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }
            }

            else if (aPos <= 4) {
                if (nbPos == 0) {
                    c = wingLength;
                }

                else if (nbPos <= 4) {
                    float angle = (Mathf.Abs(nbPos - aPos) * 20f / 180f) * 3.14159265f;
                    c = CosLaw(angle, wingLength, wingLength);
                }

                else if (nbPos == 5) {
                    float angle = ((40f + (aPos - 1) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else if (nbPos == 10) {
                    float angle = ((80f + (aPos - 1) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else {
                    float angle = ((120f + (aPos - 1) * 20f + (nbPos - 6) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, wingLength, wingLength);
                }
            }

            else {
                if (nbPos == 0) {
                    c = wingLength;
                }

                else if (nbPos <=9 ) {
                    float angle = (Mathf.Abs(nbPos - aPos) * 20f / 180f) * 3.14159265f;
                    c = CosLaw(angle, wingLength, wingLength);
                }

                else if (nbPos == 5) {
                    float angle = ((80f + (aPos - 6) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else if (nbPos == 10) {
                    float angle = ((40f + (aPos - 6) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, tipLength, wingLength);
                }

                else {
                    float angle = ((120f + (aPos - 6) * 20f + (nbPos - 1) * 20f) / 180f) * 3.14159265f;
                    c = CosLaw(angle, wingLength, wingLength);
                }
            }
            error = Mathf.Abs(dist - c);
            Vector2 dir = (100*(a.posNow - nb.posNow)).normalized;
            Vector2 change = dir * error;

            if (dist > c) {
                a.posNow -= change * 0.5f;
                positions[aPos] = a;
                nb.posNow += change * 0.5f;
                positions[nbPos] = nb;
            }

            else {
                a.posNow += change * 0.5f;
                positions[aPos] = a;
                nb.posNow -= change * 0.5f;
                positions[nbPos] = nb;
            }
        }

    }

    float CosLaw(float angle, float a, float b) {
        float c = Mathf.Sqrt(a*a+b*b-2*a*b*Mathf.Cos(angle));
        return c;
    }

    void FixedUpdate() {
        
        Move();
        
       
    }

    // Update is called once per frame
    void Update()
    {
     
        Draw();
        
      

    }
}
