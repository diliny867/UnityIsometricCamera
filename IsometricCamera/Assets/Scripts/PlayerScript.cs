using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private int hp = 100;
    private int kills = 0;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI killCountText;

    public static bool isDead=false;
    public static bool invincible = false;
    void Start()
    {
        hpText.text = hp + " hp";
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            if (isDead){return;}
            TakeDamage(10,col);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
            kills++;
            killCountText.text = kills+" kills";
        }
    }

    void TakeDamage(int dmg,Collision col)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hpText.text = "Dead!";
            isDead = true;
            var playerRb = this.gameObject.GetComponent<Rigidbody>();
            playerRb.constraints = RigidbodyConstraints.None;
            playerRb.AddExplosionForce(300f,col.transform.position-new Vector3(0,1,0),50f);
            //Destroy(this.gameObject);
        }
        else
        {
            hpText.text = hp + " hp";
        }
    }
}
