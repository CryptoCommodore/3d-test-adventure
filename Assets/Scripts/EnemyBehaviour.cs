using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public GameObject enemyBulletEmitter;
    public ParticleSystem enemyMuzzleFlash;
    public ParticleSystem enemyImpactFlash;
    public float enemyBulletForwardForce ;
    public AudioSource enemyAudio;
    public AudioClip enemyShoot;

    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating( "LookAround" , .05f , .05f );
    }

    // Update is called once per frame
    void Update()
    {   
        transform.LookAt(player.transform);
        enemyMuzzleFlash.transform.position = enemyBulletEmitter.transform.position;
        LookAround();
    }
    private void LookAround(){
        // enemyMuzzleFlash.Play(true);
        Ray ray;
        RaycastHit bulletHit;
        ray = new Ray( enemyBulletEmitter.transform.position , enemyBulletEmitter.transform.forward );
        if(Physics.Raycast(ray , out bulletHit)){
            // Debug.Log(bulletHit.collider.transform.name);
            // if(bulletHit.collider.transform.name == "Player2"){
            if(bulletHit.distance > 50f){
                transform.position += transform.forward * .2f;
            }
            else if(bulletHit.distance < 50f && bulletHit.distance > 30f){
                transform.position += transform.forward * .2f;
                StartCoroutine(Shoot());
            }
            else if(bulletHit.distance < 30f){
                 StartCoroutine(Shoot());
            }
        }
    }
    IEnumerator Shoot(){
        GameObject enemyBulletHandler = Instantiate(bullet, enemyBulletEmitter.transform.position , enemyBulletEmitter.transform.rotation) as GameObject;
        enemyAudio.PlayOneShot(enemyShoot);
        enemyMuzzleFlash.Play(true);
        ParticleSystem impactTemp;
        // enemyBulletHandler.transform.Rotate(Vector3.left * 90);
        Rigidbody bulletRigidBody = enemyBulletHandler.GetComponent<Rigidbody>();
            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force. 
        bulletRigidBody.AddForce(-enemyBulletEmitter.transform.forward * enemyBulletForwardForce);
        RaycastHit bulletHit;
        Ray ray = new Ray( bulletRigidBody.transform.position , -bulletRigidBody.transform.forward );
        if(Physics.Raycast(ray , out bulletHit)){
            impactTemp = Instantiate(enemyImpactFlash , bulletHit.point , Quaternion.LookRotation(bulletHit.normal) );
            yield return new WaitForSeconds(.2f);
            impactTemp.Stop(true);
        }
        
        Destroy( enemyBulletHandler , 1.0f);

        yield return true;
    }
}
