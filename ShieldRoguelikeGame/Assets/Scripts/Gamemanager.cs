using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
struct Points
{
    public Transform SpawnPoint;
    public bool AvailableSpawn;
}

public class Gamemanager : MonoBehaviour {

    private enum Enemies { Chaser, Turret };

    private Enemies mEnemy;

    [SerializeField]
    private Points[] SpawnPoints;

    private void Awake()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].AvailableSpawn = true;
        }
    }

    private void Start ()
    {
        InvokeRepeating("Spawn", 2, 10);
	}
	

    private void Spawn()
    {
        Debug.Log(Time.time);
        ChoosePoint();
       
    }

    private void ChoosePoint()
    {
        Points[] points = SpawnPoints.Where (o => o.AvailableSpawn == true).ToArray();

        if (points.Length != 0)
        {
            Debug.Log(points[Random.Range(0, points.Length)].SpawnPoint.name);
        }
        else
        {
            Debug.Log("Boop");
        }
    }

    private void PlayerDied()
    {
        Debug.Log("oof");
    }
}
