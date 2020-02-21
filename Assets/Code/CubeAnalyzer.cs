using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AmountOfActions
{
    Minimal = 6,
    MinimalWithClockwise = 12,
    Minimal180and270 = 18
}
public class CubeAnalyzer : MonoBehaviour
{
    public CubeVisualizer cubeVisualizer;
    public CubeModel cube;
    public CubeModel startCube;
    //CubeModel[] solvedCubes = new CubeModel[24];
    CubeModel solvedCube;
    public int chaosMeasure;
    public int currentChaosMeasure;
    public int depthSearchLimit = 5;
    public AmountOfActions actionsToUse = AmountOfActions.Minimal180and270;
    public int currentCountOfActions;
    public Path choosenActions = new Path();
    public Path previouslyAppliedPathes = new Path();
    public Path pathToGetStartCubeFromSolved = new Path();
    public bool search;
    public bool fullyAutomaticSearch;
    public bool pause;
    public bool oneStep;
    public int currentDepth;
    public Text etaText;
    int indexOfPreviouslyAppliedAction = -1;
    public List<Action> actionsToDeleteFromPreviouslyAppliedList = new List<Action>();
    System.Random rand = new System.Random();
    bool coroutineEnded = true;
    public delegate void StartSearchAction();
    StartSearchAction startSearchAction;
    private void Start()
    {
        currentCountOfActions = (int)actionsToUse;
        chaosMeasure = 54;
        cube = new CubeModel();
        cube.Init();
        cube.SetSolved();

        startCube = new CubeModel();
        startCube.Init();
        startCube.SetCells(cube.Cells);

        solvedCube = new CubeModel();
        solvedCube.Init();
        solvedCube.SetSolved();
        cubeVisualizer.Init(cube, startCube);

        for (var colorSide = 0; colorSide < 6; colorSide++)
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; column++)
                {
                    var changeColorButton = cubeVisualizer.cellImagesCurrentCube[colorSide, row, column].gameObject.GetComponent<ChangeColorButton>();
                    changeColorButton.myColorIndexSide = colorSide;
                    changeColorButton.myRow = row;
                    changeColorButton.myColumn = column;
                }
                    
    }

    public void ChangeCellColor(int sideColorIndex, int row, int column)
    {
        if (cube.Cells[sideColorIndex, row, column] == CellColor.Red) cube.Cells[sideColorIndex, row, column] = CellColor.White;
        else
        {
            var nextColor = (int)cube.Cells[sideColorIndex, row, column];
            nextColor++;
            cube.Cells[sideColorIndex, row, column] = (CellColor) nextColor;
        }
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

    public void Update()
    {
        startSearchAction?.Invoke();
    }

    public void SetCurrentCubeCellsToStartCube()
    {
        startCube.SetCells(cube.Cells);
        pathToGetStartCubeFromSolved = new Path(previouslyAppliedPathes);
        previouslyAppliedPathes.actions.Clear();
        actionsToDeleteFromPreviouslyAppliedList.Clear();
    }

    public void SetStartCubeCellsToCurrentCube()
    {
        cube.SetCells(startCube.Cells);
        previouslyAppliedPathes.actions.Clear();
        actionsToDeleteFromPreviouslyAppliedList.Clear();
    }

    public void RotateAny(RotationCells rotationCells, int rightAngleCount = 1)
    {
        cube.Rotate(rotationCells, rightAngleCount);
        var action = new Action(rotationCells, rightAngleCount, MeasureChaos(cube));
        previouslyAppliedPathes.actions.Add(action);
        chaosMeasure = MeasureChaos(cube);
    }

    public int MeasureChaos(CubeModel cube)
    {
        var chaosMeasure = 0;
        for (var colorIndex = 0; colorIndex < 6; colorIndex++)
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    if (cube.Cells[colorIndex, i, j] != solvedCube.Cells[colorIndex, i, j]) chaosMeasure++;
        return chaosMeasure;
    }

    public void StartSearch()
    {
        search = true;
        if (MeasureChaos(cube) == 0)
        {
            startSearchAction = null;
            return;
        }
        choosenActions.actions.Clear();
        if (coroutineEnded) StartCoroutine(FindPathToDecreaseChaose(cube));
        if (fullyAutomaticSearch) startSearchAction = StartSearch;
    }


    //Now it trying to find solution and accept only full solution.
    //Maybe it's good idea to trying to decrease chaose measure. And start from point with less chaos  


    IEnumerator FindPathToDecreaseChaose(CubeModel cube)
    {
        coroutineEnded = false;
        foreach (var actionToDelete in actionsToDeleteFromPreviouslyAppliedList)
            previouslyAppliedPathes.actions.Remove(actionToDelete);
        indexOfPreviouslyAppliedAction = -100;
        var start = DateTime.Now;
        currentDepth = 0;
        var success = false;
        var actionPaths = new List<Path>();
        currentCountOfActions = (int) actionsToUse;
        FillUpPathsListWithFirst9Actions(actionPaths);
        var startChaosMeasure = MeasureChaos(cube);
        var currentPathCount = 0;
        var totalPathCountsForIteratingDeepSearch = 0;
        for (var i = 1; i <= depthSearchLimit; i++)
            totalPathCountsForIteratingDeepSearch += (int)Mathf.Pow(currentCountOfActions, i);
        print($"Total path count: {totalPathCountsForIteratingDeepSearch}");
        while (search && MeasureChaos(cube) > 0 && currentDepth < depthSearchLimit)
        {
            if(!pause)
            {
                for(var  i = 0; i < Mathf.Pow(currentCountOfActions, currentDepth + 1); i++)
                {
                    if (!success)
                    {
                        //print($"<color=blue>PathIndex {i} / {Mathf.Pow(27, currentDepth + 1)} </color>");
                        GoThroughPath(cube, actionPaths[i]);
                        //yield return new WaitForEndOfFrame();
                        //yield return new WaitForSeconds(1);
                        if (actionPaths[i].actions[actionPaths[i].actions.Count - 1].ChaosMeasureOfAppliedRotation == 0)
                        {
                            print("<color=green>Success! Added!</color>");
                            success = true;
                            choosenActions = actionPaths[i];
                            break;
                        }
                        //print($"Chaos: {actionPaths[i].actions[actionPaths[i].actions.Count - 1].ChaosMeasureOfAppliedRotation}");
                        for (var j = 0; j < currentCountOfActions; j++)
                        {
                            //print($"Path {i} Trying action: {j} ( {GetActionByIndex(j).Rotation}, {GetActionByIndex(j).RightAngleCount} ) pathCount: {actionPaths.Count}");
                            var currentAction = GetActionByIndex(j);
                            cube.Rotate(currentAction.Rotation, currentAction.RightAngleCount);
                            currentAction.ChaosMeasureOfAppliedRotation = MeasureChaos(cube);
                            if(currentAction.ChaosMeasureOfAppliedRotation == 0)
                            {
                                print("<color=green>TryingAction Success! Added!</color>");
                                success = true;
                                actionPaths[i].actions.Add(currentAction);
                                choosenActions = actionPaths[i];
                                break;
                            }
                            else
                            {
                                //yield return new WaitForEndOfFrame();
                                RevertAction(cube, currentAction);
                            }
                            //print($"End of Action {j}. Chaos {MeasureChaos(cube)}");
                            //yield return new WaitForSeconds(1);
                            //yield return new WaitForEndOfFrame();
                        }
                        currentPathCount++;
                        var timePerPath = (DateTime.Now - start).TotalSeconds / currentPathCount;
                        etaText.text = TimeSpan.FromSeconds(timePerPath * (totalPathCountsForIteratingDeepSearch - currentPathCount)).ToString();
                        yield return new WaitForEndOfFrame();
                        if(!success) RevertPath(cube, actionPaths[i]);
                    }
                    else
                        break;
                }
                currentDepth++;
                if(!success && currentDepth < depthSearchLimit) ExtendPathes(actionPaths);

                oneStep = false;
            }
            yield return new WaitForEndOfFrame();
        }
        print($"Elapsed: {(DateTime.Now - start).TotalSeconds}");
        if (success)
        {
            print("<color=green> SOLUTION IS FOUNDED!</color>");
            foreach (var action in choosenActions.actions)
                previouslyAppliedPathes.actions.Add(action);
        }
        else
        {
            print("<color=red> SOLUTION WAS NOT FOUND!</color>");
            var bestPath = FindAndAddPathWithTheLowestChaos(actionPaths);
            if (bestPath.actions.Count > 0)
            {
                print("<color=yellow> Best path was found!</color>");
                GoThroughPath(cube, bestPath);
                previouslyAppliedPathes.AddActionsToList(bestPath);
                chaosMeasure = bestPath.actions[bestPath.actions.Count - 1].ChaosMeasureOfAppliedRotation;
            }
            else print("<color=red> BEST PATH WAS NOT FOUND!</color>");
        }
        indexOfPreviouslyAppliedAction = previouslyAppliedPathes.actions.Count - 1;
        coroutineEnded = true;
    }

    //Need to filtrate 360degrees actions!
    Path FindAndAddPathWithTheLowestChaos(List<Path> pathes)
    {
        if (pathes.Count < 1) return new Path();
        if (pathes[0].actions.Count < 1) return new Path();
        var lowestChaos = 54;
        var currentLowestPath = 0;
        var numberOfCurrentLowestAction = 0;
        for (var i = 0; i < pathes.Count; i++)
        {
            var pathLowestChaos = 54;
            for (var j = 0; j < pathes[i].actions.Count; j++)
            {             
                if (pathes[i].actions[j].ChaosMeasureOfAppliedRotation < pathLowestChaos)
                    pathLowestChaos = pathes[i].actions[j].ChaosMeasureOfAppliedRotation;
                if (pathes[i].actions[j].ChaosMeasureOfAppliedRotation < lowestChaos)
                {
                    currentLowestPath = i;
                    numberOfCurrentLowestAction = j;
                    lowestChaos = pathes[i].actions[j].ChaosMeasureOfAppliedRotation;
                }
            }
            //print($"Analyzed path: {i}. Lowest chaos:  {pathLowestChaos}");
        }
        //print($"Best path index: {currentLowestPath}");
        if (lowestChaos >= chaosMeasure && !fullyAutomaticSearch) return new Path();
        return new Path(pathes[currentLowestPath].CloneActions(numberOfCurrentLowestAction));
    }

    //need to exclude 360 actions!
    void ExtendPathes(List<Path> pathes)
    {
        var startPathCount = pathes.Count;
        for(var i = 0; i < startPathCount; i++)
        {
            for(var j = 0; j < currentCountOfActions; j++)
            {
                if (j == 0)
                {
                    var action = GetActionByIndex(j);
                    cube.Rotate(action.Rotation, action.RightAngleCount);
                    action.ChaosMeasureOfAppliedRotation = MeasureChaos(cube);
                    RevertAction(cube, action);
                    pathes[i].actions.Add(action);
                }
                else
                {
                    var newPath = new Path(pathes[i]);
                    var action = GetActionByIndex(j);
                    cube.Rotate(action.Rotation, action.RightAngleCount);
                    action.ChaosMeasureOfAppliedRotation = MeasureChaos(cube);
                    RevertAction(cube, action);
                    newPath.actions.Add(action);
                    pathes.Add(newPath);
                }
            }
        }
    }

    void FillUpPathsListWithFirst9Actions(List<Path> listToFil)
    {
        for (var i = 0; i < currentCountOfActions; i++)
        {
            var firstAction = GetActionByIndex(i);
            var firstActionList = new Path(firstAction);
            listToFil.Add(firstActionList);
        }
    }

    void GoThroughPath(CubeModel cube, Path path)
    {
        foreach(var action in path.actions)
            cube.Rotate(action.Rotation, action.RightAngleCount);
        path.actions[path.actions.Count - 1].ChaosMeasureOfAppliedRotation = MeasureChaos(cube);
        //print("<color=red> THROUTH PATH</color>");
        //for(var i = 0; i < path.actions.Count; i++)
        //    print($"action[{i}] chaos = {path.actions[i].ChaosMeasureOfAppliedRotation}");
    }

    void RevertPath(CubeModel cube, Path path)
    {
        for (var i = path.actions.Count - 1; i >= 0; i--)
            RevertAction(cube, path.actions[i]);
    }

    List<(RotationCells, int)> ClonePath(List<(RotationCells, int)> pathToClone)
    {
        var newPath = new List<(RotationCells, int)>();
        foreach (var action in pathToClone)
            newPath.Add(action);
        return newPath;
    }

    bool TryNextAction(CubeModel cube, int actionIndex, int stepChaosMeasure, int currentPathIndex)
    {
        var currentAction = GetActionByIndex(actionIndex);
        
        cube.Rotate(currentAction.Rotation, currentAction.RightAngleCount);
        var actionChaosMeasure = MeasureChaos(cube);
        //print($"CA: {currentAction.Rotation.ToString()} {currentAction.RightAngleCount} CHAOSTRUE? : {MeasureChaos(cube)}");
        RevertAction(cube, currentAction);
        return actionChaosMeasure == 0;
        //return (actionCaosMeasure < stepChaosMeasure);      
    }

    Action GetActionByIndex(int actionIndex)
    {
        switch(actionsToUse)
        {
            case AmountOfActions.Minimal:
                return GetActionFromReducedTo6(actionIndex);
            case AmountOfActions.MinimalWithClockwise:
                return GetActionFromRedeucedTo12(actionIndex);
            case AmountOfActions.Minimal180and270:
                return GetfActionFromReducedTo18(actionIndex);
        }
        return new Action(RotationCells.Left, 0);
    }

    Action GetfActionFromReducedTo18(int actionIndex)
    {
        if (actionIndex == 0) return new Action(RotationCells.Left, 1);
        else if (actionIndex == 1) return new Action(RotationCells.Left, 2);
        else if (actionIndex == 2) return new Action(RotationCells.Left, 3);
        //else if (actionIndex == 3) return new Action(RotationCells.MiddleParralelSide, 1);
        //else if (actionIndex == 4) return new Action(RotationCells.MiddleParralelSide, 2);
        //else if (actionIndex == 5) return new Action(RotationCells.MiddleParralelSide, 3);
        else if (actionIndex == 3) return new Action(RotationCells.Right, 1);
        else if (actionIndex == 4) return new Action(RotationCells.Right, 2);
        else if (actionIndex == 5) return new Action(RotationCells.Right, 3);
        else if (actionIndex == 6) return new Action(RotationCells.Back, 1);
        else if (actionIndex == 7) return new Action(RotationCells.Back, 2);
        else if (actionIndex == 8) return new Action(RotationCells.Back, 3);
        //else if (actionIndex == 12) return new Action(RotationCells.MiddleParallelMe, 1);
        //else if (actionIndex == 13) return new Action(RotationCells.MiddleParallelMe, 2);
        //else if (actionIndex == 14) return new Action(RotationCells.MiddleParallelMe, 3);
        else if (actionIndex == 9) return new Action(RotationCells.Front, 1);
        else if (actionIndex == 10) return new Action(RotationCells.Front, 2);
        else if (actionIndex == 11) return new Action(RotationCells.Front, 3);
        else if (actionIndex == 12) return new Action(RotationCells.Top, 1);
        else if (actionIndex == 13) return new Action(RotationCells.Top, 2);
        else if (actionIndex == 14) return new Action(RotationCells.Top, 3);
        //else if (actionIndex == 21) return new Action(RotationCells.MiddleParrallelFloor, 1);
        //else if (actionIndex == 22) return new Action(RotationCells.MiddleParrallelFloor, 2);
        //else if (actionIndex == 23) return new Action(RotationCells.MiddleParrallelFloor, 3);
        else if (actionIndex == 15) return new Action(RotationCells.Bottom, 1);
        else if (actionIndex == 16) return new Action(RotationCells.Bottom, 2);
        else if (actionIndex == 17) return new Action(RotationCells.Bottom, 3);
        return new Action(RotationCells.Left, 0);
    }

    Action GetActionFromReducedTo6(int actionIndex)
    {
        if (actionIndex == 0) return new Action(RotationCells.Left, 1);
        else if (actionIndex == 1) return new Action(RotationCells.Right, 1);
        else if (actionIndex == 2) return new Action(RotationCells.Back, 1);
        else if (actionIndex == 3) return new Action(RotationCells.Front, 1);
        else if (actionIndex == 4) return new Action(RotationCells.Top, 1);
        else if (actionIndex == 5) return new Action(RotationCells.Bottom, 1);
        return new Action(RotationCells.Left, 0);
    }

    Action GetActionFromRedeucedTo12(int actionIndex)
    {
        if (actionIndex == 0) return new Action(RotationCells.Left, 1);
        else if (actionIndex == 1) return new Action(RotationCells.Left, 3);
        else if (actionIndex == 2) return new Action(RotationCells.Right, 1);
        else if (actionIndex == 3) return new Action(RotationCells.Right, 3);
        else if (actionIndex == 4) return new Action(RotationCells.Back, 1);
        else if (actionIndex == 5) return new Action(RotationCells.Back, 3);
        else if (actionIndex == 6) return new Action(RotationCells.Front, 1);
        else if (actionIndex == 7) return new Action(RotationCells.Front, 3);
        else if (actionIndex == 8) return new Action(RotationCells.Top, 1);
        else if (actionIndex == 9) return new Action(RotationCells.Top, 3);
        else if (actionIndex == 10) return new Action(RotationCells.Bottom, 1);
        else if (actionIndex == 11) return new Action(RotationCells.Bottom, 3);
        return new Action(RotationCells.Left, 0);
    }

    (RotationCells, int) GetRandomAction()
    {
        return ((RotationCells)rand.Next(0, 9), rand.Next(1, 4));
    }

    public void RevertPreviouslyAppliedAction()
    {
        if (indexOfPreviouslyAppliedAction < 0) return;
        if (indexOfPreviouslyAppliedAction > previouslyAppliedPathes.actions.Count) return;
        if (indexOfPreviouslyAppliedAction == previouslyAppliedPathes.actions.Count) indexOfPreviouslyAppliedAction = previouslyAppliedPathes.actions.Count - 1;
        RevertAction(cube, previouslyAppliedPathes.actions[indexOfPreviouslyAppliedAction]);
        actionsToDeleteFromPreviouslyAppliedList.Add(previouslyAppliedPathes.actions[indexOfPreviouslyAppliedAction]);
        indexOfPreviouslyAppliedAction--;
        chaosMeasure = MeasureChaos(cube);
    }

    public void ApplyPreviouslyRevertedAction()
    {
        if (indexOfPreviouslyAppliedAction < -1) return;
        if (indexOfPreviouslyAppliedAction > previouslyAppliedPathes.actions.Count - 1) return;
        if (indexOfPreviouslyAppliedAction == -1) indexOfPreviouslyAppliedAction = 0;
        cube.Rotate(previouslyAppliedPathes.actions[indexOfPreviouslyAppliedAction].Rotation, previouslyAppliedPathes.actions[indexOfPreviouslyAppliedAction].RightAngleCount);
        actionsToDeleteFromPreviouslyAppliedList.Remove(previouslyAppliedPathes.actions[indexOfPreviouslyAppliedAction]);
        indexOfPreviouslyAppliedAction++;
        chaosMeasure = MeasureChaos(cube);
    }

    void RevertAction(CubeModel cube, Action currentAction)
    {
        cube.Rotate(currentAction.Rotation, 4 - currentAction.RightAngleCount);
    }
}
