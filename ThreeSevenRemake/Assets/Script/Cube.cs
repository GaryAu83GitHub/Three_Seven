using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public ParticleSystem mParticleSystem;
    //public Material mMaterial;

    private TextMesh mTextMesh;
    private MeshRenderer mRenderer;
    private Color mTransparentColor = new Color(0f, 0f, 0f, 0f);

    private Block mParentBlockDeveloping;
    public Block ParentBlockDeveloping { get { return mParentBlockDeveloping; } }

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
            //mRenderer.material.color = Color.Lerp(mRenderer.material.color, mTransparentColor, mFadingTime);
            mTextMesh.color = Color.Lerp(mTextMesh.color, mTransparentColor, mFadingTime);
            if (mFadingTime < 1)
            {
                mFadingTime += Time.deltaTime / 1f;//mParticleSystem.main.duration;
                mRenderer.material.SetFloat("Vector1_8984A549", mFadingTime);
            }
            //if (mRenderer.material.color.a <= 0f)
            if (mRenderer.material.GetFloat("Vector1_8984A549") > 1)
                Destroy(this.gameObject);
        }
    }

    public void Init(OldBlock aParentBlock, int aNumber)
    {
        //mParentBlock = aParentBlock;
        SetCubeNumber(aNumber);

        mLinkedCubes.Add(Vector2Int.up, null);
        mLinkedCubes.Add(Vector2Int.left, null);
        mLinkedCubes.Add(Vector2Int.down, null);
        mLinkedCubes.Add(Vector2Int.right, null);
    }

    public void Init(Block aParentBlock, int aNumber)
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
        Color color = SupportTools.GetCubeHexColorOf(mCubeNumber);

        mTextMesh.text = mCubeNumber.ToString();
        //mRenderer.material.color = color;
        mRenderer.material.SetColor("Color_5774DDCC", color);
        ParticleSystem.MainModule _main = mParticleSystem.main;
        _main.startColor = color;
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
            //mParticleSystem.Play();
            mParentBlockDeveloping.DestroyJoint();
            mAnimationIsPlaying = !mAnimationIsPlaying;
            mIsFading = !mIsFading;
        }
    }
}
