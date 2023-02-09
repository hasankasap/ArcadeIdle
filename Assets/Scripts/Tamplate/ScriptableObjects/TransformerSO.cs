using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "TransformerSO", menuName = "ScriptableObjects/TransformerSO")]
	public class TransformerSO : ScriptableObject
	{
		public float transformDelay;
		public GameObject transformedPrefab;
	}
}