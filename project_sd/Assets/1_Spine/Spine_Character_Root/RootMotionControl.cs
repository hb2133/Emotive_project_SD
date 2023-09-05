using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RootMotionControl : MonoBehaviour
{
    // A reference to the Skeleton Animation component of the Spine object
    public SkeletonAnimation skeletonAnimation;

    // List of animation names for which root motion should be enabled
    public List<string> rootMotionAnimations;

    public SkeletonRootMotion skeletonRootMotion;

    private void Start()
    {
        if (skeletonAnimation == null)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
        }

        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation not found on the GameObject.");
            return;
        }

        
        if (!TryGetComponent<SkeletonRootMotion>(out skeletonRootMotion))
        {
            Debug.LogError("SkeletonRootMotion not found on the GameObject.");
            return;
        }

        skeletonAnimation.state.Complete += HandleAnimationComplete;
    }

    private void HandleAnimationComplete(Spine.TrackEntry trackEntry)
    {
        // Check if the completed animation's name is in the list
        if (rootMotionAnimations.Contains(trackEntry.Animation.Name))
        {
            // Enable root motion if the animation is in the list
            skeletonRootMotion.enabled = true;
        }
        else
        {
            // Disable root motion if the animation is not in the list
            skeletonRootMotion.enabled = false;
        }
    }

    // Remember to unregister the event on destruction to prevent potential memory leaks
    private void OnDestroy()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.state.Complete -= HandleAnimationComplete;
        }
    }
}
