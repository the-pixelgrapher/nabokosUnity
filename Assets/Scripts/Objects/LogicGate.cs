using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    private PowerSource power;
    private Vector2 gridPos;

    public enum GateType
    {
        AND,
        XOR
    }
    public GateType gate;

    private SpriteRenderer sprite;
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private Transform aTrans;
    private Transform bTrans;
    private Wire a;
    private Wire b;

    void Start()
    {
        power = GetComponent<PowerSource>();
        gridPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        aTrans = transform.Find("InputA");
        bTrans = transform.Find("InputB");

        UpdateInputs();
    }


    void Update()
    {
        gridPos = transform.position;

        switch (gate)
        {
            case GateType.AND:
                power.isPowered = (a.isPowered && b.isPowered) ? true : false;
                break;

            case GateType.XOR:
                power.isPowered = (a.isPowered ^ b.isPowered) ? true : false;
                break;
        }

        sprite.sprite = power.isPowered ? powerOnSprite : powerOffSprite;
    }

    void UpdateInputs()
    {
        if (Physics2D.OverlapPoint(aTrans.position, LayerMask.GetMask("Wire")) && Physics2D.OverlapPoint(bTrans.position, LayerMask.GetMask("Wire")))
        {
            a = Physics2D.OverlapPointAll(aTrans.position, LayerMask.GetMask("Wire"))[0].GetComponent<Wire>();
            b = Physics2D.OverlapPointAll(bTrans.position, LayerMask.GetMask("Wire"))[0].GetComponent<Wire>();
        }
    }
}
