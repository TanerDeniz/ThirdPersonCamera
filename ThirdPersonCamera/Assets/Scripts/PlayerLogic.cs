using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Start is called before the first frame update
    float horizontalInput, verticalInput, movementSpeed=5.0f;
    float jumpHeight = 0.25f;
    float gravity = 0.981f;
    bool jump = false;

    Vector3 movementInput;
    Vector3 heightMovement;
    Vector3 verticalMovement;
    Vector3 horizontalMovement;

    CharacterController characterController;
    Animator animator;

    [ SerializeField]
    List<AudioClip> footStepStoneSound = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> footStepEarthSound = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> footStepGrassSound = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> footStepPuddleSound = new List<AudioClip>();


    [SerializeField]
    Transform leftFoot;
    [SerializeField]
    Transform rightFoot;

    AudioSource audioSource;

    GameObject camera;
    CameraLogic cameraLogic;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        camera = Camera.main.gameObject;
        if (camera)
        {
            cameraLogic = camera.GetComponent<CameraLogic>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movementInput = new Vector3(horizontalInput, 0, verticalInput);

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            jump = true;
        }
        if (animator)
        {
            animator.SetFloat("horizontalInput",horizontalInput);
            animator.SetFloat("verticalInput",verticalInput);
        }
    }
    private void FixedUpdate()
    {
        if (jump)
        {
            heightMovement.y = jumpHeight;
            jump = false;
        }
        heightMovement.y -= gravity * Time.deltaTime;
        verticalMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        horizontalMovement = transform.right * horizontalInput * movementSpeed * Time.deltaTime;
        
        characterController.Move(horizontalMovement+verticalMovement+heightMovement);
        if (characterController.isGrounded)
        {
            heightMovement.y = 0.0f;
        }
        if(cameraLogic &&  Mathf.Abs(horizontalInput)>0.1f  || Mathf.Abs(verticalInput) > 0.1f)
        {
            transform.forward = cameraLogic.GetForwadVector();
        }
    }
    public void PlayFootStepSound(int footIndex)
    {
        if (footIndex == 0)
        {
            RaycastTerrain(leftFoot.position);
        }
        else if(footIndex==1)
        {
            RaycastTerrain(rightFoot.position);
        }
    }
    void RaycastTerrain(Vector3 positiion)
    {
        LayerMask layerMask = LayerMask.GetMask("Terrain");
        Ray ray = new Ray(positiion,Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, layerMask))
        {
            string hitTag = hit.collider.gameObject.tag;
            if (hitTag == "Earth")
            {
                PlayRandomSound(footStepEarthSound);
            }else if (hitTag == "Stone")
            {
                PlayRandomSound(footStepStoneSound);
            }
            else if (hitTag == "Grass")
            {
                PlayRandomSound(footStepGrassSound);
            }
            else if (hitTag == "Puddle")
            {
                PlayRandomSound(footStepPuddleSound);
            }
        }

    }
    void PlayRandomSound(List<AudioClip> audioClips)
    {
        if (audioClips.Count > 0 && audioSource)
        {
            int randomNum = Random.Range(0, audioClips.Count - 1);
            audioSource.PlayOneShot(audioClips[randomNum]);
        }

    }

}
