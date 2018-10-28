using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    protected Transform player;

    [SerializeField]
    protected CameraShake cam;

    protected GameObject GM;

    protected int scoreValue;

    protected virtual void OnDisable()
    {
        Disabling();
    }

    protected void Disabling()
    {
        cam.ShakeCamera(1f, 0.4f);
        SpawnParticles();

        if (GM != null)
            GM.SendMessage("IncreaseScore", scoreValue);
    }

    protected void FindPlayer()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        if (cam == null)
            cam = GameObject.Find("myCam").GetComponent<CameraShake>();

        if (GM == null)
            GM = GameObject.Find("Gamemanager").gameObject;
    }

    protected void SpawnParticles()
    {
        GameObject par = ObjectPooler.SharedInstance.GetPooledObject(4);
        par.transform.position = transform.position;
        par.SetActive(true);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerAttack")
        {         
            cam.ShakeCamera(0.6f, 0.1f);
            gameObject.SetActive(false);
            col.gameObject.SetActive(false);
        }
    }
}
