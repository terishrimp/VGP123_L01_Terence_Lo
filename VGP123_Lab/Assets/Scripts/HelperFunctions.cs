using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class HelperFunctions
{
    public static void AnimTrigger (Animator animator, string triggerName)
    {
        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(p.name);
            }
        }
        animator.SetTrigger(triggerName);
    }
}
