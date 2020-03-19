/*
 Código sacado de la documentación oficial de unity
 https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorsMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public ArrayList nodes;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                //moveDirection = subtractVectors3(transform.position, ((Node)nodes[0]).position);
                //nodes.RemoveAt(0);

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= speed;
            }
            else
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= speed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
    private Vector3 subtractVectors3(Vector3 v1, Vector3 v2)
    {
        Vector3 result;

        result.x = v1.x - v2.x;
        result.y = 0;
        result.z = v1.z - v2.z;

        return result;
    }
}

