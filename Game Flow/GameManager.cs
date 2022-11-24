using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject segment;
    GameObject currentlyHandling;

    public int startSegments = 5;

    //public float moveSpeed = 6f;

    //public float laneX = 1.5f;

    public int score;

    public GameObject gameOverScreen;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        PlayerControl.ObstacleHit += ObstacleHit;
        PlayerControl.ObstacleHit += ObstaclePassed;
    }

    void OnDisable()
    {
        PlayerControl.ObstacleHit -= ObstacleHit;
        PlayerControl.ObstacleHit -= ObstaclePassed;
    }

    void Start()
    {
        StartGame();
        gameOverScreen.SetActive(false);
        //PlayerControl.haveControl = true;
    }

    public void StartGame()
    {
        Vector3 spawnPos = Vector3.zero;

        for (int i = 0; i < startSegments; i++)
        {
            currentlyHandling = Instantiate(segment, spawnPos, Quaternion.Euler(90f, 0f, 0f));

            WorldMoverManager.AddThing(currentlyHandling);

            //spawnPos.z += 10f;
            spawnPos.y -= 10f;
        }
    }

    void ObstacleHit()
    {
        //PlayerControl.haveControl = false;
        gameOverScreen.SetActive(true);
    }

    void ObstaclePassed()
    {
        score++;
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}