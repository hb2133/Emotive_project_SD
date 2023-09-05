using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Knight_Animation : MonoBehaviour
{
    [SpineAnimation]
    public string idleAnimationName;
    [SpineAnimation]
    public string back_idleAnimationName;
    [SpineAnimation]
    public string moveAnimationName;
    [SpineAnimation]
    public string attackAnimationName;
    [SpineAnimation]
    public string back_attackAnimationName;

    SkeletonAnimation skeletonAnimation;

    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public float idleDuration = 1.0f;
    public float moveDuration = 1.0f;
    public float attackDuration = 1.0f;

    public Vector3 offset;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        transform.position += offset;
        StartCoroutine(DoDemoRoutine());
    }

    IEnumerator DoDemoRoutine()
    {
        while (true)
        {
            skeleton.SetSkin("Front_1");
            skeleton.SetToSetupPose();
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
            yield return new WaitForSeconds(idleDuration);

            skeleton.SetSkin("Front_2");
            skeleton.SetToSetupPose();
            spineAnimationState.SetAnimation(0, moveAnimationName, true);
            yield return new WaitForSeconds(moveDuration);

            skeleton.SetSkin("Front_1");
            skeleton.SetToSetupPose();
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
            yield return new WaitForSeconds(idleDuration);

            skeleton.SetSkin("Back_1");
            skeleton.SetToSetupPose();
            spineAnimationState.SetAnimation(0, back_idleAnimationName, true);
            yield return new WaitForSeconds(idleDuration);

            skeleton.SetSkin("Back_3");
            skeleton.SetToSetupPose();
            spineAnimationState.SetAnimation(0, attackAnimationName, false);
            yield return new WaitForSeconds(attackDuration);
        }
    }


}
