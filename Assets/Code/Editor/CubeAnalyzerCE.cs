using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeAnalyzer))]
public class CubeAnalyzerCE : Editor
{
    CubeAnalyzer targetScript;
    GUIStyle style;

    bool showPathToGetStartCubeFromSolved = false;

    void OnEnable()
    {
        targetScript = (CubeAnalyzer)target;
    }

    public override void OnInspectorGUI()
    {
        style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = Color.red;
        base.OnInspectorGUI();
        //if (GUILayout.Button("Rotate Left")) targetScript.RotateLeftMiddleRight(0);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateLeftMiddleRight(1);
        //if (GUILayout.Button("Rotate Right")) targetScript.RotateLeftMiddleRight(2);

        //if (GUILayout.Button("Rotate Back")) targetScript.RotateBackMiddleFront(0);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateBackMiddleFront(1);
        //if (GUILayout.Button("Rotate Front")) targetScript.RotateBackMiddleFront(2);

        //if (GUILayout.Button("Rotate Top")) targetScript.RotateTopMiddleBottom(0);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateTopMiddleBottom(1);
        //if (GUILayout.Button("Rotate Bottom")) targetScript.RotateTopMiddleBottom(2);

        if (GUILayout.Button("Set current cube cells to start cube")) targetScript.SetCurrentCubeCellsToStartCube();
        if (GUILayout.Button("Set start cube cells to current cube")) targetScript.SetStartCubeCellsToCurrentCube();
        GUILayout.Space(20);
        if (GUILayout.Button("Rotate Left")) targetScript.RotateAny(RotationCells.Left);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParralelSide);
        if (GUILayout.Button("Rotate Right")) targetScript.RotateAny(RotationCells.Right);

        if (GUILayout.Button("Rotate Back")) targetScript.RotateAny(RotationCells.Back);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParallelMe);
        if (GUILayout.Button("Rotate Front")) targetScript.RotateAny(RotationCells.Front);

        if (GUILayout.Button("Rotate Top")) targetScript.RotateAny(RotationCells.Top);
        //if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParrallelFloor);
        if (GUILayout.Button("Rotate Bottom")) targetScript.RotateAny(RotationCells.Bottom);
        GUILayout.Space(20);
        GUILayout.Space(20);
        if (GUILayout.Button("Start")) targetScript.StartSearch();
        if (GUILayout.Button("Stop")) targetScript.StopSearch();
        if (GUILayout.Button("Pause")) targetScript.pause = true;
        if (GUILayout.Button("Resume")) targetScript.pause = false;
        if (GUILayout.Button("Step")) targetScript.oneStep = true;
        GUILayout.Space(20);
        GUILayout.Space(20);
        if (GUILayout.Button("Measure chaos")) targetScript.MeasureChaos(targetScript.cube);
        GUILayout.Label(targetScript.choosenActions.actions.Count.ToString());

        GUILayout.Space(20);
        GUILayout.Space(20);
        showPathToGetStartCubeFromSolved = GUILayout.Toggle(showPathToGetStartCubeFromSolved, "Show how to get start cube from solved");
        if(showPathToGetStartCubeFromSolved)
        {

            GUILayout.Label("Path to get start cube from solved:");
            GUILayout.BeginHorizontal();
            GUILayout.Label("#");
            GUILayout.Label("Roration");
            GUILayout.Label("Right angles");
            GUILayout.Label("Chaos");
            GUILayout.EndHorizontal();
            for (var i = 0; i < targetScript.pathToGetStartCubeFromSolved.actions.Count; i++)
                ShowActionsList(targetScript.pathToGetStartCubeFromSolved.actions[i], i);
        }


        GUILayout.Space(20);
        GUILayout.Space(20);
        if (GUILayout.Button("Step one action back")) targetScript.RevertPreviouslyAppliedAction();
        if (GUILayout.Button("Step one action forward")) targetScript.ApplyPreviouslyRevertedAction();
        GUILayout.Label("Previously applied actions:");
        GUILayout.BeginHorizontal();
        GUILayout.Label("#");
        GUILayout.Label("Roration");
        GUILayout.Label("Right angles");
        GUILayout.Label("Chaos");
        GUILayout.EndHorizontal();
        for (var i = 0; i < targetScript.previouslyAppliedPathes.actions.Count; i++)
            ShowPreviouslyAppliedActionsList(targetScript.previouslyAppliedPathes.actions[i], i);

        GUILayout.Label("Solution path");
        GUILayout.BeginHorizontal();
        GUILayout.Label("#");
        GUILayout.Label("Roration");
        GUILayout.Label("Right angles");
        GUILayout.Label("Chaos");
        GUILayout.EndHorizontal();
        for (var i = 0; i < targetScript.choosenActions.actions.Count; i++)
            ShowActionsList(targetScript.choosenActions.actions[i], i);
    }

    void ShowActionsList(Action action, int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(index.ToString());
        GUILayout.Label(action.Rotation.ToString());
        GUILayout.Label(action.RightAngleCount.ToString());
        GUILayout.Label(action.ChaosMeasureOfAppliedRotation.ToString());
        GUILayout.EndHorizontal();
    }

    void ShowPreviouslyAppliedActionsList(Action action, int index)
    {
        GUILayout.BeginHorizontal();

        if (targetScript.actionsToDeleteFromPreviouslyAppliedList.Contains(action))
        {
            GUILayout.Label(index.ToString(), style);
            GUILayout.Label(action.Rotation.ToString(), style);
            GUILayout.Label(action.RightAngleCount.ToString(), style);
            GUILayout.Label(action.ChaosMeasureOfAppliedRotation.ToString(), style);
        }
        else
        {
            GUILayout.Label(index.ToString());
            GUILayout.Label(action.Rotation.ToString());
            GUILayout.Label(action.RightAngleCount.ToString());
            GUILayout.Label(action.ChaosMeasureOfAppliedRotation.ToString());
        }
        GUILayout.EndHorizontal();

    }
}