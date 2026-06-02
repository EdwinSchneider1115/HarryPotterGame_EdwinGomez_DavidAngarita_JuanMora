using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [Header("Comportamiento")]
    public float tiempoVida = 5f;
    public float gravedad = 2f;

    private Rigidbody rb;
    private Vector3 velocidadActual;
    private bool lanzado = false;
    private GameObject enemigo;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;
    }

    public void Lanzar(Vector3 direccion, float fuerza, GameObject quienDisparo = null)
    {
        velocidadActual = direccion * fuerza;
        lanzado = true;
        enemigo = quienDisparo;

        // Ignorar colisión con el enemigo que disparó
        if (enemigo != null)
        {
            Collider[] collidersEnemigo = enemigo.GetComponentsInChildren<Collider>();
            Collider miCollider = GetComponent<Collider>();
            if (miCollider != null)
            {
                foreach (Collider c in collidersEnemigo)
                {
                    Physics.IgnoreCollision(miCollider, c);
                }
            }
        }

        Destroy(gameObject, tiempoVida);
    }

    void FixedUpdate()
    {
        if (!lanzado || rb == null) return;

        velocidadActual += Vector3.down * gravedad * Time.fixedDeltaTime;
        rb.linearVelocity = velocidadActual;

        if (velocidadActual != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(velocidadActual) * Quaternion.Euler(90f, 180f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil")) return;

        if (enemigo != null && other.transform.IsChildOf(enemigo.transform)) return;

        if (other.CompareTag("Jugador"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null) stats.RecibirDanio(1);
        }

        Destroy(gameObject);
    }
}