using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy {

    protected enum States { Normal, Attacking, Stunned }

    [SerializeField]
    protected float speed;

    protected States mState;

    protected Rigidbody2D rb;

    protected SpriteRenderer sprRndr;

    protected delegate void Actions();

    protected Actions Move;
    protected Actions Attack;

    protected Coroutine AttackingCoroutine;

    private ChaserEntry binding;

    void Awake ()
    {
        FindPlayer();

        mState = States.Normal;
        sprRndr = transform.GetComponent<SpriteRenderer>();
        sprRndr.color = Color.cyan;
        rb = transform.GetComponent<Rigidbody2D>();
        Move = Walk;
        Attack = Charge;
    }

    private void Start()
    {
        binding = FindObjectOfType<ChaserFlockManager>().AddChaser(this);

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
        if (player == null || Vector2.Distance(transform.position, binding.WantedPosition) < 0.2f)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = (binding.WantedPosition - transform.position).normalized * speed;     
    }

    protected void Charge()
    {
        if (AttackingCoroutine != null)
            StopCoroutine(AttackingCoroutine);
        AttackingCoroutine = StartCoroutine(Leap());
    }

    IEnumerator Leap()
    {
        yield return new WaitForSeconds(Random.Range(3,10));

        mState = States.Attacking;

        sprRndr.color = Color.magenta;

        yield return new WaitForSeconds(0.2f);

        sprRndr.color = Color.red;
        Vector2 target = binding.WantedPosition;
        float elapsed = 0;

        if (player != null && player.gameObject.activeInHierarchy)
        {
            rb.velocity = (target - (Vector2)transform.position).normalized * speed * 7;

            while (Vector2.Distance(transform.position, target) > 0.2f && elapsed < 1)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        rb.velocity = Vector2.zero;
        sprRndr.color = Color.cyan;
        mState = States.Normal;

        Charge();
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Shield")
        {
            StopCoroutine(AttackingCoroutine);
            StartCoroutine(Stunned(col.transform.position));
        }
        else if(col.transform.tag == "Player")
        {
            col.gameObject.SendMessage("Die");
        }
        else if (col.transform.tag == "PlayerAttack")
        {
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    IEnumerator Stunned(Vector2 pos)
    {
        mState = States.Stunned;

        gameObject.layer = 14;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = ((Vector2)transform.position - pos).normalized;
        sprRndr.color = Color.yellow;

        yield return new WaitForSeconds(0.5f);

        if (player != null)
            while (Vector2.Distance(transform.position, binding.WantedPosition) < 2f)
            { 
                yield return null;
            }

        gameObject.layer = 12;
        rb.bodyType = RigidbodyType2D.Dynamic;
        mState = States.Normal;
        sprRndr.color = Color.cyan;
        Charge();

    }

}
