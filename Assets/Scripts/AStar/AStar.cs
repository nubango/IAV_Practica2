/*
 Código sacado del repositorio prorcionado en el enunciado de la práctica
 https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition/tree/master/Chapter07
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar
{
    #region List fields

    public static PriorityQueue openList;
    public static HashSet<Node> closedList;

    #endregion

    /// <summary>
    /// Devuelve el camino final (se llama cuando se ha llegado a la salida)
    /// </summary>
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    /// <summary>
    /// Calcula el coste de el nodo actual al nodo final 
    /// (la heurística usada es la distancia hasta el nodo final)
    /// </summary>
    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        ArrayList actionArea = Minotaur.instance.getActionArea();

        float minotaurCost = 0;
        int i = 0;
        bool find = false;
        while (i < actionArea.Count && !find)
        {
            NodeMinotaur nodeMinotaur = (NodeMinotaur)actionArea[i];
            if (curNode.position == nodeMinotaur.position || goalNode.position == nodeMinotaur.position)
            {
                find = true;
                minotaurCost = nodeMinotaur.cost;
            }
            i++;
        }


        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude + minotaurCost;
    }

    /// <summary>
    /// Encuentra el camino más corto entre Teseo y la salida
    /// </summary>
    public static ArrayList FindPath(Node start, Node goal)
    {
        // Creamos la lista de nodos abiertos y metemos el nodo inicial (Teseo)
        // el coste del nodo inicial es cero
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new HashSet<Node>();
        Node node = null;

        while (openList.Length != 0)
        {
            node = openList.First();

            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }

            ArrayList neighbours = new ArrayList();
            GridManager.instance.GetNeighbours(node, neighbours);

            #region CheckNeighbours

            // Recorro la lista de vecinos por los que se puede ir (los que no tienen obstaculos)
            for (int i = 0; i < neighbours.Count; i++)
            {
                // Cojo el vecino y compruebo si no es un nodo ya explorado (no está en la lista de nodos cerrados)
                Node neighbourNode = (Node)neighbours[i];

                if (!closedList.Contains(neighbourNode))
                {
                    // Coste estimado por la huristica del nodo actual al nodo vecino 
                    float cost = HeuristicEstimateCost(node, neighbourNode);

                    // Coste total hasta el nodo vecino = coste real hasta el nodo actual + coste al nodo vecino estimado por la huristica
                    float totalCost = node.nodeTotalCost + cost;

                    // Coste estimado por la huristica desde el nodo vecino al nodo final
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    // Asigno los valores al nodo vecino ()
                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;

                    // Añado el nodo vecino a la lista de nodos abiertos si todavia no ha sido añadido
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }

            #endregion

            // añado el nodo actual a la lista de nodos cerrados y lo elimino de la de abiertos
            closedList.Add(node);
            openList.Remove(node);
        } // fin while

        // Si acaba el bucle y no ha encontrado la salida lanza un mensaje 
        if (node.position != goal.position)
        {
            Debug.LogError("Salida no encontrada");
            return null;
        }

        // Calcula el camino desde la salida hasta la entrada (va de nodo padre en nodo padre hasta llegar al primer nodo)
        return CalculatePath(node);
    }
}
