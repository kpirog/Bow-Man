using System;
using Elympics;

public class DestroyHandler : ElympicsMonoBehaviour
{
    public ElympicsBool destroyed = new();

    private void Awake()
    {
        destroyed.ValueChanged += OnDestroyed;
    }

    private void OnDestroy()
    {
        destroyed.ValueChanged -= OnDestroyed;
    }

    private void OnDestroyed(bool oldValue, bool newValue)
    {
        Destroy(gameObject);
    }
}
