using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 
 El movimiento del minotauro está implementado usando el AStar y cambiando aleatoriamente el nodo destino cada cierto tiempo
 El seguimiento del camino está implementado igual que el automovimiento del personaje
    
 */

public class MinotaurMovement : MonoBehaviour
{
    public float intervalTime;
    public float speed = 6.0f;
    public float gravity = 20.0f;

    float Xlimit;
    float Zlimit;
    float elapsedTime;
    int numOfRows;
    int numOfColumns;

    ArrayList pathArray;
    ArrayList smoothArray;

    private Vector3 moveDirection = Vector3.zero;
    Vector3 oldPosition = Vector3.zero;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        numOfRows = GridManager.instance.numOfRows;
        numOfColumns = GridManager.instance.numOfColumns;

        Xlimit = (numOfColumns * GridManager.instance.gridCellSize) + GridManager.instance.Origin.x;
        Zlimit = (numOfRows * GridManager.instance.gridCellSize) + GridManager.instance.Origin.z;

        pathArray = new ArrayList();
        smoothArray = new ArrayList();
        Vector3 newPos = FindNewPos();
        // calcular ruta con el star
        FindPath(transform.position, newPos);
    }

    void Update()
    {
        // calculo el nodo destino (aleatorio)
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            Vector3 newPos = FindNewPos();
            // calcular ruta con el star
            FindPath(transform.position, newPos);
        }
        // Moverse hasta el nodo final
        if (pathArray.Count > 1)
        {
            moveDirection = subtractVectors3(((Node)smoothArray[1]).position, transform.position).normalized;
            moveDirection *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        if (oldPosition != endPos)
        {
            oldPosition = endPos;
            //Assign StartNode and Goal Node
            Node startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos)));
            Node goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos)));

            pathArray = AStar.FindPath(startNode, goalNode);
            SmoothPath();
        }
    }
    void SmoothPath()
    {
        if (pathArray.Count <= 2)
        {
            smoothArray = pathArray;
            return;
        }

        smoothArray.Clear();
        smoothArray.Add(pathArray[0]);

        int inputIndex = 2;

        while (inputIndex < pathArray.Count - 1)
        {
            //If true, no linecast hit (blocked line)
            if (Physics.Linecast(((Node)smoothArray[smoothArray.Count - 1]).position, ((Node)pathArray[inputIndex]).position))
            {
                smoothArray.Add(pathArray[inputIndex - 1]);
            }
            inputIndex++;
        }

        smoothArray.Add(pathArray[pathArray.Count - 1]);
    }

    Vector3 FindNewPos()
    {
        bool correctPos = false;
        Vector3 newPos = Vector3.zero;
        while (!correctPos)
        {
            float x = Random.Range(0.0f, Xlimit);
            float z = Random.Range(0.0f, Zlimit);

            int indexPosition = GridManager.instance.GetGridIndex(new Vector3(x, 0.0f, z));
            int row = GridManager.instance.GetRow(indexPosition);
            int column = GridManager.instance.GetColumn(indexPosition);


            if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
            {
                Node node = GridManager.instance.nodes[row, column];
                if (!node.bObstacle)
                {
                    correctPos = true;
                    newPos = node.position;
                }
                else
                {   // Recorro las casillas adyacentes en todas las direcciones
                    for (int r = -1; r < 2 && !correctPos; r++)
                    {
                        for (int c = -1; c < 2 && !correctPos; c++)
                        {
                            int newRow = row + r;
                            int newColumn = column + c;
                            if (newRow != -1 && newColumn != -1 && newRow < numOfRows && newColumn < numOfColumns)
                            {
                                node = GridManager.instance.nodes[newRow, newColumn];
                                if (!node.bObstacle)
                                {
                                    correctPos = true;
                                    newPos = node.position;
                                }
                            }
                        }
                    }
                }
            }
        }

        return newPos;
    }
    private Vector3 subtractVectors3(Vector3 v1, Vector3 v2)
    {
        Vector3 result;

        result.x = v1.x - v2.x;
        result.y = 0;
        result.z = v1.z - v2.z;

        return result;
    }
}