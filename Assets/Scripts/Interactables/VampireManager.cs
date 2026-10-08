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
    [SerializeField] GameObject[] barPositionGameObjects; //lets you scootch around bar positions
    private BarPosition[] barPositions;

    private float lastSpawned = float.MinValue;

    void Initialize()
    {
        lastSpawned = Time.time - (SpawnRate * 0.5f);
    }

    private Vector3 GetEmptyBarPos()
    {
        foreach (BarPosition item in barPositions)
        {
    
        }
        return Vector3.zero; //< LEFT OFF HERE
    }

    private void TrySpawnVamp()
    {
        if(vampirePrefabs == null) return;
        
    }


    void Update()
    {
        //sending out
        if(Time.time - lastSpawned > SpawnRate)
        {
            TrySpawnVamp();
        }

        //
    }
}
