using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurveManager : MonoBehaviour
{
    public Vector3 origin;
    public float curve;
    public float flatThreshold;

    int originID;
    int curveID;
    int flatID;

    void Start() {
        originID = Shader.PropertyToID("_Origin");
        curveID = Shader.PropertyToID("_CurvatureStrength");
        flatID = Shader.PropertyToID("_FlatThreshold");

        
    }
}
