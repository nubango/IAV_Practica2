/*
 C�digo sacado del repositorio prorcionado en el enunciado de la pr�ctica
 https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition/tree/master/Chapter07
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 TestCode.cs
En esta clase lo que se hace es guardar el inicio y el fin del camino y llama al A* para calcularlo.
Tambi�n es aqui d�nde est� el m�todo que pinta el camino (OnDrawGizmos)
     */
public class TestCode : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject nodePanelTextGO;
    public GameObject nodeTextGO;
    private Text nodeText;
    private string baseNodeText;

    public GameObject timePanelTextGO;
    public GameObject timeTextGO;
    private Text timeText;
    private string baseTimeText;

    public CursorsMovement cursorsMovement;

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray;
    private ArrayList smoothArray;

    bool smoothActive = false;

    GameObject objStartCube, objEndCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    public float totalTimeOfCalculation;

    // Use this for initialization
    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        pathArray = new ArrayList();
        smoothArray = new ArrayList();
        FindPath();
        lineRenderer.transform.position = new Vector3(0, 1, 0);
        nodeText = nodeTextGO.GetComponent<Text>();
        baseNodeText = nodeText.text;

        timeText = timeTextGO.GetComponent<Text>();
        baseTimeText = timeText.text;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    elapsedTime += Time.deltaTime;
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        if (elapsedTime >= intervalTime)
    //        {
    //            elapsedTime = 0.0f;
    //            FindPath();
    //        }
    //    }
    //}

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            if (elapsedTime >= intervalTime)
            {
                elapsedTime = 0.0f;
                totalTimeOfCalculation = Time.realtimeSinceStartup * 1000;
                FindPath();
                totalTimeOfCalculation = Time.realtimeSinceStartup * 1000 - totalTimeOfCalculation;
                Debug.Log("TIEMPO: " + totalTimeOfCalculation.ToString("F3"));
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (smoothActive) smoothActive = false;
            else
            {
                smoothActive = true;
                smoothPath();
            }
        }
        updateUI();
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

    void smoothPath()
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

    void updateUI()
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
            timeTextGO.SetActive(false);
            timePanelTextGO.SetActive(false);
            return;
        }
        nodePanelTextGO.SetActive(true);
        nodeTextGO.SetActive(true);
        nodeText.text = baseNodeText + pathArray.Count.ToString();

        timePanelTextGO.SetActive(true);
        timeTextGO.SetActive(true);
        timeText.text = baseTimeText + totalTimeOfCalculation.ToString("F3") + " ms";

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