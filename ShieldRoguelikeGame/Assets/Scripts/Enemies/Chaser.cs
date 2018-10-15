using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {

    protected enum States { Normal, Attacking, Stunned }

    [SerializeField]
    protected float speed;

    protected States mState;

    protected Rigidbody2D rb;

    protected delegate void Actions();

    protected Actions Move;
    protected Actions Attack;

    protected Coroutine AttackingCoroutine;


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
        if (AttackingCoroutine != null)
            StopCoroutine(AttackingCoroutine);
        AttackingCoroutine = StartCoroutine(Leap());
    }

    IEnumerator Leap()
    {
        ///
        /// Yeah, yeah..I'm not storing the Sprite Renderer..I know..This is still a prototype
        /// Please don't cringe :)
        ///

        yield return new WaitForSeconds(Random.Range(3,10));

        mState = States.Attacking;

        transform.GetComponent<SpriteRenderer>().color = Color.magenta;

        yield return new WaitForSeconds(0.2f);

        transform.GetComponent<SpriteRenderer>().color = Color.red;

        Vector2 target = player.transform.position + (player.transform.position - transform.position).normalized;

        rb.velocity = (target - (Vector2)transform.position).normalized * speed * 7;

        while(Vector2.Distance(transform.position, target) > 0.2f)
        {
            yield return null;
        }

        rb.velocity = Vector2.zero;
        transform.GetComponent<SpriteRenderer>().color = Color.cyan;
        mState = States.Normal;

        Charge();
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Shield")
        {
            StopCoroutine(AttackingCoroutine);
            StartCoroutine(Stunned());
        }
        else if(col.transform.tag == "Player")
        {
            Destroy(col.transform.gameObject);
        }
    }

    IEnumerator Stunned()
    {
        mState = States.Stunned;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.GetComponent<SpriteRenderer>().color = Color.white;

        yield return new WaitForSeconds(0.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;
        mState = States.Normal;     
        transform.GetComponent<SpriteRenderer>().color = Color.cyan;
        Charge();

    }

}
