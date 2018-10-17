using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChaserFlockManager
{
    private static Transform player;
    private static List<ChaserEntry> chasers = new List<ChaserEntry>();

    public static ChaserEntry AddChaser(Chaser chaser)
    {
        ChaserEntry entry = new ChaserEntry(chaser);

        chasers.Add(entry);
        return entry;
    }

    public static void RecalculatePositions()
    {
        for (int i = 0; i < chasers.Count; i++)
        {
            chasers[i].WantedPosition = GetPlayerPosition();
        }
    }

    private static Vector3 GetPlayerPosition()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

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