using System.Collections;
using System.Net.Mime;
using BiruisredEngine;
using Firebase.Database;
using Sirenix.OdinInspector;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace IdleGear
{
    public class IdleGearManager : App
    {
        [Range(5, 60)]
        [SerializeField] private int targetFrameRate = 60;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            Application.targetFrameRate = targetFrameRate;
#else
            Application.targetFrameRate = 60;
#endif
        }

        protected override IEnumerator Start()
        {
            yield return base.Start();

            EVENT.Invoke(new PlayerInit
            {
                PlayerName = SYSTEM.User.Name,
                GoldAmount = 0
            });
        }
    }
}