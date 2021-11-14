using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVEGhostMove : MonoBehaviour
{
    private bool _canMove;
    private Vector3 _nextPos;
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
        EVEManager.Maze[(int) (position.x + 24.5f), (int) (position.z + 24.5f)] = "0";
        EVEManager.Maze[(int) (_nextPos.x + 24.5f), (int) (_nextPos.z + 24.5f)] = "4";
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
        var num = 0;
        var moves = new Vector3[4];
        foreach (var dir in _dirs)
        {
            if (Valid(dir))
            {
                moves[num] = dir;
                num += 1;
            }
        }

        if (num == 1)
        {
            var dir = moves[0];
            _nextPos = transform.position + dir;
            transform.rotation = Quaternion.Euler(-90, 0, (-90 - dir.x * 90) * Math.Abs(dir.x) + dir.z * 90);
            _direction = dir;
        }
        else if (num == 2 && moves[0] == -moves[1])
        {
            _nextPos = transform.position + _direction;
        }
        else
        {
            var i = _random.Next(0, 4);
            while (!Valid(_dirs[i]))
            {
                i = _random.Next(0, 4);
            }

            var dir = _dirs[i];
            _nextPos = transform.position + dir;
            transform.rotation = Quaternion.Euler(-90, 0, (-90 - dir.x * 90) * Math.Abs(dir.x) + dir.z * 90);
            _direction = dir;
        }
    }

    private bool Valid(Vector3 dir)
    {
        var pos = transform.position;
        return !Physics.Linecast(pos, pos + dir, _mask);
    }

    public void Reset()
    {
        var position = transform.position;
        EVEManager.Maze[(int) (position.x + 24.5f), (int) (position.z + 24.5f)] = "0";
        transform.position = _originPosition;
        _nextPos = _originPosition;
        EVEManager.Maze[(int) (_nextPos.x + 24.5f), (int) (_nextPos.z + 24.5f)] = "4";
        _direction = Vector3.zero;
    }
}