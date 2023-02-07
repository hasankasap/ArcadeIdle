using UnityEngine;
namespace Game
{
	[CreateAssetMenu(fileName = "GameInfoSO", menuName = "ScriptableObjects/GameInfoSO")]
	public class GameInfoSO : DataSO
	{
        public int currentLevel;
        public int playerMoney;
        public int rescueTargetValue;
        public override void resetData()
        {
            currentLevel = 0;
        }
        public void increaseLevelCount()
        {
            currentLevel++;
        }
        public int getLevelData()
        {
            return currentLevel;
        }
        public void sumPlayerMoney(int value)
        {
            playerMoney += value;
        }
        public float getPlayerMoney()
        {
            return playerMoney;
        }
        public int getRescueTargetValue()
        {
            return rescueTargetValue;
        }
    }
}