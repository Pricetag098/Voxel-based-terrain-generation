using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCam : MonoBehaviour
{
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Time.deltaTime * Input.GetAxis("Vertical") * transform.forward * speed;
        transform.localPosition += Time.deltaTime * Input.GetAxis("Horizontal") * transform.right * speed;
        transform.localEulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
    }
}
