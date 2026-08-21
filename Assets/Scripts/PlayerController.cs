using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private GlobalData globalData;
    private CharacterController charController;
    [SerializeField] Animator anim;
    [SerializeField] float movementSpeed = 12f;
    [SerializeField] float rotationSpeed = 1f;
    private Vector3 movePos;
    private Vector3 rawMovePos;
    [Header("Read-Only")]
    [SerializeField] bool allowInput = true;
    void Start() 
    {
        globalData = GlobalData.instance;
        charController = gameObject.GetComponent<CharacterController>();
    }
    void Update() 
    {
        if (allowInput == false)
            return;

        PlayerMovePos();
        PlayerRotate();
        PlayerMove();
    }
    private void PlayerMove()
    {
        charController.Move(movePos * movementSpeed * Time.deltaTime);
        anim.SetFloat("Move", movePos == Vector3.zero? 0 : 1f, 0.1f, Time.deltaTime);
    }
    public void PlayAnim(string animName, int layer, float fadeDuration = 0.25f)
    {
        anim.CrossFadeInFixedTime(animName, fadeDuration, layer);
    }
    private void PlayerRotate()
    {
        if (movePos == Vector3.zero)
            return;
        Quaternion targetRotation = Quaternion.LookRotation(movePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void PlayerMovePos()
    {
        movePos = Vector3.zero;
        rawMovePos = Vector3.zero;

        if(Input.GetKey(globalData.KeyBindsClass.PlayerMoveForwards))
            rawMovePos.z = 1f;
        if(Input.GetKey(globalData.KeyBindsClass.PlayerMoveBackwards))
            rawMovePos.z = -1f;
        if(Input.GetKey(globalData.KeyBindsClass.PlayerMoveLeft))
            rawMovePos.x = -1f;
        if(Input.GetKey(globalData.KeyBindsClass.PlayerMoveRight))
            rawMovePos.x = 1f;

        if(rawMovePos == Vector3.zero)
            return;

        movePos = rawMovePos.normalized;
    }
    public void SetAllowInput(bool value)
    {
        allowInput = value;
        if (allowInput == false)
        {
            anim.SetFloat("Move", 0f);
        }
    }
}