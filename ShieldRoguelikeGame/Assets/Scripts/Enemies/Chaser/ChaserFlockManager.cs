using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserFlockManager : MonoBehaviour
{
    public enum Formations { None, Line, Surround };
    
    [HideInInspector] public Formations formation = Formations.None;
    [HideInInspector] public List<ChaserEntry> chasers;

    [SerializeField] private int updateDelay;

    [Header("No Formation")]

    [SerializeField] private float repulsionThreshold = 2; // beyond this distance, the chasers won't try to move away from eachother
    [SerializeField] private float repulsionPower = 2; // how much the chasers want to get away from eachother

    [Header("Line Formation")]

    [SerializeField] private float distance = 2; // the distance between the chasers

    [Header("Surround Formation")]

    [SerializeField] private float surroundRadius = 5; // the radius that the chasers will leave around the player
    [SerializeField] private float surroundRotateSpeed = 1; // the speed that the chasers will rotate around the player

    private float surroundAngleOffset = 0;
    private Transform player;

    private int framesSinceUpdate = 0;

    private void Awake()
    {
        chasers = new List<ChaserEntry>();
    }

    private void Update()
    {
        framesSinceUpdate++;
        
        surroundAngleOffset += Time.deltaTime * surroundRotateSpeed;

        while (surroundAngleOffset > 360)
        {
            surroundAngleOffset -= 360;
        }

        PruneChasers();

        if (framesSinceUpdate > updateDelay)
        {
            framesSinceUpdate = 0;
            RecalculatePositions();
        }
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
                        chasers[i].WantedPosition = GetHeading(chasers[i].CurrentPosition, nearby[j].CurrentPosition) * repulsionPower;
                        //chasers[i].Reference.transform.position = Vector2.MoveTowards(chasers[i].CurrentPosition, chasers[i].CurrentPosition + (chasers[i].CurrentPosition - nearby[j].CurrentPosition).normalized * 100, repulsionPower * Time.deltaTime);
                    }
                }

                break;

            case Formations.Line:
                float lineOffset = ((chasers.Count - 1) * distance) / 2;

                for (int i = 0; i < chasers.Count; i++)
                {
                    float cur = distance * i;
                    chasers[i].WantedPosition = GetPlayerPosition() + new Vector3(cur - lineOffset, 0, 0);
                }

                break;

            case Formations.Surround:
                float key = 360 / chasers.Count;

                for (int i = 0; i < chasers.Count; i++)
                {
                    chasers[i].WantedPosition = GetSurroundPosition(GetPlayerPosition(), surroundRadius, key * i);
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
        Vector3 offset;
        offset.x = radius * Mathf.Cos(angle * Mathf.Deg2Rad + surroundAngleOffset);
        offset.y = radius * Mathf.Sin(angle * Mathf.Deg2Rad + surroundAngleOffset);
        offset.z = 0;

        return center + offset;
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