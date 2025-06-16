using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAndReachTargetController : MonoBehaviour
{
    public Transform characterRoot; // Référence du torse ou de la racine
    public Transform target;        // L’objet à regarder/atteindre

    public MultiAimConstraint headAimConstraint;   // Pour la tête
    public TwoBoneIKConstraint armIKConstraint;    // Pour le bras

    [Header("Activation")]
    [Range(-1f, 1f)]
    public float dotThreshold = 0.3f;     // Si la target est devant (dot > seuil)
    public float smoothSpeed = 5f;        // Interpolation poids

    private float headWeight = 0f;
    private float armWeight = 0f;

    void Update()
    {
        Vector3 toTarget = (target.position - characterRoot.position).normalized;
        Vector3 forward = characterRoot.forward;

        float dot = Vector3.Dot(forward, toTarget);
        bool isTargetInFront = dot > dotThreshold;

        float targetWeight = isTargetInFront ? 1f : 0f;

        // Interpolation douce
        headWeight = Mathf.Lerp(headWeight, targetWeight, Time.deltaTime * smoothSpeed);
        armWeight = Mathf.Lerp(armWeight, targetWeight, Time.deltaTime * smoothSpeed);

        if (headAimConstraint != null)
            headAimConstraint.weight = headWeight;

        if (armIKConstraint != null)
            armIKConstraint.weight = armWeight;
    }
}
