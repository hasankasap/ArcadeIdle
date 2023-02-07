using UnityEngine;

namespace Game
{
    public interface ICharacter
    {
        public void takeAsset(GameObject asset);
        public bool canTakeAsset();
        public GameObject dropAsset();
        public bool canDropAsset();
        public float speed { get; set; }
        public float sensitivity { get; set; }
        public float rotationSpeed { get; set; }
        public float acceleration { get; set; }
    }
}