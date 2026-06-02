using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Vidas")]
    public int vidasMaximas = 5;
    private int vidasActuales;

    [Header("Inventario")]
    private int cantVelocidad = 0;
    private int cantInvisibilidad = 0;
    private int cantVida = 0;

    private PowerUpManager powerUpManager;
    private Movimiento movimiento;

    void Start()
    {
        vidasActuales = vidasMaximas;
        powerUpManager = GetComponent<PowerUpManager>();
        movimiento = GetComponent<Movimiento>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UsarVida();
        if (Input.GetKeyDown(KeyCode.Alpha2)) UsarVelocidad();
        if (Input.GetKeyDown(KeyCode.Alpha3)) UsarInvisibilidad();
    }

    // ───── VIDAS ─────

    public void RecibirDanio(int cantidad = 1)
    {
        vidasActuales -= cantidad;
        Debug.Log($"Vidas restantes: {vidasActuales}/{vidasMaximas}");
        if (vidasActuales <= 0)
        {
            vidasActuales = 0;
            Morir();
        }
    }

    public void CurarVida(int cantidad = 1)
    {
        vidasActuales = Mathf.Min(vidasActuales + cantidad, vidasMaximas);
    }

    private void Morir()
    {
        Debug.Log("Jugador muerto");
        movimiento.Respawn();
        vidasActuales = vidasMaximas;
    }

    // ───── RECOGER POWERUPS ─────

    public void RecogerPowerUp(TipoPowerUp tipo)
    {
        switch (tipo)
        {
            case TipoPowerUp.Velocidad: cantVelocidad++; break;
            case TipoPowerUp.Invisibilidad: cantInvisibilidad++; break;
            case TipoPowerUp.Vida: cantVida++; break;
        }
        Debug.Log($"PowerUp recogido: {tipo}");
    }

    // ───── USAR POWERUPS ─────

    private void UsarVelocidad()
    {
        if (cantVelocidad <= 0) return;
        cantVelocidad--;
        powerUpManager.ActivarVelocidad();
    }

    private void UsarInvisibilidad()
    {
        if (cantInvisibilidad <= 0) return;
        cantInvisibilidad--;
        powerUpManager.ActivarInvisibilidad();
    }

    private void UsarVida()
    {
        if (cantVida <= 0) return;
        cantVida--;
        CurarVida(1);
    }

    // ───── GETTERS ─────

    public int GetVidas() => vidasActuales;
    public int GetVidasMaximas() => vidasMaximas;
    public int GetCantVelocidad() => cantVelocidad;
    public int GetCantInvisibilidad() => cantInvisibilidad;
    public int GetCantVida() => cantVida;
}