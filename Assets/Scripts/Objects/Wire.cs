using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private PowerSource power;
    public List<PowerSource> powerSources;

    private SpriteRenderer sprite;
    public Sprite powerOffSprite;
    public Sprite powerOnSprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        power = GetComponent<PowerSource>();
        power.isPowered = false;
    }

    private void Update()
    {
        sprite.sprite = power.isPowered ? powerOnSprite : powerOffSprite;

        bool poweredState = false;
        for (int i = 0; i < powerSources.Count; i++)
        {
            if (powerSources[i].isPowered) { poweredState = true; }
        }
        power.isPowered = poweredState;
    }
}