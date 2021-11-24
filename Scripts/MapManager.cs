using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Tooltip("Draws the entire map no matter the camera position (might reduce performance)")]
    public bool drawall;
    public float buffer;
    public float updateInverval = 1;
    float time = 0;
    public GameObject[] MapChunks;
    public bool[] mapChunkLoaded;

    private void Start()
    {
        mapChunkLoaded = new bool[MapChunks.Length];
    }


    void Update()
    {
        this.time += Time.deltaTime;
        if (this.time >= updateInverval)
        {
            this.time = 0;
            UpdateMap();
        }



    }

    private void UpdateMap()
    {
        Vector3 DownLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        Vector3 UpRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));

        for (int i = 0; i < MapChunks.Length; i++)
        {
            if (drawall)
            {
                MapChunks[i].GetComponent<MapBuilder>().Reload();
                mapChunkLoaded[i] = true;
            }
            else if (MapChunks[i].transform.position.x > (UpRightPos.x + buffer) || (MapChunks[i].transform.position.y - (MapChunks[i].GetComponent<MapBuilder>().mapSizeInTiles) > (UpRightPos.y + buffer)))
            {
                MapChunks[i].GetComponent<MapBuilder>().Clear();
                mapChunkLoaded[i] = false;

            }
            else if (MapChunks[i].transform.position.x + (MapChunks[i].GetComponent<MapBuilder>().mapSizeInTiles) < (DownLeftPos.x - buffer) || (MapChunks[i].transform.position.y < (DownLeftPos.y - buffer)))
            {
                MapChunks[i].GetComponent<MapBuilder>().Clear();
                mapChunkLoaded[i] = false;
            }
            else if (!mapChunkLoaded[i])
            {
                MapChunks[i].GetComponent<MapBuilder>().Reload();
                mapChunkLoaded[i] = true;
            }
            else
            {
                //its already loaded then
            }
        }
    }

    //for debugging
    public void ReloadAll()
    {
        for (int i = 0; i < MapChunks.Length; i++)
        {
            MapChunks[i].GetComponent<MapBuilder>().Reload();
        }
    }
}
