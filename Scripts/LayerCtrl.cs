using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class LayerCtrl : MonoBehaviour
{
    SpriteRenderer render;

    // Use this for initialization
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int sortPos = Mathf.FloorToInt(transform.position.y);
        render.sortingOrder = -sortPos;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2" || collision.gameObject.tag == "Enemy") //needs to be adjusted later to also apply to NPCs and enemies
    //    {
    //        if (collision.transform.position.y > transform.position.y)
    //        {
    //            if (collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder == this.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
    //            {
    //                this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
    //            }
    //            else if (collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder > this.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
    //            {
    //                int tmp = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    //                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    //                this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tmp;
    //            }
    //            else
    //            {
    //                //nothing, layers are already correct
    //            }
    //        }
    //        else
    //        {
    //            if (collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder == this.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
    //            {
    //                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
    //            }
    //            else if (collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder < this.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
    //            {
    //                int tmp = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    //                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    //                this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tmp;
    //            }
    //            else
    //            {
    //                //nothing, layers are already correct
    //            }
    //        }
    //    }
    //}
}
