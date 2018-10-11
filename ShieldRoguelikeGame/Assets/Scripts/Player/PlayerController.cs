using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    protected enum States { Normal, Dying }

    protected delegate void Actions();

    #region variables

    protected States mState;

    private Rigidbody2D rb;

    protected Actions ControlActions;

    [SerializeField]
    private float speed;

    #endregion

    #region Init

    protected void Initialization()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        ControlActions = Moving;
    }

    #endregion

    #region Moving

    protected void Moving()
    {
        float Input_x = Input.GetAxisRaw("Horizontal");
        float Input_y = Input.GetAxisRaw("Vertical");

        if (mState == States.Normal)     
            rb.velocity = new Vector2(Input_x, Input_y).normalized * speed;       
    }

    #endregion
}
