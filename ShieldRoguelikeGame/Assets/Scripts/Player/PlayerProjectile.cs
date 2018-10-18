using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}
