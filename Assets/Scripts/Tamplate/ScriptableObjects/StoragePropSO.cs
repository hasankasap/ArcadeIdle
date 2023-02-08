using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "StoragePropSO", menuName = "ScriptableObjects/StoragePropSO")]
	public class StoragePropSO : ScriptableObject
	{
        public float storageLineOffset, storageColumnOffset;
        public int storageLineCapacity;
    }
}