using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Movimiento player = other.GetComponentInParent<Movimiento>();

        if (player != null)
        {
            player.Respawn();
        }
    }
}