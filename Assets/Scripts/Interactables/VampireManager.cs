using System.Collections.Generic;

using UnityEngine;


public class BarPosition
{
    public Transform pos;
    public bool occupied;
}
public class VampireManager : MonoBehaviour
{
    [SerializeField] int SpawnRate; //delay in seconds from when another vamp is attempted to be spawned
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject[] vampirePrefabs;
    private List<GameObject> prevUsedVamps = new List<GameObject>();
    [SerializeField] int repeatCount;
    [SerializeField] GameObject[] barPositionGameObjects; //lets you scootch around bar positions
    private BarPosition[] barPositions;

    private float lastSpawned = float.MinValue;

    public void Initialize()
    {
        lastSpawned = Time.time - (SpawnRate * 0.5f);
        barPositions = new BarPosition[barPositionGameObjects.Length];
        for (int i = 0; i < barPositions.Length; i++)
        {
            BarPosition curBarPos = new BarPosition();
            curBarPos.pos = barPositionGameObjects[i].transform;
            curBarPos.occupied = false;
            barPositions[i] = curBarPos;
        }
    }

    private BarPosition GetEmptyBarPos()
    {
        foreach (BarPosition item in barPositions)
        {
            if(item.occupied == true) continue;
            item.occupied = true;
            return item;
        }
        return null; //< LEFT OFF HERE
    }

    private GameObject GetUnusedVampPrefab()
    {
        if (vampirePrefabs == null || vampirePrefabs.Length == 0) return null;

        foreach (GameObject item in vampirePrefabs)
        {
            if (item == null || prevUsedVamps.Contains(item)) continue;

            prevUsedVamps.Add(item);
            if(prevUsedVamps.Count > repeatCount) prevUsedVamps.RemoveAt(0);

            return item;
        }
        GameObject oldest = prevUsedVamps[0];
        prevUsedVamps.RemoveAt(0);

        prevUsedVamps.Add(oldest);

        return oldest;
    }

    private void TrySpawnVamp()
    {
        BarPosition pos = GetEmptyBarPos();
        if (pos == null) return;

        GameObject vamp = GetUnusedVampPrefab();
        if(vamp == null) return;
        
        
        vamp = Instantiate(vamp);
        VampireGeneralController vampController = vamp.GetComponent<VampireGeneralController>();

        vampController.transform.position = spawnPos.position;
        vampController.WalkTo(pos.pos.position);
        vampController.barPosition = pos;
        vampController.ReturnPoint = spawnPos.position;

        lastSpawned = Time.time;
    }

    void ResetData()
    {
        prevUsedVamps = new List<GameObject>();
    }

    void Update()
    {
        if(ServiceHub.Instance.gameState.gameplayState != GameState.GameplayState.Playing) return; //if youre not playing it doesnt bother
        //sending out
        //Debug.Log(Time.time + " | " + lastSpawned);
        if(Time.time - lastSpawned > SpawnRate)
        {
            TrySpawnVamp();
        }
        //
    }
}
