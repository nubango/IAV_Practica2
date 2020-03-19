using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodeMinotaur
{
    public Vector3 position;
    public float cost;
}
public class Minotaur : MonoBehaviour
{
    private static Minotaur s_Instance = null;

    public static Minotaur instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(Minotaur)) as Minotaur;
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
