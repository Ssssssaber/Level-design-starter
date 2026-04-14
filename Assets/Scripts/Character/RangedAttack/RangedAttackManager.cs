using UnityEngine;

namespace RangedAttack
{
    public class RangedAttackManager : MonoBehaviour
    {
        [SerializeField] private GameObject _projectileReference;
        [SerializeField] private Transform _projectlieLaunchOrigin;
        private Projectile _currentProjectlie;
        public void LaunchProjectlie(Vector3 target)
        {
            _currentProjectlie = Instantiate(_projectileReference, _projectlieLaunchOrigin.position, Quaternion.identity).GetComponent<Projectile>();
            GameManager.Instance.MoveObjectToEnvironment(_currentProjectlie.gameObject);
            _currentProjectlie.Launch(CalculateDirection(target));
        }

        private Vector3 CalculateDirection(Vector3 target)
        {
            return (target - _projectlieLaunchOrigin.position).normalized;
        }
    }
}