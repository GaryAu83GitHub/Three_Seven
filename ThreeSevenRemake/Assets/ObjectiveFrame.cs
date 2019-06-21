using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveFrame : MonoBehaviour
{
    

    public Text MultiplyOneTimesObjectiveNumber;
    public Text MultiplyFiveTimesObjectiveNumber;
    public Text MultiplyTenTimesObjectiveNumber;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.achiveObjective += SetObjectiveNumbersFor;
    }

    private void OnDestroy()
    {
        GameManager.achiveObjective -= SetObjectiveNumbersFor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjectiveNumbersFor(Objectives anObjective, int anObjectiveNumber)
    {
        if(anObjective == Objectives.X1)
            MultiplyOneTimesObjectiveNumber.text = anObjectiveNumber.ToString();
        else if(anObjective == Objectives.X5)
            MultiplyFiveTimesObjectiveNumber.text = anObjectiveNumber.ToString();
        else if(anObjective == Objectives.X10)
            MultiplyTenTimesObjectiveNumber.text = anObjectiveNumber.ToString();
    }
}
