using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPad : MonoBehaviour
{
    private PowerSource power;
    private Vector2 gridPos;

    private bool sourceFound;
    private PowerSource source;

    private Collider2D coll;

    void Start()
    {
        power = GetComponent<PowerSource>();
        gridPos = transform.position;
        coll = GetComponent<Collider2D>();
    }


    void Update()
    {

        if (Physics2D.OverlapPoint(gridPos, LayerMask.GetMask("Crate")))
        {
            if (!sourceFound)
            {
                source = Physics2D.OverlapPointAll(gridPos, LayerMask.GetMask("Crate"))[0].GetComponent<PowerSource>();
                sourceFound = true;
            }
        }
        else
        {
            sourceFound = false;
        }

        power.isPowered = (sourceFound) ? true : false;
        coll.enabled = (sourceFound) ? true : false;
    }
}
