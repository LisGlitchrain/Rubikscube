﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeAnalyzer))]
public class CubeAnalyzerCE : Editor
{
    CubeAnalyzer targetScript;

    void OnEnable()
    {
        targetScript = (CubeAnalyzer)target;
    }

    public override void OnInspectorGUI()
    {
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

        if (GUILayout.Button("Rotate Left")) targetScript.RotateAny(RotationCells.Left);
        if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParralelSide);
        if (GUILayout.Button("Rotate Right")) targetScript.RotateAny(RotationCells.Right);

        if (GUILayout.Button("Rotate Back")) targetScript.RotateAny(RotationCells.Back);
        if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParallelMe);
        if (GUILayout.Button("Rotate Front")) targetScript.RotateAny(RotationCells.Front);

        if (GUILayout.Button("Rotate Top")) targetScript.RotateAny(RotationCells.Top);
        if (GUILayout.Button("Rotate Middle")) targetScript.RotateAny(RotationCells.MiddleParrallelFloor);
        if (GUILayout.Button("Rotate Bottom")) targetScript.RotateAny(RotationCells.Bottom);
        GUILayout.Space(20);
        GUILayout.Space(20);
        if (GUILayout.Button("Start")) targetScript.StartSearch();
        if (GUILayout.Button("Stop")) targetScript.search = false;
        if (GUILayout.Button("Pause")) targetScript.pause = true;
        if (GUILayout.Button("Resume")) targetScript.pause = false;
        if (GUILayout.Button("Step")) targetScript.oneStep = true;
        GUILayout.Space(20);
        GUILayout.Space(20);
        if (GUILayout.Button("Measure chaos")) targetScript.MeasureChaos(targetScript.cube);
        GUILayout.Label(targetScript.actions.Count.ToString());
        foreach (var action in targetScript.actions)
            ShowActionsList(action.Item1, action.Item2);
    }

    void ShowActionsList(RotationCells rotationCells, int rightAngleCount)
    {
        GUILayout.Label(rotationCells.ToString());
        GUILayout.Label(rightAngleCount.ToString());
    }
}