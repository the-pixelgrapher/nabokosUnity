using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public bool isPowered;
    public bool isDirectSource;
    private Vector2 gridPos;
    private string id;
    private int maxRecursions;

    private SpriteRenderer sprite;
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    void Start()
    {
        gridPos = transform.position;
        id = GlobalData.GetAdj(gridPos, "Wire");
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sprite.sprite = isPowered ? powerOnSprite : powerOffSprite;

        isPowered = false;
        isDirectSource = false;

        CheckPower();
    }

    private void LateUpdate()
    {


    }

    private void CheckPower()
    {
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

            while (!wireEnd && !sourceFound)
            {
                if (Physics2D.OverlapPoint(gridPos + dir[i] * j, LayerMask.GetMask("Wire")))
                {
                    var wire = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Wire"))[0].GetComponent<Wire>();
                    if (wire.isPowered && wire.isDirectSource)
                    {
                        isPowered = true;
                        //sourceFound = true;
                    }

                    j++;
                }
                else if (Physics2D.OverlapPoint(gridPos + dir[i] * j, LayerMask.GetMask("Crate")))
                {
                    var source = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Crate"))[0];
                    isPowered = true;
                    sourceFound = true;
                    isDirectSource = true;
                }
                else
                {
                    //isDirectSource = false;
                    wireEnd = true;
                }

            }
        }
    }
}
