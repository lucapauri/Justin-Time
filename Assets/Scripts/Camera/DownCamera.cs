using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownCamera : MonoBehaviour
{
    private float _inputSpeed;
    private float _speed = 5f;

    private float verticalShift;
    private float offset = 3f; //trovare un modo furbo per definire l'offset

    private CharacterController _characterController;
    private GlobalVariables globalVariables;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        globalVariables = FindObjectOfType<GlobalVariables>();

    }

    // Update is called once per frame
    void Update()
    {
        // _inputSpeed = Input.GetAxis("Horizontal");
        //_characterController.Move(transform.right * _inputSpeed * _speed * Time.deltaTime);

        if (globalVariables.currentTimeline == 1)
        {
            verticalShift = globalVariables.justin.transform.position.y - globalVariables.upPlaneHeight;
        }
        if (globalVariables.currentTimeline == 0)
        {
            verticalShift = globalVariables.justin.transform.position.y - globalVariables.downPlaneHeight;
        }


        transform.position = new Vector3(globalVariables.justin.transform.position.x, globalVariables.downPlaneHeight + verticalShift + offset, transform.position.z);

    }
}