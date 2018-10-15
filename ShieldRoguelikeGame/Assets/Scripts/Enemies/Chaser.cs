using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {

    protected enum States { Normal, Attacking }

    [SerializeField]
    protected float speed;

    protected States mState;

    protected Rigidbody2D rb;

    protected delegate void Actions();

    protected Actions Move;
    protected Actions Attack;


    void Awake ()
    {
        FindPlayer();

        mState = States.Normal;
        rb = transform.GetComponent<Rigidbody2D>();
        Move = Walk;
        Attack = Charge;
    }

    private void Start()
    {
        if(Attack != null)
            Attack();
    }

    void FixedUpdate ()
    {
        if (Move != null && mState == States.Normal)
            Move();
	}

    protected void Walk()
    {
        if (player == null)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = (player.transform.position - transform.position).normalized * speed;     
    }

    protected void Charge()
    {
        StartCoroutine(Leap());
    }

    IEnumerator Leap()
    {
        ///
        /// Yeah, yeah..I'm not storing the Sprite Renderer..I know..This is still a prototype
        /// Please don't cringe :)
        ///

        mState = States.Attacking;

        transform.GetComponent<SpriteRenderer>().color = Color.magenta;

        yield return new WaitForSeconds(0.2f);

        transform.GetComponent<SpriteRenderer>().color = Color.red;

        Vector2 target = player.transform.position;

        rb.velocity = (target - (Vector2)transform.position).normalized * speed * 5;

        while(Vector2.Distance(transform.position, target) > 0.2f)
        {
            yield return null;
        }

        rb.velocity = Vector2.zero;
        transform.GetComponent<SpriteRenderer>().color = Color.cyan;
        mState = States.Normal;
    }

}
