﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject fadePanel;
    public float fadeTime = 0.5f;
    private float fadeInDelay = 0.25f;
    private Image fadeImage;
    private string targetScene;
    private string curScene;
    private bool isFading;
    private bool isSwitching;
    private float fadeTimeCur;

    public AudioClip clickSound;

    private void Awake()
    {
        curScene = SceneManager.GetActiveScene().name;
        GlobalData.curScene = curScene;
        Debug.Log("Current scene: " + curScene);
    }

    private void Start()
    {
        if (curScene == "TitleScreen")
        {
            //GlobalData.LastScene = curScene;
        }

        if (fadePanel == null)
        {
            fadePanel = GameObject.Find("Panel");
        }
        fadePanel.SetActive(true);
        fadeImage = fadePanel.GetComponent<Image>();

        Invoke("ExitFade", fadeInDelay);
    }

    private void Update()
    {
        fadeTimeCur = Mathf.MoveTowards(fadeTimeCur, 0f, Time.unscaledDeltaTime);
        if (fadeTimeCur != 0)
        {
            isFading = true;
        }
        else
        {
            isFading = false;
        }

        if (isSwitching && !isFading)
        {
            if (targetScene == "")
            {
                ExitFade();
            }

            if (targetScene == "Quit")
            {
                Application.Quit();
            }
            else
            {
                fadeImage.DOKill(true);
                SceneManager.LoadScene(targetScene);
                Debug.Log("Switched from " + curScene + " to " + targetScene);
            }

            isSwitching = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (curScene == "TitleScreen")
            {
                QuitGame();
            }
            else
            {
                SceneSwitch("TitleScreen");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { SceneSwitch("TestScene"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SceneSwitch("TestScene 1"); }
    }

    public void SceneSwitch(string scene)
    {
        StartFade();
        targetScene = scene;
    }

    public void StartFade()
    {
        if (!isSwitching && !isFading)
        {
            fadeImage.DOFade(1, fadeTime).SetEase(Ease.InOutSine);

            isSwitching = true;
            fadeTimeCur = fadeTime;

            AudioSource source = GetComponent<AudioSource>();

            if (source != null)
            {
                source.clip = clickSound;
                source.Play();
            }
        }
    }

    public void ExitFade()
    {
        isFading = true;
        isSwitching = false;
        fadeTimeCur = fadeTime;
        Vector4 initialColor = fadeImage.color;
        fadeImage.DOFade(0, fadeTime).SetEase(Ease.InOutSine);
        FindObjectOfType<AudioManager>().Play("Swoosh");
    }

    public void QuitGame()
    {
        StartFade();
        targetScene = "Quit";
        Debug.Log("Silly human. You know you can't quit the game from the editor!");
    }
}