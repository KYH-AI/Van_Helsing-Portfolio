using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원


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
    /// 사용자 입력 바탕으로 X, Y축 이동
    /// </summary>
    private void UserTouchInput()
    {
        moveVector.x = joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
        moveVector.y = joystick.Vertical;//Input.GetAxisRaw("Vertical");

        moveVector.Normalize();
    
       CheckAnimation(playerObject.Move(moveVector));  //bool return 값을 받아 애니메이션 작동

    }

    /// <summary>
    /// 현재 캐릭터의 이동 유무를 확인해 Run 또는 Idle 애니메이션 진행
    /// </summary>
    /// <param name="isMove">캐릭터의 velocity 값을 확인</param>
    private void CheckAnimation(bool isMove)
    {
        if (isMove) // 움직이면 Run 애니메이션 작동
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
        else if(!isMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ) // 아니면 Idle 애니메이션 작동
        {
            playerAnimator.Play("Idle");
        }
    }
}
