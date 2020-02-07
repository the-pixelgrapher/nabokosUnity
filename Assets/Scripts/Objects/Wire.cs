using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private Vector2 gridPos;
    private string id;
    public bool isPowered;

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
        isPowered = false;
        CheckPower();

        sprite.sprite = isPowered ? powerOnSprite : powerOffSprite;
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
                    j++;
                }
                else if (Physics2D.OverlapPoint(gridPos + dir[i] * j, LayerMask.GetMask("Crate")))
                {
                    var source = Physics2D.OverlapPointAll(gridPos + dir[i] * j, LayerMask.GetMask("Crate"))[0];
                    isPowered = true;
                    sourceFound = true;
                }
                else
                {
                    wireEnd = true;
                }

            }
        }
    }
}
