using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    protected enum States { Normal, Dashing, Dying }

    protected delegate void Actions();

    #region variables

    protected States mState;

    private Rigidbody2D rb;

    protected Actions ControlActions;

    protected Actions Inputs;

    [SerializeField]
    protected Transform shield;

    [SerializeField]
    private float speed;

    #endregion

    #region Init

    protected void Initialization()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (shield == null)
            shield = GameObject.Find("ShieldController").transform;

        ControlActions = Moving;
        Inputs = Dashing;
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

    #region Dashing

    protected void Dashing()
    {
        if (Input.GetButtonDown("Dash") && mState == States.Normal)
            StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        mState = States.Dashing;
        Vector2 direction = (shield.transform.position - transform.position).normalized;

        //if (Vector3.Dot(rb.velocity.normalized, direction) < 0)
        //direction *= -1;

        rb.velocity = direction * 5 * speed;

        yield return new WaitForSeconds(0.2f);

        rb.velocity = Vector2.zero;

        mState = States.Normal;
    }

    #endregion
}
