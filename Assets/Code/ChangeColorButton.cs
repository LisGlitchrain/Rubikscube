using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeColorButton : Button
{
    public int myColorIndexSide;
    public int myRow;
    public int myColumn;
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FindObjectOfType<CubeAnalyzer>().ChangeCellColor(myColorIndexSide, myRow, myColumn);
    }

}
