
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeSystem : MonoBehaviour
{
    private float lifePointWidth;
    private float lifePointHeight;
    private int numOfLives = 7;
    private RectTransform livesRect;
    private Animator anim;

    private void Awake()
    {
        livesRect = transform.GetChild(0).GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        lifePointWidth = livesRect.rect.width / numOfLives;
        lifePointHeight = livesRect.rect.height;
    }

    public void UpdateLives(int lives)
    {
        if (lives > numOfLives)
            lives = numOfLives;
        else
        {
            livesRect.sizeDelta = new Vector2(lives * lifePointWidth, lifePointHeight);
            anim.Play("textShake");
        }
        if (lives == 1)
            anim.Play("oneLive");
    }
}
