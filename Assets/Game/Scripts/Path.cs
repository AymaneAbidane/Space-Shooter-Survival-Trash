using Sirenix.OdinInspector;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathAxisType { Vertical, Horizontal }

    public bool redirectedPath;

    public PathAxisType axisType;

    [SerializeField, ChildGameObjectsOnly] private Transform[] pathPoints;
    [SerializeField, ChildGameObjectsOnly] private Transform[] redirectedPathPoints;

    public Transform[] GetPathPoints()
    {
        if (redirectedPath == true)
        {
            return redirectedPathPoints;

        }
        else
        {
            return pathPoints;
        }
    }
}
