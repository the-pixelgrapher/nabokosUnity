using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Entity
{
    public bool isPowered;                  // Is magnet powered on?
    public LayerMask solidLayers;
    private bool isPulling;
    private Vector2 direction;

    private SpriteRenderer sprite;          // Player sprite component
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private InputHandler iman;              // Input handler for input reading
    private SceneSwitcher scene;
    private GameManager gameMan;
    private AudioManager aud;

    private bool madeMove;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        iman = FindObjectOfType<InputHandler>();
        scene = FindObjectOfType<SceneSwitcher>();
        gameMan = FindObjectOfType<GameManager>();
        aud = FindObjectOfType<AudioManager>();
        SetRotation();
        Tween();
    }

    void Update()
    {
        madeMove = false;

        // Toggle magnet
        if (iman.InputRead("confirmD"))
        {
            isPowered = !isPowered;
            if (isPowered)
            {
                SetRotation();
                Tween();
                aud.Play("Power");
            }
            aud.Play("Switch");
        }

        sprite.sprite = isPowered ? powerOnSprite : powerOffSprite;

        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.R))
        {
            // restart
            scene.SceneSwitch(GlobalData.curScene);
        }
    }

    private void LateUpdate()
    {
        if (madeMove)
        {
            gameMan.GlobalRecordState();
        }
    }

    private void PlayerMovement()
    {
        direction = new Vector2(0, 0);

        // Get directional inputs
        if (iman.InputRead("rightD"))
        {
            direction = Vector2.right;
        }
        if (iman.InputRead("upD"))
        {
            direction = Vector2.up;
        }
        if (iman.InputRead("leftD"))
        {
            direction = Vector2.left;
        }
        if (iman.InputRead("downD"))
        {
            direction = Vector2.down;
        }

        // Collision check
        if (!Physics2D.OverlapCircle(gridPos + direction, 0.475f, solidLayers))
        {
            if (direction != Vector2.zero)
            {
                madeMove = true;

                MagnetPull();
                gridPos += direction;

                SetRotation();
                Tween();

                aud.Play("Move");
            }
        }
    }

    private void MagnetPull()
    {
        isPulling = false;

        if (!isPowered)
            return;

        GameObject crate;
        switch (curRot)
        {
            case Rot.Right:
                if (Physics2D.OverlapPoint(gridPos + Vector2.right, LayerMask.GetMask("Crate")))
                {
                    crate = Physics2D.OverlapPointAll(gridPos + Vector2.right, LayerMask.GetMask("Crate"))[0].gameObject;

                    if (direction == Vector2.left)
                    {
                        crate.transform.DOMoveX(gridPos.x, 0.1667f);
                        crate.GetComponent<Entity>().gridPos.x = gridPos.x;
                        isPulling = true;
                    }
                }
                break;

            case Rot.Up:
                if (Physics2D.OverlapPoint(gridPos + Vector2.up, LayerMask.GetMask("Crate")))
                {
                    crate = Physics2D.OverlapPointAll(gridPos + Vector2.up, LayerMask.GetMask("Crate"))[0].gameObject;

                    if (direction == Vector2.down)
                    {
                        crate.transform.DOMoveY(gridPos.y, 0.1667f);
                        crate.GetComponent<Entity>().gridPos.y = gridPos.y;
                        isPulling = true;
                    }
                }
                break;

            case Rot.Left:
                if (Physics2D.OverlapPoint(gridPos + Vector2.left, LayerMask.GetMask("Crate")))
                {
                    crate = Physics2D.OverlapPointAll(gridPos + Vector2.left, LayerMask.GetMask("Crate"))[0].gameObject;

                    if (direction == Vector2.right)
                    {
                        crate.transform.DOMoveX(gridPos.x, 0.1667f);
                        crate.GetComponent<Entity>().gridPos.x = gridPos.x;
                        isPulling = true;
                    }
                }
                break;

            case Rot.Down:
                if (Physics2D.OverlapPoint(gridPos + Vector2.down, LayerMask.GetMask("Crate")))
                {
                    crate = Physics2D.OverlapPointAll(gridPos + Vector2.down, LayerMask.GetMask("Crate"))[0].gameObject;

                    if (direction == Vector2.up)
                    {
                        crate.transform.DOMoveY(gridPos.y, 0.1667f);
                        crate.GetComponent<Entity>().gridPos.y = gridPos.y;
                        isPulling = true;
                    }
                }
                break;
        }
    }

    private void Tween()
    {
        transform.DOMove(gridPos, 0.1667f);

        switch (curRot)
        {
            case Rot.Right:
                transform.DORotate(new Vector3(0, 0, 0), 0.1667f);
                break;
            case Rot.Up:
                transform.DORotate(new Vector3(0, 0, 90), 0.1667f);
                break;
            case Rot.Left:
                transform.DORotate(new Vector3(0, 0, 180), 0.1667f);
                break;
            case Rot.Down:
                transform.DORotate(new Vector3(0, 0, 270), 0.1667f);
                break;
        }
    }

    private void SetRotation()
    {
        string mag = GlobalData.GetAdj(gridPos, "Crate");
        // Right, Up, Left, Down

        if (!isPulling && isPowered)
        {
            switch (mag)
            {
                case "0000":
                    break;

                case "1000":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Right;
                            break;
                        case Rot.Up:
                            curRot = Rot.Right;
                            break;
                        case Rot.Left:
                            curRot = Rot.Right;
                            break;
                        case Rot.Down:
                            curRot = Rot.Right;
                            break;
                    }
                    break;

                case "0100":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Up;
                            break;
                        case Rot.Up:
                            curRot = Rot.Up;
                            break;
                        case Rot.Left:
                            curRot = Rot.Up;
                            break;
                        case Rot.Down:
                            curRot = Rot.Up;
                            break;
                    }
                    break;

                case "0010":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Left;
                            break;
                        case Rot.Up:
                            curRot = Rot.Left;
                            break;
                        case Rot.Left:
                            curRot = Rot.Left;
                            break;
                        case Rot.Down:
                            curRot = Rot.Left;
                            break;
                    }
                    break;

                case "0001":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Down;
                            break;
                        case Rot.Up:
                            curRot = Rot.Down;
                            break;
                        case Rot.Left:
                            curRot = Rot.Down;
                            break;
                        case Rot.Down:
                            curRot = Rot.Down;
                            break;
                    }
                    break;

                case "1100":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Right;
                            break;
                        case Rot.Up:
                            curRot = Rot.Up;
                            break;
                        case Rot.Left:
                            curRot = Rot.Up;
                            break;
                        case Rot.Down:
                            curRot = Rot.Right;
                            break;
                    }
                    break;

                case "0110":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Up;
                            break;
                        case Rot.Up:
                            curRot = Rot.Up;
                            break;
                        case Rot.Left:
                            curRot = Rot.Left;
                            break;
                        case Rot.Down:
                            curRot = Rot.Left;
                            break;
                    }
                    break;

                case "0011":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Down;
                            break;
                        case Rot.Up:
                            curRot = Rot.Left;
                            break;
                        case Rot.Left:
                            curRot = Rot.Left;
                            break;
                        case Rot.Down:
                            curRot = Rot.Down;
                            break;
                    }
                    break;

                case "1001":
                    switch (curRot)
                    {
                        case Rot.Right:
                            curRot = Rot.Right;
                            break;
                        case Rot.Up:
                            curRot = Rot.Right;
                            break;
                        case Rot.Left:
                            curRot = Rot.Down;
                            break;
                        case Rot.Down:
                            curRot = Rot.Down;
                            break;
                    }
                    break;

                case "1010":
                    break;

                case "0101":
                    break;

                case "1101":
                    break;

                case "1110":
                    break;

                case "0111":
                    break;

                case "1011":
                    break;

                case "1111":
                    break;
            }
        }

    }

    private void OnDestroy()
    {
        transform.DOKill(true);
    }

}
