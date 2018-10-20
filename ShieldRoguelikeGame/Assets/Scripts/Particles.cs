using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(Deactivation());
    }

    IEnumerator Deactivation()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
