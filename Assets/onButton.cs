using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onButton : MonoBehaviour
{
    public void onClick(Rigidbody rb)
    {
        rb.mass++;
        rb.AddForce(0,1/Time.deltaTime,0, ForceMode.Impulse);
    }
}
