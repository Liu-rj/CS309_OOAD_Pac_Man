using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerUser : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;
    public static int player_self = 0;
    public  static  int player_opposite = 0;
    public int player_self_hp = 3;
    public int player_opposite_hp = 3;
    public GameObject redball;
    public GameObject[] shp = new GameObject[3];
    public GameObject[] ohp = new GameObject[3];
    public AudioSource ads;
    public AudioClip eatmusic;
    public AudioClip propmusic;
    public AudioClip attackmusic;
    public AudioClip killmusic;
    public bool music;
    public float speed;

    private Text score_self;
    private Text score_opposite;
    private Text win;
    private GameObject victory;
    private Renderer rend;
    private Material peace;
    private int _hp;
    private bool strong;
    private bool suck;
    public static bool exit;
    private float exit_time;
    private float originTime;
    private float originTime_suck;
    
    private int total_score = 0;
    private Vector3 _dest;
    private Vector3 _direction;
    private Vector3 _pre;
    private Quaternion _rotation;
    private LayerMask _wallMask;
    private LayerMask _pacmanMask;
    private Vector3 _originPosition;
    private PhotonView photonView;
    private int _width;
    private int _height;

    // Start is called before the first frame update
    void Start()
    {
        // total_score = logic_scene.here_tot;
        total_score = 40;
        Debug.Log("here");
        Debug.Log(total_score);
        changeViewNetwork camera_network = this.gameObject.GetComponent<changeViewNetwork>();
        redball.SetActive(false);

        photonView = this.GetComponent<PhotonView>();
        if (camera_network != null && photonView.IsMine)
        {
            camera_network.Startnow();
        }

        exit = false;
        _hp = 3;
        rd = GetComponent<Rigidbody>();
        _dest = transform.position;
        _direction = Vector3.zero;
        _pre = Vector3.zero;
        speed = 0.1f;
        _wallMask = LayerMask.GetMask("Wall", "Pacman");
        _pacmanMask = LayerMask.GetMask("Pacman");
        _originPosition = _dest;


        shp[0] = GameObject.Find("hp1");
        shp[1] = GameObject.Find("hp2");
        shp[2] = GameObject.Find("hp3");
        ohp[0] = GameObject.Find("hp4");
        ohp[1] = GameObject.Find("hp5");
        ohp[2] = GameObject.Find("hp6");

        score_self = GameObject.Find("Text_score1").GetComponent<Text>();
        score_opposite = GameObject.Find("Text_score2").GetComponent<Text>();
        win = GameObject.Find("victory").GetComponent<Text>();
    }
    
    public void Init(Vector3 pos, int width, int height)
    {
        transform.position = pos;
        _originPosition = pos;
        _dest = pos;
        _width = width;
        _height = height;
    }

    // Update is called once per frame
    

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        int hihi = this.gameObject.GetComponent<changeViewNetwork>().currentCamera;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (hihi <= 1)
            {
                _pre = Vector3.forward;
                _rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (hihi == 2)
            {
                _pre = Vector3.back;
                _rotation = Quaternion.Euler(0, 90, 0);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (hihi <= 1)
            {
                _pre = Vector3.back;
                _rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (hihi == 2)
            {
                _pre = Vector3.forward;
                _rotation = Quaternion.Euler(0, -90, 0);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (hihi <= 1)
            {
                _pre = Vector3.left;
                _rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (hihi == 2)
            {
                _pre = Vector3.right;
                _rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (hihi <= 1)
            {
                _pre = Vector3.right;
                _rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (hihi == 2)
            {
                _pre = Vector3.left;
                _rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        Vector3 temp = Vector3.MoveTowards(transform.position, _dest, speed);
        rd.MovePosition(temp);
        // 必须先达到上一个dest的位置才可以发出新的dest设置指令
        if (transform.position == _dest)
        {
            if (Valid(_direction))
            {
                _dest = transform.position + _direction;
            }

            if (Valid(_pre) && _pre != Vector3.zero)
            {
                var transform1 = transform;
                _dest = transform1.position + _pre;
                transform1.rotation = _rotation;
                _direction = _pre;
                _pre = Vector3.zero;
            }

            if (_dest.x < 0)
            {
                _dest.x = _width - 1;
                transform.position = _dest;
            }
            else if (_dest.x >= _width)
            {
                _dest.x = 0;
                transform.position = _dest;
            }
        }

        if (exit)
        {
            exit_time += Time.deltaTime;
            if (exit_time > 2)
            {
                SceneManager.LoadScene(2);
            }
        }

        if (strong)
        {
            originTime += Time.deltaTime;
            if (originTime > 10)
            {
                redball.SetActive(false);
                strong = false;
                originTime = 0;
            }
        }

        if (suck)
        {
            originTime_suck += Time.deltaTime;
            if (originTime_suck > 5)
            {
                suck = false;
                originTime_suck = 0;
            }

            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 20);
            foreach (var item in colliders)
            {
                if (item.tag.Equals("food"))
                {
                    //让金币的开始移动
                    item.GetComponent<bean>().moving = true;
                }
            }
        }
    }

    private bool Valid(Vector3 dir)
    {
        Vector3 pos = transform.position;
        if (!Physics.Linecast(pos, pos + dir, _wallMask))
        {
            if (!Physics.Linecast(pos, pos + dir * 2, _pacmanMask))
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (col.gameObject.CompareTag("food"))
        {
            col.gameObject.GetComponent<PhotonView>().RPC("food_destroy", RpcTarget.All);
            photonView.GetComponent<PhotonView>().RPC("setScore", RpcTarget.All);
            if ((player_self + player_opposite) == total_score)
            {
                if (player_self>player_opposite)
                {
                    photonView.GetComponent<PhotonView>().RPC("setvictory", RpcTarget.All, exit);
                }else{
                    photonView.GetComponent<PhotonView>().RPC("setlost", RpcTarget.All, exit);
                }
            }
        }

        if (col.gameObject.CompareTag("bigball"))
        {
            col.gameObject.GetComponent<PhotonView>().RPC("bigball_destroy", RpcTarget.All);
            if (photonView.IsMine)
            {
                redball.SetActive(true);
                if (music)
                {
                    ads.clip = propmusic;
                    ads.Play();
                }
                strong = true;
                originTime = 0;
            }
        }


        if (col.gameObject.CompareTag("ghost"))
        {
            if (strong)
            {
                if (photonView.IsMine)
                {
                    if (music)
                    {
                        ads.clip = killmusic;
                        ads.Play();
                    }
                }
                col.gameObject.GetComponent<PhotonView>().RPC("Reset", RpcTarget.All);
            }
            else
            {
                if (photonView.IsMine)
                {
                    photonView.GetComponent<PhotonView>().RPC("sethp", RpcTarget.All);
                }

                Debug.Log(player_opposite_hp);
                Debug.Log(player_self_hp);
                if (player_self_hp == 0 || player_opposite_hp == 0)
                {
                    photonView.RPC("setlost", RpcTarget.All, exit);
                }

                Debug.Log(exit);
                if (photonView.IsMine)
                {
                    photonView.RPC("Reset", RpcTarget.All);
                }
            }
        }
    }


    [PunRPC]
    private void Reset()
    {
        transform.position = _originPosition;
        _direction = Vector3.zero;
        _dest = _originPosition;
        _pre = Vector3.zero;
    }


    [PunRPC]
    public void setScore()
    {
        Debug.Log("score");
        if (photonView.IsMine)
        {
            if (music)
            {
                ads.clip = eatmusic;
                ads.Play();
            }
            Debug.Log("in");
            player_self++;
            score_self.text = "score: " + player_self;
        }
        else
        {
            Debug.Log("out");
            player_opposite++;
            score_opposite.text = "score: " + player_opposite;
        }
    }

    [PunRPC]
    public void sethp()
    {
        if (photonView.IsMine)
        {
            if (music)
            {
                ads.clip = attackmusic;
                ads.Play();
            }

            if (player_opposite_hp>0)
            {
                player_self_hp--;
            }
            shp[player_self_hp].SetActive(false);
        }
        else
        {
            if (player_opposite_hp>0)
            {
                player_opposite_hp--;
            }
            ohp[player_opposite_hp].SetActive(false);
        }
    }

    [PunRPC]
    public void setvictory(bool quit)
    {
        if (photonView.IsMine)
        {
            win.text = "Win!";
            exit = true;
            Debug.Log(exit);
        }
        else
        {
            win.text = "lose!";
            exit = true;
            Debug.Log(exit);
        }
    }

    [PunRPC]
    public void setlost(bool quit)
    {
        if (photonView.IsMine)
        {
            win.text = "Lose!";
            exit = true;
            Debug.Log(exit);
        }
        else
        {
            win.text = "Win!";
            exit = true;
            Debug.Log(exit);
        }
    }
    [PunRPC]
    public void setplayerrun(){
        this.gameObject.SetActive(true);
    }
    
    [PunRPC]
    public void gogogo (){
        SceneManager.LoadScene(2);
    }
}