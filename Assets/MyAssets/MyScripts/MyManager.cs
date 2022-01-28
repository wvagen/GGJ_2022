using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MyManager : MonoBehaviour
{

    public GameObject[] rowOneObstacles;

    public Transform[] spawnPositions;

    public AudioSource myAudioSource;

    public Renderer tapisRendererMat;

    public TextAsset recordedMomentsTxtFile;

    public float tapisRotationSpeed = 50;
    public float SNAP_HORIZONTAL = 0.22f;

    public List<float> timeMoments = new List<float>();

    public static bool isGameRunning = true;

    int timeMomentIndex = 0;

    float customizedTime = 0;
    float elementSpeed = 2;

    bool isWin = false;

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
                Spawn_Row_One_Obstacle();
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
        MyElement element = Instantiate(rowOneObstacles[Random.Range(0, rowOneObstacles.Length)],
            parent : spawnPositions[Random.Range(0, spawnPositions.Length)]).GetComponent<MyElement>();
        element.Set_Me_Up(elementSpeed);

    }

    void Rotate_Tapis_Texture_Speed()
    {
        if (isGameRunning)
        {
            tapisRendererMat.material.SetTextureOffset("_MainTex", Vector2.left * tapisRotationSpeed * Time.deltaTime);
        }
    }

}
