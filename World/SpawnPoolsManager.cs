using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObject
{
    public Spawn name;
    public GameObject obj;
    public int poolSize;
}

public class SpawnPoolsManager : MonoBehaviour
{
    static Dictionary<Spawn, Stack<GameObject>> spawnableObjects;

    public SpawnableObject[] objects;

    void Awake()
    {
        CreateSpawnPools();
    }

    void CreateSpawnPools()
    {
        int amount = objects.Length;
        spawnableObjects = new Dictionary<Spawn, Stack<GameObject>>(amount);

        for (int i = 0; i < objects.Length; i++)
        {
            SpawnableObject currentSO = objects[i];

            spawnableObjects.Add(currentSO.name, new Stack<GameObject>(currentSO.poolSize));

            for (int j = 0; j < currentSO.poolSize; j++)
            {
                spawnableObjects[currentSO.name].Push(Instantiate(currentSO.obj));
            }
        }

        System.GC.Collect(); //???
    }

    public static GameObject GetFromPool(Spawn name, Lane spawnLane, SliceableFrom sliceableFrom = SliceableFrom.Auto)
    {
        
        if (spawnableObjects[name].Count > 0)
        {
            GameObject obj = spawnableObjects[name].Pop();
            obj.SetActive(true);
            obj.GetComponent<Obstacle>().SetLaneAndSliceable(spawnLane, sliceableFrom);
            return obj;
        }
        else
        {
            print($"Pool of {name} is empty");
            return null;
        }
    }

    public static void PutIntoPool(GameObject obj, Spawn name)
    {
        obj.SetActive(false);
        obj.transform.parent = null;
        spawnableObjects[name].Push(obj);
    }

    public static void DisplayPoolsInfo()
    {
        foreach (var kv in spawnableObjects)
        {
            print($"{kv.Key} - {kv.Value.Count}");
        }
    }
}
