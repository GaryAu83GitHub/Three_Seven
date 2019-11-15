using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualOperatorBox : OperatorBoxBase
{
    public override void Start()
    {
        base.Start();
        OperatorText.text = "=";
    }

    public override void Update()
    {
        base.Update();
    }
}
