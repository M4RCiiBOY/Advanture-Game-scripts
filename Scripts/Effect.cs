using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public int id;
    public string connection;
    public GameObject[] connections;
    public GameObject[] idConnections;
    private SpriteRenderer render;

    // Use this for initialization
    void Start()
    {
        if (id == 0)
            return;
        connections = GameObject.FindGameObjectsWithTag(connection);
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].GetComponent<Switch>().id != this.id)
            {
                List<GameObject> tmp = new List<GameObject>(connections);
                tmp.RemoveAt(i);
                connections = tmp.ToArray();
                i--;
            }
        }
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 0)
            return;
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].GetComponent<Switch>().isTriggered)
            {

            }
            else
            {
                return;
            }

        }
        render.enabled = true;
    }
}
