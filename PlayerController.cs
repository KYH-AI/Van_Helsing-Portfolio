using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����


public class PlayerController : MonoBehaviour
{
    private Player playerObject;
    private Vector2 moveVector;
    private Animator playerAnimator;
    [SerializeField] private FloatingJoystick joystick;

    // Start is called before the first frame update
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerObject = GetComponent<Player>();
    }

    private void Update()
    {
        UserTouchInput();
    }
    /// <summary>
    /// ����� �Է� �������� X, Y�� �̵�
    /// </summary>
    private void UserTouchInput()
    {
        moveVector.x = joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
        moveVector.y = joystick.Vertical;//Input.GetAxisRaw("Vertical");

        moveVector.Normalize();
    
       CheckAnimation(playerObject.Move(moveVector));  //bool return ���� �޾� �ִϸ��̼� �۵�

    }

    /// <summary>
    /// ���� ĳ������ �̵� ������ Ȯ���� Run �Ǵ� Idle �ִϸ��̼� ����
    /// </summary>
    /// <param name="isMove">ĳ������ velocity ���� Ȯ��</param>
    private void CheckAnimation(bool isMove)
    {
        if (isMove) // �����̸� Run �ִϸ��̼� �۵�
        {
            playerAnimator.Play("Run");
 
            if (moveVector.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(moveVector.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if(!isMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ) // �ƴϸ� Idle �ִϸ��̼� �۵�
        {
            playerAnimator.Play("Idle");
        }
    }
}
