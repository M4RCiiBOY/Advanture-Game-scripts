using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    //public GameObject player1; 
    //public GameObject player2;

    [HideInInspector]
    public Vector2 savePos;
    [HideInInspector]
    public bool savePosExist = false;

    private GameObject[] players;
    private Vector3 midPos;
    private float zoom2Test;

    private List<GameObject> camList = new List<GameObject>();
    private List<GameObject> slaveList = new List<GameObject>();
    [Tooltip("Determines how close slaves to the camera can get to the boundry.")]
    [Range(0, 1)]
    public float boundry = 0.9f;
    private Vector3 posi;
    private float horizontalSize;


    //Set for current assets
    [Range(0, 10)]
    public float zoom = 0.7f;
    [Range(0, 10)]
    public float maxZoom = 1.6f;
    [Range(0, 10)]
    public float minZoom = 1f;
    [Range(0, 10)]
    public float singleZoom = 1f;

    private void Start()
    {
        /*StartCoroutine(WaitToGetPlayers());*/ //if players were spawned
    }

    public void PlacePlayers(float x, float y)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.transform.position = new Vector2(x, y);
        }
    }

    /// <summary>
    /// Call if player array needs to be changed.
    /// </summary>
    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    /// <summary>
    /// Adds gameobjects to the camera and stopps slaves from going outside the cameras view.
    /// </summary>
    /// <param name="obj">The gameobject beig added to the camera.</param>
    /// <param name="slave">True if gameobject needs to stay in view.</param>
    public void AddToCamera(GameObject obj, bool slave)
    {
        camList.Add(obj);
        if (slave) slaveList.Add(obj);
    }

    /// <summary>
    /// Removes gameobjects to the camera and stopps slaves from going outside the cameras view.
    /// </summary>
    /// <param name="obj">The gameobject beig added to the camera.</param>
    /// <param name="slave">True if gameobject needs to stay in view.</param>
    public void RemoveFromCamera(GameObject obj, bool slave)
    {
        camList.Remove(obj);
        if (slave) slaveList.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    {
        //Adjust zoom of camera
        switch (camList.Count)
        {
            case 0: return;
            case 1:
                Camera.main.transform.position = new Vector3(camList[0].transform.position.x, camList[0].transform.position.y, -10);
                Camera.main.orthographicSize = singleZoom;
                break;
            case 2:
                Camera.main.orthographicSize = Vector2.Distance(camList[0].transform.position, camList[1].transform.position) * zoom;
                midPos = (camList[0].transform.position + camList[1].transform.position) / 2;
                Camera.main.transform.position = new Vector3(midPos.x, midPos.y, -10);
                break;
            default:
                Camera.main.orthographicSize = Vector2.Distance(camList[0].transform.position, camList[1].transform.position) * zoom;
                if (camList.Count > 2)
                {
                    for (int i = 0; i < camList.Count; i++)
                    {
                        for (int j = i + 1; j < camList.Count; j++)
                        {
                            zoom2Test = Vector2.Distance(camList[i].transform.position, camList[j].transform.position) * zoom;
                            if (Camera.main.orthographicSize < zoom2Test) Camera.main.orthographicSize = zoom2Test;
                        }
                    }
                }
                midPos = Vector2.zero;
                for (int i = 0; i < camList.Count; i++)
                {
                    midPos += camList[i].transform.position;
                }
                midPos /= camList.Count;
                Camera.main.transform.position = new Vector3(midPos.x, midPos.y, -10);
                break;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        //Keep slaves in view of Camera
        horizontalSize = Screen.width / (float)Screen.height * Camera.main.orthographicSize;
        for (int i = 0; i < slaveList.Count; i++)
        {
            posi.y = Mathf.Clamp(slaveList[i].transform.position.y, Camera.main.transform.position.y - boundry * Camera.main.orthographicSize, Camera.main.transform.position.y + boundry * Camera.main.orthographicSize);
            posi.x = Mathf.Clamp(slaveList[i].transform.position.x, Camera.main.transform.position.x - boundry * horizontalSize, Camera.main.transform.position.x + boundry * horizontalSize);
            slaveList[i].transform.position = posi;
        }



        //if (player1 == null && player2 == null)
        //{
        //    return;
        //}

        //if (player1 == null)
        //{
        //    Camera.main.transform.position = new Vector3(player2.transform.position.x, player2.transform.position.y, -1);
        //    Camera.main.orthographicSize = singleZoom;
        //    if (Camera.main.orthographicSize > maxZoom) { Camera.main.orthographicSize = maxZoom; }
        //    if (Camera.main.orthographicSize < minZoom) { Camera.main.orthographicSize = minZoom; }
        //}

        //if (player2 == null)
        //{
        //    Camera.main.transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, -1);
        //    Camera.main.orthographicSize = singleZoom;
        //    if (Camera.main.orthographicSize > maxZoom) { Camera.main.orthographicSize = maxZoom; }
        //    if (Camera.main.orthographicSize < minZoom) { Camera.main.orthographicSize = minZoom; }
        //}

        //if (player1 != null && player2 != null)
        //{
        //    Camera.main.orthographicSize = Vector2.Distance(player1.transform.position, player2.transform.position) * zoom;
        //    if (Camera.main.orthographicSize > maxZoom) { Camera.main.orthographicSize = maxZoom; }
        //    if (Camera.main.orthographicSize < minZoom) { Camera.main.orthographicSize = minZoom; }
        //    Camera.main.transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x) * 0.5f, (player1.transform.position.y + player2.transform.position.y) * 0.5f, -10/*, (player1.transform.position.x + player2.transform.position.x + player1.transform.position.y + player2.transform.position.y) * zoom*/); //Part used for perspectice camera
        //}


    }


}