using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField]
    private Camera Cam;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject myProjectile;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float projectileCounter = 0;

    [SerializeField]
    private float projectileMax;

    private void Awake()
    {
        if (Cam == null)
            Cam = GameObject.Find("Camera").GetComponent<Camera>();        
    }

    private void Update()
    {
        ShieldPos();
        ShootProjectile();
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
        if (Input.GetButtonDown("Attack") && projectileCounter > 0)
        {
            GameObject proj = Instantiate(myProjectile, transform.position, transform.rotation, null);
            proj.name = myProjectile.name;
            proj.GetComponent<Rigidbody2D>().velocity = transform.up * 15;
        }
    }

    private void ProjectileDeflected(int projectilesAdded)
    {
        if (projectileCounter + projectilesAdded > projectileMax)
            projectileCounter = projectileMax;
        else
            projectileCounter += projectilesAdded;
        Debug.Log(projectileCounter);
    }

    #endregion

}
