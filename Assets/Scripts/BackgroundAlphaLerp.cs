using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAlphaLerp : MonoBehaviour
{
    public float LerpTime = 1f;
    public float TargetAlpha = 219f;

    private void OnEnable()
    {
        var image = GetComponent<Image>();

        StartCoroutine(AlphaLerpCR(image));
    }

    private IEnumerator AlphaLerpCR(Image image)
    {
        var color = image.color;
        color.a = 0f;

        var t = 0f;
        while (t < 1f)
        {
            color.a = Mathf.Lerp(0f, TargetAlpha / 255, t);
            image.color = color;

            t += Time.deltaTime / LerpTime;
            yield return null;
        }
        
        color.a = TargetAlpha / 255;
        image.color = color;
    }
}
