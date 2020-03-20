using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Clase que sirve para el seguimiento de la cámara al jugador 
*/

public class camFollow : MonoBehaviour
{
    public GameObject personaje;
    private Vector3 posicionRelativa;
    // Start is called before the first frame update
    public void Start()
    {
        posicionRelativa = transform.position - personaje.transform.position;
    }

    public void LateUpdate()
    {
        transform.position = personaje.transform.position + posicionRelativa;
    }
}

