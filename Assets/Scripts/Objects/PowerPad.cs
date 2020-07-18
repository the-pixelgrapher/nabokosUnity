using UnityEngine;

public class PowerPad : PowerSource
{
    private Vector2 gridPos;
    private bool sourceFound;

    private void Start()
    {
        gridPos = transform.position;
        FindWires(gridPos);
    }

    private void Update()
    {
        if (Physics2D.OverlapPoint(gridPos, LayerMask.GetMask("Crate")))
        {
            if (!sourceFound)
            {
                sourceFound = true;
            }
        }
        else
        {
            sourceFound = false;
        }

        isPowered = sourceFound;
    }
}