using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
   private void Start() {
       objectPooler = ObjectPooler.Instance;
   }
   void FixedUpdate() {
       //blast phenkna 
        if(Input.GetKeyDown("h")){
            StartCoroutine(Radial());
        }
        if(Input.GetKey("z")){
            StartCoroutine(Linear());
        }

        }
        // StartCoroutine(Shrapnel());
       IEnumerator Radial(){
           yield return new WaitForSeconds(1.10f);
           for (int i = 0 ; i<150 ; i++){
                objectPooler.SpawnFromPool("Cube", transform.position , Quaternion.identity);
           }
       }
       IEnumerator Linear(){
           Vector3 pos = transform.position + transform.forward * 4;
           objectPooler.SpawnFromPool("Cube", pos , Quaternion.identity);
           yield return true;
           
       }

}
