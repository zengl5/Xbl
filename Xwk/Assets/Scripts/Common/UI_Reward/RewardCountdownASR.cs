using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class RewardCountdownASR : RewardCountdown
    {
        /// <summary>
        /// 麦克风动画
        /// </summary>
        private GameObject _MicAni = null;

        protected override void Awake()
        {
            base.Awake();
            _MicAni = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/UICover_reward_shengyin", true);//麦克风动画
            _MicAni.SetActive(true);
            _MicAni.transform.SetParent(transform);
        }

        protected override void OnDestroy()
        {
            Destroy(_MicAni);
            _MicAni = null;
            Resources.UnloadUnusedAssets();
            base.OnDestroy();
        }
    }
}