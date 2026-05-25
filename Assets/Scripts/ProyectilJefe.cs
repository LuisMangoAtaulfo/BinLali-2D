using UnityEngine;

public class ProyectilJefe : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            VidaJugadora vida = col.GetComponent<VidaJugadora>();
            if (vida != null) vida.RecibirDanio(1);
            Destroy(gameObject);
        }
    }
}