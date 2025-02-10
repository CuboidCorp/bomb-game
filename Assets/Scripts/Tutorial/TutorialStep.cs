using System;
using System.Collections;

public class TutorialStep
{
    public Action StepAction;

    public Func<IEnumerator> WaitCondition;

    public TutorialStep(Action stepAction, Func<IEnumerator> waitCondition)
    {
        StepAction = stepAction;
        WaitCondition = waitCondition;
    }
}
