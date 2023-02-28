using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1.0f;
    public Vector2 odist;
    

    public void Startshake()
    {
        StopAllCoroutines();
        StartCoroutine(Shaking());
        
    }

    IEnumerator Shaking()
    {
        
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;
        while(elapsedTime< duration)
        {
            Debug.Log(odist);
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/duration);
            transform.localPosition = startPosition + Random.insideUnitSphere * strength * (1/Mathf.Sqrt(odist.x*odist.x+odist.y*odist.y));
            yield return null;
        }

        transform.localPosition = startPosition;
    }
}
