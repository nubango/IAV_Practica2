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
    
    using System;

    /// <summary>
    /// Edge es la conexión, un objeto que mantiene un vértice y su coste
    /// </summary>
    [System.Serializable]
    public class Edge : IComparable<Edge>
    {
        public float cost;
        public Vertex vertex;

        public Edge(Vertex vertex = null, float cost = 1f)
        {
            this.vertex = vertex;
            this.cost = cost;
        }

        public int CompareTo(Edge other)
        {
            float result = cost - other.cost;
            int idA = vertex.GetInstanceID();
            int idB = other.vertex.GetInstanceID();
            if (idA == idB)
                return 0;
            return (int)result;
        }

        public bool Equals(Edge other)
        {
            return (other.vertex.id == this.vertex.id);
        }

        public override bool Equals(object obj)
        {
            Edge other = (Edge)obj;
            return (other.vertex.id == this.vertex.id);
        }

        public override int GetHashCode()
        {
            return this.vertex.GetHashCode();
        }
    }
}