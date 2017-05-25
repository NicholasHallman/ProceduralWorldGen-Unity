using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Chunk : MonoBehaviour {

    public int x;
    public int y;
    public int distance;
    public GameObject blocks;
    public int biom = 0;
    public bool filled = false;
    private float[,] noiseMap;
    
   
    private bool first = true;

    public void drawChunk(int x, int y, int distance)
    {
        if (filled)
        {
            DeleteChunk();
        }
        if(distance == 0)
        {
            distance = 1;
        }
        this.x = x;
        this.y = y;
        noiseMap = Noise.GenerateNoiseMapPlayer((x) * 32, (y) * 32, distance, biom);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        blocks = display.DrawMapMesh(noiseMap, (x) * 32, (y) * 32, distance);
        filled = true;
    }
	
    public void DeleteChunk()
    {
        Destroy(blocks);
        filled = false;
    }
}
