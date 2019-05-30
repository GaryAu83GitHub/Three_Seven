using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public ParticleSystem mParticleSystem;
    private TextMesh mTextMesh;
    private MeshRenderer mRenderer;
    private Color mTransparentColor = new Color(0f, 0f, 0f, 0f);

    //private Block mParentBlock;
    //public Block ParentBlock { get { return mParentBlock; } }

    private BlockDeveloping mParentBlockDeveloping;
    public BlockDeveloping ParentBlockDeveloping { get { return mParentBlockDeveloping; } }

    private Dictionary<Vector2Int, Cube> mLinkedCubes = new Dictionary<Vector2Int, Cube>();

    [SerializeField]
    private Vector2Int mGridPosition;
    public Vector2Int GridPos { get { return mGridPosition; } set { mGridPosition = value; } }

    [SerializeField]
    private int mCubeNumber;
    public int Number { get { return mCubeNumber; } }

    private bool mIsScoring = false;
    public bool IsScoring
    {
        get { return mIsScoring; }
        set
        {
            mIsScoring = value;
        }
    }

    private bool mAnimationIsPlaying = false;
    public bool AnimationIsPlaying { get { return mAnimationIsPlaying; } }

    private float mFadingTime = 0f;

    private bool mIsFading = false;

    private void Awake()
    {
        mTextMesh = transform.GetChild(0).GetComponent<TextMesh>();
        mRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(mIsFading)
        {
            mRenderer.material.color = Color.Lerp(mRenderer.material.color, mTransparentColor, mFadingTime);
            mTextMesh.color = Color.Lerp(mTextMesh.color, mTransparentColor, mFadingTime);
            if (mFadingTime < 1)
                mFadingTime += Time.deltaTime / mParticleSystem.main.duration;
            if (mRenderer.material.color.a <= 0f)
                Destroy(this.gameObject);
        }
    }

    public void Init(Block aParentBlock, int aNumber)
    {
        //mParentBlock = aParentBlock;
        SetCubeNumber(aNumber);

        mLinkedCubes.Add(Vector2Int.up, null);
        mLinkedCubes.Add(Vector2Int.left, null);
        mLinkedCubes.Add(Vector2Int.down, null);
        mLinkedCubes.Add(Vector2Int.right, null);
    }

    public void Init(BlockDeveloping aParentBlock, int aNumber)
    {
        mParentBlockDeveloping = aParentBlock;
        SetCubeNumber(aNumber);

        mLinkedCubes.Add(Vector2Int.up, null);
        mLinkedCubes.Add(Vector2Int.left, null);
        mLinkedCubes.Add(Vector2Int.down, null);
        mLinkedCubes.Add(Vector2Int.right, null);
    }

    /// <summary>
    /// Set the cube number both internal and external and set the material colour
    /// The cube colour are pre-set and will change according to the given number
    /// </summary>
    /// <param name="aNumber">The given number to this cube</param>
    public void SetCubeNumber(int aNumber)
    {
        mCubeNumber = aNumber;
        Vector3 color = SupportTools.GetCubeColorVectorOf(mCubeNumber);

        mTextMesh.text = mCubeNumber.ToString();
        mRenderer.material.color = SupportTools.GetCubeColorOf(mCubeNumber);
        ParticleSystem.MainModule _main = mParticleSystem.main;
        _main.startColor = new Color(color.x, color.y, color.z);
    }

    public void RotateCube(int aDir)
    {
        transform.Rotate(Vector3.back, aDir * 90);
        mTextMesh.transform.Rotate(Vector3.back, -aDir * 90);
    }

    public void PlayAnimation()
    {
        if (mIsFading == false)
        {
            mParticleSystem.Play();
            mParentBlockDeveloping.DestroyJoint();
            mAnimationIsPlaying = !mAnimationIsPlaying;
            mIsFading = !mIsFading;
        }
    }

    #region LinkedCube devision
    /// <summary>
    /// Get the cube from the given slot index of a vector
    /// </summary>
    /// <param name="aDir">Slot index</param>
    /// <returns></returns>
    public Cube GetCubeFrom(Vector2Int aDir)
    {
        if (mLinkedCubes[aDir] != null)
            return mLinkedCubes[aDir];
        return null;
    }

    /// <summary>
    /// Put in the cube to the given slot index of a vector.
    /// </summary>
    /// <param name="aCube">Inserted cube</param>
    /// <param name="aDir">Direction of the slot</param>
    public void PutCubeTo(Cube aCube, Vector2Int aDir)
    {
        if (!mLinkedCubes.Values.Contains(aCube) && IsThisSlotVacant(aDir))
            mLinkedCubes[aDir] = aCube;
    }

    /// <summary>
    /// Link the inserting cube to the right slot of the dictionary unless this cube already containing the cube
    /// In which slot the cube will put in is determined of this and inserted cube's grid position.
    /// In the same time the inserted cube will link itself with this cube and put it into the slot of the inverted direction
    /// </summary>
    /// <param name="aCube">Inserted cube</param>
    public void LinkNeighourCube(Cube aCube)
    {
        if (mLinkedCubes.Values.Contains(aCube))
            return;

        if (aCube.GridPos.y > this.GridPos.y)
            PutCubeTo(aCube, Vector2Int.up);
        if (aCube.GridPos.y < this.GridPos.y)
            PutCubeTo(aCube, Vector2Int.down);
        if (aCube.GridPos.x > this.GridPos.x)
            PutCubeTo(aCube, Vector2Int.right);
        if (aCube.GridPos.x < this.GridPos.x)
            PutCubeTo(aCube, Vector2Int.left);

        aCube.LinkNeighourCube(this);
    }

    /// <summary>
    /// Dislink the given cube that don't belongs to the same parent block
    /// </summary>
    /// <param name="aCube">A given cube from another block</param>
    public void DislinkNeighourCube(Cube aCube)
    {
        if (!mLinkedCubes.Values.Contains(aCube))
            return;

        Vector2Int key = mLinkedCubes.FirstOrDefault(x => x.Value == aCube).Key;
        mLinkedCubes[key] = null;
    }

    /// <summary>
    /// Dislink every single link to the other that this cube was linked to
    /// </summary>
    public void TerminateAllLinks()
    {
        foreach(Cube c in mLinkedCubes.Values)
        {
            // disconnect the linked cube from this main cube
            c.DislinkNeighourCube(this);

            // disconnect this cube  from the linked cube
            DislinkNeighourCube(c);
        }
    }

    public void NullifyThisCube(Vector2Int aDir)
    {
        if (mLinkedCubes[aDir] != null)
            mLinkedCubes[aDir] = null;
    }

    public bool IsThisSlotVacant(Vector2Int aDir)
    {
        if (mLinkedCubes[aDir] == null)
            return true;

        return false;
    }
    #endregion


    private Color ColorConverter(float pRed, float pGreen, float pBlue)
    {
        return new Color(pRed / 255f, pGreen / 255f, pBlue / 255f);
    }
}
