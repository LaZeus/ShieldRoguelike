using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected float healthPoints;

    [SerializeField]
    protected Transform player;

    protected void FindPlayer()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
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
