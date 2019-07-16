using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveFrame : MonoBehaviour
{
    public GameObject GreenObjective;
    public GameObject BlueObjective;
    public GameObject RedObjective;

    private Text GreenFrameText;
    private Text BlueFrameText;
    private Text RedFrameText;

    private Animation GreenAnimation;
    private Animation BlueAnimation;
    private Animation RedAnimation;

    // Start is called before the first frame update
    void Start()
    {
        BlockManager.achieveScoring += DisplayScoring;
        Objective.achiveObjective += SetObjectiveNumbersFor;

        GreenFrameText = GreenObjective.transform.GetChild(1).GetComponent<Text>();
        BlueFrameText = BlueObjective.transform.GetChild(1).GetComponent<Text>();
        RedFrameText = RedObjective.transform.GetChild(1).GetComponent<Text>();

        GreenAnimation = GreenObjective.GetComponent<Animation>();
        BlueAnimation = BlueObjective.GetComponent<Animation>();
        RedAnimation = RedObjective.GetComponent<Animation>();
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        Objective.achiveObjective -= SetObjectiveNumbersFor;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetObjectiveNumbersFor(Objectives anObjective, int anObjectiveNumber)
    {
        if(anObjective == Objectives.X1)
            GreenFrameText.text = anObjectiveNumber.ToString();
        else if(anObjective == Objectives.X5)
            BlueFrameText.text = anObjectiveNumber.ToString();
        else if(anObjective == Objectives.X10)
            RedFrameText.text = anObjectiveNumber.ToString();
    }

    public void PlayAnimationOn(Objectives anObjective)
    {
        if (anObjective == Objectives.X1)
            GreenAnimation.Play();
        else if (anObjective == Objectives.X5)
            BlueAnimation.Play();
        else if (anObjective == Objectives.X10)
            RedAnimation.Play();
    }

    private void DisplayScoring(Objectives anObjective, List<Cube> someScoringCube)
    {
        PlayAnimationOn(anObjective);
    }
}
