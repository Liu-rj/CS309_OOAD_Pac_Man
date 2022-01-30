using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    public float speed;

    private Rigidbody _rd;
    private Vector3 _dest;
    private Vector3 _direction;
    private System.Random _random;
    private Vector3[] _dirs;
    private LayerMask _wallMask;
    private LayerMask _ghostMask;
    private Vector3 _originPosition;
    private int _width;
    private int _height;

    // Start is called before the first frame update
    void Start()
    {
        _rd = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(-90, 0, 90);
        _direction = Vector3.zero;
        _random = new System.Random();
        _dirs = new[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        speed = 0.1f;
        _wallMask = LayerMask.GetMask("Wall", "Ghost");
        _ghostMask = LayerMask.GetMask("Ghost");
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
        Vector3 temp = Vector3.MoveTowards(transform.position, _dest, speed);
        _rd.MovePosition(temp);
        // 必须先达到上一个dest的位置才可以发出新的dest设置指令
        if (transform.position == _dest)
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
                _dest = transform.position;
            }
            else
            {
                var i = _random.Next(0, 4);
                while (!Valid(_dirs[i]))
                {
                    i = _random.Next(0, 4);
                }

                var nextDir = _dirs[i];
                _dest = transform.position + nextDir;
                transform.rotation =
                    Quaternion.Euler(-90, 0, (-90 - nextDir.x * 90) * Math.Abs(nextDir.x) + nextDir.z * 90);
                _direction = nextDir;
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
    }

    private bool Valid(Vector3 dir)
    {
        Vector3 pos = transform.position;
        if (!Physics.Linecast(pos, pos + dir, _wallMask))
        {
            if (!Physics.Linecast(pos, pos + dir * 2, _ghostMask))
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        transform.position = _originPosition;
        _dest = _originPosition;
        _direction = Vector3.zero;
    }
}