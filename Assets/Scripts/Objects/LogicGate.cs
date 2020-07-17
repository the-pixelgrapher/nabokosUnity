using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : PowerSource
{
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

    private bool inputsFound;
    private Transform aTrans;
    private Transform bTrans;
    private Transform output;
    private PowerSource a;
    private PowerSource b;
    private Collider2D coll;

    private AudioManager aud;
    private bool powerSoundPlayed;

    void Start()
    {
        output = transform.Find("Output");
        aTrans = transform.Find("InputA");
        bTrans = transform.Find("InputB");
        gridPos = output.position;
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        aud = FindObjectOfType<AudioManager>();

        FindWires(gridPos);
        Debug.Log(gridPos);
        UpdateInputs();
    }


    void LateUpdate()
    {

        if (inputsFound)
        {
            switch (gate)
            {
                case GateType.AND:
                    isPowered = (a.isPowered && b.isPowered) ? true : false;
                    break;

                case GateType.XOR:
                    isPowered = (a.isPowered ^ b.isPowered) ? true : false;
                    break;
            }
        }

        if (isPowered && !powerSoundPlayed)
        {
            aud.Play("Power");
            aud.Play("Switch");
            powerSoundPlayed = true;
        }

        if (!isPowered && powerSoundPlayed)
        {
            aud.Play("Switch");
            powerSoundPlayed = false;
        }

        //coll.enabled = power.isPowered ? true : false;
        sprite.sprite = isPowered ? powerOnSprite : powerOffSprite;
    }

    void UpdateInputs()
    {

        if (Physics2D.OverlapPoint(aTrans.position, LayerMask.GetMask("Wire")) && Physics2D.OverlapPoint(bTrans.position, LayerMask.GetMask("Wire")))
        {
            a = Physics2D.OverlapPointAll(aTrans.position, LayerMask.GetMask("Wire"))[0].GetComponent<PowerSource>();
            b = Physics2D.OverlapPointAll(bTrans.position, LayerMask.GetMask("Wire"))[0].GetComponent<PowerSource>();

            inputsFound = true;
        }
        else if (Physics2D.OverlapPoint(aTrans.position, LayerMask.GetMask("Power")) && Physics2D.OverlapPoint(bTrans.position, LayerMask.GetMask("Power")))
        {
            a = Physics2D.OverlapPointAll(aTrans.position, LayerMask.GetMask("Power"))[0].GetComponent<PowerSource>();
            b = Physics2D.OverlapPointAll(bTrans.position, LayerMask.GetMask("Power"))[0].GetComponent<PowerSource>();

            inputsFound = true;
        }

    }
}
