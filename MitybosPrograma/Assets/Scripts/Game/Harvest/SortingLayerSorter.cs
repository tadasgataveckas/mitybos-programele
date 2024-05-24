using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerSorter : MonoBehaviour
{
    public bool parent = false;
    public bool stationary = true;
    public int offset;
    public bool grandparent = false;
    void Update()
    {
        if (!stationary)
        {
            float position = transform.position.y;
            if (parent) position = transform.parent.transform.position.y;
            if (grandparent) position = transform.parent.parent.transform.position.y;
            GetComponent<SpriteRenderer>().sortingOrder = Mathf.CeilToInt(position * 1000) * -1 + offset;
        }

    }
    private void Start()
    {

        float position = transform.position.y;
        if (parent) position = transform.parent.transform.position.y;
        if (grandparent) position = transform.parent.parent.transform.position.y;
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.CeilToInt(position * 1000) * -1 + offset;
    }
}
