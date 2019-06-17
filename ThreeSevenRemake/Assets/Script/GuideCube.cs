using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCube : MonoBehaviour
{
    [SerializeField]
    private Vector2Int mGridPosition;
    public Vector2Int GridPos { get { return mGridPosition; } set { mGridPosition = value; } }

    private MeshRenderer mRenderer;


    private void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
    }

    public void SetCubeColor(Color aMaterialColor)
    {
        mRenderer.material.SetColor("Color_D988B56A", aMaterialColor);
    }
}
