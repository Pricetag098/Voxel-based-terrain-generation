using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector3 gravSource;
    public Transform head, cam;
    public float G = 1;
    public float acceleation = 10;
    public float slowForce;
    public float maxSpeed = 10;
    public float jetpack = 10;
    Rigidbody rb;
    public float gcRad;
    public LayerMask groundLayer;
    public float gcOffset;
    public bool grounded;
    public float jumpForce;
    
    Vector2 inputDir;
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        grounded = Physics.CheckSphere(transform.position + transform.up * gcOffset, gcRad, groundLayer);
        inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        DoCamRot();
        head.transform.localEulerAngles = new Vector3(0, Camera.main.transform.localEulerAngles.y, 0);
        //transform.forward = Camera.main.transform.forward;
        
        //transform.up = transform.position - gravSource.transform.position;
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            rb.AddForce(transform.up * jetpack);
        }
        
        if (!grounded)
            rb.AddForce(gravSource * G);

        if (Vector3.Dot(rb.velocity, head.transform.forward * Mathf.Sign(inputDir.y)) < Mathf.Abs(inputDir.y) * maxSpeed)
            rb.AddForce(inputDir.y * acceleation * head.transform.forward);

        if (Vector3.Dot(rb.velocity, head.transform.right * Mathf.Sign(inputDir.x)) < Mathf.Abs(inputDir.x) * maxSpeed)
            rb.AddForce(inputDir.x * acceleation * head.transform.right);
        if (inputDir.y == 0)
            rb.AddForce(slowForce * Vector3.Dot(rb.velocity, head.transform.forward) * -head.transform.forward);
        if (inputDir.x == 0)
            rb.AddForce(slowForce * Vector3.Dot(rb.velocity, head.transform.right) * -head.transform.right);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * gcOffset, gcRad);

    }

    Vector3 camRot = Vector3.zero;
    void DoCamRot()
    {
        camRot.y += Input.GetAxisRaw("Mouse X");
        if (camRot.y < 0)
            camRot.y = 360;
        if (camRot.y > 360)
            camRot.y = 0;
        camRot.x = Mathf.Clamp(camRot.x + -Input.GetAxisRaw("Mouse Y"), -90, 90);

        cam.localEulerAngles = (camRot);
    }

    
}