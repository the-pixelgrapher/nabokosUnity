using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Vector2 gridPos;                 // Player position on grid
    public bool isPowered;                  // Is magnet powered on?
    public int moveCount;
    public LayerMask solidLayers;
    private bool isPulling;
    private Vector2 direction;

    public enum Rot
    {
        Right,
        Up,
        Left,
        Down
    }
    public Rot magRotation;
    private List<Rot> rotRecord = new List<Rot>();
    private List<Vector2> moveRecord = new List<Vector2>();

    private SpriteRenderer sprite;          // Player sprite component
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private InputHandler iman;              // Input handler for input reading
    private SceneSwitcher scene;
    private AudioManager aud;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        iman = FindObjectOfType<InputHandler>();
        scene = FindObjectOfType<SceneSwitcher>();
        aud = FindObjectOfType<AudioManager>();
        SetRotation();
        Tween();
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoMove();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // restart
            scene.SceneSwitch(GlobalData.curScene);
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
                // record movement for undo
                moveRecord.Add(gridPos);
                moveRecord[moveCount] = gridPos;

                MagnetPull();
                gridPos += direction;

                SetRotation();
                Tween();

                aud.Play("Move");

                moveCount++;
            }
        }
    }

    private void MagnetPull()
    {
        isPulling = false;

        if (!isPowered)
            return;

        GameObject crate = null;

        switch (magRotation)
        {
            case Rot.Right:
                if (Physics2D.OverlapPoint(gridPos + Vector2.right, LayerMask.GetMask("Crate")))
                {
                    crate = Physics2D.OverlapPointAll(gridPos + Vector2.right, LayerMask.GetMask("Crate"))[0].gameObject;

                    if (direction == Vector2.left)
                    {
                        crate.transform.DOMoveX(gridPos.x, 0.1667f);
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
                        isPulling = true;
                    }
                }
                break;
        }
    }

    private void UndoMove()
    {
        if (moveCount > 0)
        {
            gridPos = moveRecord[moveCount - 1];
            magRotation = rotRecord[moveCount - 1];
            moveCount--;

            Tween();
        }
    }

    private void Tween()
    {
        transform.DOMove(gridPos, 0.1667f);

        switch (magRotation)
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
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Right;
                            break;
                    }
                    break;

                case "0100":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Up;
                            break;
                    }
                    break;

                case "0010":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Left;
                            break;
                    }
                    break;

                case "0001":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Down;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Down;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Down;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Down;
                            break;
                    }
                    break;

                case "1100":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Right;
                            break;
                    }
                    break;

                case "0110":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Up;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Left;
                            break;
                    }
                    break;

                case "0011":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Down;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Left;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Down;
                            break;
                    }
                    break;

                case "1001":
                    switch (magRotation)
                    {
                        case Rot.Right:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Up:
                            magRotation = Rot.Right;
                            break;
                        case Rot.Left:
                            magRotation = Rot.Down;
                            break;
                        case Rot.Down:
                            magRotation = Rot.Down;
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

        // Record rotation for undo
        rotRecord.Add(magRotation);
        rotRecord[moveCount] = magRotation;
    }

    private void OnDestroy()
    {
        transform.DOKill(true);
    }

}
