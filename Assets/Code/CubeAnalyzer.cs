using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnalyzer : MonoBehaviour
{
    public CubeVisualizer cubeVisualizer;
    public CubeModel cube;
    //CubeModel[] solvedCubes = new CubeModel[24];
    CubeModel solvedCube;
    public int chaosMeasure;
    public int depthSearchLimit = 5;
    public List<(RotationCells, int)> actions = new List<(RotationCells, int)>();
    public bool search;
    public bool pause;
    public bool oneStep;
    System.Random rand = new System.Random();

    private void Start()
    {
        cube = new CubeModel();
        cube.Init();
        cube.SetSolved();

        solvedCube = new CubeModel();
        solvedCube.Init();
        solvedCube.SetSolved();
        cubeVisualizer.Init(cube);
    }
    #region For tests
    //public void RotateLeftMiddleRight(int numberOfSide)
    //{
    //    cube.RotateLeftMiddleRightSide(numberOfSide);
    //}

    //public void RotateBackMiddleFront(int numberOfSide)
    //{
    //    cube.RotateBackMiddleFront(numberOfSide);
    //}

    //public void RotateTopMiddleBottom(int numberOfSide)
    //{
    //    cube.RotateTopMiddleBottomSide(numberOfSide);
    //}
    #endregion

    public void RotateAny(RotationCells rotationCells, int rightAngleCount = 1)
    {
        cube.Rotate(rotationCells, rightAngleCount);
    }

    public int MeasureChaos(CubeModel cube)
    {
        chaosMeasure = 0;
        for (var colorIndex = 0; colorIndex < 6; colorIndex++)
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    if (cube.Cells[colorIndex, i, j] != solvedCube.Cells[colorIndex, i, j]) chaosMeasure++;
        return chaosMeasure;
    }

    public void StartSearch()
    {
        search = true;
        actions.Clear();
        StartCoroutine(FindPathToDecreaseChaose(cube));
    }

    IEnumerator FindPathToDecreaseChaose(CubeModel cube)
    {
        var currentAction = (actionName: RotationCells.Left, rightAngleCount: 0);
        var currentChaosMeasure = MeasureChaos(cube);
        var depth = 0;
        while(search && MeasureChaos(cube) > 0 && depth < depthSearchLimit)
        {
            if(!pause || oneStep)
            {
                var successAction = false;
                for (var i = 0; i < 27; i++)
                {
                    if (TryNextAction(cube, ref currentAction, currentChaosMeasure))
                    {
                        successAction = true;
                        break;
                    }
                }
                if (successAction)
                {
                    print("Success!");
                    actions.Add(currentAction);
                }
                else
                {
                    print("Random action");
                    currentAction = GetRandomAction();
                    actions.Add(currentAction);
                }
                cube.Rotate(currentAction.actionName, currentAction.rightAngleCount);
                depth++;
                
                oneStep = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    bool TryNextAction(CubeModel cube, ref (RotationCells, int) currentAction, int stepChaosMeasure)
    {
        currentAction = GetNextAction(currentAction);
        print($"CA: {currentAction.Item1.ToString()} {currentAction.Item2}");
        cube.Rotate(currentAction.Item1, currentAction.Item2);
        var actionCaosMeasure = MeasureChaos(cube);
        RevertCurrentAction(cube, currentAction);
        return (actionCaosMeasure < stepChaosMeasure);      
    }

    (RotationCells, int) GetNextAction((RotationCells, int) currentAction)
    {
        if (currentAction.Item2 < 3) return (currentAction.Item1, ++currentAction.Item2);
        else return (++currentAction.Item1, 1);
    }

    (RotationCells, int) GetRandomAction()
    {
        return ((RotationCells)rand.Next(0, 9), rand.Next(1, 4));
    }

    void RevertCurrentAction(CubeModel cube, (RotationCells, int) currentAction)
    {
        cube.Rotate(currentAction.Item1, 4 - currentAction.Item2);
    }
}
