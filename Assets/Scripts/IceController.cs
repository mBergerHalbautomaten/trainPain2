using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class IceController : MonoBehaviour
{
    public delegate void VerspaedungsEventHaendler(int verspaedungInSegunda);
    public static event VerspaedungsEventHaendler OnVerspaedungKassiert;
    
    private bool isMoving = false;
    [FormerlySerializedAs("speedModifier")] [SerializeField]
    private float speed = 1;

    private float jumpDistance = 5;
    private Animator animator;
    private TrainTrackPosition trackPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        trackPosition = TrainTrackPosition.Center;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        DriveForward();
    }

    void DriveForward()
    {
        transform.Translate(Vector3.forward * speed, Space.World);
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Steer left
            SteeringDirection direction = SteeringDirection.Left;
            InitiateMovement(direction);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Steer right
            SteeringDirection direction = SteeringDirection.Right;
            InitiateMovement(direction);
        }
    }

    private void InitiateMovement(SteeringDirection direction)
    {
        TrainTrackPosition newPosition = GetNextTrainTrackPosition(direction);
        float jumpXPosition = GetJumpTargetXPosition(newPosition);
        MoveTo(jumpXPosition);
        trackPosition = newPosition;
    }

    private TrainTrackPosition GetNextTrainTrackPosition(SteeringDirection direction)
    {
        switch (trackPosition)
        {
            case TrainTrackPosition.Center:
                switch (direction)
                {
                    case SteeringDirection.Left:
                        //Jump to left track
                        return TrainTrackPosition.Left;
                        break;
                    case SteeringDirection.Right:
                        //Jump to right track
                        return TrainTrackPosition.Right;
                    default:
                        return trackPosition;
                }
            case TrainTrackPosition.Left:
                switch (direction)
                {
                    case SteeringDirection.Left:
                        //Do nothing, stupid input
                        return TrainTrackPosition.Left;
                    case SteeringDirection.Right:
                        //Jump to center
                        return TrainTrackPosition.Center;
                    default:
                        return trackPosition;
                }
            case TrainTrackPosition.Right:
                switch (direction)
                {
                    case SteeringDirection.Left:
                        //Jump to center
                        return TrainTrackPosition.Center;
                    case SteeringDirection.Right:
                        //Do nothing, stupid input
                        return TrainTrackPosition.Right;
                    default:
                        return trackPosition;
                }
                break;
            default:
                return trackPosition;
        }
    }

    private float GetJumpTargetXPosition(TrainTrackPosition trackPosition)
    {
        switch (trackPosition)
        {
            case TrainTrackPosition.Left:
                return -jumpDistance;
                break;
            case TrainTrackPosition.Center:
                return 0;
                break;
            case TrainTrackPosition.Right:
                return jumpDistance;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(trackPosition), trackPosition, null);
        }
    }

    private void MoveTo(float targetXPosition)
    {
        if (isMoving == true) return;
        StartCoroutine(MoveRoutine(targetXPosition));
    }

    private IEnumerator MoveRoutine(float targetX)
    {
        isMoving = true;
        if(animator) animator.SetTrigger("jump");
        Vector3 startPosition = transform.position;
        float t = 0;
        while (t < 1)
        {
            float y = 0;
            if (t <= 0.5f)
            {
                y = Mathf.Lerp(0.5f, 2f, t * 2);
            }
            else
            {
                y = Mathf.Lerp(2f, 0.5f, (t - 0.5f) * 2);
            }
            t += Time.fixedDeltaTime / 0.5f;
            float newXPosition = Mathf.Lerp(startPosition.x, targetX, t);
            transform.position = new Vector3(newXPosition, y, transform.position.z);
            yield return null;
        }
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            //Wir haben ein defektes Signal detektiert. Das kostet ja mal olli dolli Verspätung, Junge.
            //Anschlusszug kannsch voll kniggn
            Debug.Log($"AUTSCHI BAUTSCHI");
            animator.SetTrigger("explode");
            OnVerspaedungKassiert?.Invoke(10);
        }
    }
}
