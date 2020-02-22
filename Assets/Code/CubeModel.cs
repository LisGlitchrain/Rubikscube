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
                    RotateLeftSide();
                    RotateSnailCounterClockWise(4);
                    break;
                case RotationCells.Right:
                    RotateRightSide();
                    RotateSnailCounterClockWise(5);
                    break;
                case RotationCells.Back:
                    RotateBack();
                    RotateSnailCounterClockWise(3);
                    break;
                case RotationCells.Front:
                    RotateFront();
                    RotateSnailCounterClockWise(1);
                    break;
                case RotationCells.Top:
                    RotateTop();
                    RotateSnailCounterClockWise(0);
                    break;
                case RotationCells.Bottom:
                    RotateBottom();
                    RotateSnailCounterClockWise(2);
                    break;

                case RotationCells.MiddleParallelMe:
                    //RotateBackMiddleFront(1);
                    break;
                case RotationCells.MiddleParralelSide:
                    //RotateLeftMiddleRightSide(1);
                    break;
                case RotationCells.MiddleParrallelFloor:
                    //RotateTopMiddleBottomSide(1);
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide">0 is left side, 1 is middle side,</param>
    public void RotateLeftSide()
    {
        CellColor tempCell;
        for(var i = 0; i < 3; i++)
        {
            tempCell = cells[0, i, 0];
            cells[0, i, 0] = cells[1, i, 0];
            cells[1, i, 0] = cells[2, i, 0];
            cells[2, i, 0] = cells[3, i, 0];
            cells[3, i, 0] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide">0 is left side, 1 is middle side, 2 is right side.</param>
    public void RotateRightSide()
    {
        CellColor tempCell;
        for (var i = 0; i < 3; i++)
        {
            tempCell = cells[3, i, 2];
            cells[3, i, 2] = cells[2, i, 2];
            cells[2, i, 2] = cells[1, i, 2];
            cells[1, i, 2] = cells[0, i, 2];
            cells[0, i, 2] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is back side, 1 is middle side, 2 is front side.</param>
    public void RotateBack()
    {
        CellColor tempCell;
        for(var i = 0; i < 3; i ++)
        {
            tempCell = cells[0, 0, i];
            cells[0, 0, i] = cells[4, 0, i];
            cells[4, 0, i] = cells[2, 2, 2 - i];

            cells[2, 2 - 0, 2 - i] = cells[5, 0, i];
            cells[5, 0, i] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is back side, 1 is middle side, 2 is front side.</param>
    public void RotateFront()
    {
        for (var i = 0; i < 3; i++)
        {
            var tempCell = cells[0, 2, i];
            cells[0, 2, i] = cells[5, 2, i];
            cells[5, 2, i] = cells[2, 2 - 2, 2 - i];

            cells[2, 2 - 2, 2 - i] = cells[4, 2, i];
            cells[4, 2, i] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is top side, 1 is middle side, 2 is bottom side.</param>
    public void RotateTop()
    {
        CellColor tempCell;
        for (var i = 0; i < 3; i++)
        {
            tempCell = cells[1, 0, i];
            cells[1, 0, i] = cells[4, i, 2];
            cells[4, i, 2] = cells[3, 2, 2 - i];
            cells[3, 2, 2 - i] = cells[5, 2 - i, 0];
            cells[5, 2 - i, 0] = tempCell;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is top side, 1 is middle side, 2 is bottom side.</param>
    public void RotateBottom()
    {
        CellColor tempCell;
        for (var i = 0; i < 3; i++)
        {
            tempCell = cells[5, 2 - i, 2];
            cells[5, 2 - i, 2] = cells[3, 2 - 2, 2 - i];
            cells[3, 2 - 2, 2 - i] = cells[4, i, 2 - 2];
            cells[4, i, 2 - 2] = cells[1, 2, i];
            cells[1, 2, i] = tempCell;

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="side">Number of side to rotate.</param>
    public void RotateSnailCounterClockWise(int side)
    {
        var tempCell = cells[side,0,0];
        cells[side, 0, 0] = cells[side, 0, 2];
        cells[side, 0, 2] = cells[side, 2, 2];
        cells[side, 2, 2] = cells[side, 2, 0];
        cells[side, 2, 0] = tempCell;

        tempCell = cells[side, 0, 1];
        cells[side, 0, 1] = cells[side, 1, 2];
        cells[side, 1, 2] = cells[side, 2, 1];
        cells[side, 2, 1] = cells[side, 1, 0];
        cells[side, 1, 0] = tempCell;
    }

}




