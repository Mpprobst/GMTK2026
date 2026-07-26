using UnityEngine;

public class Mirage : Tile
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject model;
    bool triggered = false;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (triggered) return;
            triggered = true;
            base.OnTriggerEnter(other);
            particles.Play();
            Invoke("Hide", 0.1f);
        }
    }

    private void Hide()
    {
        model.SetActive(false);
    }
}
