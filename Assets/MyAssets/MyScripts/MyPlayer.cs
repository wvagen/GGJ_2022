using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public MyManager man;
    public GameObject player;

    public GameObject explosionEffect;
    public GameObject fireEffect;

    public GameObject transitionEffect;

    public Material allThePlaceMat;

    int playerPosIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        allThePlaceMat.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Shift();
    }

    void Rotate()
    {
        if (MyManager.isGameRunning || MyManager.isTutorialRunning)
        {
            transform.Rotate(Vector3.left * man.tapisRotationSpeed * 10 * Time.deltaTime);
        }
    }

    void Shift()
    {
        if (MyManager.isGameRunning || MyManager.isTutorialRunning)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && playerPosIndex < 1)
            {
                Destroy(Instantiate(transitionEffect, player.transform.position, Quaternion.identity), 3);
                player.transform.position += Vector3.left * man.SNAP_HORIZONTAL;
                playerPosIndex++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerPosIndex > -1)
            {
                Destroy(Instantiate(transitionEffect, player.transform.position, Quaternion.identity), 3);
                player.transform.position += Vector3.right * man.SNAP_HORIZONTAL;
                playerPosIndex--;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 3);
            Instantiate(fireEffect, transform.position, Quaternion.identity);
            allThePlaceMat.color = Color.black;
            man.Lose();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Switch_2D_3D")
        {
            Destroy(other.transform.parent.gameObject);
            man.Switch_2D_3D();
        }
        if (other.gameObject.tag == "letsgo")
        {
            Destroy(other.transform.parent.gameObject);
            man.isStartTheDrop = true;
        }
    }

}
