using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration , float magnitude ){
        Vector3 originalPos = transform.localPosition;
        // Vector3 originalAngle = transform.localRotation;
        float elasped = 0.0f;
        while( elasped < duration){
            Debug.Log("elasped is " + elasped);
            float x= Random.Range(-1f, 1f) * magnitude;
            float y= Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3 (x, y, originalPos.z);
            elasped += Time.deltaTime;
            yield return null;

        }
        transform.localPosition = originalPos;
     }
}