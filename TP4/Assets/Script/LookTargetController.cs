using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookTargetController : MonoBehaviour
{
    public Transform characterRoot;         // Transform du personnage (souvent le GameObject parent ou torse)
    public Transform target;                // L'objet à regarder
    public MultiAimConstraint aimConstraint;
    public float dotThreshold = 0.3f;       // Plus tu montes ce seuil, plus la tête se limite au devant
    public float smoothSpeed = 5f;          // Vitesse d'interpolation du poids (plus grand = plus rapide)

    private float currentWeight = 0f;

    void Update()
    {
        Vector3 toTarget = (target.position - characterRoot.position).normalized;
        Vector3 forward = characterRoot.forward;

        float dot = Vector3.Dot(forward, toTarget);

        // Détermine le poids cible (0 ou 1)
        float targetWeight = dot > dotThreshold ? 1f : 0f;

        // Interpolation fluide vers le poids cible
        currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * smoothSpeed);

        aimConstraint.weight = currentWeight;
    }
}
