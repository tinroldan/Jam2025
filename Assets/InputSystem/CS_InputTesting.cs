using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputTesting : MonoBehaviour
{

    private Rigidbody sphereRigidbody;
    [SerializeField] float movementSpeed = 0.5f;
    //[SerializeField]private MeshRenderer attackSphere;

    private void Awake()
    {
        sphereRigidbody = GetComponent<Rigidbody>();
        //attackSphere = GetComponentInChildren<MeshRenderer>();
    }

    private void FixedUpdate()
    {

    }

    public void BasicAttack(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {

            Debug.Log("AttackingBasically" + inputContext.duration);
            sphereRigidbody.AddForce(Vector3.forward * 0.3f, ForceMode.Impulse);
            //attackSphere.enabled = true;
        }
        //if (inputContext.canceled)
        //{
        //    Debug.Log("AttackingBasically" + inputContext.duration);
        //    sphereRigidbody.AddForce(Vector3.forward * 5f, ForceMode.Impulse);
        //    //attackSphere.enabled = true;
        //}
    }
    public void HoldAttack(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            Debug.Log("Hol' up!" + inputContext.phase);
            sphereRigidbody.AddForce(Vector3.forward * 0.3f, ForceMode.Impulse);
            //attackSphere.enabled = true;
        }
        if (inputContext.canceled)
        {
            Debug.Log("Dont hol' up" + inputContext.phase);
            sphereRigidbody.AddForce(Vector3.forward * 5f, ForceMode.Impulse);
            //attackSphere.enabled = true;
        }
    }
    public void PickObject(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            Debug.Log("PickingObject");
            sphereRigidbody.AddForce(Vector3.forward * -0.3f, ForceMode.Impulse);
        }
    }
    public void UsePowerUp(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            Debug.Log("PoweringUp");
            sphereRigidbody.AddForce(Vector3.left * -0.3f, ForceMode.Impulse);
        }
    }
    public void ResetGame(InputAction.CallbackContext inputContext)
    {
        if (inputContext.performed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Move(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started)
        {
            Vector2 movementVector = inputContext.ReadValue<Vector2>();
            sphereRigidbody.AddForce(new Vector3(movementVector.x, 0, movementVector.y) * movementSpeed, ForceMode.Force);
        }
    }


}
