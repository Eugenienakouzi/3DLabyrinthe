using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class ExplosionTrigger : MonoBehaviour
{
    public ParticleSystem explosionFX;
    public ParticleSystem smokeFX;
    public ParticleSystem gasFX;

    public Rig targetRig;
    public float transitionDuration = 1f; // Durée de la transition en secondes

    private Coroutine rigTransition;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explosionFX.Play();
            smokeFX.Play();
            gasFX.Play();

            if (targetRig != null)
            {
                // Lance une transition douce vers weight = 1
                if (rigTransition != null)
                    StopCoroutine(rigTransition);

                rigTransition = StartCoroutine(ChangeRigWeight(targetRig, 1f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetRig != null)
            {
                // Transition douce vers weight = 0
                if (rigTransition != null)
                    StopCoroutine(rigTransition);

                rigTransition = StartCoroutine(ChangeRigWeight(targetRig, 0f));
            }
        }
    }

    IEnumerator ChangeRigWeight(Rig rig, float targetWeight)
    {
        float startWeight = rig.weight;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            rig.weight = Mathf.Lerp(startWeight, targetWeight, t);
            yield return null;
        }

        rig.weight = targetWeight; // Assure le poids final exact
    }
}
