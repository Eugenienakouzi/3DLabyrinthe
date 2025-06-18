using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmAimTriggerController : MonoBehaviour
{
    [Header("Références")]
    public MultiAimConstraint leftArmAim;
    public MultiAimConstraint rightArmAim;
    public Transform aimTarget; // L'objet vers lequel les bras doivent pointer

    [Header("Interpolation")]
    public float smoothSpeed = 5f;

    private float leftWeight = 0f;
    private float rightWeight = 0f;

    private bool isInZone = false;

    void Update()
    {
        float targetWeight = isInZone ? 1f : 0f;

        leftWeight = Mathf.Lerp(leftWeight, targetWeight, Time.deltaTime * smoothSpeed);
        rightWeight = Mathf.Lerp(rightWeight, targetWeight, Time.deltaTime * smoothSpeed);

        if (leftArmAim != null)
            leftArmAim.weight = leftWeight;

        if (rightArmAim != null)
            rightArmAim.weight = rightWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optionnel : filtre avec un tag, ou autre logique
        isInZone = true;

        // Assure que la cible est bien assignée à la contrainte
        SetTargetIfNeeded();
    }

    private void OnTriggerExit(Collider other)
    {
        isInZone = false;
    }

    private void SetTargetIfNeeded()
    {
        if (aimTarget == null) return;

        // Applique la cible à chaque contrainte si ce n'est pas déjà fait
        //if (leftArmAim != null && leftArmAim.data.sourceObject != aimTarget)
        //{
        //    var sources = leftArmAim.data.sourceObjects;
        //    sources.SetTransform(0, aimTarget);
        //    leftArmAim.data.sourceObjects = sources;
        //}

        //if (rightArmAim != null && rightArmAim.data.sourceObject != aimTarget)
        //{
        //    var sources = rightArmAim.data.sourceObjects;
        //    sources.SetTransform(0, aimTarget);
        //    rightArmAim.data.sourceObjects = sources;
        //}
    }
}
