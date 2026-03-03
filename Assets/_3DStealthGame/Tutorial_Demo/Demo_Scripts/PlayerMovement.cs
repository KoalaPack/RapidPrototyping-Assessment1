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
        public float sprintSpeed = 3.0f;
        public float turnSpeed = 80f;

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

            float turnInput = moveInput.x;     // A / D
            float moveInputY = moveInput.y;    // W / S

            // Sprint logic
            float sprintValue = SprintAction.ReadValue<float>();
            bool sprintHeld = sprintValue > 0.5f;
            bool isMoving = Mathf.Abs(moveInputY) > 0.01f;
            bool isSprinting = sprintHeld && isMoving;

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            // --- ROTATION (A/D) ---
            if (Mathf.Abs(turnInput) > 0.01f)
            {
                float turnAmount = turnInput * turnSpeed * Time.deltaTime;
                Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
            }

            // --- MOVEMENT (W/S) ---
            Vector3 forwardMove = transform.forward * moveInputY * currentSpeed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + forwardMove);

            // --- ANIMATION ---
            m_Animator.SetBool("IsWalking", isMoving);

            // --- AUDIO ---
            if (isMoving)
            {
                if (!m_AudioSource.isPlaying)
                    m_AudioSource.Play();
            }
            else
            {
                m_AudioSource.Stop();
            }
        }
    }
}