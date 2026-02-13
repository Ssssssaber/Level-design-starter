using UnityEngine;

public class TriggerProxy : MonoBehaviour {
    public TriggerZoneType zoneType;
    private StateMachine _parentAI;

    void Awake()
    {
        _parentAI = GetComponentInParent<StateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _parentAI.NotifyZoneEnter(zoneType, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _parentAI.NotifyZoneExit(zoneType, other);
    }
}
