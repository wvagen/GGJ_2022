using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Over_Level_Btn : MonoBehaviour
{
    public byte myLevelIndex;

    public Image myImg;
    public GameObject starsPanel;

    public GameObject levelIndexGameObject;

    public Sprite lockedSprite;
    public GameObject[] stars;

    Game_Over_Manager gameOverMan;

    public bool isFinishedLevel = false;

    bool isLevelReached = false;
    // Start is called before the first frame update
    void Start()
    {
        gameOverMan = FindObjectOfType<Game_Over_Manager>();
        Reached_Level_Stars_Counting_Behaviour();
    }

    void Reached_Level_Stars_Counting_Behaviour()
    {
        if (PlayerPrefs.HasKey(Game_Over_Manager.GAME_DATA_KEY + gameOverMan.levelMapGameName + "_" + myLevelIndex))
        {
            for (int i = 0; i < PlayerPrefs.GetInt(Game_Over_Manager.GAME_DATA_KEY + gameOverMan.levelMapGameName + "_" + myLevelIndex, 0); i++)
            {
                stars[i].SetActive(true);
            }
            isLevelReached = true;
            isFinishedLevel = true;
        }
        else if (myLevelIndex == 0 || PlayerPrefs.HasKey(Game_Over_Manager.GAME_DATA_KEY + gameOverMan.levelMapGameName + "_" + (myLevelIndex - 1)))
        {
            starsPanel.SetActive(false);
            isLevelReached = true;
        }
        else
        {
            myImg.sprite = lockedSprite;
            starsPanel.SetActive(false);
            levelIndexGameObject.SetActive(false);
            isLevelReached = false;
        }
    }

    public void Start_Level()
    {
        if (isLevelReached)
        {
            Game_Over_Manager.levelIndex = myLevelIndex;
            gameOverMan.Start_Game();
            Game_Over_Manager.isLevel = true;
        }
        else
        {
            gameOverMan.Open_Info_Panel("عَلَيْكَ إِنْهَاءُ المَرَاحِلِ السَّابِقَةِ أوّلًا");
        }
    }

}
