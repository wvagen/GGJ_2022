using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public MyManager man;
    public GameObject player;

    public float SNAP_HORIZONTAL = 0.22f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Shift();
    }

    void Rotate()
    {
        if (MyManager.isGameRunning)
        {
            transform.Rotate(Vector3.left * man.tapisRotationSpeed * 10 * Time.deltaTime);
        }
    }

    void Shift()
    {
        if (MyManager.isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                player.transform.position += Vector3.left * SNAP_HORIZONTAL;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.transform.position += Vector3.right * SNAP_HORIZONTAL;
            }
        }
    }
}
