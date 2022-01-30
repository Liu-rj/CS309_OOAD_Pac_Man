using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class EVEPlayerMove : MonoBehaviour
{
    private bool _canMove;
    private float _speed = 0.03f;
    private Vector3 _nextPos;
    private int strong;
    public int score;
    public Text st;
    public bool mine;

    // Start is called before the first frame update
    void Start()
    {
        _canMove = false;
    }
    
    public void Init(Vector3 pos)
    {
        transform.position = pos;
    }

    public void FixedUpdate()
    {
        if (_canMove)
        {
            if (Vector3.Distance(transform.position, _nextPos) >= 2)
            {
                transform.position = _nextPos;
                _canMove = false;
                EVEManager.Moving = false;
                return;
            }
            Vector3 temp = Vector3.MoveTowards(transform.position, _nextPos, _speed);
            GetComponent<Rigidbody>().MovePosition(temp);
            if (transform.position == _nextPos)
            {
                _canMove = false;
                EVEManager.Moving = false;
            }
        }
    }

    public void MoveTo(Vector3 pos)
    {
        _nextPos = pos;
        strong -= 1;
        if (Vector3.Distance(transform.position, _nextPos) < 2)
        {
            Vector3 dir = _nextPos - transform.position;
            transform.rotation = Quaternion.Euler(90 * (Math.Abs(dir.z) + dir.x), 90, 90 * (1 + dir.z));
        }
        _canMove = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("food"))
        {
            if (mine)
            {
                score++;
                st.text = "score: " + score;
            }
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("bigball"))
        {
            Destroy(col.gameObject);
        }
     
    }
}