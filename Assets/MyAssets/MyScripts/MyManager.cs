using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MyManager : MonoBehaviour
{
    public GameObject[] rowOneObstacles;
    public GameObject[] rowTwoObstacles;

    public Transform[] spawnPositions;

    public AudioSource myAudioSource;

    public Renderer tapisRendererMat;

    public TextAsset recordedMomentsTxtFile;

    public float tapisRotationSpeed = 50;
    public float SNAP_HORIZONTAL = 0.25f;

    public List<float> timeMoments = new List<float>();

    public static bool isGameRunning = true;

    int timeMomentIndex = 0;

    float customizedTime = 0;
    float elementSpeed = 2;

    bool isWin = false;

    const float MARGIN_OF_TWO_ROWS_OBST = 0.16f;

    void Start()
    {
        ExtractMomentsFromFile();
        StartCoroutine(Start_Game());
    }

    private void Update()
    {
        Rotate_Tapis_Texture_Speed();
        Spawn_Obstacle();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator Start_Game()
    {
        yield return new WaitForSeconds(3);
        myAudioSource.enabled = true;
    }

    void ExtractMomentsFromFile()
    {
        StreamReader reader = new StreamReader(new MemoryStream(recordedMomentsTxtFile.bytes));
        string line = "";
        do
        {
            line = reader.ReadLine();
            if (line != null)
                timeMoments.Add(float.Parse(line));
        } while (line != null);
    }

    void Spawn_Obstacle()
    {
        if (isGameRunning && !isWin) {
            customizedTime += Time.deltaTime;
            if (customizedTime > timeMoments[timeMomentIndex])
            {
                //Spawn Obstacles Behaviour here

                if (Random.Range(0, 2) % 2 == 0)
                    Spawn_Row_One_Obstacle();
                else
                    Spawn_Row_Two_Obstacle();


                timeMomentIndex++;

                if (timeMomentIndex >= timeMoments.Count)
                {
                    //Win Behaviour
                    isWin = true;
                }
            }
        }
    }

    void Spawn_Row_One_Obstacle()
    {
        byte randomSideA = (byte) Random.Range(0, spawnPositions.Length);
        byte randomSideB;

        do
        {
            randomSideB = (byte)Random.Range(0, spawnPositions.Length);
        } while (randomSideB == randomSideA);

        for (int i = 0; i < 2; i++)
        {
            MyElement element = Instantiate(rowOneObstacles[Random.Range(0, rowOneObstacles.Length)],
                parent: spawnPositions[i % 2 == 0 ? randomSideA : randomSideB]).GetComponent<MyElement>();
            element.Set_Me_Up(elementSpeed,0);
        }
    }

    void Spawn_Row_Two_Obstacle()
    {
        byte randomSide = (byte) (Random.Range(0, 2) % 2 == 0 ? 0 : 2);


            MyElement element = Instantiate(rowTwoObstacles[Random.Range(0, rowTwoObstacles.Length)],
                parent: spawnPositions[randomSide]).GetComponent<MyElement>();
            element.Set_Me_Up(elementSpeed, randomSide == 0 ? -MARGIN_OF_TWO_ROWS_OBST : MARGIN_OF_TWO_ROWS_OBST);
    }

    void Rotate_Tapis_Texture_Speed()
    {
        if (isGameRunning)
        {
            tapisRendererMat.material.SetTextureOffset("_MainTex", Vector2.left * tapisRotationSpeed * Time.deltaTime);
        }
    }

}
