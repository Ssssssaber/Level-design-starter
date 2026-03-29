using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FinishGameArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.GameWon?.Invoke();
    } 
}