using UnityEngine;


public class Move : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Camera Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float followOffset = 2f;

    [Header("Camera Limits")]
    [SerializeField] private bool useClamp = true;
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 36f;

    [Header("Input")]
    public bool useKeyboardInput = true;

    private float uiMoveDirection = 0f; // -1 left, 1 right
    [Header("Dialogue Box")]

    public GameObject dialogueBox;
    public bool followPlayer;

    void Start()
    {
        dialogueBox.SetActive(false);
    }
    void LateUpdate()
    {



        if (followPlayer)
        {

            HandleCameraFollow();
            ClampCamera();
        }
        else { return; }
    }

    void Update()
    {
        if (gameObject.transform.position.x >= 31.18045f)
        {
            followPlayer = false;

            dialogueBox.SetActive(true);
        }
    }


    void HandleCameraFollow()
    {
        float playerX = player.position.x;
        float cameraX = transform.position.x;

        float delta = playerX - cameraX;

        // Move camera only if player crosses offset
        if (Mathf.Abs(delta) > followOffset)
        {
            float moveDir = Mathf.Sign(delta);
            transform.Translate(Vector3.right * moveDir * speed * Time.deltaTime);
        }

        // Keyboard / UI manual camera move
        ManualCameraMove();
    }

    // ================= MANUAL MOVE (KEYBOARD + UI) =================

    void ManualCameraMove()
    {
        float input = 0f;

        if (useKeyboardInput)
        {
            if (Input.GetKey(KeyCode.A)) input = -1f;
            if (Input.GetKey(KeyCode.D)) input = 1f;
        }

        if (input == 0)
            input = uiMoveDirection;

        transform.Translate(Vector3.right * input * speed * Time.deltaTime);
    }

    // ================= UI BUTTON METHODS =================

    public void MoveCameraLeftDown()
    {
        uiMoveDirection = -1f;
    }

    public void MoveCameraLeftUp()
    {
        uiMoveDirection = 0f;
    }

    public void MoveCameraRightDown()
    {
        uiMoveDirection = 1f;
    }

    public void MoveCameraRightUp()
    {
        uiMoveDirection = 0f;
    }

    // ================= CLAMP =================

    void ClampCamera()
    {
        if (!useClamp) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
