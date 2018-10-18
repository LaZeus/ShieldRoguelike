using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserFlockManager : MonoBehaviour
{
    private enum Formations { None, Line, Surround };
    [SerializeField] private Formations formation = Formations.None;

    [Header("No Formation")]

    [SerializeField] private float repulsionThreshold = 2; // beyond this distance, the chasers won't try to move away from eachother
    [SerializeField] private float repulsionPower = 2; // how much the chasers want to get away from eachother

    [Header("Line Formation")]

    [SerializeField] private float distance = 2; // the distance between the chasers

    [Header("Surround Formation")]

    [SerializeField] private float surroundRadius = 5; // the radius that the chasers will leave around the player (in surround mode)
    [SerializeField] private float surroundAngleOffset = 0; // the surround angle offset

    private Transform player;
    private List<ChaserEntry> chasers;

    private void Start()
    {
        chasers = new List<ChaserEntry>();
    }

    private void Update()
    {
        RecalculatePositions();
        PruneChasers();
    }

    public ChaserEntry AddChaser(Chaser chaser)
    {
        ChaserEntry entry = new ChaserEntry(chaser);
        chasers.Add(entry);
        return entry;
    }

    public void RecalculatePositions()
    {
        switch (formation)
        {
            case Formations.None: 
                for (int i = 0; i < chasers.Count; i++)
                {
                    chasers[i].WantedPosition = GetPlayerPosition();

                    List<ChaserEntry> nearby = new List<ChaserEntry>();

                    for (int j = 0; j < chasers.Count; j++)
                    {
                        if (IsNearby(chasers[i], chasers[j]))
                            nearby.Add(chasers[j]);
                    }
                    
                    for (int j = 0; j < nearby.Count; j++)
                    {
                        // FIXME: shouldn't directly apply movement to the reference
                        chasers[i].Reference.transform.position = Vector2.MoveTowards(chasers[i].CurrentPosition, chasers[i].CurrentPosition + (chasers[i].CurrentPosition - nearby[j].CurrentPosition).normalized * 100, repulsionPower * Time.deltaTime);
                    }
                }

                break;

            case Formations.Line:
                float lineOffset = ((chasers.Count - 1) * distance) / 2;

                for (int i = 0; i < chasers.Count; i++)
                {
                    Debug.Log(chasers[i].Reference);
                    float cur = distance * i;
                    chasers[i].WantedPosition = GetPlayerPosition() + new Vector3(cur - lineOffset, 0, 0);
                }

                break;

            case Formations.Surround:
                for (int i = 0; i < chasers.Count; i++)
                {
                    chasers[i].WantedPosition = GetSurroundPosition(GetPlayerPosition(), surroundRadius, 360 / chasers.Count * i + surroundAngleOffset);
                }

                break;
        }
    }

    private void PruneChasers()
    {
        for (int i = 0; i < chasers.Count; i++)
        {
            if (chasers[i].Reference == null || !chasers[i].Reference.gameObject.activeInHierarchy)
                chasers.RemoveAt(i);
        }
    }

    private Vector3 GetSurroundPosition(Vector3 center, float radius, float angle)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle);
        pos.y = center.y + radius * Mathf.Cos(angle);
        pos.z = center.z;
        Debug.Log(pos);

        return pos;
    }

    private bool IsNearby(ChaserEntry a, ChaserEntry b)
    {
        return a != b && Vector3.Distance(a.CurrentPosition, b.CurrentPosition) < repulsionThreshold;
    }

    private Vector3 GetHeading(Vector3 target, Vector3 current)
    {
        return (target - current).normalized;
    }

    private Vector3 GetPlayerPosition()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        if (!player.gameObject.activeInHierarchy)
            return Vector3.zero;

        return player.position;
    }
}

public class ChaserEntry
{
    public Chaser Reference;
    public Vector3 CurrentPosition { get { return this.Reference.transform.position; } }
    public Vector3 WantedPosition;

    public ChaserEntry(Chaser reference)
    {
        Reference = reference;
        WantedPosition = reference.transform.position;
    }
}