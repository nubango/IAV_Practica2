namespace UCM.IAV.Movimiento
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
}
