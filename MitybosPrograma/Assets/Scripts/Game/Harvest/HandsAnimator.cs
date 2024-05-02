using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAnimator : MonoBehaviour
{
    private Animator parentAnimator;
    private Animator childAnimator;
    private SpriteRenderer parentSpriteRenderer;
    private SpriteRenderer childSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the parent animator component
        parentAnimator = transform.parent.GetComponent<Animator>();

        // Get the child animator component
        childAnimator = GetComponent<Animator>();

        // Get the parent sprite renderer component
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

        // Get the child sprite renderer component
        childSpriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure all necessary components are not null
        if (parentAnimator == null || childAnimator == null || parentSpriteRenderer == null || childSpriteRenderer == null)
        {
            Debug.LogError("Parent, child animator, or sprite renderer not found.");
            return;
        }

        // Copy initial state and parameters from parent to child animator
        CopyAnimatorStateAndParameters();
    }

    // Update is called once per frame
    void Update()
    {
        // Synchronize state, parameters, and frame order every frame
        CopyAnimatorStateAndParameters();
        //SynchronizeFrameOrder();
    }

    // Function to copy state and parameters from parent to child animator
    private void CopyAnimatorStateAndParameters()
    {
        // Copy the parent's current state
        childAnimator.Play(parentAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash);

        // Copy parameters from parent to child animator
        int paramCount = parentAnimator.parameterCount;
        for (int i = 0; i < paramCount; i++)
        {
            AnimatorControllerParameter param = parentAnimator.GetParameter(i);
            switch (param.type)
            {
                case AnimatorControllerParameterType.Float:
                    childAnimator.SetFloat(param.name, parentAnimator.GetFloat(param.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    childAnimator.SetInteger(param.name, parentAnimator.GetInteger(param.name));
                    break;
                case AnimatorControllerParameterType.Bool:
                    childAnimator.SetBool(param.name, parentAnimator.GetBool(param.name));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (parentAnimator.GetBool(param.name))
                        childAnimator.SetTrigger(param.name);
                    break;
            }
        }

        // Copy flipX state from parent to child sprite renderer
        childSpriteRenderer.flipX = parentSpriteRenderer.flipX;
    }
}
