using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlave : MonoBehaviour
{
	// Use this for initialization
	public bool SlaveToCameraSize;
	void Start ()
	{
        Camera.main.GetComponent<CameraCtrl>().AddToCamera(this.gameObject, SlaveToCameraSize);
    }

    void OnDestroy()
    {
        Camera.main.GetComponent<CameraCtrl>().RemoveFromCamera(this.gameObject, SlaveToCameraSize);
    }

}
