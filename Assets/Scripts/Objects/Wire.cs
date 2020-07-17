using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wire : MonoBehaviour
{
    //public bool isPowered;
    public int depth;
    private int maxDepth = 32;
    private Vector2 gridPos;
    private string id;
    private PowerSource power;
    public List<PowerSource> powerSources;

    private SpriteRenderer sprite;
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    void Start()
    {
        depth = 63;
        gridPos = transform.position;
        id = GlobalData.GetAdj(gridPos, "Wire");
        sprite = GetComponent<SpriteRenderer>();
        power = GetComponent<PowerSource>();
        power.isPowered = false;
    }

    void FixedUpdate()
    {
        gridPos = transform.position;
        //isPowered = false;

        depth = maxDepth;
        //CheckPower();
        sprite.sprite = power.isPowered ? powerOnSprite : powerOffSprite;

        bool poweredState = false;
        for (int i = 0; i < powerSources.Count; i++)
        {
            if (powerSources[i].isPowered) { poweredState = true; }
        }
        power.isPowered = poweredState;

    }

    private void CheckPower()
    {

        if (GameObject.FindWithTag("Power") == null)
            return;


        GameObject[] sources = GameObject.FindGameObjectsWithTag("Power");
        int sourcesFound = 0;

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].GetComponent<PowerSource>().isPowered)
            {
                sourcesFound++;
            }
        }

        if (sourcesFound == 0)
            return;



        Vector2[] dir = new Vector2[4];
        dir[0] = Vector2.right;
        dir[1] = Vector2.up;
        dir[2] = Vector2.left;
        dir[3] = Vector2.down;


        for (int i = 0; i < dir.Length; i++)
        {
            bool wireEnd = false;
            bool sourceFound = false;
            int j = 0;

            if (j > maxDepth)
                return;

            while (!wireEnd && !sourceFound && j <= maxDepth)
            {
                if (Physics2D.OverlapPoint(gridPos + dir[i] * j, LayerMask.GetMask("Power")))
                {
                    bool source = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Power"))[0].GetComponent<PowerSource>().isPowered;
                    power.isPowered = source;
                    sourceFound = true;
                    depth = j;
                }
                else if (Physics2D.OverlapPoint(gridPos + dir[i] * j, LayerMask.GetMask("Wire")))
                {
                    var wire = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Wire"))[0].GetComponent<Wire>();
                    var source = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Wire"))[0].GetComponent<PowerSource>();
                    if (wire.depth != maxDepth && wire.depth < depth)
                    {
                        power.isPowered = source.isPowered;
                        sourceFound = source.isPowered;
                        depth = wire.depth + j;
                    }
                }
                else
                {   
                    wireEnd = true;
                }

                j++;

            }

        }

    }



    private void OnDrawGizmos()
    {
        //Handles.Label(transform.position, depth.ToString());
    }
}
