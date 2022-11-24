using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Spawn objectName;

    public GameObject spotLeft;
    public GameObject spotMiddle;
    public GameObject spotRight;

    public Collider left;
    public Collider middle;
    public Collider right;

    public Lane currentLane;
    public GameObject currentlyActive;
    public Lane sliceableFromLane;
    public SliceableFrom sliceableFrom;

    void OnEnable()
    {
        //SetRandomSpotActive();

    }

    void OnDisable()
    {
        // spotLeft.SetActive(false);
        // spotMiddle.SetActive(false);
        // spotRight.SetActive(false);

        currentlyActive.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            HandleContact(player.currentLane);
        }
    }

    void HandleContact(Lane contactDirection)
    {
        if (contactDirection == sliceableFromLane)
        {
            print("Object Sliced");
        }
    }

    //TRY SOMETHING ELSE!
    void FixedUpdate()
    {
        if (transform.position.y > 10f)
        {
            SpawnPoolsManager.PutIntoPool(this.gameObject, objectName);
        }
    }

    public void SetLaneAndSliceable(Lane _currentLane, SliceableFrom _sliceableFrom)
    {
        currentLane = _currentLane;

        if (_sliceableFrom == SliceableFrom.Auto)
        {
            SetRandomSpotActive();
        }
        else
        {
            sliceableFrom = _sliceableFrom;
            SetSpotActive(sliceableFrom);
        }

        SetSliceableFromLane();
    }

    void SetSpotActive(SliceableFrom _sliceableFrom)
    {
        switch (_sliceableFrom)
        {
            case SliceableFrom.Left:
                spotLeft.SetActive(true);
                currentlyActive = spotLeft;
                return;
            case SliceableFrom.Middle:
                spotMiddle.SetActive(true);
                currentlyActive = spotMiddle;
                return;
            case SliceableFrom.Right:
                spotRight.SetActive(true);
                currentlyActive = spotRight;
                return;
        }
    }

    void SetSliceableFromLane()
    {
        if (sliceableFrom == SliceableFrom.Middle)
        {
            sliceableFromLane = currentLane;
        }
        else
        {
            if (sliceableFrom == SliceableFrom.Left)
            {
                switch (currentLane)
                {
                    case Lane.Left:
                        sliceableFromLane = Lane.None;
                        return;
                    case Lane.Middle:
                        sliceableFromLane = Lane.Left;
                        return;
                    case Lane.Right:
                        sliceableFromLane = Lane.Middle;
                        return;
                }
            }
            else if (sliceableFrom == SliceableFrom.Right)
            {
                switch (currentLane)
                {
                    case Lane.Left:
                        sliceableFromLane = Lane.Middle;
                        return;
                    case Lane.Middle:
                        sliceableFromLane = Lane.Right;
                        return;
                    case Lane.Right:
                        sliceableFromLane = Lane.None;
                        return;
                }
            }
        }
    }

    void SetRandomSpotActive()
    {
        switch (currentLane)
        {
            case Lane.Left:
                int fate = Random.Range(0, 2);

                if (fate == 0)
                {
                    spotMiddle.SetActive(true);
                    currentlyActive = spotMiddle;
                    sliceableFrom = SliceableFrom.Middle;

                    //sliceableFromLane = Lane.Left;
                    return;
                }
                else
                {
                    spotRight.SetActive(true);
                    currentlyActive = spotRight;
                    sliceableFrom = SliceableFrom.Right;

                    //sliceableFromLane = Lane.Middle;
                    return;
                }

            case Lane.Middle:
                fate = Random.Range(0, 3);

                switch (fate)
                {
                    case 0:
                        spotLeft.SetActive(true);
                        currentlyActive = spotLeft;
                        sliceableFrom = SliceableFrom.Left;

                        //sliceableFromLane = Lane.Left;
                        return;
                    case 1:
                        spotMiddle.SetActive(true);
                        currentlyActive = spotMiddle;
                        sliceableFrom = SliceableFrom.Middle;

                        //sliceableFromLane = Lane.Middle;
                        return;
                    case 2:
                        spotRight.SetActive(true);
                        currentlyActive = spotRight;
                        sliceableFrom = SliceableFrom.Right;

                        //sliceableFromLane = Lane.Right;
                        return;
                }
                return;

            case Lane.Right:
                fate = Random.Range(0, 2);

                if (fate == 0)
                {
                    spotMiddle.SetActive(true);
                    currentlyActive = spotMiddle;
                    sliceableFrom = SliceableFrom.Middle;

                    //sliceableFromLane = Lane.Right;
                    return;
                }
                else
                {
                    spotLeft.SetActive(true);
                    currentlyActive = spotLeft;
                    sliceableFrom = SliceableFrom.Left;

                    //sliceableFromLane = Lane.Middle;
                    return;
                }
        }
    }
}