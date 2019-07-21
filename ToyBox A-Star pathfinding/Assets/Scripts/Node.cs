using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private bool mWalkable = true;
    public bool Walkable { get { return mWalkable; } set {mWalkable = value; } }

    private Vector3 mWorldPosition = new Vector3();
    public Vector3 WorldPosition { get { return mWorldPosition; } set { mWorldPosition = value; } }

    public Node(bool aWalkable, Vector3 aWorldPosition)
    {
        mWalkable = aWalkable;
        mWorldPosition = aWorldPosition;
    }
}
