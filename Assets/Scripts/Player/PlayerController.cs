using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Vector2 gridPos;                 // Player position on grid
    public bool isPowered;                  // Is magnet powered on?

    public enum Rot
    {
        Right,
        Up,
        Left,
        Down
    }
    public Rot magRotation;

    private SpriteRenderer sprite;          // Player sprite component
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private InputHandler iman;              // Input handler for input reading

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        iman = FindObjectOfType<InputHandler>();
        SetRotation();
    }

    void Update()
    {
        // Toggle magnet
        if (iman.InputRead("confirmD"))
        {
            isPowered = !isPowered;
        }

        sprite.sprite = isPowered ? powerOnSprite : powerOffSprite;

        PlayerMovement();

    }

    private void PlayerMovement()
    {
        Vector2 direction = new Vector2(0, 0);

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
        if (!Physics2D.OverlapPoint(gridPos + direction))
        {
            gridPos += direction;
            //transform.position = gridPos;
            if (direction != Vector2.zero)
            {
                // Tween player to target position
                transform.DOMove(gridPos, 0.2f);
                SetRotation();
            }
        }
    }

    private void SetRotation()
    {
        string mag = GetAdjMag(gridPos);
        // Right, Up, Left, Down

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

        switch (magRotation)
        {
            case Rot.Right:
                transform.DORotate(new Vector3(0, 0, 0), 0.15f);
                break;
            case Rot.Up:
                transform.DORotate(new Vector3(0, 0, 90), 0.15f);
                break;
            case Rot.Left:
                transform.DORotate(new Vector3(0, 0, 180), 0.15f);
                break;
            case Rot.Down:
                transform.DORotate(new Vector3(0, 0, 270), 0.15f);
                break;
            default:
                break;
        }
    }

    private string GetAdjMag(Vector2 pos)
    {
        string magID = "0000";
        int right = Physics2D.OverlapPoint(pos + Vector2.right, LayerMask.GetMask("Crate")) ? 1: 0;
        int up = Physics2D.OverlapPoint(pos + Vector2.up, LayerMask.GetMask("Crate")) ? 1 : 0;
        int left = Physics2D.OverlapPoint(pos + Vector2.left, LayerMask.GetMask("Crate")) ? 1 : 0;
        int down = Physics2D.OverlapPoint(pos + Vector2.down, LayerMask.GetMask("Crate")) ? 1 : 0;

        magID = right + "" + up + "" + left + "" + down;
        Debug.Log(magID);

        return magID;
    }
}
