using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected float healthPoints;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerAttack")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
