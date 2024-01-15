using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("�÷��̾� �ִϸ��̼�")]
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
