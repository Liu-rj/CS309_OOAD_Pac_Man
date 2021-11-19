using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EVEGhostMove : MonoBehaviour
{
    private bool _canMove;
    private float _speed = 0.03f;
    private Vector3 _direction;
    private Vector3[] _dirs;
    private Vector3 _nextPos;

    private void Start()
    {
        _direction = Vector3.zero;
        _dirs = new[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        _canMove = false;
    }
    
    public void Init(Vector3 pos)
    {
        transform.position = pos;
    }

    public void MoveTo(Vector3 pos)
    {
        _nextPos = pos;
        _canMove = true;
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            if (Vector3.Distance(transform.position, _nextPos) >= 2)
            {
                Debug.Log("now position: " + transform.position);
                Debug.Log("next position: " + _nextPos);
                Debug.Log(Vector3.Distance(transform.position, _nextPos));
                transform.position = _nextPos;
                _canMove = false;
                EVEManager.Moving = false;
                return;
            }
            var temp = Vector3.MoveTowards(transform.position, _nextPos, _speed);
            GetComponent<Rigidbody>().MovePosition(temp);
            if (transform.position == _nextPos)
            {
                _canMove = false;
                EVEManager.Moving = false;
            }
        }
    }
}