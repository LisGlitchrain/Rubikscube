using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Path
{
    public List<Action> actions;

    public Path() => actions = new List<Action>();

    public Path(Action action)
    {
        actions = new List<Action>();
        actions.Add(action);
    }
    public Path(Path path) => actions = path.CloneActions();
    public Path(List<Action> actionsToSet) => actions = actions = actionsToSet;

    public List<Action> CloneActions()
    {
        var list = new List<Action>();
        foreach (var action in actions)
            list.Add(action.CloneMe());
        return list;
    }

    public List<Action> CloneActions(int maxIndexIncluded)
    {
        var list = new List<Action>();
        for(var i = 0; i <= maxIndexIncluded; i++)
        {
            list.Add(actions[i].CloneMe());
            //Debug.Log($"Path cloning index: {i}/ {maxIndexIncluded}");
        }
        return list;
    }

    public void AddActionsToList(List<Action> actionsToAdd)
    {
        foreach (var action in actionsToAdd)
            actions.Add(action);
    }

    public void AddActionsToList(Path pathToAdd)
    {
        foreach (var action in pathToAdd.actions)
            actions.Add(action);
    }

}

