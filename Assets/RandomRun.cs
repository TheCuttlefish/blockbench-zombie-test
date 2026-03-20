using UnityEngine;

public class RandomRun : MonoBehaviour
{
    public float speed = 2f;
    public float maxRotationAngle = 30f;
    public float maxDistanceFromCenter = 20f;
    public Vector3 centerPoint = Vector3.zero;

    public float turnSpeed = 120f;

    public float runDuration = 3f;
    public float waitDuration = 2f;

    public Animator anim;

    private float timer;
    private float currentInterval;
    private float stateTimer;
    private bool isWaiting;

    void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        SetNewInterval();

        // Randomize starting state + offset
        isWaiting = Random.value > 0.5f;
        stateTimer = Random.Range(0f, isWaiting ? waitDuration : runDuration);

        if (anim != null)
            anim.SetBool("isRunning", !isWaiting);
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        if (!isWaiting && stateTimer >= runDuration)
        {
            StartWait();
        }
        else if (isWaiting && stateTimer >= waitDuration)
        {
            StartRun();
        }

        if (isWaiting)
            return;

        Vector3 flatPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatCenter = new Vector3(centerPoint.x, 0f, centerPoint.z);

        float distanceFromCenter = Vector3.Distance(flatPosition, flatCenter);

        if (distanceFromCenter > maxDistanceFromCenter)
        {
            Vector3 directionToCenter = (flatCenter - flatPosition).normalized;

            if (directionToCenter != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-directionToCenter);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= currentInterval)
            {
                float randomAngle = Random.Range(-maxRotationAngle, maxRotationAngle);
                transform.Rotate(0f, randomAngle, 0f);

                timer = 0f;
                SetNewInterval();
            }
        }

        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    void StartRun()
    {
        isWaiting = false;
        stateTimer = 0f;

        if (anim != null)
            anim.SetBool("isRunning", true);
    }

    void StartWait()
    {
        isWaiting = true;
        stateTimer = 0f;

        if (anim != null)
            anim.SetBool("isRunning", false);
    }

    void SetNewInterval()
    {
        currentInterval = Random.Range(0f, 2f);
    }
}