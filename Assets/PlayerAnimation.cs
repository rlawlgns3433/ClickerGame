using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("플레이어 애니메이션")]
    public Animator mAnimator;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void PlayUpgradeAnimation()
    {
        mAnimator.Play("run");
    }

    public void PlayIdleAnimation()
    {
        mAnimator.Play("idle");
    }
}
