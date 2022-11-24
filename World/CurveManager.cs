using UnityEngine;

public class CurveManager : MonoBehaviour
{
    public float curveX;
    public float curveY;

    public Material[] materials;
    int amount;

    int shaderIDx;
    int shaderIDy;

    void Start()
    {
        amount = materials.Length;
        GetPropertyIDs();
        SetCurve(curveX, curveY);

        // Shader.SetGlobalFloat(shaderIDx, curveX);
        // Shader.SetGlobalFloat(shaderIDy, curveY);
    }

    void OnDestroy()
    {
        SetCurve(0f, 0f);
    }

    void SetCurve(float x, float y)
    {   
        for (int i = 0; i < amount; i++)
        {
            materials[i].SetFloat(shaderIDx, x);
            materials[i].SetFloat(shaderIDy, y);
        }
    }

    [ContextMenu("Set Curve")]
    void callSetCurve()
    {
        SetCurve(curveX, curveY);
    }

    void GetPropertyIDs(){
        shaderIDx = Shader.PropertyToID("CurveX");
        shaderIDy = Shader.PropertyToID("CurveY");
    }

    // void SetCurve(float x, float y)
    // {
    //     Shader.SetGlobalFloat(shaderIDx, x);
    //     Shader.SetGlobalFloat(shaderIDy, y);
    // }
}