using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveWorld : MonoBehaviour
{
    // public static event Action<GameObject> segmentSpawned;

    // void Update()
    // {
    //     this.transform.position += Vector3.back * GameManager.Instance.moveSpeed * Time.deltaTime;

    //     if (this.transform.position.z <= -10f)
    //     {
    //         Vector3 spawnPos = this.transform.position;

    //         spawnPos.z += GameManager.Instance.startSegments * 10f;

    //         transform.position = spawnPos;

    //         segmentSpawned?.Invoke(this.gameObject);

    //         //GameObject obj = Instantiate(this.gameObject, spawnPos, Quaternion.identity);
    //         //Destroy(this.gameObject);
    //     }
    // }
}