using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public float laneDistance = 1.5f;

    static Dictionary<Lane, Vector3> laneCoordinates;

    void Start()
    {
        SetLaneCoordinates();
    }

    void SetLaneCoordinates()
    {
        laneCoordinates = new Dictionary<Lane, Vector3>();

        Vector3 laneCoord = GameObject.Find("Player").transform.position;
        laneCoordinates.Add(Lane.Middle, laneCoord);
        //print(laneCoord);

        laneCoord.x = laneDistance;
        laneCoordinates.Add(Lane.Right, laneCoord);
        //print(laneCoord);

        laneCoord.x = -laneDistance;
        laneCoordinates.Add(Lane.Left, laneCoord);
        //print(laneCoord);
    }

    public static Lane GetRandomLane()
    {
        // float fate = Random.value;

        // if(fate <= 0.33f){
        //     return Lane.Left;
        // }else if(fate >= 0.66f){
        //     return Lane.Right;
        // }else{
        //     return Lane.Middle;
        // }

        int fate = Random.Range(0, 3);

        switch(fate){
            case 0:
                return Lane.Left;
            case 1:
                return Lane.Middle;
            default:
                return Lane.Right;
        }
    }

    public static Vector3 GetLaneCoordinates(Lane lane){
        switch(lane){

            case Lane.Left:
                return laneCoordinates[Lane.Left];

            case Lane.Middle:
                return laneCoordinates[Lane.Middle];

            case Lane.Right:
                return laneCoordinates[Lane.Right];

            default:
                return laneCoordinates[GetRandomLane()];
        }
    }
}
