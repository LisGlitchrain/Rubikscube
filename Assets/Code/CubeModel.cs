using UnityEngine;

public enum CellColor
{
    White,
    Green,
    Yellow,
    Blue,
    Orange,
    Red
}

public enum RotationCells
{
    Left,
    MiddleParralelSide,
    Right,
    Back,
    MiddleParallelMe,
    Front,
    Top,
    MiddleParrallelFloor,
    Bottom
}


public class CubeModel
{
    CellColor[,,] cells;
    public CellColor[,,] Cells => cells;

    public void Init()
    {
        cells = new CellColor[6,3,3];
    }

    public void SetCells(CellColor[,,] cellsToSet)
    {
        if (cellsToSet.GetLength(0) != 6 || cellsToSet.GetLength(1) != 3 || cellsToSet.GetLength(2) != 3)
            return;
        for (var colorIndex = 0; colorIndex < 6; colorIndex++)
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                {
                    cells[colorIndex, i, j] = cellsToSet[colorIndex, i, j];
                }
    }

    public void SetSolved()
    {
        for(var colorIndex = 0; colorIndex < 6; colorIndex++)
            for(var i = 0; i < 3; i++)
                for(var j = 0; j < 3; j++)
                {
                    cells[colorIndex,i, j] = (CellColor) colorIndex;
                    //Debug.Log($"cell[{colorIndex}, {i}, {j}] color= {cells[colorIndex, i, j].ToString()}");
                }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="side"></param>
    /// <param name="rightAnglesCount">Rotation is always counter clockwise from normal.</param>
    public void Rotate(RotationCells rotationCells, int rightAnglesCount)
    {
        for (var i = 0; i < rightAnglesCount; i++)
        {
            switch (rotationCells)
            {
                case RotationCells.Left:
                    RotateLeftMiddleRightSide(0);
                    RotateOnCircleAnySideSnailClockWise(4);
                    break;
                case RotationCells.Right:
                    RotateLeftMiddleRightSide(2);
                    RotateOnCircleAnySideSnailClockWise(5);
                    break;
                case RotationCells.Back:
                    RotateBackMiddleFront(0);
                    RotateOnCircleAnySideSnailClockWise(3);
                    break;
                case RotationCells.Front:
                    RotateBackMiddleFront(2);
                    RotateOnCircleAnySideSnailClockWise(1);
                    break;
                case RotationCells.Top:
                    RotateTopMiddleBottomSide(0);
                    RotateOnCircleAnySideSnailClockWise(0);
                    break;
                case RotationCells.Bottom:
                    RotateTopMiddleBottomSide(2);
                    RotateOnCircleAnySideSnailClockWise(2);
                    break;

                case RotationCells.MiddleParallelMe:
                    RotateBackMiddleFront(1);
                    break;
                case RotationCells.MiddleParralelSide:
                    RotateLeftMiddleRightSide(1);
                    break;
                case RotationCells.MiddleParrallelFloor:
                    RotateTopMiddleBottomSide(1);
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide">0 is left side, 1 is middle side, 2 is right side.</param>
    public void RotateLeftMiddleRightSide(int numberOfSide)
    {
        if (numberOfSide < 0 || numberOfSide > 3) return;
        CellColor tempCell;
        for(var i = 0; i < 3; i++)
        {
            tempCell = cells[0, i, numberOfSide];
            cells[0, i, numberOfSide] = cells[1, i, numberOfSide];
            cells[1, i, numberOfSide] = cells[2, i, numberOfSide];
            cells[2, i, numberOfSide] = cells[3, i, numberOfSide];
            cells[3, i, numberOfSide] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is back side, 1 is middle side, 2 is front side.</param>
    public void RotateBackMiddleFront(int numberOfSide)
    {
        if (numberOfSide < 0 || numberOfSide > 3) return;
        CellColor tempCell;
        for(var i = 0; i < 3; i ++)
        {
            tempCell = cells[0, numberOfSide, i];
            cells[0, numberOfSide, i] = cells[5, numberOfSide, i];
            cells[5, numberOfSide, i] = cells[2, 2 - numberOfSide, 2 - i];

            cells[2, 2 - numberOfSide, 2 - i] = cells[4, numberOfSide, i];
            cells[4, numberOfSide, i] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is top side, 1 is middle side, 2 is bottom side.</param>
    public void RotateTopMiddleBottomSide(int numberOfSide)
    {
        if (numberOfSide < 0 || numberOfSide > 3) return;
        CellColor tempCell;
        for (var i = 0; i < 3; i++)
        {
            tempCell = cells[1, numberOfSide, i];
            cells[1, numberOfSide, i] = cells[4, i, 2 - numberOfSide];
            cells[4, i, 2 - numberOfSide] = cells[3, 2 - numberOfSide, 2 - i];
            cells[3, 2 - numberOfSide, 2 - i] = cells[5, 2 - i, numberOfSide];
            cells[5, 2 - i, numberOfSide] = tempCell;

            //cells[1, numberOfSide, i] = cells[5, 2 - i, numberOfSide];
            //cells[5, 2 - i, numberOfSide] = cells[3, 2 - numberOfSide, 2 - i];

            //cells[3, 2 - numberOfSide, 2 - i] = cells[4, i, 2 - numberOfSide];
            //cells[4, i, 2 - numberOfSide] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startColumn">Column of the top left conrer of 3x3 square to rotate.</param>
    public void RotateOnCircleAnySideSnailClockWise(int side)
    {
        var tempCell = cells[side,0,0];
        //Debug.Log($"Cells: {cells[side, 0, 0]} {cells[side, 0, 2]} {cells[side, 2, 2]} {cells[side, 2, 0]}");
        cells[side, 0, 0] = cells[side, 0, 2];
        //Debug.Log($"Cells: {cells[side, 0, 0]} {cells[side, 0, 2]} {cells[side, 2, 2]} {cells[side, 2, 0]}");
        cells[side, 0, 2] = cells[side, 2, 2];
        //Debug.Log($"Cells: {cells[side, 0, 0]} {cells[side, 0, 2]} {cells[side, 2, 2]} {cells[side, 2, 0]}");
        cells[side, 2, 2] = cells[side, 2, 0];
        //Debug.Log($"Cells: {cells[side, 0, 0]} {cells[side, 0, 2]} {cells[side, 2, 2]} {cells[side, 2, 0]}");
        cells[side, 2, 0] = tempCell;
        //Debug.Log($"Cells: {cells[side, 0, 0]} {cells[side, 0, 2]} {cells[side, 2, 2]} {cells[side, 2, 0]}");


        tempCell = cells[side, 0, 1];
        cells[side, 0, 1] = cells[side, 1, 2];
        cells[side, 1, 2] = cells[side, 2, 1];
        cells[side, 2, 1] = cells[side, 1, 0];
        cells[side, 1, 0] = tempCell;
    }

}




