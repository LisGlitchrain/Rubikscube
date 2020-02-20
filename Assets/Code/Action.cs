
public class Action
{
    public RotationCells Rotation { get; set; }

    public int RightAngleCount { get; set; }

    public int ChaosMeasureOfAppliedRotation { get; set; }

    public Action() { }

    public Action(RotationCells rotationCells, int rightAngleCount)
    {
        Rotation = rotationCells;
        RightAngleCount = rightAngleCount;
    }
    public Action(RotationCells rotationCells, int rightAngleCount, int chaosMeasure)
    {
        Rotation = rotationCells;
        RightAngleCount = rightAngleCount;
        ChaosMeasureOfAppliedRotation = chaosMeasure;
    }

    public Action CloneMe() => new Action(Rotation, RightAngleCount, ChaosMeasureOfAppliedRotation);
}

