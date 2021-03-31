using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{   
    public GameObject bulletEmitter1;
    public GameObject bulletEmitter2;
    public GameObject bullet;
    public ParticleSystem muzzleFlash1;
    public ParticleSystem muzzleFlash2;
    public GameObject impactFlash;
    public GameObject aimer;
    public float Bullet_Forward_Force = 5000.0f;
    private ParticleSystem muzzleFlash1Temp;    private ParticleSystem muzzleFlash2Temp;  private GameObject impactFlashTemp;
    private ParticleSystem bulletTrail;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash1Temp = Instantiate( muzzleFlash1, bulletEmitter1.transform.position , bulletEmitter1.transform.rotation);
        muzzleFlash2Temp = Instantiate( muzzleFlash2, bulletEmitter2.transform.position , bulletEmitter2.transform.rotation);
        muzzleFlash1Temp.transform.parent = player.transform;
        muzzleFlash2Temp.transform.parent = player.transform;
    }

    // Update is called once per frame
    void Update()
    {   
        
        if(Input.GetMouseButtonDown(0)){
            StartCoroutine(PlayerShoot());
        }
    }
    IEnumerator PlayerShoot(){
        muzzleFlash1Temp.Play(true);
            muzzleFlash2Temp.Play(true);
            GameObject bulletHandler1 = Instantiate(bullet, bulletEmitter1.transform.position , bulletEmitter1.transform.rotation) as GameObject;
            GameObject bulletHandler2 = Instantiate(bullet, bulletEmitter2.transform.position, bulletEmitter2.transform.rotation) as GameObject;
            
            while(bulletHandler1 != null){
            // bulletHandler1.transform.Rotate(Vector3.left * 90);
            // bulletHandler2.transform.Rotate(Vector3.left * 90);
            
            Rigidbody bulletRigidBody1 = bulletHandler1.GetComponent<Rigidbody>();
            Rigidbody bulletRigidBody2 = bulletHandler2.GetComponent<Rigidbody>();
            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force. 
            bulletRigidBody1.AddForce(-bulletEmitter1.transform.forward * Bullet_Forward_Force);
            bulletRigidBody2.AddForce(-bulletEmitter2.transform.forward * Bullet_Forward_Force);
            
            RaycastHit hitbullet1,hitbullet2 ;
            Ray ray1,ray2;
            ray1 = new Ray(bulletRigidBody1.transform.position, -bulletHandler1.transform.forward) ;
            if(Physics.Raycast(ray1, out hitbullet1 )){
                // Debug.Log(hitbullet1.collider.name);
                // Debug.Log(hitbullet1.distance);
                if(hitbullet1.collider.transform.name != "Bullet"){
                    impactFlashTemp = Instantiate( impactFlash , hitbullet1.point , Quaternion.LookRotation(hitbullet1.normal));
                    Destroy(impactFlashTemp, .50f);
                    }
                }
            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(bulletHandler1, 1.0f);
            // Destroy(bulletHandler2, 5.0f);
            yield return null;
            }
            // Destroy(bulletHandler1);

    }
    private void OnCollisionEnter(Collision col) {
        Debug.Log(" on trigger " + col.gameObject.name);
     //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
     if(col.gameObject.tag == "Enemy")
     {
     }
     }
}
