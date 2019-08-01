using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public ParticleSystem mActiveParticle;
    public ParticleSystem mPassiveParticle;
    //public Material mMaterial;

    private TextMesh mTextMesh;
    private MeshRenderer mRenderer;
    private Color mTransparentColor = new Color(0f, 0f, 0f, 0f);

    private Block mParentBlock;
    public Block ParentBlock { get { return mParentBlock; } }

    private Cube mSiblingCube;
    public Cube SiblingCube { get { return (mSiblingCube ?? null); } }

    [SerializeField]
    private Vector2Int mGridPosition;
    public Vector2Int GridPos { get { return mGridPosition; } set { mGridPosition = value; } }

    private Color mColor;
    public Color Color { get { return mColor; } }

    [SerializeField]
    private int mCubeNumber;
    public int Number { get { return mCubeNumber; } }

    private bool mIsScoring = false;
    public bool IsScoring
    {
        get { return mIsScoring; }
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
    
    public void Init(Block aParentBlock, int aNumber)
    {
        mParentBlock = aParentBlock;
        SetCubeNumber(aNumber);
    }

    public void ConnectSiblingCube(Cube aSiblingCube)
    {
        mSiblingCube = aSiblingCube;
    }

    public bool IsSiblingsPosition(Vector2Int aPos)
    {
        if (mSiblingCube == null)
            return false;
        return (mSiblingCube.GridPos == aPos);
    }

    /// <summary>
    /// Set the cube number both internal and external and set the material colour
    /// The cube colour are pre-set and will change according to the given number
    /// </summary>
    /// <param name="aNumber">The given number to this cube</param>
    public void SetCubeNumber(int aNumber)
    {
        mCubeNumber = aNumber;
        mColor = SupportTools.GetCubeHexColorOf(mCubeNumber);

        mTextMesh.text = mCubeNumber.ToString();
        //mRenderer.material.color = color;
        mRenderer.material.SetColor("Color_5774DDCC", mColor);
        //ParticleSystem.MainModule _main = mActiveParticleSystem.main;
        //_main.startColor = mColor;
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
            mParentBlock.DestroyJoint();
            mAnimationIsPlaying = !mAnimationIsPlaying;
            mIsFading = !mIsFading;
        }
    }

    public void PlayActiveParticlar()
    {
        mActiveParticle.Play();
        mIsScoring = true;
    }

    public void PlayPassiveParticlar()
    {
        mPassiveParticle.Play();
        mIsScoring = true;
    }
}
