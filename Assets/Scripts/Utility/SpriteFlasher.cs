using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlasher : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private float _fadeDuration = 0.3f;

    [Header("Events")]
    public UnityEvent OnFlashRequested;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _flashRoutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void OnEnable()
    {
        OnFlashRequested.AddListener(StartFlash);
    }

    private void OnDisable()
    {
        OnFlashRequested.RemoveListener(StartFlash);
    }

    public void StartFlash()
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }
        _flashRoutine = StartCoroutine(FlashRoutine(_flashColor, _flashDuration, _fadeDuration));
    }

    private IEnumerator FlashRoutine(Color flashColor, float flashDuration, float fadeDuration)
    {
        _spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            _spriteRenderer.color = Color.Lerp(flashColor, _originalColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = _originalColor;
    }
}
