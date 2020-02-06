using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float repeatDelay = 0.5f;
    public float repeatGap = 0.1f;

    private float rightTimer;
    private float upTimer;
    private float leftTimer;
    private float downTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InputRead(string inputButton)
    {
        bool isTrue = false;

        switch (inputButton)
        {
            case "rightD": // Right direction down
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isTrue = true;
                }
                break;

            case "upD": // Up direction down
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isTrue = true;
                }
                break;

            case "leftD": // Left direction down
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isTrue = true;
                }
                break;

            case "downD": // down direction down
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    isTrue = true;
                }
                break;

            case "confirmD": // Confirm button down
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                {
                    isTrue = true;
                }
                break;
        }

        return isTrue;
    }
}
