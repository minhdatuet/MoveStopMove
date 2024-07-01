using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTouchController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Chuyển đổi hướng 2D thành 3D
        Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y);

        // Di chuyển nhân vật
        controller.Move(move * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (context.phase == InputActionPhase.Performed)
        {
            // Nhận hướng di chuyển từ input
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Khi người chơi bỏ tay khỏi màn hình, dừng di chuyển
            moveDirection = Vector2.zero;
        }
    }
}
