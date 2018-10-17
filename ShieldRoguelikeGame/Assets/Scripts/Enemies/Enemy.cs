using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected float healthPoints;

    [SerializeField]
    protected Transform player;

    [SerializeField]
    protected CameraShake cam;

    protected int c = 0;

    protected virtual void OnDisable()
    {        
        if(c != 0)
            cam.ShakeCamera(1f, 0.4f);

        c++;
    }


    protected void FindPlayer()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        if (cam == null)
            cam = GameObject.Find("myCam").GetComponent<CameraShake>();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerAttack")
        {
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);                      
        }
    }
}
