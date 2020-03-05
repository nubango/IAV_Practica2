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

    // Punto de visibilidad (de su correspondiente esquema de división)
    public class VertexVisibility : Vertex
    {
        
        // Despertar con una lista vacía de vecinos
        void Awake()
        {
            neighbours = new List<Edge>();
        }

        // Encontrar a los vecinos (por visibilidad)
        public void FindNeighbours(List<Vertex> vertices)
        {
            Collider c = gameObject.GetComponent<Collider>();
            c.enabled = false;
            Vector3 direction = Vector3.zero;
            Vector3 origin = transform.position;
            Vector3 target = Vector3.zero;
            RaycastHit[] hits;
            Ray ray;
            float distance = 0f;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] == this)
                    continue;
                target = vertices[i].transform.position;
                direction = target - origin;
                distance = direction.magnitude;
                ray = new Ray(origin, direction);
                hits = Physics.RaycastAll(ray, distance);
                if (hits.Length == 1)
                {
                    if (hits[0].collider.gameObject.tag.Equals("Vertex"))
                    {
                        Edge e = new Edge();
                        e.cost = distance;
                        GameObject go = hits[0].collider.gameObject;
                        Vertex v = go.GetComponent<Vertex>();
                        if (v != vertices[i])
                            continue;
                        e.vertex = v;
                        neighbours.Add(e);
                    }
                }
            }
            c.enabled = true;
        }

        /// <summary>
        /// ESTO ES SÓLO PARA TESTEAR
        /// </summary>
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Vector3 originPos, targetPos;
            originPos = transform.position;
            foreach (Edge e in neighbours)
            {
                targetPos = e.vertex.transform.position;
                Gizmos.DrawLine(originPos, targetPos);
            }
        }
    }
}