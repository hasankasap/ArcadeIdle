using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/CharacterSO")]
	public class CharacterSO : ScriptableObject
	{
		public float speed, rotationSpeed, sensitivity, acceleration;
	}
}