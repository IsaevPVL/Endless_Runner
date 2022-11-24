using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool isSpawning = true;
    public float dramaticPause = 1f;

    public SpawnCue currentCue;

    public CueBunch[] cueBunches;
    CueBunch currentCueBunch;
    int cueBunchLength;
    int currentCueBunchIndex;

    Lane currentSpawnLane;

    void Start()
    {
        StartCoroutine("Director");
    }


    IEnumerator Director()
    {
        yield return Helpers.GetWait(3);

        while (isSpawning)
        {
            //SpawnPoolsManager.DisplayPoolsInfo();

            if (currentCueBunch == null)
            {
                yield return Helpers.GetWait(dramaticPause);
                
                currentCueBunch = GetRandomCueBunch();
                cueBunchLength = currentCueBunch.cues.Length;
                currentCueBunchIndex = 0;
            }

            currentCue = currentCueBunch.cues[currentCueBunchIndex];

            yield return Helpers.GetWait(currentCue.delayBeforeSpawn);
            ExecuteCurrentCue();
        }
    }

    CueBunch GetRandomCueBunch()
    {
        int fate = Random.Range(0, cueBunches.Length);
        return cueBunches[fate];
    }

    void ExecuteCurrentCue()
    {
        switch (currentCue.spawnLane)
        {
            case Lane.None:
                RaiseIndex();
                return;
            case Lane.Random:
                currentSpawnLane = LaneManager.GetRandomLane();
                break;
            default:
                currentSpawnLane = currentCue.spawnLane;
                break;
        }
        //print(currentSpawnLane);

        GameObject obj = SpawnPoolsManager.GetFromPool(currentCue.objectName, currentSpawnLane, currentCue.sliceableFrom);

        if (obj != null)
        {
            Vector3 pos = Vector3.zero;
            pos.z = 1f;
            pos.y = -80f;
            pos.x = LaneManager.GetLaneCoordinates(currentSpawnLane).x;

            obj.transform.position = pos;
            obj.transform.SetParent(ObstacleManager.lastSegment.transform);
        }

        RaiseIndex();

        void RaiseIndex()
        {
            currentCueBunchIndex++;
            if (currentCueBunchIndex == cueBunchLength)
            {
                currentCueBunch = null;
                currentCueBunchIndex = 0;
            }
        }
    }
}