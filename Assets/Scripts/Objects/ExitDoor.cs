﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExitDoor : MonoBehaviour
{
    private Collider2D coll;
    private Vector2 gridPos;
    public bool isPowered;

    private bool sourceFound;
    private PowerSource source;
    private Transform inputTrans;

    private GameObject door;
    private bool doorOpen;

    private AudioManager aud;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        gridPos = transform.position;
        inputTrans = transform.Find("Input");
        door = transform.Find("ExitDoor").gameObject;
        aud = FindObjectOfType<AudioManager>();

        UpdateInputs();
    }


    void LateUpdate()
    {
        if (sourceFound)
        {
            coll.enabled = source.isPowered ? false : true;
            isPowered = source.isPowered;
        }

        if (isPowered && !doorOpen)
        {
            // Tween open
            door.transform.DOLocalMoveY(1.0f, 0.3333f).SetEase(Ease.OutQuint);
            doorOpen = true;
            aud.Play("Door");
        }

        if (!isPowered && doorOpen)
        {
            // Tween closed
            door.transform.DOLocalMoveY(0.0f, 0.3333f).SetEase(Ease.OutQuint);
            doorOpen = false;
            aud.Play("Door");
        }
    }

    void UpdateInputs()
    {
        if (Physics2D.OverlapPoint(inputTrans.position, LayerMask.GetMask("Wire")) )
        {
            source = Physics2D.OverlapPointAll(inputTrans.position, LayerMask.GetMask("Wire"))[0].GetComponent<PowerSource>();
            sourceFound = true;
        }
    }
}
