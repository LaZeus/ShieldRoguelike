using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed;

    private int turretSpot;

    public static int counter = 0;

	private void Awake ()
    {       
        scoreValue = 5;
	}
	
	private void Start()
    {
        FindPlayer();
    }

    private void OnEnable()
    {
        InvokeRepeating("Shoot", 2f, 0.75f);
    }

    protected override void OnDisable()
    {
        Disabling();

        if(GM != null)
            GM.SendMessage("TurretDied", turretSpot);

        CancelInvoke("Shoot");
    }

    private void Positioned(int t)
    {
        turretSpot = t;
    }

    private void Shoot()
    {
        if (player == null)
            return;

        counter++;
        Debug.Log(counter);

        Vector3 dif = (player.position - transform.position).normalized;
        Vector3 summonPos = transform.position;

        float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        Quaternion summonRot = Quaternion.Euler(0, 0, rot - 90);

        GameObject bul = ObjectPooler.SharedInstance.GetPooledObject(2); // 2 is bullet

        if (bul == null)
            Debug.LogWarning("bullet is null");

        bul.transform.position = summonPos;
        bul.transform.rotation = summonRot;

        bul.name = bullet.name;

        bul.SetActive(true);

        bul.GetComponent<Rigidbody2D>().velocity = dif * bulletSpeed;
    }
}
