using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Jugador"))
        {
            Movimiento player = other.GetComponent<Movimiento>();
            player.SetCheckpoint(transform.position);

            activated = true;

            CheckpointUI.instancia.Mostrar("Checkpoint guardado");
        }
    }
}