using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Vector2 gridPos;
    public bool isPowered;

    private SpriteRenderer sprite;
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private InputHandler iman;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        iman = FindObjectOfType<InputHandler>();
    }

    // Update is called once per frame
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

        gridPos += direction;

        //transform.position = gridPos;
        if (direction != Vector2.zero)
        {
            transform.DOMove(gridPos, 0.2f);
        }
    }
}
