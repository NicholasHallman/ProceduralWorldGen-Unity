using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RelativeMapGenerator : MonoBehaviour
{

    private float playerX;
    private float playerY;

    private int size = 16;
    private int chunkX = 0;
    private int chunkY = 0;
    private bool init = false;
    private System.Random ran = new System.Random();

    private Chunk[,] world;

    private GameObject player;

    // Use this for initialization
    void Start()
    {
        world = new Chunk[64, 64];
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                var chunk = gameObject.AddComponent<Chunk>();
                chunk.x = j;
                chunk.y = i;
                chunk.biom = 1;
                world[j, i] = chunk;
            }
        }
        chunkX = 0;
        chunkY = 0;
        player = GameObject.FindWithTag("Player");
        playerX = 0;
        playerY = 0;
        int dist = 1;
        for (int i = (size * -1) + 1; i < size; i++)
        {
            for (int j = (size * -1) + 1; j < size; j++)
            {
                int currI = Math.Abs(i) + 32;
                int currJ = Math.Abs(j) + 32;
                double distance = Math.Sqrt(Math.Pow((currI - 32), 2) + Math.Pow((currJ - 32), 2));

                int x = (int)distance;

                if (x < 0)
                {
                    x = 0;
                }
                --x;
                x |= x >> 1;
                x |= x >> 2;
                x |= x >> 4;
                x |= x >> 8;
                x |= x >> 16;
                x += 1;

                distance = x;
                Vector3 newPosition = player.transform.position;
                int currentChunkX = ((int)newPosition.x) / 32;
                int currentChunkY = ((int)newPosition.z) / 32;
                world[currentChunkX + j, currentChunkY + i].drawChunk(currentChunkX + j, currentChunkY + i, (int)distance);
        world[currentChunkX + j, currentChunkY + i].distance = (int)distance;

            }
        }
        init = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            Vector3 newPosition = player.transform.position;
            int currentChunkX = ((int)newPosition.x) / 32;
            int currentChunkY = ((int)newPosition.z) / 32;
            if (chunkX != currentChunkX || chunkY != currentChunkY)
            {
                Debug.Log("Ummmmm");
                // Loop for spawning chuncks around player, 
                //looped because of generation of new chunck object without needing a new name for each individual object haha

                chunkX = currentChunkX;
                chunkY = currentChunkY;

                StartCoroutine(chunkUpdate(currentChunkX, currentChunkY));

            }
        }
    }

    IEnumerator chunkUpdate(int currentChunkX, int currentChunkY)
    {

        int x = 0;
        int y = 0;
        int dx = 0;
        int dy = -1;
        int t = 32;
        for (var i = 0; i <= 1024; i++)
        {
            if ((-32 / 2 <= x && x <= 32 / 2) && (-32 / 2 <= y && y <= 32 / 2))
            {

                int relx = x + currentChunkX;
                int rely = y + currentChunkY;
                if ((currentChunkX - world[relx, rely].x != 0 || currentChunkY - world[relx, rely].y != 0))
                {

                    double distance = Math.Sqrt(Math.Pow((currentChunkX - world[relx, rely].x), 2) + Math.Pow((currentChunkY - world[relx, rely].y), 2));
                    //double next = Math.Pow(Math.Round(Math.Log(distance, 2)), 2);

                    int dis = (int)distance;

                    if (dis < 0)
                    {
                        dis = 0;
                    }
                    --dis;
                    dis |= dis >> 1;
                    dis |= dis >> 2;
                    dis |= dis >> 4;
                    dis |= dis >> 8;
                    dis |= dis >> 16;
                    dis += 1;

                    distance = dis;
                    //distance = (int)next;
                    yield return null;
                    if (distance > 32)
                    {
                        world[relx, rely].DeleteChunk();
                    }
                    else if (distance != world[relx, rely].distance)
                    {
                        int j = relx;
                        int k = rely;
                        world[relx, rely].drawChunk(relx, rely, (int)distance);
                        world[relx, rely].distance = (int)distance;

                    }
                }
            }
            if ((x == y) || (x < 0) && (x == -y) || ((x > 0) && (x == 1 - y)))
            {
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
            
        }
    }
}
