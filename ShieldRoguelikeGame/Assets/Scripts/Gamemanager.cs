﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private TextMeshProUGUI highscore;

    [SerializeField]
    private float scorePoints;

    [SerializeField]
    private float scoreOverTime;

    [SerializeField]
    private GameObject EndScreenMenu;


    static private int highscorePoints;

    private float[] spawnQuantity = new float[2];

    private void Awake()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].AvailableSpawn = true;
        }

        for (int i = 0; i < spawnQuantity.Length; i++)
        {
            spawnQuantity[i] = 1;
        }

        scoreOverTime = 3f / 600f;

        UpdateScore();
    }

    private void Start ()
    {
        InvokeRepeating("Spawn", 3, 6);
	}

    private void Update()
    {
        scorePoints += scoreOverTime;
        UpdateScore();
    }

    private void IncreaseScore(int value)
    {
        scorePoints += value;
    }

    private void UpdateScore()
    {
        score.text = "Score: " + (int)scorePoints;
    }

    private void Spawn()
    {
        Debug.Log(Time.time);
        StartCoroutine(ChoosePoint());
       
    }

    IEnumerator ChoosePoint()
    {
        Points[] points = SpawnPoints.Where (o => o.AvailableSpawn == true).ToArray();

        for (int i = 0; i < spawnQuantity[0]; i++)
        {
            if (points.Length != 0)
            {
                GameObject turret = ObjectPooler.SharedInstance.GetPooledObject(1);
                turret.transform.position = points[Random.Range(0, points.Length)].SpawnPoint.position;
                turret.SetActive(true);
            }
            yield return new WaitForSeconds(0.15f);
        }

        for (int i = 0; i < spawnQuantity[1]; i++)
        {
            GameObject chaser = ObjectPooler.SharedInstance.GetPooledObject(0);
            chaser.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].SpawnPoint.position;
            chaser.SetActive(true);

            yield return new WaitForSeconds(0.1f);
        }

        spawnQuantity[0] += 0.4f;
        spawnQuantity[1] += 0.8f;
    }

    private void PlayerDied()
    {
        Debug.Log("oof");
        Time.timeScale = 0;

        score.gameObject.SetActive(false);
        EndScreenMenu.SetActive(true);
        EndScreenMenu.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "SCORE: " + (int)scorePoints;

        if (highscorePoints < scorePoints)
            highscorePoints = (int)scorePoints;

        highscore.text = "HIGHSCORE: " + highscorePoints;
    }
}
