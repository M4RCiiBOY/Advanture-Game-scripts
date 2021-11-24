using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeTileChange : MonoBehaviour
{
    AIBody boss;

    public void Starting()
    {
        boss = GetComponentInChildren<AIBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss != null && boss.Health <= 0)
        {
            GetComponent<MapBuilder>().MapName = "Gold/boss_8alt";
            GetComponent<MapBuilder>().Reload();
        }
    }
}
