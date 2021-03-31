using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{   
    private Camera _camera;
    private GameObject crosshair ;
    // public GameObject gun;
    public int size = 180;
    public ParticleSystem muzzleFlash;
    public GameObject impactFlash; 
    private RaycastHit hitbullet;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        // Cursor.lockState = CursorLockMode.Locked; //fps code
        Cursor.visible = false; 
    }
    void OnGUI(){
        float posX = _camera.pixelWidth/1.95f - size/4;
        float posY = _camera.pixelHeight/2.7f - size/2;
        GUI.Label( new Rect (posX, posY , size , size),"+");
        // GUI.Label(new Rect(Input.mousePosition.x, _camera.pixelHeight - Input.mousePosition.y , size ,size ), "+");
    }

    // Update is called once per frame
    // void Update()
    // {   
    //     Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y , 0);
    //     Ray ray = _camera.ScreenPointToRay(point);
    //     RaycastHit hitbullet;
    //     Physics.Raycast(ray, out hitbullet);
    //     // Debug.Log(ray.direction);
    //     Debug.Log(point);
    //     gun.transform.LookAt(new Vector3(1.0f,1.0f,1.0f));
    //     // // gun.transform.LookAt(new Vector3(Mathf.Clamp( hit. , -35.0f , 60.0f), Mathf.Clamp( Input.mousePosition.y , -35.0f , 60.0f) , 0));
    //     if(Input.GetMouseButtonDown(0)){
    //         // for fps
    //         // Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2 , 0);
    //         // Ray ray =_camera.ScreenPointToRay(point);  
            
    //         if(Physics.Raycast(ray, out hitbullet)){
    //         //     GameObject hitObject = hit.transform.gameObject;
    //         //     Debug.Log(hitObject.name);
    //         //     // ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
    //         //     // if(target !=null){
    //         //     //     Debug.Log("Target hit");
    //         //     //     target.ReactToHit();
    //         //     // }
    //         //     // else{
    //             StartCoroutine(SphereIndicator(hitbullet.transform.position));
    //             }
    //         }
    //     }
        void Update()
    {   
        Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y , 0);
        // Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2 , 0); for fps
        // Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        // Vector3 point = new Vector3(_camera.pixelWidth/1.95f - size/4, _camera.pixelHeight/2.7f - size/2 , 0); //for fps
        Ray ray = _camera.ScreenPointToRay(point);
        if(Physics.Raycast(ray, out hitbullet )){
            // gun.transform.LookAt(hitbullet.point);
            if(Input.GetMouseButtonDown(0)){
                if(hitbullet.collider.transform.root.name != "Player2" ){
                // GameObject impactFlashTemp = Instantiate(impactFlash, hitbullet.point , Quaternion.LookRotation(hitbullet.normal));
                // Destroy(impactFlashTemp ,1.0f);
                }
                if(hitbullet.collider.transform.root.name == "Destobj" && hitbullet.distance <= 100.0f){
                    hitbullet.collider.attachedRigidbody.AddForceAtPosition( -hitbullet.normal * 20.0f ,hitbullet.point, ForceMode.Impulse );
                    StartCoroutine(SphereIndicator(hitbullet.point));
                }
            }
            } 
    }
    private IEnumerator SphereIndicator(Vector3 pos){
        // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // sphere.transform.position = pos;
        // Destroy(sphere);  
        // Instantiate(muzzleFlash, pos);
        //  Debug.Log("making flash");
        // GameObject impactFlashTemp = Instantiate(impactFlash, hitbullet.point , Quaternion.LookRotation(hitbullet.normal));
        // Destroy(impactFlashTemp ,1.0f);
        yield return true;   
    }
}
