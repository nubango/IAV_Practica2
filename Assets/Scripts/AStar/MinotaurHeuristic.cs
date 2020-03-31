using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase sirve para calcular la heurística en el A*.

getActionArea:
    sirve para calcular el peso de cada una de las casillas del rango del minotauro 
    que afectan en el cálculo del coste de una casilla en la huerística.
    Cuyando más alejada está la casilla menos afectará al calculo del camino.
MÉTODOS Y ATRIBUTOS:
- range:
    rango de accion del minotauro (unidades en casillas 3 = 3 casillas).
- height: 
    es un atributo para calibrar cuanto peso tiene el área de acción de minotauro.
- ActivateVertexArea:
    sirve para activar visualmente el área de acción del minotauro.
*/

public struct NodeMinotaur
{
    public Vector3 position;
    public float cost;
}
public class MinotaurHeuristic : MonoBehaviour
{
    private static MinotaurHeuristic s_Instance = null;

    public static MinotaurHeuristic instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(MinotaurHeuristic)) as MinotaurHeuristic;
                if (s_Instance == null)
                    Debug.Log("No se ha encontrado ningun objeto tipo Minotaur en la escena.");
            }
            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }

    public bool ActivateVertexArea;
    public int range = 3; // Rango a partir del cual el minotauro afecta al A* para calcular el camino más corto
    public float height = 2;
    public GameObject vertexPrefab;
    private GridManager gridManager;
    private Vector3[] directions = { new Vector3(0,0,1), new Vector3(1, 0, 0),
                                     new Vector3(0, 0, -1), new Vector3(-1, 0, 0),
                                     new Vector3(1,0,1), new Vector3(-1, 0, -1),
                                     new Vector3(1,0,-1), new Vector3(-1, 0, 1)
    };
    private Vector3 oldPosition;
    private ArrayList actionAreaVertex;
    void Start()
    {
        gridManager = GridManager.instance;
        oldPosition = transform.position;
        actionAreaVertex = new ArrayList();
    }

    public ArrayList getActionArea()
    {
        Vector3 cellPosition = gridManager.GetGridCellCenter(gridManager.GetGridIndex(transform.position));

        ArrayList actionArea = new ArrayList();

        if (oldPosition != cellPosition)
        {
            for (int i = 0; i < actionAreaVertex.Count; i++)
            {
                Destroy((GameObject)actionAreaVertex[i]);
            }
            actionAreaVertex.Clear();
        }

        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                int realRangeX = (int)(x * gridManager.gridCellSize);
                int realRangeY = (int)(y * gridManager.gridCellSize);

                Vector3 offSetRange = new Vector3(realRangeX, 0, realRangeY);

                Vector3 posFinal = cellPosition + offSetRange;

                if (gridManager.IsInBounds(posFinal))
                {
                    NodeMinotaur node = new NodeMinotaur();
                    node.position = posFinal;

                    if (oldPosition != cellPosition)
                    {
                        GameObject obj = Instantiate(vertexPrefab, posFinal, new Quaternion());
                        obj.SetActive(ActivateVertexArea);
                        actionAreaVertex.Add(obj);
                    }
                    int index = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                    float minotaurCost = 1 / (Mathf.Sqrt(index * index) + 1);

                    node.cost = minotaurCost * height;
                    actionArea.Add(node);
                }
            }
        }

        if (oldPosition != cellPosition)
            oldPosition = cellPosition;

        return actionArea;
    }
}
