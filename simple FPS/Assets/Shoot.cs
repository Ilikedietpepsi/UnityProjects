using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;

    private bool alreadyFired = false; //Whether there is already a bullet in the scence;
    public Camera gunCam;
    public Transform startPoint;
    private bool readyToShoot = true;
    public float pushForce = 1.0f;
    private bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //check if the shoot button is pressed and there are no other bullets in the scene
        if (!alreadyFired && readyToShoot && shooting && !Bullet.hasBulletInScene)
        {
            alreadyFired = true;
            Bullet.hasBulletInScene = true;
            PlayerShoot();
            alreadyFired = false;
        }
    }

    void PlayerShoot()
    {

        readyToShoot = false;
        Ray ray = gunCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check to see if the ray hit something
        Vector3 target;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
        }

        else
        {
            target = ray.GetPoint(100);
        }
        //Find the distance
        Vector3 direction = target - startPoint.position;
        //Spawn the bullet
        GameObject firedBullet = Instantiate(bullet, startPoint.position, Quaternion.identity);
        //Get the bullet to move towards the direction
        firedBullet.transform.forward = direction.normalized;
        firedBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * pushForce, ForceMode.Impulse);
        readyToShoot = true;

    }
}
