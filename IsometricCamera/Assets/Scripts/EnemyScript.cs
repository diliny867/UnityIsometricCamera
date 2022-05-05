using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private GameObject player;
    public Rigidbody rb;
    private float speed = 3f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.LookAt(new Vector3(player.transform.position.x,transform.localScale.y/2,player.transform.position.z));
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
}
