using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerController {

    private void Awake()
    {
        Initialization();
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

}
