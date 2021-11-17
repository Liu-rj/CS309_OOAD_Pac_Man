using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVEGhostMove : MonoBehaviour
{
    private bool _canMove;
    private Vector3 _nextPos;
    private string _preTag;
    private float _speed = 0.05f;
    private Vector3 _direction;
    private System.Random _random;
    private Vector3[] _dirs;
    private LayerMask _mask;
    private Vector3 _originPosition;

    private void Start()
    {
        _originPosition = transform.position;
        _nextPos = _originPosition;
        _preTag = "0";
        _direction = Vector3.zero;
        _random = new System.Random();
        _dirs = new[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        _mask = LayerMask.GetMask("Wall");
    }

    public void Move()
    {
        Decide();
        UpdateMaze();
        _canMove = true;
    }

    private void UpdateMaze()
    {
        var position = transform.position;
        EVEManager.Maze[(int) (-position.z), (int) (position.x)] = _preTag;
        _preTag = EVEManager.Maze[(int) (-_nextPos.z), (int) (_nextPos.x)];
        EVEManager.Maze[(int) (-_nextPos.z), (int) (_nextPos.x)] = "4";
    }
    
    private void FixedUpdate()
    {
        if (_canMove)
        {
            var temp = Vector3.MoveTowards(transform.position, _nextPos, _speed);
            GetComponent<Rigidbody>().MovePosition(temp);
            if (transform.position == _nextPos)
            {
                _canMove = false;
                EVEManager.Moving = false;
            }
        }
    }

    private void Decide()
    {
        if (Valid(_direction) && _direction != Vector3.zero)
        {
            _nextPos = transform.position + _direction;
        }
        else
        {
            var num = 0;
            foreach (var dir in _dirs)
            {
                if (Valid(dir))
                {
                    num += 1;
                }
            }

            if (num == 0)
            {
                _nextPos = transform.position;
            }
            else
            {
                var i = _random.Next(0, 4);
                while (!Valid(_dirs[i]))
                {
                    i = _random.Next(0, 4);
                }

                var nextDir = _dirs[i];
                _nextPos = transform.position + nextDir;
                transform.rotation = Quaternion.Euler(-90, 0, (-90 - nextDir.x * 90) * Math.Abs(nextDir.x) + nextDir.z * 90);
                _direction = nextDir;
            }
        }
    }

    private bool Valid(Vector3 dir)
    {
        var pos = transform.position;
        return !Physics.Linecast(pos, pos + dir, _mask);
    }

    public void Reset()
    {
        var transform1 = transform;
        var position = transform1.position;
        EVEManager.Maze[(int) (-position.z), (int) (position.x)] = _preTag;
        transform1.position = _originPosition;
        _nextPos = _originPosition;
        EVEManager.Maze[(int) (-_nextPos.z), (int) (_nextPos.x)] = "4";
        _preTag = "0";
        _direction = Vector3.zero;
    }
}