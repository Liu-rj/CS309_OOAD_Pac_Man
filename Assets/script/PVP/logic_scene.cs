using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class logic_scene : MonoBehaviour
{
    public GameObject wall;
    public GameObject food;
    public GameObject strong_tool;
    public static int here_tot = 0;
    public GameObject player;
    public GameObject player_temp;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject player1;
    public GameObject player2;

    private string[,] _maze;
    private GameObject[] _players;
    public GameObject[] ghosts = new GameObject[4];
    public int[] i_temp = new int[4];
    public int[] j_temp = new int[4];
    private int swit_offline = 0;
    private float exitone_time = 0;
    private int exit_one = 0;

    void Start()
    {
        _players = new[] {blinky, clyde, inky, pinky, player};
        PlayerUser.exit = false;
        _maze = networkPVP.Maze;
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        PlayerUser.player_self = 0;
        PlayerUser.player_opposite = 0;
        DrawMaze();
    }

    void FixedUpdate()
    {
        Debug.Log(networkPVP.State);
        if (exit_one == 1)
        {
            exitone_time += Time.deltaTime;
        }

        if (exitone_time > 2)
        {
            SceneManager.LoadScene(2);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            if (player1 != null && swit_offline != 1)
            {
                player1.GetComponent<Transform>().localPosition = new Vector3(13f, 0.5F, -17f);
                for (int i = 0; i < 4; i++)
                {
                    ghosts[i].GetComponent<Transform>().localPosition = new Vector3(i_temp[i], 0.5f, j_temp[i]);
                }
            }
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            swit_offline = 1;
            if (player1 != null)
            {
                player1.GetComponent<PlayerUser>().enabled = true;
                for (int i = 0; i < 4; i++)
                {
                    ghosts[i].GetComponent<ghostmovenet>().enabled = true;
                    //    ghosts[i].GetComponent<BoxCollider>().enabled=true;
                }
            }
            else
            {
                Debug.Log("null p2");
            }
        }
    }

    private void DrawMaze()
    {
        int ddd = 0;

        if (networkPVP.State == 1)
        {
            for (int i = 0; i < _maze.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.GetLength(1); j++)
                {
                    if (_maze[i, j] == "1")
                    {
                        // GameObject Wall = PhotonNetwork.Instantiate(wall.name, new Vector3(j, 0.5f, -i), wall.transform.rotation);
                        GameObject Wall =
                            GameObject.Instantiate(wall, new Vector3(j, 0.5f, -i), wall.transform.rotation);
                        Wall.SetActive(true);
                    }
                    else if (_maze[i, j] == "2")
                    {
                        GameObject Strong_tool = PhotonNetwork.Instantiate(strong_tool.name, new Vector3(j, 0.5f, -i),
                            strong_tool.transform.rotation);
                        Strong_tool.SetActive(true);
                    }
                    else if (_maze[i, j] == "3")
                    {
                        GameObject Food = PhotonNetwork.Instantiate(food.name, new Vector3(j, 0.5f, -i),
                            food.transform.rotation);
                        Food.SetActive(true);
                        here_tot++;
                    }
                    // else if (_maze[i, j] == "a")
                    // {
                    //     var obj = PhotonNetwork.Instantiate(suckBall, new Vector3(j, 0.5f, -i), food.transform.rotation);
                    //     obj.SetActive(true);
                    // }
                    // else if (_maze[i, j] == "b")
                    // {
                    //     var obj = PhotonNetwork.Instantiate(accBall, new Vector3(j, 0.5f, -i), food.transform.rotation);
                    //     obj.SetActive(true);
                    // }
                    else if (int.Parse(_maze[i, j]) > 0 && int.Parse(_maze[i, j]) < 8)
                    {
                        var pos = new Vector3(j, 0.5f, -i);
                        GameObject obj = PhotonNetwork.Instantiate(_players[int.Parse(_maze[i, j]) - 4].name, pos,
                            _players[int.Parse(_maze[i, j]) - 4].transform.rotation);
                        obj.SetActive(true);
                        obj.GetComponent<ghostmovenet>().Init(pos, 28, 31);

                        obj.GetComponent<ghostmovenet>().enabled = false;
                        // obj.GetComponent<BoxCollider>().enabled=false;
                        ghosts[ddd] = obj;
                        i_temp[ddd] = j;
                        j_temp[ddd] = -i;
                        ddd++;
                    }
                }
            }

            player1 = PhotonNetwork.Instantiate(player.name, new Vector3(13f, 0.5F, -17f), player.transform.rotation);
            player1.SetActive(true);
            player1.GetComponent<PlayerUser>().Init(new Vector3(13f, 0.5F, -17f), 28, 31);

            player1.GetComponent<PlayerUser>().enabled = false;
        }
        else
        {
            for (int i = 0; i < _maze.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.GetLength(1); j++)
                {
                    if (_maze[i, j] == "1")
                    {
                        // GameObject Wall = PhotonNetwork.Instantiate(wall.name, new Vector3(j, 0.5f, -i), wall.transform.rotation);
                        GameObject Wall =
                            GameObject.Instantiate(wall, new Vector3(j, 0.5f, -i), wall.transform.rotation);
                        Wall.SetActive(true);
                    }
                }
            }

            player2 = PhotonNetwork.Instantiate(player_temp.name, new Vector3(14f, 0.5F, -17f),
                player.transform.rotation);
            player2.GetComponent<PlayerUser>().Init(new Vector3(14f, 0.5F, -17f), 28, 31);
            if (player1 != null)
            {
                Debug.Log("player 1 here haha ");
            }
            else
            {
                Debug.Log("nonono");
            }
        }
    }
}