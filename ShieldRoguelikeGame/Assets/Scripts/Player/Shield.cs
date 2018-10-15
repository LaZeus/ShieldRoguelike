using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public enum States { Normal, Storing, Shooting };


    public States mState;

    [SerializeField]
    private Camera Cam;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private SpriteRenderer spRndr;

    [SerializeField]
    private GameObject myProjectile;

    [SerializeField]
    private float distance;

    [SerializeField]
    private int projectileCounter = 0;

    [SerializeField]
    private int projectileMax;

    private float startTime;
    private float chargingTime = 2f;

    private void Awake()
    {
        if (Cam == null)
            Cam = GameObject.Find("Camera").GetComponent<Camera>();

        mState = States.Normal;
    }

    private void Update()
    {
        ShieldPos();
        StoreProjectiles();
    }
    #region Shield

    private void ShieldPos()
    {
        Vector2 point = Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 target = point - (Vector2)player.position;

        // position
        transform.localPosition = target.normalized * distance;

        // rotation
        float rot = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot - 90);
    }

    #endregion

    #region Shooting

    private void ShootProjectile()
    {
        /*if (Input.GetButtonDown("Attack") && projectileCounter > 0)
        {
            Shoot();
        }*/
    }

    /// 
    /// REFACTORING
    /// 

    private void Shoot()
    {
        projectileCounter--;
        GameObject proj = Instantiate(myProjectile, transform.position, transform.rotation, null);
        proj.name = myProjectile.name;
        proj.GetComponent<Rigidbody2D>().velocity = transform.up * 15;       
    }

    /// 
    /// REFACTORING
    /// 

    private void StoreProjectiles()
    {
        if (Input.GetButton("Attack"))
        {
            if (mState == States.Normal)
            {
                mState = States.Storing;
                startTime = Time.time;
                spRndr.color = Color.green;
            }
            else if(Time.time - startTime > chargingTime && mState !=States.Shooting)
            {
                mState = States.Shooting;
                StartCoroutine(ShootChargedProjectiles(projectileCounter));
            }
        }
        else if(mState == States.Storing)
        {
            mState = States.Shooting;
            StartCoroutine(ShootChargedProjectiles(projectileCounter));
        }
    }

    /// 
    /// REFACTORING
    /// 

    IEnumerator ShootChargedProjectiles(int projCounter)
    {
        spRndr.color = Color.red;

        for (int i = 0; i < projCounter; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f);
        }

        mState = States.Normal;
        spRndr.color = Color.yellow;
    }

    /// 
    /// REFACTORING
    /// 

    private void ProjectileDeflected(int projectilesAdded)
    {
        if (projectileCounter + projectilesAdded > projectileMax)
            projectileCounter = projectileMax;
        else
            projectileCounter += projectilesAdded;
    }

    #endregion

}
