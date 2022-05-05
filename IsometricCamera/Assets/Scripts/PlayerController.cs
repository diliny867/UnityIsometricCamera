using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;
    private float speed=5f;
    private float rotationSpeed=360f*2.5f;
    private Vector3 input;
    private Matrix4x4 matrix;
    
    private float DashCooldown=1;
    private float NextDash;
    private bool dashPressed = false;
    private float dashMinKillVelocity=4f;
    private int dashMinInvincibility=10;//frames
    private int dashDuration=0;
    private bool isDashing=false;

    public Image dashIndicatorImage;
    
    void Awake()
    {
        matrix = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));
    }

    void Start()
    {
        rb.drag = 3;
    }

    void Update()
    {
        if (PlayerScript.isDead) {return;}
        GetInput();
        Look();
        PressedDash();
        
    }
    void FixedUpdate()
    {
        if (PlayerScript.isDead) {return;}
        Move();
    }

    void GetInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input.Normalize();
    }

    void PressedDash()
    {
        if (Time.time > NextDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) //dash
            {
                NextDash = Time.time + DashCooldown;
                dashPressed = true;
                dashIndicatorImage.color = Color.white;
            }
            else
            {
                dashIndicatorImage.color = new Color(204f/255f,0,0,1);
            }
        }
    }

    void Dash()
    {
        if (dashPressed)
        {
            collider.isTrigger = true;
            PlayerScript.invincible = true;
            rb.AddForce(transform.forward * 100 * speed);
            dashPressed = false;
            isDashing = true;
        }
        if(dashDuration>dashMinInvincibility)
        {
            dashDuration = 0;
            isDashing = false;
        }
        if (isDashing)
        {
            dashDuration++;
        }
        if(rb.velocity.magnitude<=dashMinKillVelocity && !isDashing)
        {
            collider.isTrigger = false;
            PlayerScript.invincible = false;
        }
    }
    
    void Move()
    {
        rb.MovePosition(transform.position + transform.forward * input.magnitude * speed * Time.deltaTime);
        Dash();
    }

    void Look()
    {
        if (input != Vector3.zero)
        {
            var rotatedInput = matrix.MultiplyPoint3x4(input);
            var rel = (transform.position + rotatedInput) - transform.position;
            var rot = Quaternion.LookRotation(rel);
            
            if ((transform.forward+rotatedInput).magnitude > 0.3)//small change of direction
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);//rotates gradually
            }
            else//big change of direction
            {
                transform.rotation = rot;//rotates immediately
                //transform.rotation *= Quaternion.Euler(0,180,0);
            }
        }
    }

    public void ModifyDefaultAngles(float angle)
    {
        matrix = Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0));
    }
}
