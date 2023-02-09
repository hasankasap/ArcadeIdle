using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public interface ICharacter
    {
        public void takeProducts(Product asset);
        public bool canTakeProducts();
        public Product dropProductsWithType(ProductTypes type);
        public bool canDropWantedProductTypes(ProductTypes type);
        public Product dropToTrash();
        public bool canDropProductToTrash();
        public float speed { get; set; }
        public float sensitivity { get; set; }
        public float rotationSpeed { get; set; }
        public float acceleration { get; set; }
    }
}