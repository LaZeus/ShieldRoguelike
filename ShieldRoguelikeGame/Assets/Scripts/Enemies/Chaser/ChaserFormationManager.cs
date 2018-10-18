using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserFormationManager : MonoBehaviour
{
    [SerializeField] private Formation[] formations;

    private ChaserFlockManager flockManager;

    private void Start()
    {
        flockManager = FindObjectOfType<ChaserFlockManager>();

        flockManager.formation = ChaserFlockManager.Formations.None;
    }

    private void Update()
    {
        for (int i = 0; i < formations.Length; i++)
        {
            if (flockManager.chasers.Count > formations[i].MaxChasers || flockManager.chasers.Count < formations[i].MinChasers)
                continue;

            int num = Random.Range(0, 10000);

            if (num < formations[i].ChangeChance)
            {
                flockManager.formation = formations[i].FormationType;
                break;
            }
        }
    }
}

[System.Serializable]
public struct Formation
{
    public ChaserFlockManager.Formations FormationType;

    [Space]

    public int MinChasers;
    public int MaxChasers;

    [Space]

    public int ChangeChance;
}
