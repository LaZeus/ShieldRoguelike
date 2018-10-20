using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed;

	private void Awake ()
    {
        FindPlayer();
	}
	
	private void Start()
    {
        InvokeRepeating("Shoot", 2f, 0.5f);
    }

    protected override void OnDisable()
    {
        if (c != 0)
        {
            cam.ShakeCamera(1f, 0.4f);
            SpawnParticles();
        }
        c++;

        CancelInvoke("Shoot");
    }

    private void Shoot()
    {
        if (player == null)
            return;
        
        Vector3 dif = (player.position - transform.position).normalized;
        Vector3 summonPos = transform.position;

        float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        Quaternion summonRot = Quaternion.Euler(0, 0, rot - 90);

        GameObject bul = ObjectPooler.SharedInstance.GetPooledObject(2); // 2 is bullet

        bul.transform.position = summonPos;
        bul.transform.rotation = summonRot;

        bul.name = bullet.name;

        bul.SetActive(true);

        bul.GetComponent<Rigidbody2D>().velocity = dif * bulletSpeed;
    }
}
