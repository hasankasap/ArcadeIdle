using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "TrashCanSO", menuName = "ScriptableObjects/TrashCanSO")]
	public class TrashCanSO : ScriptableObject
	{
		public float takeDelay;
	}
}