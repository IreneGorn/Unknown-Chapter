using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private float _speedWalk = 4f;
    [SerializeField] private float _speedSprint = 12f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 5f; // Adjust this to control camera zoom speed
    [SerializeField] private float _orthographicSizeMin = 2f;
    [SerializeField] private float _orthographicSizeMax = 15f;

    private float _moveSpeed = 0f;
    private Animator _characterAnimator;
    
    private float Velocity = 0f;
    private int _velocityHash;
    private float velocityTransitionTime = 0.5f;
    private float velocityTransitionTimer = 0f;
    

    private Transform characterTransform;
    private Transform cameraTransform;
    private bool isMouseWheelDown = false;
    private bool isSprinting = false;
    
    
    
    private float _originalOrthographicSize;
    
    
    
    [SerializeField] private float _zoomOffset = 1.0f;
    

    private void Start()
    {
        characterTransform = transform;
        cameraTransform = Camera.main.transform;
        _characterAnimator = GetComponent<Animator>();
        // _velocityHash = _characterAnimator.StringToHash("Velocity");
        _velocityHash = Animator.StringToHash("Velocity");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Velocity <= 0.5)
            {
                Velocity += Time.deltaTime * 1.5f;
            }
            Move();
            _characterAnimator.SetFloat(_velocityHash, Velocity);
        }
        else
        {
            if (Velocity >= 0)
            {
                Velocity -= Time.deltaTime;
            }
            _characterAnimator.SetFloat(_velocityHash, Velocity);
        }
        
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Velocity >= 0.5f)
        {
            // isSprinting = true;
            // _characterAnimator.SetBool("IsSprint", true);
            if (Velocity <= 1f)
            {
                Velocity += Time.deltaTime / 2f;
                isSprinting = true;
            }
        }
        else
        {
            // _characterAnimator.SetBool("IsSprint", false);
            if (Velocity >= 0.5f)
            {
                Velocity -= Time.deltaTime / 2f;
                isSprinting = false;

            }

        }

        // Check if the mouse wheel is pressed down or released
        if (Input.GetMouseButtonDown(2)) // Mouse wheel button
        {
            isMouseWheelDown = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isMouseWheelDown = false;
        }

        if (isMouseWheelDown)
        {
            float rotationInput = Input.GetAxis("Mouse X");
            RotateCamera(rotationInput);
        }

        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(zoomInput);

 
    }

    private void Move()
    {
        if (isSprinting)
        {
            _moveSpeed = _speedSprint;
        }
        else
        {
            _moveSpeed = _speedWalk;
        }
        Vector3 forwardDirection = cameraTransform.forward;
        forwardDirection.y = 0;
        forwardDirection = Vector3.Normalize(forwardDirection);
        Vector3 rightDirection = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardDirection;

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 rightMovement = rightDirection * _moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forwardDirection * _moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        characterTransform.forward = heading;
        characterTransform.position += rightMovement;
        characterTransform.position += upMovement;
        
        cameraTransform.position = characterTransform.position;
        
        
    }

    private void RotateCamera(float rotationInput)
    {
        if (Mathf.Abs(rotationInput) > 0.0f)
        {
            cameraTransform.RotateAround(characterTransform.position, Vector3.up, rotationInput * _rotationSpeed);
        }
    }

    private void ZoomCamera(float zoomInput)
    {
        Camera.main.orthographicSize -= zoomInput * _zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _orthographicSizeMin, _orthographicSizeMax);
    }
    
    
    // private void CheckObstacleBehindCamera()
    // {
    //     Vector3 cameraPosition = cameraTransform.position;
    //     Vector3 characterPosition = characterTransform.position;
    //     Vector3 cameraDirection = cameraTransform.forward;
    //
    //     // Определяем позицию передней стенки камеры
    //     Vector3 cameraFrontPosition = cameraPosition + cameraDirection * Camera.main.nearClipPlane;
    //
    //     Vector3 directionToCharacter = characterPosition - cameraFrontPosition;
    //     float distanceToCharacter = directionToCharacter.magnitude;
    //
    //     // Инвертируем направление луча
    //     Vector3 oppositeDirection = -directionToCharacter;
    //
    //     RaycastHit[] hits = Physics.RaycastAll(characterPosition, oppositeDirection, distanceToCharacter);
    //
    //     RaycastHit closestHit = new RaycastHit();
    //     float closestHitDistance = float.MaxValue;
    //
    //     foreach (RaycastHit hit in hits)
    //     {
    //         if (hit.collider.CompareTag("Finish")) // Замените на актуальный тег препятствия
    //         {
    //             if (hit.distance < closestHitDistance)
    //             {
    //                 closestHit = hit;
    //                 closestHitDistance = hit.distance;
    //             }
    //         }
    //     }
    //
    //     // Если было найдено препятствие, меняем зум
    //     if (closestHit.collider != null)
    //     {
    //         float newOrthographicSize = closestHit.distance + _zoomOffset;
    //         Camera.main.orthographicSize = Mathf.Clamp(newOrthographicSize, _orthographicSizeMin, _orthographicSizeMax);
    //     }
    //     // Если рэйкаст больше не попадает в объекты, восстанавливаем исходный зум
    //     else
    //     {
    //         Camera.main.orthographicSize = _originalOrthographicSize;
    //     }
    //
    //     // Рисуем луч для отладки
    //     Debug.DrawRay(characterPosition, oppositeDirection * distanceToCharacter, Color.green);
    // }

    
}
