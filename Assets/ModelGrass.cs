using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGrass : MonoBehaviour {
    public int resolution = 100;
    public int scale = 1;
    public float displacementStrength = 200.0f;
    public Material grassMaterial;
    public Mesh grassMesh;
    public Texture heightMap;

    public bool updateGrass;

    private ComputeShader initializeGrassShader, generateWindShader;
    private ComputeBuffer grassDataBuffer, argsBuffer;

    private RenderTexture wind;

    private struct GrassData {
        public Vector4 position;
        public Vector2 uv;
        public float displacement;
    }

    void OnEnable() {
        resolution *= scale;
        initializeGrassShader = Resources.Load<ComputeShader>("GrassPoint");
        generateWindShader = Resources.Load<ComputeShader>("WindNoise");
        grassDataBuffer = new ComputeBuffer(resolution * resolution, 4 * 7);
        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);

        wind = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        wind.enableRandomWrite = true;
        wind.Create();

        updateGrassBuffer();
    }

    void updateGrassBuffer() {
        initializeGrassShader.SetInt("_Dimension", resolution);
        initializeGrassShader.SetInt("_Scale", scale);
        initializeGrassShader.SetBuffer(0, "_GrassDataBuffer", grassDataBuffer);
        initializeGrassShader.SetTexture(0, "_HeightMap", heightMap);
        initializeGrassShader.SetFloat("_DisplacementStrength", displacementStrength);
        initializeGrassShader.Dispatch(0, Mathf.CeilToInt(resolution / 8.0f), Mathf.CeilToInt(resolution / 8.0f), 1);

        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        // Arguments for drawing mesh.
        // 0 == number of triangle indices, 1 == population, others are only relevant if drawing submeshes.
        args[0] = (uint)grassMesh.GetIndexCount(0);
        args[1] = (uint)grassDataBuffer.count;
        args[2] = (uint)grassMesh.GetIndexStart(0);
        args[3] = (uint)grassMesh.GetBaseVertex(0);
        argsBuffer.SetData(args);

        generateWindShader.SetTexture(0, "_WindMap", wind);
        generateWindShader.Dispatch(0, Mathf.CeilToInt(wind.width / 8.0f), Mathf.CeilToInt(wind.height / 8.0f), 1);

        grassMaterial.SetBuffer("positionBuffer", grassDataBuffer);
        grassMaterial.SetFloat("_Rotation", 0.0f);
        grassMaterial.SetFloat("_DisplacementStrength", displacementStrength);
        grassMaterial.SetTexture("_WindTex", wind);
    }

    void Update() {
        grassMaterial.SetBuffer("positionBuffer", grassDataBuffer);
        grassMaterial.SetFloat("_Rotation", 0.0f);
        grassMaterial.SetFloat("_DisplacementStrength", displacementStrength);
        Graphics.DrawMeshInstancedIndirect(grassMesh, 0, grassMaterial, new Bounds(Vector3.zero, new Vector3(-500.0f, 200.0f, 500.0f)), argsBuffer);

        if (updateGrass) {
            updateGrassBuffer();
            updateGrass = false;
        }
    }
    
    void OnDisable() {
        grassDataBuffer.Release();
        argsBuffer.Release();
        wind.Release();
        grassDataBuffer = null;
        argsBuffer = null;
        wind = null;
    }
}
