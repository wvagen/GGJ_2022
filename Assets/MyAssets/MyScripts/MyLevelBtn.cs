using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLevelBtn : MonoBehaviour
{
    public byte myLevelIndex;

    public Image myImg;

    public GameObject levelIndexGameObject;

    public Sprite lockedSprite;

    public MyAlertCanvas alertCanvas;

    public bool isFinishedLevel = false;

    bool isLevelReached = false;
    // Start is called before the first frame update
    void Start()
    {
        Reached_Level();
    }

    void Reached_Level()
    {
        if (PlayerPrefs.HasKey("ReachedLevel" + "_" + myLevelIndex))
        {
            isLevelReached = true;
            isFinishedLevel = true;
        }
        else if (myLevelIndex == 0 || PlayerPrefs.HasKey("ReachedLevel" + "_" + (myLevelIndex - 1)))
        {
            isLevelReached = true;
        }
        else
        {
            myImg.sprite = lockedSprite;
            levelIndexGameObject.SetActive(false);
            isLevelReached = false;
        }
    }

    public void Start_Level()
    {
        if (isLevelReached)
        {
            MyManager.levelIndex = myLevelIndex;
            alertCanvas.Retry();
        }
        else
        {
            alertCanvas.Display_Alert("Info", "You must finish the previous levels first");
        }
    }

}
