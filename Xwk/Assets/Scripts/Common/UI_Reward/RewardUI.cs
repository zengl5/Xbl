using Assets.Scripts.C_Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.UI_Reward
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum MotionType
    {
        None = 0,
        Click = 1,  //点击
        Slide = 2,  //滑动
        SR = 3,  //语音识别
                 //Custom  //自定义类型
    }

    /// <summary>
    /// 来源类型（选择不同星星动画表现）（作为细分场景模块的统计数据的一部分提交）
    /// </summary>
    public enum SourceType
    {
        None = 0,
        Interaction = 1,    //互动
        SpriteWindow = 2,   //精灵展示页面
        DailyBonus = 3,     //每日奖励（主页面进入游戏）
        OfflineBonus = 4,   //离线收益
        Story = 5,          //故事（故事进入游戏）
        Game = 6,           //游戏

        /// <summary>
        /// 待用
        /// </summary>
        //Basic,          //基础
        //Special,        //特殊
        //LevelUp,        //等级提升
    }

    /// <summary>
    /// 模块类型，用于显示不同的分数板 和 初始分数值
    /// </summary>
    public enum ModuleType
    {
        None = 0,
        HomePage = 1,       //主页面
        Story = 2,          //故事
        SpriteWindow = 3,   //精灵展示页面
        SpriteUnlock = 4,   //解锁精灵页面
        Guide = 5,          //新手引导
    }

    internal class RewardUI : C_MonoSingleton<RewardUI>
    {
        #region 实例引用

        [SerializeField]
        [Header("奖励星星（假参数）")]
        private GameObject _AwardStarPrefab;

        [SerializeField]
        [Header("奖励拖尾（假参数）")]
        private GameObject _AwardTrailingEffectsPrefab;

        [SerializeField]
        [Header("奖励特效（假参数）")]
        private GameObject _AwardEffectsPrefab;

        [SerializeField]
        [Header("渲染相机")]
        private Camera _Camera;

        [SerializeField]
        [Header("画布")]
        private RectTransform _Canvas;

        [SerializeField]
        [Header("星星入场停留位置")]
        private RectTransform _EnterPos;

        [Header("倒计时")]
        [SerializeField]
        [Header("数字倒计时")]
        private GameObject _CountdownNormalGO;

        [SerializeField]
        [Header("麦克风倒计时")]
        private GameObject _CountdownASRGO;

        [Header("分数板")]
        [SerializeField]
        [Header("主页分数板")]
        private GameObject _ScoreHomePage;

        [SerializeField]
        [Header("精灵二级分数板")]
        private GameObject _ScoreSpriteWindow;

        [SerializeField]
        [Header("故事分数板")]
        private GameObject _ScoreStory;

        #endregion 实例引用

        #region 属性方法

        public RectTransform Canvas
        {
            get
            {
                return _Canvas;
            }
        }

        public Camera Camera
        {
            get
            {
                return _Camera;
            }
        }

        public RectTransform EnterPos
        {
            get
            {
                return _EnterPos;
            }
        }

        public GameObject AwardEffects { get; set; }

        public GameObject StarPrefab
        {
            get
            {
                return _StarPrefab;
            }
        }

        public GameObject StarEffectPrefab
        {
            get
            {
                return _StarEffectPrefab;
            }
        }

        public GameObject CountdownNormalGO
        {
            get
            {
                return _CountdownNormalGO;
            }
        }

        public GameObject CountdownASRGO
        {
            get
            {
                return _CountdownASRGO;
            }
        }

        public RectTransform EndPos
        {
            get
            {
                return _EndPos;
            }

            set
            {
                _EndPos = value;
            }
        }

        #endregion 属性方法

        #region 成员变量

        /// <summary>
        /// Performance引用管理
        /// (内部计数从1开始)
        /// </summary>
        private Dictionary<uint, RewardPerformance> _ReferenceDict = null;

        private uint _ReferenceCount = 0;

        private uint ReferenceCount
        {
            get
            {
                return _ReferenceCount;
            }
            set
            {
                if (value >= uint.MaxValue)
                {
                    _ReferenceCount = 1;//引用数量超过最大值时重新计数（以防万一）
                }
            }
        }

        public RectTransform Score
        {
            get
            {
                return _Score;
            }

            set
            {
                _Score = value;
            }
        }

        private RewardPerformance _RewardPerformance = null;
        private uint _RewardPerformanceNumber = 0;//此引用的序号

        private GameObject _StarPrefab = null;
        private GameObject _StarEffectPrefab = null;

        /// <summary>
        /// 分数板数字图片
        /// </summary>
        private List<Sprite> _NumImgListHomePage = new List<Sprite>();

        private List<Sprite> _NumImgListStory = new List<Sprite>();

        /// <summary>
        /// 模块类型
        /// </summary>
        private ModuleType _ModuleType = ModuleType.HomePage;

        private SourceType _SourceType = SourceType.None;

        /// <summary>
        /// 星星移动目标位置
        /// </summary>
        private RectTransform _EndPos = null;

        /// <summary>
        /// 计分板
        /// </summary>
        private RectTransform _Score;

        /// <summary>
        /// 精灵二级界面模块奖励的特殊表现
        /// </summary>
        [HideInInspector]
        public int _SpriteWindowModuleGainNum = 0;

        private int _ModuleGainNum = 0;//模块获得灵气（内部使用）

        /// <summary>
        /// 模块获得灵气
        /// </summary>
        private int _SceneGainNum = 0;

        /// <summary>
        /// 场景的类型（统计模块星星数量用）
        /// </summary>
        [HideInInspector]
        public string _SceneType = string.Empty;

        #endregion 成员变量

        #region 外部调用

        public void Register(MotionType motion, SourceType sourceType, ModuleType moduleType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            ChangeModule(moduleType, sceneType);
            ChoosePerformance(sourceType);
            if (_RewardPerformance != null)
            {
                _RewardPerformance.Register(motion, sourceType, waitTime, PackCallback(animaUICallback));
            }
            else
            {
                C_DebugHelper.LogError("RewardPerformance 创建错误");
            }
        }

        public void Register(int star, SourceType sourceType, ModuleType moduleType, int waitTime, Action<bool> animaUICallback = null, string sceneType = null)
        {
            ChangeModule(moduleType, sceneType);
            ChoosePerformance(sourceType);
            if (_RewardPerformance != null)
            {
                _RewardPerformance.Register(star, sourceType, waitTime, PackCallback(animaUICallback));
            }
            else
            {
                C_DebugHelper.LogError("RewardPerformance 创建错误");
            }
        }

        #region 传递作用（待优化）

        public void StartCountDown()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.StartCountDown();
            }
        }

        public void PauseCountDown()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.PauseCountDown();
            }
        }

        public void ResumeCountDown()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.ResumeCountDown();
            }
        }

        public void SetSuccess()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.SetSuccess();
            }
        }

        public void SetFail()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.SetFail();
            }
        }

        /// <summary>
        /// 重置奖励UI状态（计分板状态除外）
        /// </summary>
        public void ResetRewardUI()
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.ResetRewardUI();
                SetCountdownVisible(false);
            }
        }

        public void SetScoreVisible(bool visible)
        {
            if (Score != null)
            {
                Score.gameObject.SetActive(visible);
            }
        }

        public void SetCountdownVisible(bool visible)
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.SetCountdownVisible(visible);
            }
        }

        public void SetStarStartPos(Vector2 pos)
        {
            if (_RewardPerformance != null)
            {
                _RewardPerformance.SetStarStartPos(pos);
            }
        }

        public void ClearModuleGainNum()
        {
            _SpriteWindowModuleGainNum = 0;
            _ModuleGainNum = 0;
        }

        #endregion 传递作用（待优化）

        /// <summary>
        /// 按照引用的序列号删除
        /// </summary>
        /// <param name="referenceCount"></param>
        public void RemovePerformanceReference(uint referenceCount)
        {
            if (_ReferenceDict.ContainsKey(referenceCount))
            {
                _ReferenceDict[referenceCount] = null;
                _ReferenceDict.Remove(referenceCount);
                if (_RewardPerformanceNumber == referenceCount)
                {
                    _RewardPerformance = null;//跟当前保存的引用序列号相同则置空
                    _RewardPerformanceNumber = 0;
                }
            }
            else
            {
                C_DebugHelper.LogError("删除Performance引用出错 " + referenceCount);
            }
        }

        #endregion 外部调用

        protected override void Init()
        {
            _ReferenceDict = new Dictionary<uint, RewardPerformance>();

            AwardEffects = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/uiCover_effect_jiangli", false);
            AwardEffects = GameObject.Instantiate(AwardEffects, Canvas, true);
            AwardEffects.layer = LayerMask.NameToLayer("RewardUI");
            AwardEffects.transform.SetSiblingIndex(2);
            AwardEffects.transform.localPosition = new Vector3(0, 0, 0);
            AwardEffects.transform.localScale = new Vector3(108, 108, 108);
            AwardEffects.SetActive(false);

            _StarPrefab = GameResMgr.Instance.LoadResource<GameObject>("c_framework/ui/package_ui_prefab/UI_RewardStar", false);
            _StarEffectPrefab = GameResMgr.Instance.LoadResource<GameObject>("public/Hero_Effect/prefab/uiCover_effect_jltw", false);

            //TODO 需不需要改为图集
            for (int i = 0; i < 10; i++)
            {
                Sprite tex1 = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/reward/score/homepage/number/" + i.ToString() + ".png", false);
                _NumImgListHomePage.Add(tex1);
                Sprite tex2 = GameResMgr.Instance.LoadResource<Sprite>("c_framework/ui/package_ui_sprite/reward/score/story/number/" + i.ToString() + ".png", false);
                _NumImgListStory.Add(tex2);
            }
        }

        //TODO 切换模块的逻辑不够严谨
        //模块类型和表现不挂钩，额外接口调用切换模块
        public void ChangeModule(ModuleType toModuleType, string sceneType)
        {
            //改模块
            if (_ModuleType != toModuleType)
            {
                if (_ModuleType == ModuleType.Story || _ModuleType == ModuleType.SpriteUnlock)
                {
                    if (toModuleType != ModuleType.Guide)
                    {
                        //从故事或精灵解锁页面返回时，记录模块增加数据
                        _SpriteWindowModuleGainNum = _ModuleGainNum;
                    }
                }

                _ModuleType = toModuleType;
                //清空模块记录数据
                _ModuleGainNum = 0;
            }

            //先获得模块数据再更新分数板，以防模块数据被修改
            SceneDataStatistics(sceneType);
            ChooseScoreBoard(_ModuleType);
        }

        /// <summary>
        /// 细分场景模块星星数量统计
        /// 格式：SourceType@sceneType
        /// </summary>
        /// <param name="sceneType">具体的统计字段：游戏+名字 / 故事+名字 </param>
        private void SceneDataStatistics(string sceneType)
        {
            if (!string.IsNullOrEmpty(_SceneType) && !string.Equals(_SceneType, sceneType))
            {
                //模块星星数量统计
#if UNITY_EDITOR
                string str = string.Concat("场景星星数量统计  moudle_reward@", _SourceType, "@", _SceneType, "@", _SceneGainNum.ToString());
                C_DebugHelper.LogError(str);
#else
                GameHelper.Instance.SendDataStatistics(EnumDataStatistics.Chick, string.Concat("moudle_reward@", _SourceType, "@", _SceneType), _SceneGainNum.ToString());
#endif
                _SceneType = string.Empty;
                _SceneGainNum = 0;
            }
            if (!string.IsNullOrEmpty(sceneType))
            {
                _SceneType = sceneType;
            }
        }

        /// <summary>
        /// 根据不同来源选择不同表现
        /// </summary>
        /// <param name="sourceType"></param>
        private void ChoosePerformance(SourceType sourceType)
        {
            _SourceType = sourceType;
            switch (sourceType)
            {
                case SourceType.Story:
                case SourceType.Interaction:
                    {
                        GameObject reward = new GameObject("InteractionPerformance");
                        _RewardPerformance = reward.AddComponent<PerformanceInteraction>();
                        _ReferenceDict.Add(++_ReferenceCount, _RewardPerformance);
                        _RewardPerformance.SetSerialNumber(_ReferenceCount);
                        _RewardPerformanceNumber = _ReferenceCount;
                    }
                    break;

                case SourceType.SpriteWindow:
                    {
                        GameObject reward = new GameObject("SpriteWindowPerformance");
                        _RewardPerformance = reward.AddComponent<PerformanceSpriteWindow>();
                        _ReferenceDict.Add(++_ReferenceCount, _RewardPerformance);
                        _RewardPerformance.SetSerialNumber(_ReferenceCount);
                        _RewardPerformanceNumber = _ReferenceCount;
                    }
                    break;

                case SourceType.Game:
                case SourceType.DailyBonus:
                    {
                        GameObject reward = new GameObject("DailyBonusPerformance");
                        _RewardPerformance = reward.AddComponent<PerformanceSpriteWindow>();
                        _ReferenceDict.Add(++_ReferenceCount, _RewardPerformance);
                        _RewardPerformance.SetSerialNumber(_ReferenceCount);
                        _RewardPerformanceNumber = _ReferenceCount;
                    }
                    break;

                case SourceType.OfflineBonus:
                    {
                        GameObject reward = new GameObject("PerformanceOfflineBonus");
                        _RewardPerformance = reward.AddComponent<PerformanceOfflineBonus>();
                        _ReferenceDict.Add(++_ReferenceCount, _RewardPerformance);
                        _RewardPerformance.SetSerialNumber(_ReferenceCount);
                        _RewardPerformanceNumber = _ReferenceCount;
                    }
                    break;

                default:
                    C_DebugHelper.LogError("来源类型错误");
                    break;
            }
        }

        /// <summary>
        /// 选择分数板
        /// </summary>
        /// <param name="moduleType"></param>
        private void ChooseScoreBoard(ModuleType moduleType)
        {
            _ScoreHomePage.SetActive(false);
            _ScoreSpriteWindow.SetActive(false);
            _ScoreStory.SetActive(false);

            switch (moduleType)
            {
                case ModuleType.None:
                    break;

                case ModuleType.HomePage:
                    InitScoreHomePage();
                    UpdateScore(0);
                    break;

                case ModuleType.Story:
                    InitScoreStory();
                    UpdateScore(0);
                    break;

                case ModuleType.SpriteWindow:
                    InitScoreSpriteWindow();
                    UpdateScore(0);
                    break;

                case ModuleType.SpriteUnlock:
                    InitScoreStory();
                    UpdateScore(0);
                    break;

                default:
                    break;
            }
        }

        public void UpdateScore(int score)
        {
            if (Score != null)
            {
                switch (_ModuleType)
                {
                    case ModuleType.None:
                        break;

                    case ModuleType.HomePage:
                    case ModuleType.SpriteWindow:
                        AnimaData.TotalNimbus += score;
                        SetNum(AnimaData.TotalNimbus);
                        break;

                    case ModuleType.Story:
                    case ModuleType.SpriteUnlock:
                    case ModuleType.Guide:
                        _ModuleGainNum += score;
                        AnimaData.TotalNimbus += score;
                        SetNum(_ModuleGainNum);
                        break;

                    default:
                        break;
                }
                _SceneGainNum += score;
            }
        }

        private void InitScoreHomePage()
        {
            _ScoreHomePage.SetActive(true);
            Score = _ScoreHomePage.GetComponent<RectTransform>();
            EndPos = Score.Find("scoreicon").GetComponent<RectTransform>();
        }

        private void InitScoreStory()
        {
            _ScoreStory.SetActive(true);
            Score = _ScoreStory.GetComponent<RectTransform>();
            EndPos = Score.Find("scoreicon").GetComponent<RectTransform>();
        }

        private void InitScoreSpriteWindow()
        {
            _ScoreSpriteWindow.SetActive(true);
            Score = _ScoreSpriteWindow.GetComponent<RectTransform>();
            EndPos = Score.Find("scoreicon").GetComponent<RectTransform>();
        }

        private void SetNum(int number)
        {
            //TODO 滚轮效果
            number = Math.Min(number, 999999);
            number = Math.Max(number, 0);

            int copynumber = number;
            int[] bits = new int[6];
            for (int i = 0; i < 6; i++)
            {
                bits[i] = copynumber % 10;
                copynumber /= 10;
            }

            int bitNum = 0;
            for (; number > 0; bitNum++)
            {
                number /= 10;
            }

            Transform shiwan, wan, qian, bai, shi, ge = null;
            shiwan = Score.Find("score/shiwan");
            SetNumImage(shiwan, bits[5], bitNum > 5);
            wan = Score.Find("score/wan");
            SetNumImage(wan, bits[4], bitNum > 4);
            qian = Score.Find("score/qian");
            SetNumImage(qian, bits[3], bitNum > 3);
            bai = Score.Find("score/bai");
            SetNumImage(bai, bits[2], bitNum > 2);
            shi = Score.Find("score/shi");
            SetNumImage(shi, bits[1], bitNum > 1);
            ge = Score.Find("score/ge");
            SetNumImage(ge, bits[0], true);
        }

        private void SetNumImage(Transform numTransform, int num, bool isValid)
        {
            if (numTransform == null)
            {
                return;
            }
            if (!isValid)
            {
                numTransform.gameObject.SetActive(false);
                return;
            }
            Sprite tex = null;
            switch (_ModuleType)
            {
                case ModuleType.None:
                    break;

                case ModuleType.HomePage:
                case ModuleType.SpriteWindow:
                    tex = _NumImgListHomePage[num];
                    break;

                case ModuleType.Story:
                case ModuleType.SpriteUnlock:
                case ModuleType.Guide:
                    tex = _NumImgListStory[num];
                    break;

                default:
                    break;
            }
            if (tex != null)
            {
                numTransform.gameObject.SetActive(true);
                numTransform.GetAddComponent<Image>().sprite = tex;
                numTransform.GetAddComponent<Image>().SetNativeSize();
            }
        }

        /// <summary>
        /// 包装传递给Performance的回调
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private Action<bool> PackCallback(Action<bool> callback)
        {
            Action<bool> action = (bool b) =>
            {
                if (callback != null)
                {
                    Action<bool> tAction = callback;
                    tAction(b);
                    callback = null;
                }
            };
            return action;
        }

        protected override void OnDestroy()
        {
            _NumImgListHomePage.Clear();
            _NumImgListStory.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}