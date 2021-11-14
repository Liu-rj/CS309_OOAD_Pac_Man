using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class EVEPlayerMove : MonoBehaviour
{
    private bool _canMove;
    private int _color;
    private Vector3 _nextPos;
    private Vector3 _originPosition;
    private float _speed = 0.05f;
    private bool _strong;
    private bool _suck;
    private bool _exit;
    private float _exitTime;
    private float _originTime;
    private float _originTimeSuck;
    
    public int score;
    public int id;
    public int opp;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        _canMove = false;
    }
    
    private void EatDot()
    {
        score += 1;
        EVEManager.DotNum -= 1;
    }

    public void Init(int pId, int pOpp, int color, Vector3 initPos)
    {
        id = pId;
        opp = pOpp;
        _color = color;
        var transform1 = transform;
        transform1.position = initPos;
        _nextPos = transform1.position;
        _originPosition = initPos;
    }

    public void FixedUpdate()
    {
        if (_canMove)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, _nextPos, _speed);
            GetComponent<Rigidbody>().MovePosition(temp);
            if (transform.position == _nextPos)
            {
                _canMove = false;
                EVEManager.Moving = false;
            }
        }
        
        if (_exit)
        {
            _exitTime += Time.deltaTime;
            if (_exitTime > 2)
            {
                SceneManager.LoadScene(2);
            }
        }

        if (_strong)
        {
            _originTime += Time.deltaTime;
            if (_originTime > 10)
            {
                _strong = false;
                _originTime = 0;
            }
        }

        if (_suck)
        {
            _originTimeSuck += Time.deltaTime;
            if (_originTimeSuck > 5)
            {
                _suck = false;
                _originTimeSuck = 0;
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

    public void Move()
    {
        Decide();
        UpdateMaze();
        _canMove = true;
    }

    private void Decide()
    {
        Process process = new Process();
        process.StartInfo.FileName = @"python.exe";
        string path = Application.dataPath;
        path += "/script/python_interface/AI_Interface.py";
        path = path + " --player " + id + " --color " + _color + " --player2 " + opp;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Arguments = path;
        process.Start();
        process.BeginOutputReadLine();
        process.OutputDataReceived += DataReceiver;
        Console.ReadLine();
        process.WaitForExit();
    }

    private void DataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string rawData = e.Data;
            string[] blankSplit = rawData.Split(' ');
            int x = Int32.Parse(blankSplit[0]);
            int z = Int32.Parse(blankSplit[1]);
            _nextPos = new Vector3(x - 24.5f, 0.5f, z - 24.5f);
            var direction = _nextPos - transform.position;
            if (direction == Vector3.forward)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (direction == Vector3.back)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (direction == Vector3.left)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (direction == Vector3.right)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            Debug.Log("Pacman decided next position: " + _nextPos);
        }
    }

    private void UpdateMaze()
    {
        var position = transform.position;
        EVEManager.Maze[(int) (position.x + 24.5f), (int) (position.z + 24.5f)] = "0";
        EVEManager.Maze[(int) (_nextPos.x + 24.5f), (int) (_nextPos.z + 24.5f)] = _color.ToString();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("food"))
        {
            EatDot();
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("bigball"))
        {
            Destroy(col.gameObject);
            _strong = true;
            _originTime = 0;
        }

        if (col.gameObject.CompareTag("suckball"))
        {
            Destroy(col.gameObject);
            _suck = true;
        }

        if (col.gameObject.CompareTag("ghost"))
        {
            if (_strong)
            {
                col.gameObject.GetComponent<GhostMove>().Reset();
            }
            else
            {
                Reset();
            }
        }
    }

    private void Reset()
    {
        var position = transform.position;
        EVEManager.Maze[(int) (position.x + 24.5f), (int) (position.z + 24.5f)] = "0";
        transform.position = _originPosition;
        _nextPos = _originPosition;
        EVEManager.Maze[(int) (_nextPos.x + 24.5f), (int) (_nextPos.z + 24.5f)] = _color.ToString();
        score = 0;
        _canMove = false;
    }
}