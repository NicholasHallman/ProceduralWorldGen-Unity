using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.tag = "block";
                cube.transform.position = new Vector3(x, noiseMap[x, y] * 255, y);
            }
        }
        
    }

    public GameObject[,] DrawNoiseMapRelative(float[,] noiseMap,int playerX, int playerY,int distance)
    {
        GameObject[,] chunk = new GameObject[32/distance ,32/distance];
        for (int y = 0; y < 32/distance; y++)
        {
            for (int x = 0; x < 32/distance; x++)
            {
                int height = (int)(noiseMap[x, y] * 255);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.tag = "block";  
                cube.transform.position = new Vector3((x * distance) + playerX, 0, (y * distance) + playerY);
                cube.transform.localScale = new Vector3(distance, height, distance);
                chunk[x, y] = cube;
                /*
                if(distance == 1)
                {
                    cube.GetComponent<Renderer>().material.color = Color.magenta;
                } else if( distance == 2)
                {
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (distance == 4)
                {
                    cube.GetComponent<Renderer>().material.color = Color.cyan;
                }
                else if (distance == 8)
                {
                    cube.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (distance == 16)
                {
                    cube.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (distance == 32)
                {
                    cube.GetComponent<Renderer>().material.color = Color.red;
                }
                */
            }
        }
        return chunk;
    }

    public GameObject DrawMapMesh(float[,] noiseMap, int playerX, int playerY, int distance)
    {
        GameObject chunk = new GameObject();
        Mesh mesh;
        chunk.AddComponent<MeshRenderer>();
        chunk.GetComponent<MeshRenderer>().material.color = Color.white;
        chunk.AddComponent<MeshFilter>();
        chunk.GetComponent<MeshFilter>().mesh.Clear();
        chunk.GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.Clear();
        Vector3[] verts = new Vector3[(noiseMap.GetLength(0)) * (noiseMap.GetLength(1))];
        int[] tri = new int[noiseMap.GetLength(0) * noiseMap.GetLength(1) * 6];
        int i = 0;
        for(int y = 0; y < noiseMap.GetLength(0) / distance; y++)
        {
            for(int x = 0; x < noiseMap.GetLength(1) / distance; x++)
            {
                verts[i] = new Vector3((x * distance), noiseMap[x,y] * 255, (y * distance));
                if(playerX == 32 && playerY == 32)
                i++;
            }
        }

        for (int ti = 0, vi = 0, y = 0; y < (noiseMap.GetLength(0) - 3)/ distance; y++, vi++)
        {
            for (int x = 0; x < (noiseMap.GetLength(1) - 3) / distance; x++, ti += 6, vi++)
            {
                tri[ti] = vi;
                tri[ti + 3] = tri[ti + 2] = vi + 1;
                tri[ti + 4] = tri[ti + 1] = vi + (noiseMap.GetLength(1) / distance) + 1;
                tri[ti + 5] = vi + (noiseMap.GetLength(1) / distance) + 2;
            }
        }
        mesh.vertices = verts;
        mesh.triangles = tri;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        chunk.transform.position = new Vector3(playerX, 0, playerY);
        //chunk.transform.localScale = new Vector3(distance, 0, distance);


        return chunk;
    }
}
