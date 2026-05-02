using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;

    const float BaseStepDistance = 0.35f;
    const float SpeedInfluence = 0.15f;

    Transform runner;
    bool isMoving;
    public bool hasArrive;

    float totalSteps;
    int runnerIndex = -1;

    public float Steps => totalSteps;
    public int Index => runnerIndex;

    void Update()
    {
        if (!isMoving || runner == null || target == null) return;

        float moveDistance = speed * Time.deltaTime;
        totalSteps += moveDistance / (BaseStepDistance * (1f + speed * SpeedInfluence));

        Vector3 targetPos = new Vector3(target.position.x, runner.position.y, target.position.z);
        runner.position = Vector3.MoveTowards(runner.position, targetPos, moveDistance);   

        if (runner.position == targetPos)
        {
            isMoving = false;
            hasArrive = true;
        }
    }

    public void Move(Transform runnerTransform, Transform newTarget, float newSpeed)
    {
        runner = runnerTransform;
        target = newTarget;
        speed = newSpeed;
        isMoving = true;
        hasArrive = false;
    }

    public void Init(int index)
    {
        runnerIndex = index;
        totalSteps = 0;
    }
}