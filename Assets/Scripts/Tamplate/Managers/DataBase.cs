using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public class DataBase : BaseSingleton<DataBase>
    {
        public ScriptableObject[] scriptableObjects;
        public DataSO[] datas;

        #region UNITY_METHODS
        #endregion

        #region METHODS

        public void SaveScriptableObjectToJson()
        {
            if (scriptableObjects.Length == 0)
                return;
            for (int i = 0; i < scriptableObjects.Length; i++)
            {
                string filePath = Path.Combine(Application.dataPath, scriptableObjects[i].name + ".json");
                string jsonData = JsonConvert.SerializeObject(scriptableObjects[i], Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
            }

        }
        private void LoadScriptableObjectFromJson()
        {
            if (scriptableObjects.Length == 0)
                return;
            for (int i = 0; i < scriptableObjects.Length; i++)
            {
                string filePath = Path.Combine(Application.dataPath, scriptableObjects[i].name + ".json");
                //Debug.Log(File.Exists(filePath));
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    //scriptableObjects[i] = JsonConvert.DeserializeObject<ScriptableObject>(jsonData);
                    JsonConvert.PopulateObject(jsonData, scriptableObjects[i]);
                }
            }
        }
        #endregion
        #region ACTIONS
        public void saveAllData()
        {
            SaveScriptableObjectToJson();
        }
        public void loadAllData()
        {
            LoadScriptableObjectFromJson();
        }
        public void resetallData()
        {
            if (datas.Length == 0)
                return;
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i].resetData();
            }
        }
        #endregion
    }
}