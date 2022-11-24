using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnObstacle { Nothing, Red, Green, Blue }

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstacle;

    public GameObject[] thingsToSpawn;
    public SpawnObstacle whatToSpawnNext;
    public Lane whereToSpawnNext;
    Lane currentlySpawningIn;
    Dictionary<SpawnObstacle, GameObject> spawnToGO;
    public bool spawnInRandomLane = true;


    public Material green;
    public Material blue;

    Dictionary<GameObject, GameObject> obstaclesAttachedToSegment;

    Stack<GameObject> redObstacles;
    Stack<GameObject> greenObstacles;
    Stack<GameObject> blueObstacles;
    Dictionary<SpawnObstacle, Stack<GameObject>> spawnPools;
    Dictionary<GameObject, SpawnObstacle> currentlyUsedFromPools;

    public static GameObject lastSegment;

    void OnEnable()
    {
        //MoveWorld.segmentSpawned += HandleNewSegment;
        //WorldMoverManager.ThingGotOffScreen += HandleNewSegment;

        //WorldMoverManager.ThingGotOffScreen += SpawnNext;
        WorldMoverManager.ThingGotOffScreen += ChangeLastSegment;
    }

    void OnDisable()
    {
        //MoveWorld.segmentSpawned -= HandleNewSegment;
        // WorldMoverManager.ThingGotOffScreen -= HandleNewSegment;

        //WorldMoverManager.ThingGotOffScreen -= SpawnNext;
        WorldMoverManager.ThingGotOffScreen -= ChangeLastSegment;
    }

    void Start()
    {
        obstaclesAttachedToSegment = new Dictionary<GameObject, GameObject>(GameManager.Instance.startSegments);

        PopulateThingsToSpawn();
        CreateSpawnPools();
    }

    void PopulateThingsToSpawn()
    {
        thingsToSpawn = new GameObject[3];

        obstacle.name = "Red Obstacle";
        thingsToSpawn[0] = obstacle;
        thingsToSpawn[0].SetActive(false);


        thingsToSpawn[1] = Instantiate(obstacle);
        thingsToSpawn[1].SetActive(false);
        thingsToSpawn[1].GetComponentInChildren<Renderer>().material = green;
        thingsToSpawn[1].name = "Green Obstacle";

        thingsToSpawn[2] = Instantiate(obstacle);
        thingsToSpawn[2].SetActive(false);
        thingsToSpawn[2].GetComponentInChildren<Renderer>().material = blue;
        thingsToSpawn[2].name = "Blue Obstacle";

        spawnToGO = new Dictionary<SpawnObstacle, GameObject>();
        spawnToGO[SpawnObstacle.Red] = thingsToSpawn[0];
        spawnToGO[SpawnObstacle.Green] = thingsToSpawn[1];
        spawnToGO[SpawnObstacle.Blue] = thingsToSpawn[2];

        redObstacles = new Stack<GameObject>();
        FillStack(redObstacles, thingsToSpawn[0], 10);
        greenObstacles = new Stack<GameObject>();
        FillStack(greenObstacles, thingsToSpawn[1], 10);
        blueObstacles = new Stack<GameObject>();
        FillStack(blueObstacles, thingsToSpawn[2], 10);
    }

    void FillStack(Stack<GameObject> stack, GameObject obj, int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            GameObject newObject = Instantiate(obj);
            newObject.SetActive(false);

            stack.Push(newObject);
        }
    }

    void CreateSpawnPools()
    {
        spawnPools = new Dictionary<SpawnObstacle, Stack<GameObject>>(3);

        spawnPools.Add(SpawnObstacle.Red, redObstacles);
        spawnPools.Add(SpawnObstacle.Green, greenObstacles);
        spawnPools.Add(SpawnObstacle.Blue, blueObstacles);

        currentlyUsedFromPools = new Dictionary<GameObject, SpawnObstacle>(10);
    }

    GameObject PopFromPool(SpawnObstacle next)
    {
        GameObject obj = spawnPools[next].Pop();
        currentlyUsedFromPools.Add(obj, next);

        if(obj.TryGetComponent(out Obstacle obstacle)){
            obstacle.currentLane = currentlySpawningIn;
        }

        obj.SetActive(true);
        return obj;
    }

    void PushToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = null;
        spawnPools[currentlyUsedFromPools[obj]].Push(obj);
        currentlyUsedFromPools.Remove(obj);
    }

    void SpawnNext(GameObject segment)
    {
        whatToSpawnNext = GetRandomSpawn();

        //Vector3 pos = segment.transform.position;
        Vector3 pos = Vector3.zero;
        //pos.z = 80f;
        pos.y = -80f;

        if (spawnInRandomLane)
        {
            currentlySpawningIn = LaneManager.GetRandomLane();
        }
        else
        {
            currentlySpawningIn = whereToSpawnNext;
        }
        pos.x = LaneManager.GetLaneCoordinates(currentlySpawningIn).x;
        //pos.y = 0.75f;
        pos.z = 1f;

        if (obstaclesAttachedToSegment.ContainsKey(segment))
        {
            //Destroy(obstaclesAttachedToSegment[segment]);
            GameObject oldObstacle = obstaclesAttachedToSegment[segment];

            PushToPool(obstaclesAttachedToSegment[segment]);
            obstaclesAttachedToSegment.Remove(segment);
        }

        if (whatToSpawnNext != SpawnObstacle.Nothing)
        {
            //GameObject newObstacle = Instantiate(spawnToGO[whatToSpawnNext], pos, Quaternion.identity);
            GameObject newObstacle = PopFromPool(whatToSpawnNext);

            newObstacle.transform.SetParent(segment.transform);
            newObstacle.transform.position = pos;

            obstaclesAttachedToSegment.Add(segment, newObstacle);
        }

        //DisplayPoolsInfo();
    }

    void ChangeLastSegment(GameObject segment){
        lastSegment = segment;

        //SpawnNext(lastSegment);
    }

    void DisplayPoolsInfo()
    {
        print($"Red Stack: {redObstacles.Count} Green Stack: {greenObstacles.Count} Blue Stack: {blueObstacles.Count}");
    }

    SpawnObstacle GetRandomSpawn()
    {
        float fate = Random.value;

        if (fate <= 0.33f)
        {
            return SpawnObstacle.Red;
        }
        else if (fate >= 0.66f)
        {
            return SpawnObstacle.Green;
        }
        else
        {
            return SpawnObstacle.Blue;
        }
    }

    // void HandleNewSegment(GameObject segment)
    // {
    //     Vector3 pos = segment.transform.position;
    //     pos.x = LaneManager.laneCoordinates[LaneManager.GetRandomLane()].x;
    //     pos.y = 0.75f;

    //     if (!obstaclesAttachedToSegment.ContainsKey(segment))
    //     {
    //         GameObject newObstacle = Instantiate(obstacle, pos, Quaternion.identity);
    //         newObstacle.transform.SetParent(segment.transform);
    //         obstaclesAttachedToSegment.Add(segment, newObstacle);
    //     }
    //     else
    //     {
    //         obstaclesAttachedToSegment[segment].transform.position = pos;
    //     }
    // }
}