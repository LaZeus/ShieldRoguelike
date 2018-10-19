using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerController {

    [SerializeField]
    private GameObject gm;

    [SerializeField]
    private CameraShake cam;

    private void Awake()
    {
        Initialization();    
    }

    private void Start()
    {
        if (gm == null)
            gm = GameObject.Find("Gamemanager");

        if (cam == null)
            cam = GameObject.Find("myCam").GetComponent<CameraShake>();
    }

    private void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        PlayerControls();
	}

    private void PlayerControls()
    {
        if (ControlActions != null)
            ControlActions();
    }

    private void Die()
    {
        gm.SendMessage("PlayerDied");

        cam.ShakeCamera(0.3f, 2f);

        gameObject.SetActive(false);
    }

}
