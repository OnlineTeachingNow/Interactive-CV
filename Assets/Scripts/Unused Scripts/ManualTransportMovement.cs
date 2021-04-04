using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTransportMovement : MonoBehaviour
{
    public float moveSpeed = 15;
    private Vector3 moveDir;

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime); //wants this 
    }
}
