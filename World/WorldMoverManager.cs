using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Movable
{
    public GameObject thing;
    public bool isMarkedForRemoval;
}

public class WorldMoverManager : MonoBehaviour
{
    public static event Action<GameObject> ThingGotOffScreen;

    public float speed = 20f;

    public static GameObject[] thingsToMove = new GameObject[100]; //hardcoded size, check loops
    static int thingsIndex = 0;

    Vector3 passedDistance;

    void OnDisable()
    {
        Array.Clear(thingsToMove, 0, thingsToMove.Length);
        thingsIndex = 0;
    }

    void Update()
    {
        //passedDistance = Vector3.back * speed * Time.deltaTime;
        passedDistance = Vector3.up * speed * Time.deltaTime;

        for (int i = 0; i < 100; i++)
        {
            if (thingsToMove[i] != null)
            {
                MoveThings(i);
                HandleLooping(i);
            }
            else
            {
                //print("Got out of loop");
                return;
            }
        }
    }

    void MoveThings(int i)
    {
        thingsToMove[i].transform.position += passedDistance;
    }

    void HandleLooping(int i)
    {
        // if (thingsToMove[i].transform.position.z <= -10f)
        // {
        //     Vector3 spawnPos = thingsToMove[i].transform.position;

        //     spawnPos.z += GameManager.Instance.startSegments * 10f;

        //     thingsToMove[i].transform.position = spawnPos;

        //     ThingGotOffScreen?.Invoke(thingsToMove[i]);
        // }

        if (thingsToMove[i].transform.position.y >= 10f)
        {
            Vector3 spawnPos = thingsToMove[i].transform.position;

            spawnPos.y -= GameManager.Instance.startSegments * 10f;

            thingsToMove[i].transform.position = spawnPos;

            ThingGotOffScreen?.Invoke(thingsToMove[i]);
        }
    }

    public static void AddThing(GameObject thing)
    {
        thingsToMove[thingsIndex] = thing;
        thingsIndex++;
    }
}
