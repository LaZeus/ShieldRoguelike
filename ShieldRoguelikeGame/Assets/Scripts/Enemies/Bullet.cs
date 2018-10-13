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
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Shield")
            StartCoroutine(ChangeSpeed(col.transform.parent.GetComponent<Shield>()));
        else if (col.transform.tag == "Player")
        {
            Destroy(col.gameObject);
            Destroy(this.gameObject);          
            // replace this with a message to the player or GM
        }
    }

    IEnumerator ChangeSpeed(Shield shield)
    {
        shield.SendMessage("BulletDeflected", 1);

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
