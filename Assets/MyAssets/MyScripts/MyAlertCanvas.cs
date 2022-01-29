using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyAlertCanvas : MonoBehaviour
{
    public Animator myAnim;
    public Text alertTitleTxt, alertMsgTxt;
    public AudioSource myAudioSource;

    public AudioClip infoPanelAudioClip,hoverBtnSFX;

    public static bool isSfxOn = true;
    public static bool isMusicOn = true;

    GameObject hoveredGO;

    public void Display_Alert(string title,string msg)
    {
        alertTitleTxt.text = title;
        Display_Alert(msg);
    }

    public void Display_Alert(string msg)
    {
        alertMsgTxt.text = msg;
        if (isSfxOn)
        {
            myAudioSource.PlayOneShot(infoPanelAudioClip);
        }
        myAnim.Play("Display_Alert_Anim");
    }

    public void Hover_Btn()
    {
        myAudioSource.PlayOneShot(hoverBtnSFX);
        hoveredGO = EventSystem.current.currentSelectedGameObject;
        hoveredGO.transform.localScale = Vector3.one * 1.2f;
    }

    public void UnHover_Btn()
    {
        hoveredGO.transform.localScale = Vector2.one ;
    }
}
