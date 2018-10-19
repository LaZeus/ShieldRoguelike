using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float deflectedBulletSpeed;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private SpriteRenderer sprt;

    private int playerBullets = 11;

    [SerializeField]
    private TrailRenderer trail;

    private CameraShake cam;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (cam == null)
            cam = GameObject.Find("myCam").GetComponent<CameraShake>();
    }


    private void OnEnable()
    {
        trail.gameObject.SetActive(true);
        transform.gameObject.layer = 9;
        sprt.color = Color.red;
    }

    private void OnDisable()
    {
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        trail.colorGradient = gradient;

        trail.gameObject.SetActive(false);       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Turret" && gameObject.layer == playerBullets)
        {
            cam.ShakeCamera(0.6f, 0.1f);
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.layer = 9;
        }
    }

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
            ChangeColor();
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
        }
    }

    private void ChangeColor()
    {
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        trail.colorGradient = gradient;
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
        sprt.color = Color.blue;
    }
}
