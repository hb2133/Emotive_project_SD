using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Knight_Animation : MonoBehaviour
{
    [SpineAnimation]
    public string idleAnimationName;

    SkeletonAnimation skeletonAnimation;

    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public float idleDuration = 1.0f;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        StartCoroutine(DoDemoRoutine());
    }

    IEnumerator DoDemoRoutine()
    {
        spineAnimationState.SetAnimation(0, idleAnimationName, true);
        yield return new WaitForSeconds(idleDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
