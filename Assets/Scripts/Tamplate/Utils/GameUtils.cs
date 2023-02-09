using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    namespace Utils
    {
        public static class Events
        {
            #region GAME_STATE_EVENTS
            public static string LEVEL_LOADED = "LEVEL_LOADED";
            public static string LEVEL_FAILED = "LEVEL_FAILED";
            public static string LEVEL_COMPLETED = "LEVEL_COMPLETED";
            public static string GAME_STARTED = "GAME_STARTED";
            #endregion

            #region IN_GAME_EVENTS
            public static string ADD_MONEY = "ADD_MONEY";
            public static string UPDATE_TOTAL_MONEY_UI = "UPDATE_TOTAL_MONEY_UI";
            #endregion
        }
        public static class MouseUtils
        {
            public static bool mouseIsOnUI()
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
        }
        public static class Remap
        {
            public static float calculate(float from, float fromMin, float fromMax, float toMin, float toMax)
            {
                return toMin + (from - fromMin) * (toMax - toMin) / (fromMax - fromMin);
            }
        }
        public static class MoneyTextUtility
        {
            private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
            {
                {1000,"K"},
                {1000000, "M" },
                {1000000000, "B" }
            };

            public static string FloatToStringConverter(float number)
            {
                for (int i = abbrevations.Count - 1; i >= 0; i--)
                {
                    KeyValuePair<int, string> pair = abbrevations.ElementAt(i);
                    if (Mathf.Abs(number) >= pair.Key)
                    {
                        int roundedNumber = Mathf.FloorToInt(number / pair.Key);
                        return roundedNumber.ToString() + pair.Value;
                    }
                }
                if (number < 100)
                    return number.ToString("0");
                else
                    return number.ToString();
            }
        }

        public enum ProductTypes
        {
            Product,
            TransformedProduct,
        }
    }
}