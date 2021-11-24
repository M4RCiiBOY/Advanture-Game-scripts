using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is a test Script these things need to be Implemented on the Players.
public class Health : MonoBehaviour
{
    [SerializeField]
    private GameObject[] HealthPieces;
    [SerializeField]
    private Image PotTransparent;
    [SerializeField]
    private Image PotFull;
    [SerializeField]
    private Image GadgetTransparent;
    [SerializeField]
    private Image GadgetFull;
    private int maxHealth = 9;
    private int currentHealth;



	void Start ()
    {
        currentHealth = maxHealth;
        PotFull.gameObject.SetActive(false);
        PotTransparent.gameObject.SetActive(false);
        GadgetFull.gameObject.SetActive(false);
        GadgetTransparent.gameObject.SetActive(false);
    }
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentHealth <= 0) { return; }
            HealthUpdate(true, -1);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentHealth >= 9) { return; }
            HealthUpdate(false, 1);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PotTransparent.gameObject.SetActive(true);
            PotFull.gameObject.SetActive(true);
            PotFull.fillAmount = 0;
            StartCoroutine(PotFilling());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GadgetTransparent.gameObject.SetActive(true);
            GadgetFull.gameObject.SetActive(true);
            GadgetFull.fillAmount = 0;
            StartCoroutine(GadgetFilling());
        }
    }

    private IEnumerator GadgetFilling()
    {
        yield return new WaitForSeconds(0.75f);
        GadgetFull.fillAmount += 0.1f;
        if (GadgetFull.fillAmount != 1.0f)
        {
            StartCoroutine(GadgetFilling());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            GadgetFull.gameObject.SetActive(false);
            GadgetTransparent.gameObject.SetActive(false);
        }
    }

    private IEnumerator PotFilling()
    {
        yield return new WaitForSeconds(0.5f);
        PotFull.fillAmount += 0.1f;
        if(PotFull.fillAmount != 1.0f)
        {
            StartCoroutine(PotFilling());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            PotFull.gameObject.SetActive(false);
            PotTransparent.gameObject.SetActive(false);
        }
    }

    public void HealthUpdate(bool damage, int value)
    {
        //damage
        if(damage)
        {
            currentHealth -= value;
            for (int i = 9; i > currentHealth; i--)
            {
                HealthPieces[i - 1].SetActive(false);
            }
        }

        //heal
        if(!damage)
        {
            currentHealth += value;
            for (int i = 0; i < currentHealth; i++)
            {
                HealthPieces[i].SetActive(true);
            }
        }

    }
}
