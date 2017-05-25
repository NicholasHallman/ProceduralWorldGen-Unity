using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public bool autoUpdate;

    private GameObject[,] world;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

    public void ResetMap()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject obj in allObjects)
        {
            DestroyImmediate(obj);
        }
    }
    

}
