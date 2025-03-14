﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Cainos.PixelArtTopDown_Basic
{
    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class LayerTrigger : MonoBehaviour
    {
        public string sortingLayer;

        private void OnTriggerExit2D(Collider2D other)
        {

            if (other.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
                SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in srs)
                {
                    sr.sortingLayerName = sortingLayer;
                }
            }

            else if (other.gameObject.GetComponent<SortingGroup>() != null)
            {
                other.gameObject.GetComponent<SortingGroup>().sortingLayerName = sortingLayer;
                SortingGroup[] sgs = other.gameObject.GetComponentsInChildren<SortingGroup>();
                foreach (SortingGroup sg in sgs)
                {
                    sg.sortingLayerName = sortingLayer;
                }
            }
        }
    }
}
