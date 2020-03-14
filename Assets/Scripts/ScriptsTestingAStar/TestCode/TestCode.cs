/*
 Código sacado del repositorio prorcionado en el enunciado de la práctica
 https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition/tree/master/Chapter07
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 TestCode.cs
En esta clase lo que se hace es guardar el inicio y el fin del camino y llama al A* para calcularlo.
También es aqui dónde está el método que pinta el camino (OnDrawGizmos)
     */
public class TestCode : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject nodePanelTextGO;
    public GameObject nodeTextGO;
    private Text nodeText;
    private string baseNodeText;

    public CursorsMovement cursorsMovement;

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray;

    GameObject objStartCube, objEndCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    // Use this for initialization
    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        pathArray = new ArrayList();
        FindPath();
        lineRenderer.transform.position = new Vector3(0, 1, 0);
        nodeText = nodeTextGO.GetComponent<Text>();
        baseNodeText = nodeText.text;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            if (elapsedTime >= intervalTime)
            {
                elapsedTime = 0.0f;
                FindPath();
            }
        }
    }

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        //Assign StartNode and Goal Node
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
    }

    void OnDrawGizmos()
    {
        if (pathArray == null || !Input.GetKey(KeyCode.Space))
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(0, 1000, 0);
            positions[1] = new Vector3(0, 1001, 0);
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
            nodeTextGO.SetActive(false);
            nodePanelTextGO.SetActive(false);
            return;
        }
        nodePanelTextGO.SetActive(true);
        nodeTextGO.SetActive(true);
        nodeText.text = baseNodeText + pathArray.Count.ToString();

        if (pathArray.Count > 0)
        {
            Vector3[] positions = new Vector3[pathArray.Count];
            int index = 0;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    positions[index] = ((Node)pathArray[index]).position;
                    index++;
                }
            };
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
    }
}