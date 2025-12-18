using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClearSky
{
    public class DemoCollegeStudentController : MonoBehaviour
    {
        [Header("Movement")]
        public float movePower = 10f;
        public float KickBoardMovePower = 15f;
        public float jumpPower = 20f;

        [Header("Input Control")]
        public bool useKeyboardInput = true;

        [Header("Clamp Position")]
        public bool useClampPosition = true;
        public float minX = -16.13f;
        public float maxX = 18.68787f;

        [Header("References")]
        public GameObject canvas;

        private Rigidbody2D rb;
        private Animator anim;

        private int direction = 1;
        private bool isJumping = false;
        private bool alive = true;
        private bool isKickboard = false;

        // -------- UI Movement --------
        private float uiMoveDirection = 0f; // -1 = left, 1 = right, 0 = stop

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            // Flip canvas with character
            if (canvas != null)
                canvas.transform.localScale = transform.localScale;

            Restart();

            if (!alive) return;

            if (useKeyboardInput)
            {
                Hurt();
                Die();
                Attack();
                Jump();
                KickBoard();
            }

            Run();
        }

        void LateUpdate()
        {
            ClampPosition();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
        }

        // ================= UI BUTTON METHODS =================

        public void MoveLeftDown()
        {
            uiMoveDirection = -1f;
        }

        public void MoveLeftUp()
        {
            uiMoveDirection = 0f;
        }

        public void MoveRightDown()
        {
            uiMoveDirection = 1f;
        }

        public void MoveRightUp()
        {
            uiMoveDirection = 0f;
        }

        // =====================================================

        void KickBoard()
        {
            if (!useKeyboardInput) return;

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                isKickboard = !isKickboard;
                anim.SetBool("isKickBoard", isKickboard);
            }
        }

        void Run()
        {
            float horizontal = 0f;

            if (useKeyboardInput)
                horizontal = Input.GetAxisRaw("Horizontal");

            if (horizontal == 0)
                horizontal = uiMoveDirection;

            if (!isKickboard)
            {
                Vector3 moveVelocity = Vector3.zero;
                anim.SetBool("isRun", false);

                if (horizontal < 0)
                {
                    direction = -1;
                    moveVelocity = Vector3.left;
                    transform.localScale = new Vector3(direction, 1, 1);

                    if (!anim.GetBool("isJump"))
                        anim.SetBool("isRun", true);
                }
                else if (horizontal > 0)
                {
                    direction = 1;
                    moveVelocity = Vector3.right;
                    transform.localScale = new Vector3(direction, 1, 1);

                    if (!anim.GetBool("isJump"))
                        anim.SetBool("isRun", true);
                }

                transform.position += moveVelocity * movePower * Time.deltaTime;
            }
            else
            {
                Vector3 moveVelocity = Vector3.zero;

                if (horizontal < 0)
                {
                    direction = -1;
                    moveVelocity = Vector3.left;
                    transform.localScale = new Vector3(direction, 1, 1);
                }
                else if (horizontal > 0)
                {
                    direction = 1;
                    moveVelocity = Vector3.right;
                    transform.localScale = new Vector3(direction, 1, 1);
                }

                transform.position += moveVelocity * KickBoardMovePower * Time.deltaTime;
            }
        }

        void Jump()
        {
            if (!useKeyboardInput) return;

            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
                && !anim.GetBool("isJump"))
            {
                isJumping = true;
                anim.SetBool("isJump", true);
            }

            if (!isJumping) return;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            isJumping = false;
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
            }
        }

        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");

                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }

        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isKickboard = false;
                anim.SetBool("isKickBoard", false);
                anim.SetTrigger("die");
                alive = false;
            }
        }

        void Restart()
        {
            if (!useKeyboardInput) return;

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                isKickboard = false;
                anim.SetBool("isKickBoard", false);
                anim.SetTrigger("idle");
                alive = true;
            }
        }

        // ================= CLAMP =================

        void ClampPosition()
        {
            if (!useClampPosition) return;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            transform.position = pos;
        }
    }
}
