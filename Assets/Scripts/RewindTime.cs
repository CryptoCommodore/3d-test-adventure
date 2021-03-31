using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour
{   
    bool isRewinding = false ;
    public float recordTime = 10f;
    List<PointIntime> pointIntime;
    Rigidbody rb ;
    // Start is called before the first frame update
    void Start()
    {
        pointIntime = new List<PointIntime>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            StartRewind();
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            StopRewind();
        }
    }
    void FixedUpdate() {
    if(isRewinding){
        Rewind();
    }
    else{
        Record();
    }   
    }
    void Record(){
        if(pointIntime.Count > Mathf.Round(recordTime * (1f/ Time.fixedDeltaTime))){
            pointIntime.RemoveAt( pointIntime.Count - 1 );
        }
        pointIntime.Insert(0, new PointIntime(transform.position,transform.rotation));
    }
    void Rewind(){
        if(pointIntime.Count > 0 ){
            PointIntime pointIntimeTemp = pointIntime[0];
            transform.position = pointIntimeTemp.position;
            transform.rotation = pointIntimeTemp.rotation;
            pointIntime.RemoveAt(0);
        }else{
            StopRewind();
        }
    }
    public void StartRewind(){
        isRewinding = true ;
        rb.isKinematic = true ;        
    }
    public void StopRewind(){
        isRewinding = false ;
        rb.isKinematic = false ;
    }    
}
