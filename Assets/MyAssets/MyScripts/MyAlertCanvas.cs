using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyAlertCanvas : MonoBehaviour 
{
    public MyManager myMan;

    public Animator myAnim;
    public Text alertTitleTxt, alertMsgTxt;

    public Text loserTxt;

    public AudioSource myAudioSource;
    public AudioSource musicPlayer;

    public AudioClip infoPanelAudioClip,hoverBtnSFX, musicSfxSwitch;

    public AudioClip winSFX;
    public AudioClip lossSFX;

    public string[] loserStrings;

    public Image musicImg, sfxImg;

    public Sprite musicBtnOnSprite,musicBtnOffSprite;

    public static bool musicOn = true;
    public static bool sfxOn = true;

    GameObject hoveredGO;

    static int lostTxtIndex = 0;

    private void Start()
    {
        SoundsStats();
        Time.timeScale = 1;
    }

    public void Display_Alert(string title,string msg)
    {
        alertTitleTxt.text = title;
        Display_Alert(msg);
    }

    public void Display_Alert(string msg)
    {
        alertMsgTxt.text = msg;
        if (sfxOn)
        {
            myAudioSource.PlayOneShot(infoPanelAudioClip);
        }
        myAnim.Play("Display_Alert_Anim");
    }

    public void Hover_Btn(GameObject eventData)
    {
        if (sfxOn)
        myAudioSource.PlayOneShot(hoverBtnSFX);

        hoveredGO = eventData.gameObject;
        hoveredGO.transform.localScale = Vector3.one * 1.2f;
    }

    public void UnHover_Btn()
    {
        if (hoveredGO != null)
        {
            hoveredGO.transform.localScale = Vector2.one;
        }
    }

    public void Win()
    {
        myAnim.Play("Display_Victory");

        if(sfxOn)
        myAudioSource.PlayOneShot(winSFX);
    }

    public void Lose()
    {
        myAnim.Play("Display_Loss");
        musicPlayer.pitch = 0.5f;

        if (sfxOn)
            myAudioSource.PlayOneShot(lossSFX);

        if (lostTxtIndex < loserStrings.Length)
        {
            loserTxt.text = loserStrings[lostTxtIndex];
            lostTxtIndex++;
        }
        else
        {
            loserTxt.text = loserStrings[Random.Range(0, loserStrings.Length)];
        }
    }

    void SoundsStats()
    {
        musicOn = (PlayerPrefs.GetInt("Sound_Enabled", 1) == 1);

        musicPlayer.mute = !musicOn;
        myAudioSource.mute = !sfxOn;

        if (musicOn)
        {
            musicImg.sprite = musicBtnOnSprite;
        }
        else
        {
            musicImg.sprite = musicBtnOffSprite;
        }

        if (sfxOn)
        {
            sfxImg.sprite = musicBtnOnSprite;
        }
        else
        {
            sfxImg.sprite = musicBtnOffSprite;
        }
    }

    public void MusicBtn()
    {
        myAudioSource.PlayOneShot(musicSfxSwitch);
        if (musicOn)
        {
            musicOn = false;
            musicImg.sprite = musicBtnOffSprite;
            PlayerPrefs.SetInt("Sound_Enabled", 0);
        }
        else
        {
            musicOn = true;
            musicImg.sprite = musicBtnOnSprite;
            PlayerPrefs.SetInt("Sound_Enabled", 1);
        }
        musicPlayer.mute = !musicOn;
    }

    public void Pause_Btn()
    {
        Time.timeScale = 0;
        myAnim.Play("PausePanel");
    }

    public void Resume()
    {
        myAnim.Play("Resume_Anim");
        Time.timeScale = 1;
    }

    public void Settings_Btn()
    {
        myAnim.Play("Display_Settings");
    }

    public void SfxBtn()
    {
        myAudioSource.PlayOneShot(musicSfxSwitch);

        if (sfxOn)
        {
            sfxOn = false;
            sfxImg.sprite = musicBtnOffSprite;
            myAudioSource.mute = true;
        }
        else
        {
            sfxOn = true;
            sfxImg.sprite = musicBtnOnSprite;
            myAudioSource.mute = false;
        }
    }
}
