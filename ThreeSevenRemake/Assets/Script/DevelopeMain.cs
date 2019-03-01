using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Tools;

public class DevelopeMain : MonoBehaviour
{
    public GameObject BlockObject;

    public Light DirectionalLight;

    private Block mCurrentBlock;
    private Vector3 mBlockStartPosition;

    private bool mGameOver = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // When the game start, begin delay for the first block to be created
        // StartCoroutine(StartGame())
    }

    private void Update()
    {
        // If mGameOver is equal to true, don't proceed futher of this 
        // block and do the following things
            // Call the function to collapse the table
            // Call the function to display the result

        // If the currentBlock is null or undergoing scoreing progression
            // don't proceed futher of this block

        // If the currenBlock has landed
            // Call the function for Scoring calcultating
            // If not proceed further


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool BlockDropping()
    {
        // navigate the block as long the lower row still is vacant

        // check if the current block can drop to the row below it
        // if the row below is not vacant
            // put in the block into the collection of landed block
            // set the boolean for check for scoring to true
            // return the block has stopped dropping

        return true;
    }

    /// <summary>
    /// All navigation input to the block include suspend the game is done from 
    /// this block
    /// </summary>
    private void NavigationInput()
    {
        // input for move the block left if the left column is vacant

        // input for move the block right if the right column is vacant

        // input for move the block downward one row if the row below is vacant
        // and if the time between each keypress has expired

        // input for rotate the block clockwise if the column or row of where the block
        // rotate to is vacant

        // input for swaping the cubes value inside the block

        // debug input for removing the current block to a new block.
        // for testing purpose
    }

    /// <summary>
    /// Checking for if the just landed block and those that have rows belows 
    /// vacant and dropped scored
    /// if scored the cube vanish animation plays and the position of the blocks
    /// in the grid will be rearranged
    /// </summary>
    /// <returns>return if the scoring is under progress</returns>
    private bool UndergoingScoringProgression()
    {

        return true;
    }
}
