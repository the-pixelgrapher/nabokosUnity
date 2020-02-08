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

    private bool inputsFound;
    private Transform aTrans;
    private Transform bTrans;
    private PowerSource a;
    private PowerSource b;

    private AudioManager aud;
    private bool powerSoundPlayed;

    void Start()
    {
        power = GetComponent<PowerSource>();
        gridPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        aTrans = transform.Find("InputA");
        bTrans = transform.Find("InputB");
        aud = FindObjectOfType<AudioManager>();

        UpdateInputs();
    }


    void LateUpdate()
    {
        gridPos = transform.position;

        if (inputsFound)
        {
            switch (gate)
            {
                case GateType.AND:
                    power.isPowered = (a.isPowered && b.isPowered) ? true : false;
                    break;

                case GateType.XOR:
                    power.isPowered = (a.isPowered ^ b.isPowered) ? true : false;
                    break;
            }
        }

        if (power.isPowered && !powerSoundPlayed)
        {
            aud.Play("Power");
            aud.Play("Switch");
            powerSoundPlayed = true;
        }

        if (!power.isPowered && powerSoundPlayed)
        {
            aud.Play("Switch");
            powerSoundPlayed = false;
        }

        sprite.sprite = power.isPowered ? powerOnSprite : powerOffSprite;
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
