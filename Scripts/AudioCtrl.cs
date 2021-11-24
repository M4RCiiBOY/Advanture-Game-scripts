using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public AudioClip bgMusic;
    public AudioClip bossMusic;

    AudioSource source;

    // Use this for initialization
    void Start ()
    {
        source = GetComponent<AudioSource>();
        source.clip = bgMusic;
        source.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Camera.main.transform.position.x > 140 && source.clip == bgMusic)
        {
            source.clip = bossMusic;
            source.Play();
        }
        if (Camera.main.transform.position.x < 140 && source.clip == bossMusic)
        {
            source.clip = bgMusic;
            source.Play();
        }
	}
}
