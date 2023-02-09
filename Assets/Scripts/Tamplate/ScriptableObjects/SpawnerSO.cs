using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "SpawnerSO", menuName = "ScriptableObjects/SpawnerSO")]
	public class SpawnerSO : ScriptableObject
	{
		public float spawnTimer;
		public GameObject spawnPrefab;
	}
}