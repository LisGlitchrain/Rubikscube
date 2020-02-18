using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeVisualizer : MonoBehaviour
{
    public Image[] imagesToGet;
    public Image[,,] cellImages;
    public CubeModel cube;
    public Color orange;

    // Start is called before the first frame update
    public void Init(CubeModel cubeModel)
    {
        cube = cubeModel;
        cellImages = new Image[6,3,3];
        //orange = new Color(255,120, 0);
        SetImagesToCells();
    }

    private void Update()
    {
        UpdateColors();
    }

    public void UpdateColors()
    {
        for (var side = 0; side < cellImages.GetLength(0); side++)
            for (var row = 0; row < cellImages.GetLength(1); row++)
                for (var column = 0; column < cellImages.GetLength(2); column++)
                {
                    //print($"Coords: [{side}, {row}, {column}]");
                    //print($"cellCode: {cube.Cells[side, row, column].ToString()}");
                    switch (cube.Cells[side, row, column])
                    {
                        case CellColor.Blue:
                            cellImages[side, row, column].color = Color.blue;
                            break;
                        case CellColor.Green:
                            cellImages[side, row, column].color = Color.green;
                            break;
                        case CellColor.Orange:
                            cellImages[side, row, column].color = orange;
                            break;
                        case CellColor.Red:
                            cellImages[side, row, column].color = Color.red;
                            break;
                        case CellColor.White:
                            cellImages[side, row, column].color = Color.white;
                            break;
                        case CellColor.Yellow:
                            cellImages[side, row, column].color = Color.yellow;
                            break;
                    }
                }
    }

    public void SetImagesToCells()
    {
        var i = 0;
        for (var side = 0; side < cellImages.GetLength(0); side++)
            for (var row = 0; row < cellImages.GetLength(1); row++)
                for (var column = 0; column < cellImages.GetLength(2); column++)
                    cellImages[side, row, column] = imagesToGet[i++];
    }
}
