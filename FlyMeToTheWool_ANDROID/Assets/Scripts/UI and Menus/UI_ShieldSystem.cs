using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UI_ShieldSystem : MonoBehaviour
{
    private float shieldbarWidth;
    private float shieldBarHeight;
    public float shieldTime = 10;
    private RectTransform shieldBarRect;

    private void Awake()
    {
        shieldBarRect = transform.GetChild(2).GetComponent<RectTransform>();
    }

    private void Start()
    {
        shieldbarWidth = shieldBarRect.rect.width / shieldTime;
        shieldBarHeight = shieldBarRect.rect.height;
    }

    public void UpdateShieldbar(float shieldedTime, float totalShieldTime)
    {
            shieldBarRect.sizeDelta = new Vector2(shieldedTime * shieldbarWidth, shieldBarHeight);
    }
}
