using UnityEngine;

namespace XWK.Common.RedBomb
{
    public class RedBombManager : C_Singleton<RedBombManager>
    {
        private GameObject _Prefab = null;
        private RedBombUI _RedBombUI = null;

        public void StartPlay(int which = 0)
        {
            UI_Reward.RewardUIManager.Instance.SetScoreVisible(false);
            Init();
            _RedBombUI.StartPlay(which);
        }

        protected override void Init()
        {
            if (_Prefab == null)
            {
                _Prefab = GameResMgr.Instance.LoadResource<GameObject>("c_framework/ui/package_ui_prefab/UI_RedBombLayer", true);
                _RedBombUI = _Prefab.GetComponent<RedBombUI>();
            }
            if (!_Prefab.activeSelf)
            {
                _Prefab.SetActive(true);
            }
        }

        protected override void Destroy()
        {
            Object.Destroy(_Prefab);
            _Prefab = null;
        }
    }
}