using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public bool isPowered;
    private Vector2 pos;
    public List<Wire> checkedWires;
    public List<Wire> newWires;

    protected void FindWires(Vector2 _pos)
    {
        Vector2[] dir = new Vector2[4];
        dir[0] = Vector2.right;
        dir[1] = Vector2.up;
        dir[2] = Vector2.left;
        dir[3] = Vector2.down;

        // Get first adjacent wires
        for (int i = 0; i < dir.Length; i++)
        {
            if (Physics2D.OverlapPoint(_pos + dir[i], LayerMask.GetMask("Wire")))
            {
                Debug.Log("finding wires");
                newWires.Add(Physics2D.OverlapPointAll(_pos + dir[i], LayerMask.GetMask("Wire"))[0].GetComponent<Wire>());
            }
        }

        while (newWires.Count > 0)
        {
            Wire checkedWire = newWires[0];
            Vector2 checkedPos = checkedWire.transform.position;

            for (int i = 0; i < dir.Length; i++)
            {
                if (Physics2D.OverlapPoint(checkedPos + dir[i], LayerMask.GetMask("Wire")))
                {
                    Debug.Log("finding wires");

                    Wire newWire = Physics2D.OverlapPointAll(checkedPos + dir[i], LayerMask.GetMask("Wire"))[0].GetComponent<Wire>();

                    bool alreadyChecked = false;
                    for (int j = 0; j < newWires.Count; j++)
                    {
                        if (newWire == newWires[j]) { alreadyChecked = true; }
                    }
                    for (int j = 0; j < checkedWires.Count; j++)
                    {
                        if (newWire == checkedWires[j]) { alreadyChecked = true; }
                    }

                    if (!alreadyChecked) { newWires.Add(newWire); }
                }
            }

            checkedWires.Add(checkedWire);
            newWires.Remove(checkedWire);
        }

        // Add this power source to all checked wires
        for (int i = 0; i < checkedWires.Count; i++)
        {
            bool powerAdded = false;
            for (int j = 0; j < checkedWires[i].powerSources.Count; j++)
            {
                if (this == checkedWires[i].powerSources[j]) { powerAdded = true; }
            }

            if (!powerAdded) { checkedWires[i].powerSources.Add(this); }
        }
    }
}