using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target ;
    private Transform player;
    public float rotSpeed =1.5f;
    public float _rotY;
    public Vector3 _offset;
    float horInput,verInput ;
    // Start is called before the first frame update
    void Start()
    {   
        // Cursor.visible = false ;
        // Cursor.lockState = CursorLockMode.Locked ;
        _rotY = transform.eulerAngles.y;
        _offset = target.position -transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput -= Input.GetAxis("Mouse Y")*rotSpeed;
        verInput = Mathf.Clamp( verInput , -35.0f , 60.0f);

        if(horInput !=0){
            _rotY += horInput*rotSpeed;
        }
        else{
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3 ;
        }
        // Quaternion rotation = Quaternion.Euler(0, _rotY , 0);
        Quaternion rotation = Quaternion.Euler(verInput, _rotY , 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
        // target.rotation = Quaternion.Euler( verInput , horInput , 0);  
    }
}
