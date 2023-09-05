using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehavior : StateMachineBehaviour
{
    public AnimationClip motion;
    string animationClip;
    bool loop;

    [Header("스파인 모션 레이어")]
    public int layer = 0;
    public float timeScale = 1.0f;

    [Header("루트 모션 적용")]
    public bool applyRootMotion = false;

    private SkeletonAnimation skeletionAnimation;
    private Spine.AnimationState spineAnimationState;
    private Spine.TrackEntry trackEntry;

    private void Awake()
    {
        if (motion != null)
        {
            animationClip = motion.name;
            Debug.Log(animationClip);
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerindex)
    {
        animator.applyRootMotion = applyRootMotion;

        timeScale = animator.GetFloat("Walk_speed");
        if (skeletionAnimation == null)
        {
            skeletionAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletionAnimation.state;
        }
        if (animationClip != null)
        {
            loop = stateInfo.loop;
            trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
            trackEntry.TimeScale = timeScale;
        }
    }
}
