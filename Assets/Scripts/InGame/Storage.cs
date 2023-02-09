using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Storage
    {
        public StoragePropSO storageProp;
        public Transform storagePoint;
        public List<Product> storedProducts = new List<Product>();
        [HideInInspector] public int columnCount, lineCount;

        public void increase()
        {
            columnCount++;
            if (columnCount >= storageProp.storageLineCapacity)
            {
                columnCount = 0;
                lineCount++;
            }
        }
        public void decrease()
        {
            columnCount--;
            if (columnCount < 0)
            {
                lineCount--;
                columnCount = storageProp.storageLineCapacity - 1;
            }
        }
        public Product getLastProduct()
        {
            Product temp = storedProducts[storedProducts.Count - 1];
            storedProducts.Remove(temp);
            return temp;
        }
        public int getCapacity()
        {
            return storageProp.storageCapacity;
        }
        public void addProduct(Product product)
        {
            storedProducts.Add(product);
        }
        public ProductTypes getStorageType()
        {
            return storageProp.productType;
        }
        public bool isStorageFull()
        {
            return storedProducts.Count >= storageProp.storageCapacity;
        }
        public bool hasProduct()
        {
            return storedProducts.Count > 0;
        }
        public int getStoredProductCount()
        {
            return storedProducts.Count;
        }
        public Vector3 getStoragePoint()
        {
            Vector3 point = storagePoint.localPosition;
            Vector3 forwardOffset = Vector3.forward * storageProp.storageLineOffset * columnCount;
            Vector3 upwardOffset = Vector3.left * storageProp.storageColumnOffset * lineCount;
            point += (forwardOffset + upwardOffset);
            return point;
        }
    }
}