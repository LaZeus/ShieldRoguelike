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
        if (col.transform.tag == "Shield")
        {
            Vector2 target = (transform.position - col.transform.position).normalized;
            float rot = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, -rot + 90);
            //rb.velocity = (transform.position - col.transform.position).normalized * deflectedBulletSpeed;
            rb.velocity = -rb.velocity.normalized * deflectedBulletSpeed;
            transform.gameObject.layer = playerBullets;
        }
    }
}
