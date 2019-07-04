using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aniControl : MonoBehaviour
{
    Animator animator;
    bool bounce;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBounce()
    {
        bounce = !bounce;
        animator.SetBool("bounce", bounce);
    }

    public void SetBlend(float f)
    {
        animator.SetFloat("Blend", f);
    }
    public void SetBlend2(float f)
    {
        animator.SetFloat("Blend2nd", f);
    }
}
