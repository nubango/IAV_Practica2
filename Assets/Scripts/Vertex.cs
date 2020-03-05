/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Navegacion
{

    using UnityEngine;
    using System.Collections.Generic;
    
    // Puntos representativos (común a todos los esquemas de división, o la mayoría)
    [System.Serializable]
    public class Vertex : MonoBehaviour
    {
        /// <summary>
        /// Vertex ID
        /// </summary>
        public int id;
        /// <summary>
        /// Vertex neighbours
        /// </summary>
        public List<Edge> neighbours;
        [HideInInspector]
        public Vertex prev;
    }
}
