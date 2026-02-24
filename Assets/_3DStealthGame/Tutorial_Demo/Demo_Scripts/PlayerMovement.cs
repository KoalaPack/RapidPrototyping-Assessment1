using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StealthGame
{
    public class PlayerMovement : MonoBehaviour
    {
        public InputAction MoveAction;
        public InputAction JumpAction;
        public InputAction SprintAction;

        public float walkSpeed = 2.0f;
        public float sprintSpeed = 5.0f;
        public float turnSpeed = 20f;

        Animator m_Animator;
        Rigidbody m_Rigidbody;
        AudioSource m_AudioSource;
        Vector3 m_Movement;
        Quaternion m_Rotation = Quaternion.identity;

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_AudioSource = GetComponent<AudioSource>();

            MoveAction.Enable();
            JumpAction.Enable();
            SprintAction.Enable();
        }
        private List<string> m_OwnedKeys = new();

        public void AddKey(string keyName)
        {
            m_OwnedKeys.Add(keyName);
            Debug.Log($"Picked up key: {keyName}");
        }

        public bool OwnKey(string keyName)
        {
            return m_OwnedKeys.Contains(keyName);
        }
        void FixedUpdate()
        {
            Vector2 moveInput = MoveAction.ReadValue<Vector2>();

            float horizontal = moveInput.x;
            float vertical = moveInput.y;

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool isMoving = m_Movement.sqrMagnitude > 0.01f;

            // Read sprint as a float so we can see what's actually happening
            float sprintValue = SprintAction.ReadValue<float>();

            bool sprintHeld = sprintValue > 0.5f;   // manual threshold
            bool isSprinting = sprintHeld && isMoving;

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Debug.Log($"Move: {moveInput} | SprintValue: {sprintValue} | SprintHeld: {sprintHeld} | Speed: {currentSpeed}");

            // Walk animation only
            m_Animator.SetBool("IsWalking", isMoving);

            if (isMoving)
            {
                if (!m_AudioSource.isPlaying)
                    m_AudioSource.Play();
            }
            else
            {
                m_AudioSource.Stop();
            }

            Vector3 desiredForward = Vector3.RotateTowards(
                transform.forward,
                m_Movement,
                turnSpeed * Time.deltaTime,
                0f
            );

            m_Rotation = Quaternion.LookRotation(desiredForward);

            m_Rigidbody.MoveRotation(m_Rotation);
            m_Rigidbody.MovePosition(
                m_Rigidbody.position + m_Movement * currentSpeed * Time.deltaTime
            );
        }
    }
}