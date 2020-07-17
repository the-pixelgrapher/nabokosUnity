using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPad : PowerSource
{
    private Vector2 gridPos;

    private bool sourceFound;
    private PowerSource source;

    private Collider2D coll;

    private void Start()
    {
        gridPos = transform.position;
        coll = GetComponent<Collider2D>();
        FindWires(gridPos);
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

        isPowered = sourceFound;
        //coll.enabled = sourceFound;
    }
}
