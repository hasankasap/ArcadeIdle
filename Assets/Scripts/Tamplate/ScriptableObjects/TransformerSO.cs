using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "TransformerSO", menuName = "ScriptableObjects/TransformerSO")]
	public class TransformerSO : ScriptableObject
	{
		public float transformDelay, assetTakeDelay;
		public int capacity;
		public GameObject transformedPrefab;
	}
}