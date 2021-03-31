using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{   
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip blastClip;
    public AudioClip slowmotionClip;
    public AudioClip rewindClip;
    public AudioClip punchClip;
    public AudioClip blockClip;
    public AudioClip bulletClip;
    public AudioClip Clip;

    public float rotSpeed = 3.0f;
    public float moveSpeed = 10.0f;
    public float gravity = -20.8f;
    public float terminalVelocity = -200.0f;
    public float minFall = -1.5f;
    public float verticalSpeed;
    public float jumpSpeed= 30.0f;
    public CharacterController _charController; 
    private ControllerColliderHit _contact;
    private Animator _animator;
    public float pushForce = 30.0f;
    public float dashDistance = 5.0f;   public float currentDashTime = 0.0f;    public float maxDashTime = 0.0f;   ParticleSystem[] dash;
    public Collider[] hitColliders; public float blastForce = 200.0f;    public float blastRadius = 30.0f;    public float blastUpward = 20.0f;
    public GameObject destroyedObject;
    private Vector3 movement;
    public ParticleSystem blastEffect;
    public ParticleSystem dashEffect;
    public GameObject stairField; public GameObject stairCreationParticle;  private GameObject forceFieldTemp;
    public GameObject shieldField;  public GameObject shieldParticleEffect;
    public GameObject blockOutParticle; public GameObject blockFrontParticle;
    public TimeManager timeManager;
    public CameraShake cameraShake;


    


    // Start is called before the first frame update
    void Start()
    {
        verticalSpeed = minFall;
        _charController = GetComponent<CharacterController>();
        // _charController.center = new Vector3(0, 0.6f , 0);
        _animator = GetComponent<Animator>();
        dash = _charController.GetComponentsInChildren<ParticleSystem>();
        _animator.SetBool("onGround", true);
        blastEffect.Stop(true);
        GameObject forceFieldTemp;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        // sprint
        if(Input.GetKey("left shift")){
            moveSpeed = 40.0f;
        }
        if( horInput != 0 || verInput != 0 ){
            movement.x = horInput * moveSpeed;
            movement.z  = verInput * moveSpeed;
            movement = Vector3.ClampMagnitude( movement, moveSpeed);
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0 , target.eulerAngles.y , 0);

            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation,
            direction, rotSpeed * Time.deltaTime);
            // StartCoroutine(Spark()); run spark control  
        }
        else{
        }
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        if(_charController.isGrounded){
                if(Input.GetButtonDown("Jump")){
                    verticalSpeed = jumpSpeed;
                    _animator.SetBool("Jumping", true);
                }
                else{
                    verticalSpeed = minFall;
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("onGround", true);
                }
                // if(verticalSpeed == minFall){
                //     _animator.SetBool("onGround", true);
                // } 
            }
            else{
                verticalSpeed += gravity*5*Time.deltaTime;
                if( verticalSpeed < terminalVelocity){
                    verticalSpeed = terminalVelocity ;
                }
                // _animator.SetBool("Jumping", true);
                if(verticalSpeed != minFall){
                    _animator.SetBool("onGround", false);
                } 
            }
            //dash functionality
        if( Input.GetKeyUp("q")){
            StartCoroutine(Fade());
        }
        //blast functionality
        if( Input.GetKeyUp("e")){
        StartCoroutine(Blast());
        }
        //punch functionality
        if(Input.GetKeyDown("r")){
        _animator.SetBool("punch",true);
         StartCoroutine(Punch());
        }
        // kick functionality
        if(Input.GetKeyDown("f")){
        StartCoroutine(Kick());
        }
        if(Input.GetKeyDown("g")){
        StartCoroutine(ForceField(forceFieldTemp));
        }
        if(Input.GetKeyDown("h")){
        StartCoroutine(BlockOut());
        }
        if(Input.GetKeyDown("z")){
        StartCoroutine(BlockFront());
        }
        if(Input.GetKeyDown("t")){
            StartCoroutine(Shield());
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            soundSource.PlayOneShot(slowmotionClip);
            timeManager.DoSlowMotion();
        }
        if(Input.GetKeyDown("j")){
            if(_animator.GetBool("dancing").Equals(false)){
            _animator.SetBool("dancing", true);
            }
            else if(_animator.GetBool("dancing").Equals(true)){
            _animator.SetBool("dancing", false);
            }
        }
        if(Input.GetMouseButtonDown(0)){
            soundSource.PlayOneShot(bulletClip);
            
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            soundSource.PlayOneShot(rewindClip);
            
        }

        movement.y  = verticalSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
        moveSpeed = 40.0f;
    }
    // handling dash and dash particle effect
    IEnumerator Fade() 
        {   if(_charController.isGrounded){
            _animator.SetBool("Dash", true);
            yield return new WaitForSeconds(0.55f);
            movement = Vector3.Lerp(movement , movement*dashDistance, 1);
            _charController.Move(movement);
            dashEffect.enableEmission = true;
            soundSource.PlayOneShot(dashClip);
        // if during dash you want to make player invisible  
        // Renderer[] rend =  _charController.GetComponentsInChildren<Renderer>() ; 
        // for (int i = 1; i < rend.Length - 1 ; i++)
        //     {
        //         if( rend[i].enabled){    
        //         // rend[i].enabled = false;
        //         }
        //     }
        //     yield return new WaitForSeconds(0.1f);
        //     for (int i = 1; i < rend.Length - 1 ; i++)
        //         {
        //             if( !rend[i].enabled){
        //             rend[i].enabled = true;
        //             }
        //         }
            yield return new WaitForSeconds(0.40f);
            _animator.SetBool("Dash",false);
            }
            else{
                Debug.Log(movement);
                movement = Vector3.Lerp(movement , movement*( dashDistance/20), 1);
                Debug.Log(movement);
                _charController.Move(movement);
                dashEffect.enableEmission = true;
                soundSource.PlayOneShot(dashClip);
                yield return new WaitForSeconds(0.10f);
            }
            dashEffect.enableEmission = false;   
        }
    //handling blast
    IEnumerator Blast(){
        _animator.SetBool("blast", true);
        // dash[3].enableEmission = true;    
        hitColliders = Physics.OverlapSphere(_charController.transform.position, 20.0f);
        yield return new WaitForSeconds(0.40f);
        blastEffect.Play(true);
        soundSource.PlayOneShot(blastClip);
        if(hitColliders.Length > 2){
        for (int i = 0; i < hitColliders.Length; i++)
        {   
//            if(hitColliders[i].name !="Floor" && hitColliders[i].name !="Player2" && hitColliders[i].gameObject.transform.parent != null){
            if(hitColliders[i].name !="Floor" && hitColliders[i].transform.root.name !="Player2" && hitColliders[i].gameObject.transform.parent != null){
                if( hitColliders[i].gameObject.transform.parent.name == "Destobj" ||  hitColliders[i].gameObject.transform.parent.name == "Enemy" ){
                Debug.Log(hitColliders[i].gameObject.name);  
                Rigidbody blastCube = hitColliders[i].GetComponent<Rigidbody>();
                blastCube.AddExplosionForce( blastForce, _charController.transform.position , blastRadius , blastUpward , ForceMode.Impulse);
                
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        }
        _animator.SetBool("blast", false);
        blastEffect.Stop(true);
    }
    //handling punch
    IEnumerator Punch(){
    soundSource.PlayOneShot(punchClip);
    yield return new WaitForSeconds(0.2f);
    RaycastHit hitdash;
    if( Physics.SphereCast( transform.position, 1.0f , transform.TransformDirection(Vector3.forward) , out hitdash , 0.5f)) {
        if(hitdash.collider.name !="Floor" && hitdash.collider.transform.root.name !="Player2" && hitdash.collider.gameObject.transform.parent != null){
        Debug.Log(hitdash.collider.name);
        if(hitdash.collider.name == "Wooden Crate"){
            Debug.Log(hitdash.collider.transform.position);
            Instantiate(destroyedObject , hitdash.collider.transform.position , hitdash.collider.transform.rotation  ) ;
            Destroy(hitdash.collider.gameObject);
        }
        else{
        hitdash.collider.GetComponent<Rigidbody>().AddForce( transform.forward * 40.0f , ForceMode.Impulse );
        }
        }
    }
    yield return new WaitForSeconds(0.2f);
    _animator.SetBool("punch",false);
    }
    IEnumerator Kick() 
        {
            _animator.SetBool("kick", true);
            soundSource.PlayOneShot(punchClip);
            yield return new WaitForSeconds(1.05f);
            
            _animator.SetBool("kick", false);
        }
    IEnumerator ForceField( GameObject forceFieldTemp)
    {   
        GameObject stairCreationParticleTemp = Instantiate( stairCreationParticle , transform.position , transform.rotation );
        forceFieldTemp = Instantiate(stairField , transform.position , transform.rotation );
        Destroy(stairCreationParticleTemp, 0.5f);
        Destroy(forceFieldTemp, 10.0f);
        yield return true;
    }
    IEnumerator Shield(){
        _animator.SetBool("shield" , true);
        // yield return new WaitForSeconds(0.20f);
        shieldParticleEffect.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        if(shieldField.active == false){
            shieldField.SetActive(true);
        }
        else if(shieldField.active == true){
            shieldField.SetActive(false);
        }
        _animator.SetBool("shield" , false);
        shieldParticleEffect.SetActive(false);
    }
    IEnumerator BlockOut(){
        soundSource.PlayOneShot(blockClip);
        _animator.SetBool("blockOut", true);
        yield return new WaitForSeconds(.30f);
        blockOutParticle.SetActive(true);

        yield return new WaitForSeconds(.6f);
        blockOutParticle.SetActive(false);
        _animator.SetBool("blockOut", false);
    }
    IEnumerator BlockFront(){
        soundSource.PlayOneShot(blockClip);
        _animator.SetBool("blockFront", true);
        blockFrontParticle.SetActive(true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("blockFront", false);
        blockFrontParticle.SetActive(false);
    }

}
