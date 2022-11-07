using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyShaderTester : MonoBehaviour
{
    [SerializeField] int arraySize = 100;
    [SerializeField] private float scale = 20f;
    [SerializeField] private Texture2D tex;
    [SerializeField] private GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        GameObject[] GoArray = new GameObject[tex.width * tex.height];
        
        for(int x = 0; x < tex.width; x++) 
        {
            for (int z = 0; z < tex.height; z++)
            {
                GameObject go = GameObject.Instantiate(cube);
                Color c = tex.GetPixel(x, z);
                float val = c.r;
                go.transform.position = new Vector3(x, val * scale, z);
            }
        }
        
    }
}
