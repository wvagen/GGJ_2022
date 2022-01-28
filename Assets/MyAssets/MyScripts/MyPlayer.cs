using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public MyManager man;
    public GameObject player;

    public GameObject explosionEffect;
    public GameObject fireEffect;

    public Material allThePlaceMat;

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
                player.transform.position += Vector3.left * man.SNAP_HORIZONTAL;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.transform.position += Vector3.right * man.SNAP_HORIZONTAL;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("LOST HERE");
            Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 3);
            Instantiate(fireEffect, transform.position, Quaternion.identity);
            allThePlaceMat.color = Color.black;
            transform.Rotate(Vector3.forward * 90);
            gameObject.SetActive(false);
        }
    }

}
