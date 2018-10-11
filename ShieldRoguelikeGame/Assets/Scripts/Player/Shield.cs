using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField]
    private Camera Cam;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float distance;

    private void Awake()
    {
        if (Cam == null)
            Cam = GameObject.Find("Camera").GetComponent<Camera>();        
    }

    private void Update()
    {
        ShieldPos();
    }

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
}
