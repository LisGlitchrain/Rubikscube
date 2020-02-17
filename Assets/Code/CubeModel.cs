public enum CellColor
{
    White,
    Red,
    Green,
    Blue,
    Orange,
    Yellow
}

public enum RotationCells
{
    Left,
    Front,
    Right,
    Back,
    Top,
    Buttom,
    MiddleLookingToMe,
    MiddleLookingToSide
}

public class CubesModel
{
    CellColor[,] cells;
    public CellColor[,] Cells => cells;

    public void Init()
    {
        cells = new CellColor[3, 18];
    }

    public void SetCells(CellColor[,] cellsToSet)
    {
        if (cellsToSet.Length != 3 || cellsToSet.GetLength(1) != 18)
            return;
        cells = cellsToSet;
    }

    public void SetSolved()
    {
        for(var colorIndex = 0; colorIndex < 6; colorIndex++)
            for(var i = 0; i < 3; i++)
                for(var j = colorIndex * 3; j < 3; j++)
                {
                    cells[i, j] = (CellColor) colorIndex;
                }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="side"></param>
    /// <param name="rightAnglesCount">Rotation is always counter clockwise from normal.</param>
    public void Rotate(RotationCells rotationCells, int rightAnglesCount)
    {
        switch(side)
        {

        }
    }

    void RotateSide(RotationCells rotationCells, int rightAnglesCount)
    {

    }

    void RotateMiddle()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide">0 is left side, 1 is middle side, 2 is right side.</param>
    void RotateLeftMiddleRightSide(int numberOfSide)
    {
        if (numberOfSide < 0 || numberOfSide > 3) return;
        CellColor tempCell = CellColor.White;
        for (var j = 0; j < 3; j++)
        {
            tempCell = cells[numberOfSide, 0 + j];
            cells[numberOfSide, 0 + j] = cells[numberOfSide, 3 + j];
            cells[numberOfSide, 3 + j] = cells[numberOfSide, 6 + j];
            cells[numberOfSide, 6 + j] = cells[numberOfSide, 9 + j];
            cells[numberOfSide, 9 + j] = tempCell;          
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSide"> 0 is back side, 1 is middle side, 2 is front side.</param>
    void RotateFrontMiddleBackSide(int numberOfSide)
    {
        if (numberOfSide < 0 || numberOfSide > 3) return;
        CellColor tempCell = CellColor.White;
        for (var j = 0; j < 3; j++)
        {
            tempCell = cells[numberOfSide, 0 + j];
            cells[numberOfSide, 0 + j] = cells[numberOfSide, 3 + j];
            cells[numberOfSide, 3 + j] = cells[numberOfSide, 6 + j];
            cells[numberOfSide, 6 + j] = cells[numberOfSide, 9 + j];
            cells[numberOfSide, 9 + j] = tempCell;
        }
    }

    void RotateSnail(RotationCells rotationCells)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startColumn">Column of the top left conrer of 3x3 square to rotate.</param>
    void RotateOnCircleAnySideClockWise(int startColumn)
    {
        var tempCell = cells[0, startColumn];
        cells[0, startColumn] = cells[2, startColumn];
        cells[2, startColumn] = cells[2, startColumn + 2];
        cells[2, startColumn + 2] = cells[0, startColumn + 2];
        cells[0, startColumn + 2] = tempCell;

        tempCell = cells[1, startColumn];
        cells[1, startColumn] = cells[2, startColumn + 1];
        cells[2, startColumn + 1] = cells[1, startColumn + 2];
        cells[1, startColumn + 2] = cells[0, startColumn + 1];
        cells[0, startColumn + 1] = tempCell;
    }

}




