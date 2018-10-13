using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform bulletPool;

    [SerializeField]
    private float bulletSpeed;

	private void Awake ()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        if (bulletPool == null)
            bulletPool = GameObject.Find("Bullets").transform;
	}
	
	private void Start()
    {
        InvokeRepeating("Shoot", 0.5f, 1f);
    }

    private void Shoot()
    {
        if (player == null)
            return;
        
        Vector3 dif = (player.position - transform.position).normalized;
        Vector3 summonPos = transform.position;

        float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        Quaternion summonRot = Quaternion.Euler(0, 0, rot - 90);

        GameObject bul = Instantiate(bullet, summonPos, summonRot, transform);
        bul.name = bullet.name;
        bul.transform.parent = bulletPool;

        bul.GetComponent<Rigidbody2D>().velocity = dif * bulletSpeed;
    }
}
