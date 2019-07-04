using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock : MonoBehaviour
{
    [SerializeField]
    float TimeScale;

    UnityEngine.UI.Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float amount = image.fillAmount + (Time.deltaTime/TimeScale);
        amount %= 1;
        image.fillAmount = amount;
    }
}
