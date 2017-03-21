using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

    [SerializeField]
    private GameObject linePrefab;

    Line activeLine;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject lineGO = Instantiate(linePrefab);
            activeLine = lineGO.GetComponent<Line>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        if(activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);
        }
    }

    public void SetLineType(GameObject newLineType)
    {
        // Make sure that the given gameobject can be used to make a line
        if(newLineType.GetComponent<Line>() == null)
        {
            Debug.LogError("LineCreator::SetLineType: " + newLineType.name + " does not have a Line script attached.");
        } else
        {
            linePrefab = newLineType;
        }
    }
}
