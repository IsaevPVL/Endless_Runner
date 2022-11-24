using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum Go { Left, Right }
public enum ControlScheme { Swipe, Tap, DeltaSwipe }

public class PlayerControl : MonoBehaviour
{
    public bool haveControl = true;

    public static ControlScheme currentScheme = ControlScheme.Tap;

    public float swipeSensitivity = 0.25f;
    public float minSwipeSpeed = 0f;
    float swipeSensitivityDPI;
    float screenCentreX;

    Vector2 touchStart;
    Vector2 touchEnd;
    float distance;

    Rigidbody rb;

    public Lane currentLane = Lane.Middle;

    public static event Action ObstacleHit;
    public static event Action ObstaclePassed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        swipeSensitivityDPI = swipeSensitivity * Screen.dpi;
        screenCentreX = Screen.width / 2f;
        //SetLaneCoordinates();
    }

    void Update()
    {
        if (!haveControl)
        {
            return;
        }
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchLanes(Go.Left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SwitchLanes(Go.Right);
        }
#elif UNITY_ANDROID || UNITY_IOS || UNITY_REMOTE

        if (Input.touchCount > 0)
        {
            HandleTouch(Input.GetTouch(0));
        }

#endif
    }

    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("ObstacleHit"))
        // {
        //     //print("Hit");
        //     ObstacleHit?.Invoke();
        //     haveControl = false;
        // }
        // else if (other.gameObject.CompareTag("ObstaclePassed"))
        // {
        //     //print("+ Score!");
        //     ObstaclePassed?.Invoke();
        // }

        //print(other.name);

        // Obstacle obstacle = other.GetComponentInParent<Obstacle>();
        // if (obstacle != null)
        // {
        //     int hit = -1;

        //     switch (other.tag)
        //     {
        //         case "PassedLeft":
        //             //print("Left");
        //             hit = 1;
        //             break;
        //         case "PassedMiddle":
        //             //print("Middle");
        //             hit = 2;
        //             break;
        //         case "PassedRight":
        //             //print("Right");
        //             hit = 3;
        //             break;
        //         default:
        //             return;
        //     }

        //     //print(obstacle.hitActive + " : " + hit);

        //     if(hit == obstacle.hitActive){
        //         print($"Sliced from {hit}");
        //     }
            
        // }
    }

    public void HandleTouch(Touch touch)
    {
        switch (currentScheme)
        {
            case ControlScheme.Swipe:
                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    touchEnd = touch.position;
                    distance = touchEnd.x - touchStart.x;

                    if (Mathf.Abs(distance) < swipeSensitivityDPI)
                    {
                        return;
                    }

                    if (distance < 0)
                    {
                        SwitchLanes(Go.Left);
                    }
                    else
                    {
                        SwitchLanes(Go.Right);
                    }
                }
                break;

            case ControlScheme.Tap:
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < screenCentreX)
                    {
                        SwitchLanes(Go.Left);
                    }
                    else
                    {
                        SwitchLanes(Go.Right);
                    }
                }
                break;

            case ControlScheme.DeltaSwipe:
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;
                    print(delta);

                    float magnitude = delta.magnitude;
                    float touchSpeed = magnitude / Time.deltaTime;
                    print(touchSpeed);

                    if (touchSpeed > minSwipeSpeed)
                        if (delta.x < 0)
                        {
                            SwitchLanes(Go.Left);
                        }
                        else if (delta.x > 0)
                        {
                            SwitchLanes(Go.Right);
                        }
                }
                break;

            default:
                break;

        }
    }

    void SwitchLanes(Go direction)
    {
        switch (direction)
        {
            case Go.Right:
                if (currentLane == Lane.Middle)
                {
                    currentLane = Lane.Right;
                    break;
                }
                else if (currentLane == Lane.Left)
                {
                    currentLane = Lane.Middle;
                    break;
                }
                else break;

            case Go.Left:
                if (currentLane == Lane.Right)
                {
                    currentLane = Lane.Middle;
                    break;
                }
                else if (currentLane == Lane.Middle)
                {
                    currentLane = Lane.Left;
                    break;
                }
                else break;

            default:
                break;

        }
        rb.MovePosition(LaneManager.GetLaneCoordinates(currentLane));

        // Vector3 moveTo = transform.position;
        // moveTo.x = _lane;
        // //transform.position = moveTo;
        // rb.MovePosition(moveTo);
    }
}