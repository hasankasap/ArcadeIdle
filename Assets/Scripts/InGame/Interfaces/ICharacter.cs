using UnityEngine;

namespace Game
{
    public interface ICharacter
    {
        public void takeAsset(GameObject asset);
        public bool canTakeAsset();
        public GameObject dropAsset();
        public bool canDropAsset();
        public void movement();
    }
}