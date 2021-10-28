using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLose : MonoBehaviour
{
    public GameObject player;
    public GameObject winningText;
    public GameObject losingText;
    private Vector3 destination = new Vector3(4f, 40f, 47f);
    public static bool win = false;

    public static bool lose = false;
    void Update()
    {
        //Lose if player falls off the cliff
        if (player.transform.position.y < 25f && !lose)
        {
            Debug.Log("You lose!");
            Instantiate(losingText, new Vector3(-1f, 12f, 135), Quaternion.identity);
            player.transform.position = new Vector3(-1f, 3f, 116f);
            lose = true;

        }
        //Win if the maze is completed and player reaches the last row
        else if (!win && player.transform.position.y == 30f && Bullet.treesDestroyed >= 15 && player.transform.position.z > 27.25f)
        {
            Debug.Log("You win!");
            win = true;
            Instantiate(winningText, new Vector3(2.01f, 48.39f, 56.53f), Quaternion.identity);
            player.transform.position = destination;

        }

        //Lose if player falls onto an unompleted maze
        else if (!lose && player.transform.position.y == 30f && player.transform.position.x >= -3.7f && player.transform.position.x <= 13f && Bullet.treesDestroyed < 15)
        {
            Debug.Log("You lose!");
            Instantiate(losingText, new Vector3(-1f, 12f, 135), Quaternion.identity);
            player.transform.position = new Vector3(-1f, 3f, 116f);
            lose = true;


        }
        //If player jump off after they win.
        else if (win && player.transform.position.y < 35f && !lose)
        {
            Debug.Log("You lose!");
            Instantiate(losingText, new Vector3(-1f, 12f, 135), Quaternion.identity);
            player.transform.position = new Vector3(-1f, 3f, 116f);
            lose = true;
        }
    }
}
