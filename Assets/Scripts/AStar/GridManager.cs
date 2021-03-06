/*
 C�digo sacado del repositorio prorcionado en el enunciado de la pr�ctica
 https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition/tree/master/Chapter07
 */

/*
 GridManager.cs 
Lo que hace esta clase es analizar en el m�todo Awake() el escenario y coge todos los 
obst�culos y los mete en una lista de objetos y calcula la posicion de esos obst�culos 
para ajustarlos a la cuadr�cula, m�todo CalculateObstacles().

En el m�todo CalculateObstacles lo que se hace es inicializar los nodos del tablero 
(cuadr�cula) y despu�s situar los obst�culos. Coge la posicion de cada obst�culo 
(de la lista de obst�culos) y marca el nodo (la casilla de la cudricula) m�s cercano como 
obst�culo (atributo p�blico bObstacle de Node.cs)

En esta clase hay otros m�todos que sirven para pintar la rejilla del tablero y para 
asignar y acceder a los nodos vecinos.
 */

using UnityEngine;
using System.Collections;

//Grid manager class handles all the grid properties
public class GridManager : MonoBehaviour
{
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static GridManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first GridManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (s_Instance == null)
                    Debug.Log("No se ha encontrado ningun objeto tipo GridManager en la escena.");
            }
            return s_Instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        s_Instance = null;
    }

    #region Fields
    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;

    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    public Node[,] nodes { get; set; }
    #endregion

    //Origin of the grid manager
    public Vector3 Origin
    {
        get { return origin; }
    }

    //Initialise the grid manager
    void Awake()
    {
        //Get the list of obstacles objects tagged as "Obstacle"
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    /// <summary>
    /// Calculate which cells in the grids are mark as obstacles
    /// </summary>
    void CalculateObstacles()
    {
        //Initialise the nodes
        nodes = new Node[numOfColumns, numOfRows];

        int index = 0;
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;

                index++;
            }
        }

        //Run through the bObstacle list and set the bObstacle position
        if (obstacleList != null && obstacleList.Length > 0)
        {
            foreach (GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);

                //Also make the node as blocked status
                nodes[row, col].MarkAsObstacle();
            }
        }
    }

    /// <summary>
    /// Returns position of the grid cell in world coordinates
    /// </summary>
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);

        return cellPosition;
    }

    /// <summary>
    /// Returns position of the grid cell in a given index
    /// </summary>
    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;

        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    /// <summary>
    /// Get the grid cell index in the Astar grids with the position given
    /// </summary>
    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }

        pos -= Origin;

        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);

        return (row * numOfColumns + col);
    }

    /// <summary>
    /// Get the row number of the grid cell in a given index
    /// </summary>
    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }

    /// <summary>
    /// Get the column number of the grid cell in a given index
    /// </summary>
    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }

    /// <summary>
    /// Check whether the current position is inside the grid or not
    /// </summary>
    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;

        return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.z <= Origin.z + height && pos.z >= Origin.z);
    }


    /// <summary>
    /// Devuelve los nodos vecinos en las cuatro direcciones
    /// </summary>
    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        // Coge la posicion del nodo y devuelve la casilla m�s cercana 
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        // transforma la casilla en fila y columna
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);

        //Bottom
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Top
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Right
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Left
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

    /// <summary>
    /// Mete el nodo vecino en la lista de adyacentes si no hay ningun obst�culo
    /// </summary>
    /// <param name='row'>
    /// Row.
    /// </param>
    /// <param name='column'>
    /// Column.
    /// </param>
    /// <param name='neighbors'>
    /// Neighbors.
    /// </param>
    void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }

    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos()
    {
        //Draw Grid
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }

        //Grid Start Position
        Gizmos.DrawSphere(transform.position, 0.5f);

        //Draw Obstacle obstruction
        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);

            if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach (GameObject data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellSize);
                }
            }
        }
    }

    /// <summary>
    /// Draw the debug grid lines in the rows and columns order
    /// </summary>
    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);

        // Draw the horizontal grid lines
        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        // Draw the vertial grid lines
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}
