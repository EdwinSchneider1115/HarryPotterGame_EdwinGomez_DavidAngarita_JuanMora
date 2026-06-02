using UnityEngine;

public enum TipoPowerUp { Velocidad, Invisibilidad, Vida }

public class PowerUp : MonoBehaviour
{
    [Header("Tipo de PowerUp")]
    public TipoPowerUp tipo;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Jugador")) return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats == null) return;

        stats.RecogerPowerUp(tipo);
        Destroy(gameObject);
    }
}
