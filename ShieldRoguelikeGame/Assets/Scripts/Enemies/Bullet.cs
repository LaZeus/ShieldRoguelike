using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float deflectedBulletSpeed;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private int playerBullets = 11;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Turret" && gameObject.layer == 11)
        {
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.layer = 9;
        }
    }

    /// 
    /// REFACTORING
    /// 

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Shield")
        {
            Shield shield = col.transform.parent.GetComponent<Shield>();
            if(shield.mState == Shield.States.Storing)
            {
                shield.SendMessage("ProjectileDeflected", 1);
                gameObject.SetActive(false);
                return;
            }              
            StartCoroutine(ChangeSpeed(shield));
        }          
        else if (col.transform.tag == "Player")
        {
            col.gameObject.SendMessage("Die");
            gameObject.SetActive(false);          

        }
        else if(col.transform.tag == "Wall")
        {
            gameObject.SetActive(false);
            gameObject.layer = 9;
        }
    }

    IEnumerator ChangeSpeed(Shield shield)
    {       
        yield return new WaitForFixedUpdate();

        // rotation
        Vector2 vel = rb.velocity;
        float rot = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot - 90);

        // speed
        rb.velocity = rb.velocity.normalized * deflectedBulletSpeed;
        transform.gameObject.layer = playerBullets;
        transform.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
