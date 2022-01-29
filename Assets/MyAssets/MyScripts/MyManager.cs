using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MyManager : MonoBehaviour
{

    public MyAlertCanvas alertCanvas;

    public GameObject[] rowOneObstacles;
    public GameObject[] rowTwoObstacles;

    public GameObject nextStoreBlaka;

    public GameObject theCriminal;

    public GameObject cam3D, cam2D;

    public GameObject doubleBars;

    public Transform[] spawnPositions;

    public Transform rightHandOfCharacter;

    public Animator characterAnim, caissiereAnim;

    public AudioSource myAudioSource;

    public Renderer tapisRendererMat;

    public AudioClip[] musicLevels;

    public TextAsset[] recordedObstaclesMomentsTxtFiles;
    public TextAsset[] recordedBlakaMomentsTxtFiles;

    public MyCameraFollow camFollow;

    public Vector3 camOffset2D;
    public Vector3 camOffset3D;

    public PlayableDirector director;

    public float tapisRotationSpeed = 50;
    public float SNAP_HORIZONTAL = 0.25f;

    public List<float> timeMomentsObstacles = new List<float>();
    public List<float> timeMomentsBlaka = new List<float>();

    public static int levelIndex = 0;

    public static bool isGameRunning = true;
    public static bool isFirstTime = true;
    public static bool isTutorialRunning = false;

    //Tutorial Stuff

    public string tuto_phrase_0;
    public string tuto_phrase_1;
    public string tuto_phrase_2;
    public string tuto_phrase_3;
    public string tuto_phrase_4;
    public string tuto_phrase_5;
    
    //Tuto Stuff End

    int timeMomentObstacleIndex = 0;
    int timeMomentBlakaIndex = 0;

    float customizedTime = 0;
    float elementSpeed = 2;

    bool isWin = false;
    bool isSwitchingTo3D = false;

    bool canSawpObstacle = true;

    const float MARGIN_OF_TWO_ROWS_OBST = 0.16f;

    bool framesSkippedIntro = false;

    Coroutine tutoCor;

    void Start()
    {
        isGameRunning = true;

        if (isFirstTime)
        {
            isGameRunning = false;
            director.Play();
            StartCoroutine(Skip_Frame());
        }
        else
        {
            Start_Stuff();
        }
    }

    IEnumerator Skip_Frame()
    {
        yield return new WaitForSeconds(10);
        framesSkippedIntro = true;
    }

    void Start_Stuff()
    {
        if (isGameRunning)
        {
            ExtractMomentsFromFile();
            StartCoroutine(Start_Game());

            myAudioSource.clip = musicLevels[levelIndex];
            myAudioSource.PlayOneShot(musicLevels[levelIndex]);
            myAudioSource.loop = true;

            cam3D.SetActive(true);
            theCriminal.SetActive(false);
        }
    }

    void TimeLine_Stopped()
    {
        doubleBars.SetActive(false);
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            Start_Tutorial();
            cam3D.SetActive(true);
            theCriminal.SetActive(false);
        }
        else
        {
            isGameRunning = true;
            Start_Stuff();
        }
    }

    void Start_Tutorial()
    {
        tutoCor = StartCoroutine(Tutorial_Stuff());
    }

    IEnumerator Tutorial_Stuff()
    {
        isTutorialRunning = true;
        yield return new WaitForSeconds(2);

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_0);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(2);

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_1);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(2);

        Spawn_Row_Two_Obstacle();

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_2);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(3);

        Spawn_Balka();

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_3);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(3);

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_4);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        Spawn_Row_Two_Obstacle();
        yield return new WaitForSeconds(1);
        Spawn_Row_One_Obstacle();

        yield return new WaitForSeconds(1);


        Spawn_Balka();

        yield return new WaitForSeconds(2);

        alertCanvas.Display_Alert("Tutorial", tuto_phrase_5);
        Time.timeScale = 0.1f;

        while (Time.timeScale < 0.9f) yield return new WaitForEndOfFrame();

        PlayerPrefs.SetInt("tutorial", 1);

        isGameRunning = true;
        isTutorialRunning = false;
        isFirstTime = false;
        Start_Stuff();

    }

    private void Update()
    {
        Rotate_Tapis_Texture_Speed();
        Spawn_Obstacle();
        Spawn_Blaka();

        if (isGameRunning)
        {
            customizedTime += Time.deltaTime;
        }

        if (framesSkippedIntro)
        {
            framesSkippedIntro = false;
            TimeLine_Stopped();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator Start_Game()
    {
        yield return new WaitForSeconds(1.5f);
        myAudioSource.enabled = true;
    }

    void ExtractMomentsFromFile()
    {
        StreamReader reader = new StreamReader(new MemoryStream(recordedObstaclesMomentsTxtFiles[levelIndex].bytes));
        string line = "";
        do
        {
            line = reader.ReadLine();
            if (line != null)
                timeMomentsObstacles.Add(float.Parse(line));
        } while (line != null);

        reader = new StreamReader(new MemoryStream(recordedBlakaMomentsTxtFiles[levelIndex].bytes));
        line = "";
        do
        {
            line = reader.ReadLine();
            if (line != null)
                timeMomentsBlaka.Add(float.Parse(line));
        } while (line != null);

    }

    void Spawn_Obstacle()
    {
        if (isGameRunning && timeMomentObstacleIndex < timeMomentsObstacles.Count && !isWin)
        {
            if (customizedTime > timeMomentsObstacles[timeMomentObstacleIndex])
            {
                //Spawn Obstacles Behaviour here


                if (canSawpObstacle)
                {

                    if (Random.Range(0, 2) % 2 == 0)
                        Spawn_Row_One_Obstacle();
                    else
                        Spawn_Row_Two_Obstacle();
                }

                timeMomentObstacleIndex++;

                if (timeMomentObstacleIndex >= timeMomentsObstacles.Count)
                {
                    StartCoroutine(Win_Behavior());
                }
            }
        }
    }

    IEnumerator Win_Behavior()
    {
        yield return new WaitForSeconds(2);
        alertCanvas.Win();
        isGameRunning = false;
        isWin = true;
    }

    public void Lose()
    {
        alertCanvas.Lose();
        if (isTutorialRunning) StopCoroutine(tutoCor);
        characterAnim.Play("Death_Anim");
        caissiereAnim.Play("Death_Anim");
        isGameRunning = false;
    }

    void Spawn_Blaka()
    {
        if (isGameRunning && !isWin)
        {
            if (customizedTime > timeMomentsBlaka[timeMomentBlakaIndex])
            {
                //Spawn Blaka Behaviour here

                Spawn_Balka();
                StartCoroutine(Sleep_Time_Between_Blayek());

                timeMomentBlakaIndex++;
            }
        }
    }

    IEnumerator Sleep_Time_Between_Blayek()
    {
        canSawpObstacle = false;
        yield return new WaitForSeconds(1);
        canSawpObstacle = true;
    }

    void Spawn_Row_One_Obstacle()
    {

        characterAnim.Play("Pick_Up_Anim", -1, 0);

        byte randomSideA = (byte)Random.Range(0, spawnPositions.Length);
        byte randomSideB;
        byte randomElementIndex = (byte)Random.Range(0, rowOneObstacles.Length);

        do
        {
            randomSideB = (byte)Random.Range(0, spawnPositions.Length);
        } while (randomSideB == randomSideA);

        for (int i = 0; i < 2; i++)
        {
            MyElement element = Instantiate(rowOneObstacles[randomElementIndex],
                parent: spawnPositions[i % 2 == 0 ? randomSideA : randomSideB]).GetComponent<MyElement>();
            element.Set_Me_Up(elementSpeed, 0);
        }

        GameObject obj = Instantiate(rowOneObstacles[randomElementIndex],
                parent: rightHandOfCharacter);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;

        Destroy(obj, 0.5f);

    }

    void Spawn_Row_Two_Obstacle()
    {

        characterAnim.Play("Pick_Up_Anim", -1, 0);

        byte randomSide = (byte)(Random.Range(0, 2) % 2 == 0 ? 0 : 2);
        byte randomElementIndex = (byte)Random.Range(0, rowTwoObstacles.Length);

        MyElement element = Instantiate(rowTwoObstacles[randomElementIndex],
            parent: spawnPositions[randomSide]).GetComponent<MyElement>();
        element.Set_Me_Up(elementSpeed, randomSide == 0 ? -MARGIN_OF_TWO_ROWS_OBST : MARGIN_OF_TWO_ROWS_OBST);

        GameObject obj = Instantiate(rowTwoObstacles[randomElementIndex],
                parent: rightHandOfCharacter);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<BoxCollider>().enabled = false;

        Destroy(obj, 0.5f);
    }

    void Spawn_Balka()
    {
        MyElement element = Instantiate(nextStoreBlaka,
                parent: spawnPositions[1]).GetComponent<MyElement>();
        element.Set_Me_Up(elementSpeed, 0);
    }

    void Rotate_Tapis_Texture_Speed()
    {
      tapisRendererMat.material.SetTextureOffset("_MainTex", Vector2.left * tapisRotationSpeed * Time.deltaTime);
    }

    public void Switch_2D_3D()
    {
        if (isSwitchingTo3D)
        {
            camFollow.offset = camOffset3D;
        }
        else
        {
            camFollow.offset = camOffset2D;
        }
        StartCoroutine(Switch_CAMS());
        isSwitchingTo3D = !isSwitchingTo3D;
    }

    IEnumerator Switch_CAMS()
    {
        if (isSwitchingTo3D)
        {
            cam3D.SetActive(true);
            cam2D.SetActive(false);
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(1);
            cam3D.SetActive(false);
            cam2D.SetActive(true);
        }
    }

}
