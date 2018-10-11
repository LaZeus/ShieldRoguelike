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

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Shield")
        {
            Vector2 target = (transform.position - col.transform.position).normalized;

            //float rot = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, 0, -rot + 90);
            //rb.velocity = (transform.position - col.transform.position).normalized * deflectedBulletSpeed;

            Vector2 vel = Vector2.Reflect(-target, col.transform.up).normalized;
            float rot = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

            rb.velocity = vel * deflectedBulletSpeed;
                    
            transform.gameObject.layer = playerBullets;
        }
    }*/

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Shield")
            StartCoroutine(ChangeSpeed());
        else if (col.transform.tag == "Player")
        {
            Destroy(this.gameObject);
            // replace this with a message to the player or GM
        }
    }

    IEnumerator ChangeSpeed()
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
