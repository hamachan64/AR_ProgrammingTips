using UnityEngine;

public class DisableOnAutoPlay : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private CardManager cardManager;

    private bool _lastCardManagerState;

    void Start()
    {
        if (cardManager == null)
            cardManager = FindObjectOfType<CardManager>();

        if (targetObject == null) return;

        _lastCardManagerState = cardManager != null && cardManager.enabled;
        UpdateTargetVisibility();
    }

    void Update()
    {
        if (cardManager == null || targetObject == null) return;

        bool currentState = cardManager.enabled;
        if (currentState != _lastCardManagerState)
        {
            _lastCardManagerState = currentState;
            UpdateTargetVisibility();
        }
    }

    private void UpdateTargetVisibility()
    {
        if (targetObject == null) return;

        // CardManager disabled -> show target
        // CardManager enabled -> hide target
        targetObject.SetActive(!_lastCardManagerState);
    }
}