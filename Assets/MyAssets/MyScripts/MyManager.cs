using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyManager : MonoBehaviour
{

    public Renderer tapisRendererMat;

    public static bool isGameRunning = true;
    public float tapisRotationSpeed = 50;


    private void Update()
    {
        Rotate_Tapis_Texture_Speed();
    }

    void Rotate_Tapis_Texture_Speed()
    {
        if (isGameRunning)
        {
            tapisRendererMat.material.SetTextureOffset("_MainTex", Vector2.left * tapisRotationSpeed * Time.deltaTime);
        }
    }

}
