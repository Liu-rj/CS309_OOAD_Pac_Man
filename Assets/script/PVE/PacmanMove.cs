using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PacmanMove : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;
    public Text score_text;
    public GameObject[] hps = new GameObject[3];
    public int score;
    public Text victory;
    public float speed;
    public AudioSource ads;
    public AudioClip eatmusic;
    public AudioClip propmusic;
    public AudioClip attackmusic;
    public AudioClip killmusic;
    public GameObject shield;
    
    private Renderer rend;
    private Material peace;
    private int _hp;
    private bool strong;
    private bool suck;
    private bool exit;
    private float exit_time;
    private float originTime;
    private float originTime_suck;
    private int total_score = 0;
    private float _fastSpeed;
    private Vector3 _dest;
    private Vector3 _direction;
    private Vector3 _pre;
    private Quaternion _rotation;
    private LayerMask _mask;
    private Vector3 _originPosition;
    private float _originTimeAcc;
    private int _width;
    private int _height;

    // Start is called before the first frame update
    void Start()
    {
        exit = false;
        _hp = 3;
        rd = GetComponent<Rigidbody>();
        _dest = transform.position;
        _direction = Vector3.zero;
        _pre = Vector3.zero;
        speed = 0.1f;
        _fastSpeed = 0.2f;
        _mask = LayerMask.GetMask("Wall");
        _originPosition = _dest;
        shield.SetActive(false);
    }
    
    public void Init(Vector3 pos, int width, int height)
    {
        transform.position = pos;
        _originPosition = pos;
        _dest = pos;
        _width = width;
        _height = height;
        total_score = RandomMazeLoader.food_number; //!!! set total score by plk
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            // Debug.Log("up");
            _pre = Vector3.forward;
            _rotation = Quaternion.Euler(0, -90, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            // Debug.Log("Down");
            _pre = Vector3.back;
            _rotation = Quaternion.Euler(0, 90, 0);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // Debug.Log("left");
            _pre = Vector3.left;
            _rotation = Quaternion.Euler(0, 180, 0);
        }
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Debug.Log("right");
            _pre = Vector3.right;
            _rotation = Quaternion.Euler(0, 0, 0);
            
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
                GameManager.Coin += score;
                GameManager.PushUserData();
                SceneManager.LoadScene(2);
            }
        }

        if (strong)
        {
            shield.SetActive(true);
            originTime += Time.deltaTime;
            if (originTime > 10)
            {
                shield.SetActive(false);
                strong = false;
                originTime = 0;
            }
        }

        if (speed == _fastSpeed)
        {
            _originTimeAcc += Time.deltaTime;
            if (_originTimeAcc>5)
            {
                speed = 0.1f;
                _originTimeAcc = 0;
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

            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10);
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
        return !Physics.Linecast(pos, pos + dir, _mask);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("food"))
        {
            ads.clip = eatmusic;
            ads.Play();
            Destroy(col.gameObject);
            score++;
            score_text.text = "Score:  " + score;
            if (score == total_score)
            {
                GameManager.EmpValue += 100;
                Debug.Log("score:"+score+"totalscore:"+total_score);
                victory.text = "Win!";
                exit = true;
            }
        }

        if (col.gameObject.CompareTag("bigball"))
        {
            ads.clip = propmusic;
            ads.Play();
            Destroy(col.gameObject);
            strong = true;
            originTime = 0;
        }

        if (col.gameObject.CompareTag("suckball"))
        {
            ads.clip = propmusic;
            ads.Play();
            Destroy(col.gameObject);
            suck = true;
        }

        if (col.gameObject.CompareTag("ghost"))
        {
            if (strong)
            {
                ads.clip = killmusic;
                ads.Play();
                col.gameObject.GetComponent<GhostMove>().Reset();
            }
            else
            {
                ads.clip = attackmusic;
                ads.Play();
                if (_hp>0)
                {
                    _hp -= 1;
                    hps[_hp].SetActive(false);
                    if (_hp == 0)
                    {
                        victory.text = "Lose!";
                        exit = true;
                    }
                }
                

                Reset();
            }
        }
        
        if (col.gameObject.CompareTag("accelerateball"))
        {
            ads.clip = propmusic;
            ads.Play();
            Destroy(col.gameObject);
            speed = _fastSpeed;
        }
    }

    private void Reset()
    {
        transform.position = _originPosition;
        _direction = Vector3.zero;
        _dest = _originPosition;
        _pre = Vector3.zero;
    }
}