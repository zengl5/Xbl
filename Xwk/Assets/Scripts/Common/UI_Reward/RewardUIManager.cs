using System;
using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class RewardUIManager : C_Singleton<RewardUIManager>
    {
        private readonly int CameraDepthCfg = 49;
        private readonly string LayerCfg = "RewardUI";

        private RewardUI _AnimaUI;
        private GameObject _Prefab;

        #region 外部调用

        /// <summary>
        /// 改变模块（重要）
        /// 默认打开计分板
        /// </summary>
        /// <param name="moduleType"></param>
        public void ChangeModule(ModuleType moduleType, string sceneType)
        {
            _AnimaUI.ChangeModule(moduleType, sceneType);
            _AnimaUI.SetScoreVisible(true);
        }

        /// <summary>
        /// 设置计分板显示状态
        /// </summary>
        public void SetScoreVisible(bool tag)
        {
            _AnimaUI.SetScoreVisible(tag);
        }

        /// <summary>
        /// 主页面初次调用
        /// </summary>
        /// <param name="motion">操作类型</param>
        /// <param name="sourceType">来源类型</param>
        /// <param name="waitTime">超时时长</param>
        /// <param name="animaUICallback">结束回调</param>
        public void RegisterHomePage(MotionType motion, SourceType sourceType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(motion, sourceType, ModuleType.HomePage, waitTime, animaUICallback, sceneType);
            _AnimaUI.StartCountDown();
        }

        public void RegisterHomePage(int star, SourceType sourceType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(star, sourceType, ModuleType.HomePage, waitTime, animaUICallback, sceneType);
            _AnimaUI.StartCountDown();
        }

        public void RegisterStory(MotionType motion, SourceType sourceType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(motion, sourceType, ModuleType.Story, waitTime, animaUICallback, sceneType);
            _AnimaUI.StartCountDown();
        }

        public void RegisterStory(int star, SourceType sourceType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(star, sourceType, ModuleType.Story, waitTime, animaUICallback, sceneType);
            _AnimaUI.StartCountDown();
        }

        public void RegisterOfflineBonus(int star, Vector2 pos, ModuleType moduleType, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(star, SourceType.OfflineBonus, moduleType, 0, animaUICallback, sceneType);
            _AnimaUI.SetStarStartPos(pos);
        }

        #region 精灵展示页面专用

        /// <summary>
        /// 精灵展示页面专用
        /// </summary>
        /// <param name="star">灵气值</param>
        /// <param name="animaUICallback">回调</param>
        public void RegisterSpriteWindow(int star, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(star, SourceType.SpriteWindow, ModuleType.SpriteWindow, 0, animaUICallback, sceneType);
        }

        public void SetSpriteWindowScore()
        {
            _AnimaUI.UpdateScore(-_AnimaUI._SpriteWindowModuleGainNum);
            _AnimaUI._SpriteWindowModuleGainNum = 0;//模块奖励数据只生效一次
        }

        /// <summary>
        /// 模块奖励数据只生效一次，注意调用循序
        /// 必须先调用 ChangeModule
        /// </summary>
        /// <returns></returns>
        public int GetSpriteSpriteWindowModuleGainNum()
        {
            return _AnimaUI._SpriteWindowModuleGainNum;
        }

        //TODO
        //清除掉模块增长数据
        public void ClearModuleGainNum()
        {
            _AnimaUI.ClearModuleGainNum();
        }

        /// <summary>
        /// 精灵解锁页面
        /// </summary>
        /// <param name="star"></param>
        /// <param name="animaUICallback"></param>
        public void RegisterSpriteUnlock(int star, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(star, SourceType.SpriteWindow, ModuleType.SpriteUnlock, 0, animaUICallback, sceneType);
        }

        /// <summary>
        /// 离线奖励专用
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="animaUICallback"></param>
        public void RegisterOfflineBonus(Vector2 pos, Action<bool> animaUICallback = null, string sceneType = null)
        {
            _AnimaUI.Register(1, SourceType.OfflineBonus, ModuleType.SpriteWindow, 0, animaUICallback, sceneType);
            _AnimaUI.SetStarStartPos(pos);
        }

        /// <summary>
        /// 设置分数板分数
        /// </summary>
        /// <param name="score"></param>
        /// <param name="visible"></param>
        public void UpdateScore(int score, ModuleType moduleType = ModuleType.SpriteWindow, string sceneType = null)
        {
            _AnimaUI.ChangeModule(moduleType, sceneType);
            _AnimaUI.UpdateScore(score);
        }

        #endregion 精灵展示页面专用

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void PauseCountDown()
        {
            _AnimaUI.PauseCountDown();
        }

        /// <summary>
        /// 恢复计时器
        /// </summary>
        public void ResumeCountDown()
        {
            _AnimaUI.ResumeCountDown();
        }

        /// <summary>
        /// 设置成功，播放星星动画
        /// </summary>
        public void SetSuccess()
        {
            _AnimaUI.SetSuccess();
        }

        public void SetFail()
        {
            _AnimaUI.SetFail();
        }

        /// <summary>
        /// 设置倒计时显示状态
        /// </summary>
        /// <param name="tag"></param>
        public void SetCountdownVisible(bool tag)
        {
            _AnimaUI.SetCountdownVisible(tag);
        }

        /// <summary>
        /// 清空奖励UI  删除星星并隐藏所有UI
        /// </summary>
        public void ClearRewardUI()
        {
            SetScoreVisible(false);
            _AnimaUI.ResetRewardUI();
        }

        public void ChangeLayer(string layer)
        {
            Utility.SetTransformLayer(_AnimaUI.transform, LayerMask.NameToLayer(layer));
        }
        public void ChangeLayer(int layer)
        {
            Utility.SetTransformLayer(_AnimaUI.transform, layer);
        }
        public void RestoreLayer()
        {
            ChangeLayer(LayerCfg);
        }

        public void ChangeCameraDepth(int depth)
        {
            _AnimaUI.Camera.depth = depth;
        }
        public void RestoreCameraDepth()
        {
            _AnimaUI.Camera.depth = CameraDepthCfg;
        }

        #endregion 外部调用

        protected override void Init()
        {
            _Prefab = GameResMgr.Instance.LoadResource<GameObject>("c_framework/ui/package_ui_prefab/UI_Reward", true);
            if(!_Prefab.activeSelf)
            {
                _Prefab.SetActive(true);
            }
            _AnimaUI = _Prefab.GetComponent<RewardUI>();
        }

        protected override void Destroy()
        {
            UnityEngine.Object.Destroy(_Prefab);
            _Prefab = null;
        }
    }
}