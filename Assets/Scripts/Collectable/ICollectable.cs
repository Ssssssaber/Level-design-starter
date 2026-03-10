using UnityEngine;

namespace Collectable
{
    public interface ICollectable
    {
        public void Collect(GameObject collector);
        public bool CanCollect(GameObject collector);
    }
}

