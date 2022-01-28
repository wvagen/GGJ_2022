using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyElement : MonoBehaviour
{
    public Rigidbody myRig;

    bool isGrounded = false;
    float mySpeed = 0;
    float margin;

    public void Set_Me_Up(float speed, float margin)
    {
        mySpeed = speed;
        this.margin = margin;
    }

    private void Start()
    {
        transform.position += Vector3.right * margin;
        Destroy(this.gameObject, 5);
    }

    void Update()
    {
        if (MyManager.isGameRunning && isGrounded)
        myRig.velocity = Vector3.forward * mySpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
