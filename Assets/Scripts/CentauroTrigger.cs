using UnityEngine;

public class CentauroTrigger : MonoBehaviour
{
    public enum TipoTrigger { Activar, Desactivar }

    [Header("Configuracion")]
    public TipoTrigger tipo;
    public CentauroSeguidor[] centauros;
    public bool unaVez = true;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Jugador")) return;
        if (unaVez && activado) return;

        activado = true;

        foreach (CentauroSeguidor centauro in centauros)
        {
            if (centauro == null) continue;

            if (tipo == TipoTrigger.Activar)
                centauro.ActivarPersecucion();
            else
                centauro.DesactivarPersecucion();
        }
    }
}
