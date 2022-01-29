using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Image loadingFillableImg;
    public GameObject blockingImg;

    public Sprite musicBtnOnSprite,musicBtnOffSprite;

    public static bool musicOn = true;
    public static bool sfxOn = true;

    GameObject hoveredGO;

    static int lostTxtIndex = 0;

    bool isGameOver = false;

    public Material allThePlaceMat;

    private void Start()
    {
        SoundsStats();
        allThePlaceMat.color = Color.white;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale > 0)
                    Pause_Btn();
                else
                    Resume();
            }
        }
    }


    IEnumerator LoadScene(string sceneName_Path)
    {
        blockingImg.SetActive(true);
        loadingFillableImg.gameObject.SetActive(true);
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName_Path);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        Color white = Color.white;
        white.a = 0;

        while (!asyncOperation.isDone)
        {
            //Output the current progress
            white.a = asyncOperation.progress;
            loadingFillableImg.color = white;

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            blockingImg.SetActive(false);
            yield return null;
        }

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

    public void Play_Btn()
    {
        myAnim.Play("Display_Level_Selection_Panel");
    }

    public void Play_Tutorial()
    {
        PlayerPrefs.DeleteKey("tutorial");
        MyManager.levelIndex = 0;
        MyManager.isFirstTime = true;
        Retry();
    }

    public void Return_From_LevelSelection()
    {
        myAnim.Play("Hide_Level_Selection_Panel");
    }

    public void Retry()
    {
        StartCoroutine(LoadScene("MainGame"));
    }

    public void HomeBtn()
    {
        myMan.
        StartCoroutine(LoadScene("MainMenu"));
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
        isGameOver = true;

        PlayerPrefs.SetInt("ReachedLevel" + "_" + MyManager.levelIndex,1);

        if (sfxOn)
        myAudioSource.PlayOneShot(winSFX);
    }

    public void Lose()
    {
        myAnim.Play("Display_Loss");
        musicPlayer.pitch = 0.5f;
        isGameOver = true;

        if (sfxOn)
            myAudioSource.PlayOneShot(lossSFX);

        if (MyManager.isTutorialRunning)
        {
            loserTxt.text = "Bruh.. We're still in tutorial ...";
        }
        else
        {
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
        musicPlayer.Pause();
        myAnim.Play("PausePanel");
    }

    public void Resume()
    {
        myAnim.Play("Resume_Anim");

        if (musicOn)
            musicPlayer.UnPause();

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
