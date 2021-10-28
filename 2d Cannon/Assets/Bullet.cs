using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private Vector2 velocity;
    private float speed = Cannon.firePower;
    
    private float gravity = 2f;
    private float negY;
    private float newX;
    private float newY;
    private Vector2 direction;
    bool readyToCollide = true;

    public void Setup(Vector2 dir) {
        this.velocity = dir;
       
    }


    bool CheckCollision() {
        float k;
        float b;
      
        float angle1;
        float angle2;
        if ( transform.position.x < 14f && transform.position.x > 8f ) {
            if (transform.position.y <= 0.15) {
                readyToCollide = true;
                velocity = new Vector2(0.999f * velocity.x, -0.5f * velocity.y);
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
    
                return true;
            }
        }

        else if (transform.position.x <= 8f && transform.position.x > 6.15f) {
            k = -TerrainGenerator.height / 2f;
            b = 0f - 8f * k;
            angle1 = Mathf.Atan( (k - velocity.y / velocity.x) / (1 + k * velocity.y / velocity.x) );
            angle2 = Mathf.Atan(-k);
            float velocityIn = Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
            if (transform.position.y <= transform.position.x * k + b + 0.15f / Mathf.Cos(angle2)) {
                // //collide
                readyToCollide = true;
                Reflection(k);
                    
                return true;
                
               
            }

           
        }

        else if (transform.position.x <= 6.15f && transform.position.x > 4.9f) {
            if (transform.position.y <= TerrainGenerator.height + 0.08f) {
                //collide
                if (readyToCollide) {  //readyToCollide prevents continuous colliding
                    readyToCollide = false;
                    velocity = new Vector2(velocity.x * 0.999f, -0.6f * velocity.y);
                    transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
                
                    return true;
                }
                return false;
            }
            else {
                readyToCollide = true;
            }
        }

        else if (transform.position.x <= 5f && transform.position.x > 3f) {
            k = TerrainGenerator.height / 2f;
            b = 0f - 3f * k;
            angle1 = -Mathf.Atan( (k - velocity.y / velocity.x) / (1 + k * velocity.y / velocity.x) ); ////fix this!!!!
            angle2 = Mathf.Atan(k);
            
            float velocityIn = Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
            if (transform.position.y <= transform.position.x * k + b + 0.15f / Mathf.Cos(angle2)) {
                readyToCollide = true;
                Reflection(k);
                
            }
        }

        else if (transform.position.x <= 3f && transform.position.x > 0f) {
            if (transform.position.y <= 0.3) {
                readyToCollide = true;
                velocity = new Vector2(0.999f * velocity.x, -0.6f * velocity.y);
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
    
                return true;
            }
        }

    

        else if (transform.position.x <=0.15f) {
            if (readyToCollide) {  //readyToCollide prevents continuous colliding
                    readyToCollide = false;
                    velocity = new Vector2(velocity.x * -0.6f, 0.999f * velocity.y);
                    transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
                
                    return true;
                }
            return false;
        }

        return false;
    }

    void Reflection(float planeSlope) {
        float velocityIn = Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
        float planeAngle;
        float angleIn;
        float xToPlane;
        float yToPlane;
        float velocityOut;
        float angleToGround;

        //Up plane, coming in from bottom left
        if (planeSlope >= 0 && velocity.y / velocity.x < planeSlope && velocity.y / velocity.x > -1f / planeSlope) {
            planeAngle = Mathf.Atan(planeSlope);
            angleIn = Mathf.Atan( (planeSlope - velocity.y / velocity.x) / (1 + planeSlope * velocity.y / velocity.x) );
            xToPlane = 0.999f * velocityIn * Mathf.Cos(angleIn);
            yToPlane = -0.6f * velocityIn * Mathf.Sin(angleIn);
            velocityOut = Mathf.Sqrt(xToPlane * xToPlane + yToPlane * yToPlane);
            if (angleIn >= 3.14159265f / 2 - planeAngle) { 
                angleToGround= 3.14159265f - angleIn - planeAngle;
                velocity = new Vector2(-velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }

            else {
                angleToGround= angleIn + planeAngle;
                velocity = new Vector2(velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }

        }
        //up plane, coming in from top right
        else if ((planeSlope > 0f && velocity.y / velocity.x > planeSlope) || (planeSlope > 0f && velocity.y / velocity.x < -1/planeSlope)) {
            planeAngle = Mathf.Atan(planeSlope);            
            angleIn = -Mathf.Atan( (planeSlope - velocity.y / velocity.x) / (1 + planeSlope * velocity.y / velocity.x) );
            xToPlane = 0.999f * velocityIn * Mathf.Cos(angleIn);
            yToPlane = -0.6f * velocityIn * Mathf.Sin(angleIn);
            velocityOut = Mathf.Sqrt(xToPlane * xToPlane + yToPlane * yToPlane);
            if (angleIn <= planeAngle) { 
                angleToGround= planeAngle - angleIn;
                velocity = new Vector2(-velocityOut * Mathf.Cos(angleToGround), -velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);

            }
            else {
                angleToGround= angleIn - planeAngle;
                velocity = new Vector2(-velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            
            }
                        

        }
        //down plane, coming in from bottom right
        else if (planeSlope < 0f && velocity.y / velocity.x < -1f / planeSlope && velocity.y / velocity.x > planeSlope) {
            planeAngle = Mathf.Atan(-planeSlope);
            angleIn = -Mathf.Atan( (planeSlope - velocity.y / velocity.x) / (1 + planeSlope * velocity.y / velocity.x) );
     
            
            xToPlane = 0.999f * velocityIn * Mathf.Cos(angleIn);
            yToPlane = -0.6f * velocityIn * Mathf.Sin(angleIn);
            velocityOut = Mathf.Sqrt(xToPlane * xToPlane + yToPlane * yToPlane);
            if (angleIn < 3.14159265f / 2 - planeAngle) {
                angleToGround = angleIn + planeAngle;
                velocity = new Vector2(-velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }

            else {
                angleToGround = 3.14159265f - angleIn - planeAngle;
                velocity = new Vector2(velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }
        }
        //down plane, coming in from top left
        else if ((planeSlope < 0f && velocity.y / velocity.x > -1f / planeSlope) || (planeSlope < 0f && velocity.y / velocity.x < planeSlope)) {
            planeAngle = Mathf.Atan(-planeSlope);
            angleIn = Mathf.Atan( (planeSlope - velocity.y / velocity.x) / (1 + planeSlope * velocity.y / velocity.x) );
            xToPlane = 0.999f * velocityIn * Mathf.Cos(angleIn);
            yToPlane = -0.6f * velocityIn * Mathf.Sin(angleIn);
            velocityOut = Mathf.Sqrt(xToPlane * xToPlane + yToPlane * yToPlane);
            if (angleIn < planeAngle) {
                angleToGround = planeAngle - angleIn;
                velocity = new Vector2(velocityOut * Mathf.Cos(angleToGround), -velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }

            else {
                angleToGround = angleIn - planeAngle;
                velocity = new Vector2(velocityOut * Mathf.Cos(angleToGround), velocityOut * Mathf.Sin(angleToGround));
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
            }


        }
    }

    bool BallCollider() {
        float distanceSqure;
        float contactPlane;
        float k;
       
        foreach (Transform bullet in Cannon.aliveBullets)
        {
            if (bullet != null && transform != bullet) {
                
                distanceSqure = (float)(transform.position.x - bullet.position.x) * (transform.position.x - bullet.position.x) + (transform.position.y - bullet.position.y) * (transform.position.y - bullet.position.y);
                if (distanceSqure <= 0.3f) { //IF CONTACT
                    readyToCollide = true;
                    if (Mathf.Abs(transform.position.y - bullet.position.y) <= 0.00001) {
                        velocity = new Vector2(-0.6f * velocity.x, 0.999f * velocity.y);
                        transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
                    }
                    k = (bullet.position.y - transform.position.y) / (bullet.position.x - transform.position.x);
                    contactPlane = -1f / k;
                    Reflection(contactPlane);
                    return true;
                    
                }
                
            }
            
        }
        return false;
    }

    void checkDestroy(Vector2 position) {
        if (gameObject != null) {
            if (Mathf.Abs(velocity.x) <=0.0001f && Mathf.Abs(velocity.y) <=0.0001f) {
                Destroy(gameObject);
                Cannon.aliveBullets.Remove(transform);
            }
            if (transform.position.x>9f && velocity.x >=0f) {
                Destroy(gameObject);
                Cannon.aliveBullets.Remove(transform);
            }
            else if (transform.position.x > 15f || transform.position.y < 0f || transform.position.y > 8f) {
                Destroy(gameObject);
                Cannon.aliveBullets.Remove(transform);
            }

        }
    
    }



    void FixedUpdate()
    {
        Vector2 position = transform.position;
        if (gameObject != null) {
            if (!CheckCollision() && !BallCollider()) {
                velocity = new Vector2(0.9999f * velocity.x, velocity.y - gravity * Time.deltaTime);
                transform.position = new Vector2(transform.position.x + speed * velocity.x * Time.deltaTime , transform.position.y + speed * velocity.y * Time.deltaTime);
                CheckCollision();
            }
        }
        
        checkDestroy(position);
        

    }
}
