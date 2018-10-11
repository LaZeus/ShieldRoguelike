using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float bulletSpeed;

	private void Awake ()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
	}
	
	private void Start()
    {
        InvokeRepeating("Shoot", 1.0f, 1f);
    }

    private void Shoot()
    {
        Vector3 dif = (player.position - transform.position).normalized;
        Vector3 summonPos = transform.position + dif * 2;

        float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        Quaternion summonRot = Quaternion.Euler(0, 0, rot - 90);

        GameObject bul = Instantiate(bullet, summonPos, summonRot, transform);
        bul.name = bullet.name;

        bul.GetComponent<Rigidbody2D>().velocity = dif * bulletSpeed;
    }
}
