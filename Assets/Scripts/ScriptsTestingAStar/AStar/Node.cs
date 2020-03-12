using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable
{
    #region Fields
    public float nodeTotalCost;         // Coste total real hasta este nodo
    public float estimatedCost;         // Coste estimado desde este nodo hsta el nodo final
    public bool bObstacle;              // Determina si en este nodo hay un obstaculo o no
    public Node parent;                 // Nodo padre 
    public Vector3 position;            // Posicion del nodo
    #endregion

    /// <summary>
    // Constructor por defecto
    /// </summary>
    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    /// <summary>
    // Constructor añadiendole por parametro la posicion
    /// </summary>
    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;

        this.position = pos;
    }

    /// <summary>
    // Hacer un nodo obstaculo
    /// </summary>
    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    /// <summary>
    // El método CompareTo afecta a el método de ordenacion Sort
    // Se aplica cuando se llama al método Sort desde ArrayList
    // Comparar usando el coste estimado entre dos nodos
    /// </summary>
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        if (this.estimatedCost > node.estimatedCost)
            return 1;

        return 0;
    }
}


