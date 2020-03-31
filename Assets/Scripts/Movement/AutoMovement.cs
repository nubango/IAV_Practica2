/*
 Código sacado de la documentación oficial de unity
 https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
 */
/*
 Esta clase añade la funcionalidad de movimiento WASD al jugador y también 
 implementa el movimiento automático al pulsar la barra espaciadora

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    CharacterController characterController;
    TestCode testCode;

    public float speed = 6.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        testCode = GameObject.Find("A*").GetComponent<TestCode>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {

            if (Input.GetKey(KeyCode.S) && testCode.pathArray.Count > 1)
            {
                moveDirection = subtractVectors3(((Node)testCode.smoothArray[1]).position, transform.position).normalized;

                //UnityEngine.Vector3 ray = characterController.velocity.normalized;


                //UnityEngine.RaycastHit hit;

                ////COLLISION
                //if (UnityEngine.Physics.Raycast(transform.position, ray, out hit, 0.3f) && hit.collider.gameObject.tag == "Obstacle")
                //{
                //    Debug.Log("HIT");
                //    Vector3 aux = Vector3.Normalize(hit.collider.gameObject.transform.position + hit.normal) * 2.0f;
                //    moveDirection = moveDirection + aux;
                //    moveDirection.Normalize();
                //}


                moveDirection *= speed;
            }
            else if (Input.GetKey(KeyCode.Space) && testCode.pathArray.Count > 1)
            {
                if (testCode.pathArray.Count > 2 && !Physics.Linecast(transform.position, ((Node)testCode.pathArray[2]).position))
                {
                    moveDirection = subtractVectors3(((Node)testCode.pathArray[2]).position, transform.position).normalized;
                    moveDirection *= speed;
                }
                else
                {
                    moveDirection = subtractVectors3(((Node)testCode.pathArray[1]).position, transform.position).normalized;
                    moveDirection *= speed;
                }

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

