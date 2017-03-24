using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    [Tooltip("Target to follow")]
    private Transform targetActive;
    [SerializeField]
    [Tooltip("Target to follow")]
    private Transform targetIdle;
    private Transform targetCurrent;

    [SerializeField]
    private float damping = 1;
    [SerializeField]
    private float lookAheadFactor = 3;
    [SerializeField]
    private float lookAheadReturnSpeed = 0.5f;
    [SerializeField]
    private float lookAheadMoveThreshold = 0.1f;
    [SerializeField]
    private float yPosRestriction = Mathf.Infinity;
    [SerializeField]
    private float xPosRestriction = Mathf.Infinity;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    // How often try to find player if target is null
    private float nextTimeToSearch = 0;

    private void Start()
    {
        targetCurrent = targetIdle;

        m_LastTargetPosition = targetCurrent.position;
        m_OffsetZ = (transform.position - targetCurrent.position).z;
        transform.parent = null;
    }

    private void Update()
    {
        if(GameManager.gamePlaying == true)
        {
            targetCurrent = targetActive;
        } else
        {
            targetCurrent = targetIdle;
        }

        if (targetCurrent == null)
        {
            FindPlayer();
            return;
        }

        // Only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (targetCurrent.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        // FIX: Looking ahead not working in Y direction. Also should be based on where ship is pointing, not
        // where ship is traveling.
        Vector3 aheadTargetPos = targetCurrent.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        // Clamp camera position between given x and y limits
        newPos = new Vector3(Mathf.Clamp(newPos.x, -xPosRestriction, xPosRestriction),
                             Mathf.Clamp(newPos.y, -yPosRestriction, yPosRestriction),
                             newPos.z);

        transform.position = newPos;

        m_LastTargetPosition = targetCurrent.position;
    }

    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
            {
                targetCurrent = searchResult.transform;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}
