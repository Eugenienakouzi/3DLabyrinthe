using Unity.AI.Navigation;
using UnityEngine;

public class Grille_up : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject grille;      // Le mur à désactiver
    private bool joueurFin = false;

    public NavMeshSurface navMeshSurface; // Surface à reconstruire


    void Start()
    {
        grille.SetActive(false); // Assure que la grille est désactivée au début
    }

    void Update()
    {
        if (joueurFin)
        {
            grille.SetActive(true);

        }
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh(); // Mise à jour ici
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assure-toi que le joueur a bien le tag "Player"
            joueurFin = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            joueurFin = false;
    }
}
