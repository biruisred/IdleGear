using System;
using System.Collections;
using System.Collections.Generic;
using BiruisredEngine;
using Firebase.Firestore;
using Sirenix.OdinInspector;

namespace IdleGear
{
    public class IdleGearManager : App
    {
        protected override IEnumerator Start()
        {
            yield return base.Start();
            myId = SYSTEM.User.ID;
        }

        public void StartQuest()
        {
            
        }

        public string myId;
        public string testId;
        public int stamina;
        public int maxStamina;
        
        [Button]
        public async void SaveData()
        {
            var db = FirebaseFirestore.DefaultInstance;
            var docRef = db.Collection("user").Document(SYSTEM.User.ID);
            var savesRef = FirebaseFirestore.DefaultInstance.Collection("user");
            Dictionary<string, object> saveData = new Dictionary<string, object>
            {
                { "stamina", stamina },
                { "maxStamina", maxStamina }
            };
             await docRef.SetAsync(saveData, SetOptions.Overwrite);
             print("Saved");
        }
        
        [Button]
        public async void Check()
        {
            var db = FirebaseFirestore.DefaultInstance;
            var docRef = db.Collection("user").Document(SYSTEM.User.ID);
            var snapshot = await docRef.GetSnapshotAsync();
            print(snapshot.Exists);
        }

        [Button]
        public async void CheckCollection()
        {
            var db = FirebaseFirestore.DefaultInstance;
            var docRef = db.Collection("user").Document(SYSTEM.User.ID).Collection("inventory");
            var snapshot = await docRef.GetSnapshotAsync();
        }
    }

    [Serializable]
    public struct UserData
    {
        public string uid; 
        public int stamina;
        public int maxStamina;
        public int energy;
        public int maxEnergy;
    }
}
