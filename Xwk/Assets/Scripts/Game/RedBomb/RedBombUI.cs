using Assets.Scripts.C_Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YB.AnimCallbacks;

namespace XWK.Common.RedBomb
{
    internal class RedBombUI : C_MonoSingleton<RedBombUI>
    {
        [SerializeField]
        [Header("开场动画层")]
        private GameObject _StartAniLayer;

        [SerializeField]
        [Header("UI层")]
        private GameObject _UILayer;

        [SerializeField]
        [Header("分数板")]
        private GameObject _ScoreBoardGo;

        [Header("红包雨")]
        [SerializeField]
        [Header("红包雨玩法")]
        private GameObject _RainPlayer;

        [SerializeField]
        [Header("红包雨倒计时")]
        private GameObject _RainCountdownGo;

        [SerializeField]
        [Header("红包雨父节点")]
        private GameObject _RainLayer;

        [SerializeField]
        [Header("红包雨结算层")]
        private GameObject _RainSettlementLayer;

        [SerializeField]
        [Header("红包雨结算Text")]
        private GameObject _RainSettlementTextGo;

        [Header("贺卡")]
        [SerializeField]
        [Header("贺卡玩法")]
        private GameObject _CardPlayer;

        [SerializeField]
        [Header("贺卡倒计时")]
        private GameObject _CardCountdownGo;

        [SerializeField]
        [Header("贺卡父节点")]
        private GameObject _CardsGo;

        [SerializeField]
        [Header("大贺卡图片高")]
        private GameObject _BigCardHighGo;

        [SerializeField]
        [Header("大贺卡图片低")]
        private GameObject _BigCardLowGo;

        [Header("祝福贺卡")]
        [SerializeField]
        [Header("祝福贺卡层")]
        private GameObject _BlessingPlayer;

        [SerializeField]
        [Header("假祝福卡")]
        private Image _FakeCardImg;

        [SerializeField]
        [Header("假祝福字")]
        private Image _FakeWordImg;

        [SerializeField]
        [Header("祝福贺卡保存按钮角标")]
        private GameObject _BlessingSaveButtonIconGo;

        [SerializeField]
        [Header("祝福贺卡倒计时")]
        private GameObject _BlessingCountdownGo;

        [Header("祝福贺卡截屏")]
        [SerializeField]
        [Header("祝福贺卡截屏")]
        private GameObject _ScreenShotGo;

        [SerializeField]
        [Header("截屏相机")]
        private Camera _ScreenShotCamera;

        [SerializeField]
        [Header("截屏贺卡")]
        private GameObject _ScreenShotCardGo;

        [SerializeField]
        [Header("截屏贺卡文字")]
        private GameObject _ScreenShotWordGo;

        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////

        #region 成员变量

        public static readonly string BundleType = "";

        public static readonly string RedBombRes = "ui_redbomb(clone)";

        private Camera _UICamera = null;

        /// <summary>
        /// 分数板组件
        /// </summary>
        private RedBombScoreBoard _ScoreBoard = null;

        #endregion 成员变量

        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////

        #region 开场动画成员变量

        /// <summary>
        /// 喷红包特效
        /// </summary>
        private GameObject _RedBombEffectGo = null;

        //模型
        private GameObject _MeshGo = null;

        //开场end动画
        private AnimationClip _EndAniClip = null;

        #endregion 开场动画成员变量

        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////

        #region 红包雨成员变量

        private GameObject _WinEffectGo = null;

        private GameObject _FailEffectGo = null;

        private GameObject _CoinFlyEffectGo = null;

        private GameObject _ClickEffectPrefab = null;

        public GameObject ClickEffectPrefab
        {
            get
            {
                return _ClickEffectPrefab;
            }
        }

        private GameObject _RedBombPrefab = null;

        /// <summary>
        /// 红包雨倒计时组件
        /// </summary>
        private CountdownRedBomb _RainCountdown = null;

        /// <summary>
        /// 红包雨分数
        /// </summary>
        private int _Score = 0;

        #endregion 红包雨成员变量

        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////

        #region 贺卡成员变量

        /// <summary>
        /// 贺卡数组
        /// </summary>
        private Image[] _CardImages = null;

        //出现动作列表
        private List<Sequence> _ActionSeqList = null;

        private bool _IsCardConfirmed = false;

        //选中的贺卡编号
        private int _ChooseCardNo = 0;

        /// <summary>
        /// 贺卡倒计时组件
        /// </summary>
        private CountdownFontType _CardCountdown = null;

        #endregion 贺卡成员变量

        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////

        #region 祝福卡成员变量

        private CountdownFontType _BlessingCountdown = null;

        /// <summary>
        /// 本地存储数据 key 值
        /// </summary>
        private string _DataKey = null;

        /// <summary>
        /// 祝福卡下标
        /// </summary>
        private int _CardIdx = 0;

        /// <summary>
        /// 祝福字下标
        /// </summary>
        private int _WordIdx = 0;

        private readonly string[] _CardsName = new string[3]
        {
            "bg_cj_qian_1",
            "bg_cj_qian_2",
            "bg_cj_qian_3"
        };

        private readonly string[] _WordsName = new string[9]
        {
            "bg_cj_qian_zi_1", "bg_cj_qian_zi_2", "bg_cj_qian_zi_4",
            "bg_cj_qian_zi_5", "bg_cj_qian_zi_6", "bg_cj_qian_zi_7",
            "bg_cj_qian_zi_8", "bg_cj_qian_zi_9", "bg_cj_qian_zi_10",
        };

        private readonly string[] _WordsAudio = new string[9]
        {
            "xwk_hd_ns_26", "xwk_hd_ns_31", "xwk_hd_ns_25",
            "xwk_hd_ns_32", "xwk_hd_ns_27", "xwk_hd_ns_28",
            "xwk_hd_ns_24", "xwk_hd_ns_30", "xwk_hd_ns_29",
        };

        private bool _HasSaved = false;

        private string _BlessingCardFileName = "XWK_BlessingCard.png";

        #endregion 祝福卡成员变量

        ///////////////////////////////////////////////////////////////////////////////////////////

        public void StartPlay(int which = 0)
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.TimeStart, RedBombConstStatistic.redbomb_game_duration);

            switch (which)
            {
                case 0:
                    EnterAni();
                    break;

                case 1:
                    EnterRainLayer();
                    break;

                case 2:
                    EnterCard();
                    break;

                case 3:
                    EnterBlessingPlayer();
                    break;

                default:
                    break;
            }
        }

        protected override void Init()
        {
            _DataKey = string.Concat(PlayerData.UID, "RedBomb");

            if (!C_UIMgr.c_UICameraHigh)
            {
                C_UIMgr UIMgr = C_UIMgr.Instance;//初始化UI相机
            }
            _UICamera = C_UIMgr.c_UICameraHigh;
            Canvas canvas = transform.Find("Canvas").GetComponent<Canvas>();
            canvas.worldCamera = _UICamera;

            _WinEffectGo = GameResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.WinEffectPath, false);
            _WinEffectGo = Instantiate(_WinEffectGo, _RainPlayer.transform);
            _WinEffectGo.SetActive(false);
            _FailEffectGo = GameResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.FailEffectPath, false);
            _FailEffectGo = Instantiate(_FailEffectGo, _RainPlayer.transform);
            _FailEffectGo.SetActive(false);
            _CoinFlyEffectGo = GameResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.CoinFlyEffectPath, false);
            _CoinFlyEffectGo = Instantiate(_CoinFlyEffectGo, _RainPlayer.transform);
            _CoinFlyEffectGo.SetActive(false);

            _ClickEffectPrefab = GameResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.ClickEffectPath, false);

            _RedBombPrefab = GameResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.RedBombPrefabPath, false);

            _ScoreBoard = _ScoreBoardGo.GetComponent<RedBombScoreBoard>();
        }

        #region 开场动画

        private void EnterAni()
        {
            _StartAniLayer.SetActive(true);

            //延迟几帧播放音乐，避免外界影响
            Invoke("PlayOpenAudio", 0.05f);

            _RedBombEffectGo = ABResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.RedBombEffectPath, "", false, true);
            _RedBombEffectGo = Instantiate(_RedBombEffectGo, _StartAniLayer.transform, false);
            Utility.SetTransformLayer(_RedBombEffectGo.transform, LayerMask.NameToLayer("UI"));
            _RedBombEffectGo.transform.localPosition = new Vector3(0, 0, 0);
            _RedBombEffectGo.transform.localScale = new Vector3(108, 108, 108);
            _RedBombEffectGo.SetActive(false);

            _MeshGo = ABResMgr.Instance.LoadResource<GameObject>(RedBombConstRes.RedBombMeshPath, "", false, true);
            _MeshGo = Instantiate(_MeshGo, _StartAniLayer.transform, true);
            _MeshGo.SetActive(true);

            var anim = ABResMgr.Instance.LoadResource<RuntimeAnimatorController>(RedBombConstRes.StartAnimatorPath, "", false);
            Animator animator = _MeshGo.GetComponent<Animator>();
            animator.runtimeAnimatorController = anim;
            AnimationClip[] clips = anim.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.Equals(RedBombConstRes.LoopAniClipName))
                {
                    _EndAniClip = clips[i];
                    _EndAniClip.BindCallback(_MeshGo, _EndAniClip.length, EnterRainLayer);
                    break;
                }
            }

            Camera modelCamera = Utility.FetchCamera("main_model_MainCamera");
            if (modelCamera == null)
            {
                C_DebugHelper.LogError("找不到模型相机");
            }

#if UNITY_EDITOR
            if (modelCamera == null)
            {
                GameObject cameraGo = _StartAniLayer.transform.Find("Camera").gameObject;
                cameraGo.SetActive(true);
                modelCamera = cameraGo.GetComponent<Camera>();
            }
#endif
            _MeshGo.transform.position = new Vector3(modelCamera.transform.position.x, 0, 0);
        }

        private void PlayOpenAudio()
        {
            //BGM
            AudioClip bgclip = ABResMgr.Instance.LoadResource<AudioClip>(string.Concat(RedBombConstRes.SoundPath, RedBombConstRes.BGM), BundleType);
            AudioManager.Instance.PlayBgMusic(bgclip, 0.5f);

            //开场语音
            int idx = Random.Range(0, RedBombConstRes.OpenAudio.Length);
            string soundpath = string.Concat(RedBombConstRes.BlessingAudioPath, RedBombConstRes.OpenAudio[idx]);
            RedBombUI.PlaySound(soundpath);
        }

        private void ExitStartAni()
        {
            transform.Find("Canvas/Bg").gameObject.SetActive(true);
            Image bgImg = transform.Find("Canvas/Bg").GetComponent<Image>();
            bgImg.DOFade(0, 0).OnComplete(() =>
            {
                bgImg.DOFade(0.7f, 0.25f);
            });

            if (_RedBombEffectGo)
            {
                _RedBombEffectGo.SetActive(true);//播放喷红包特效
            }
            if (_MeshGo)
            {
                if (_EndAniClip)
                {
                    _EndAniClip.UnbindCallback(_MeshGo, _EndAniClip.length, EnterRainLayer);
                }
                Animator animator = _MeshGo.GetComponent<Animator>();
                if (animator)
                {
                    animator.SetBool("end", true);
                }
            }
        }

        #endregion 开场动画

        #region 下红包雨阶段

        private void EnterRainLayer()
        {
            ExitStartAni();
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_game_enter);

            _UILayer.SetActive(true);
            _RainPlayer.SetActive(true);
            StartRainTimer();
        }

        private void StartRainTimer()
        {
            InvokeRepeating("CreateRedBombTimer", 0f, 1.0f);
            _RainCountdown = _RainCountdownGo.GetComponent<CountdownRedBomb>();
            _RainCountdown.SetEndAction(() =>
            {
                StopRainTimer();

                RainFinishAni();
            });
            _RainCountdown.StartTimer();
        }

        private void StopRainTimer()
        {
            if (_RainCountdown)
            {
                _RainCountdown.StopTimer();
            }
            CancelInvoke("CreateRedBombTimer");
            CancelInvoke("RedBombTimer");
        }

        /// <summary>
        /// 增加分数
        /// </summary>
        public void UpdateRedBombScore(int score)
        {
            _Score += score;
            _ScoreBoard.UpdateRedBombScore(score);
        }

        //控制红包数量和间隔
        private void CreateRedBombTimer()
        {
            int num = Random.Range(3, 5);
            //C_DebugHelper.Log("创建 " + num + " 个红包");

            float delay = 0f;
            for (int i = 0; i < num; i++)
            {
                delay += Random.Range(0f, 0.2f);
                Invoke("RedBombTimer", delay);
            }
        }

        private void RedBombTimer()
        {
            GameObject go = null;
            go = C_MonoSingleton<C_PoolMgr>.GetInstance().Spawn(RedBombUI.RedBombRes, C_PoolChannel.UI) as GameObject;
            if (go == null)
            {
                go = Instantiate(_RedBombPrefab, _RainLayer.transform);
            }
            if (!go.activeSelf)
            {
                go.SetActive(true);
            }
            RedBomb redBomb = go.GetComponent<RedBomb>();
            redBomb.Init();
            redBomb.Fly();
        }

        //播放结算动画，跳转到选贺卡阶段
        private void RainFinishAni()
        {
            if (_RedBombEffectGo)
            {
                _RedBombEffectGo.SetActive(false);
            }
            _StartAniLayer.SetActive(false);

            _RainSettlementLayer.SetActive(true);

            //大红包加数字
            _RainSettlementTextGo.SetActive(true);

            string audio = "";
            if (_Score > 0)
            {
                UI_Reward.RewardUIManager.Instance.ChangeModule(UI_Reward.ModuleType.Story, "redbomb_moudle_reward");

                _RainSettlementTextGo.GetComponent<Text>().text = string.Concat("x", _Score.ToString());

                //成功动画特效
                Invoke("PlayWinEffect", 2f);

                Invoke("RainWinFinish", 6.0f);

                RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_win);

                audio = RedBombConstRes.WinAudio;
            }
            else
            {
                _RainSettlementTextGo.GetComponent<Text>().text = "";

                Invoke("PlayFailEffect", 2f);

                Invoke("ExitRedBombPlayer", 5f);

                RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_fail);

                audio = RedBombConstRes.FailAudio;
            }

            RedBombUI.PlaySound(string.Concat(RedBombConstRes.BlessingAudioPath, audio));
        }

        private void PlayWinEffect()
        {
            _RainSettlementLayer.SetActive(false);
            _WinEffectGo.SetActive(true);
            //金币飞动画特效
            _CoinFlyEffectGo.SetActive(true);
            _CoinFlyEffectGo.transform.SetAsLastSibling();

            RedBombUI.PlaySoundEffect(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.WinSoundEffect));
        }

        private void PlayFailEffect()
        {
            //失败动画，退出整个玩法
            _FailEffectGo.SetActive(true);
            RedBombUI.PlaySoundEffect(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.FailSoundEffect));
        }

        private void RainWinFinish()
        {
            //删除创建的红包
            for (int i = _RainLayer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_RainLayer.transform.GetChild(i).gameObject);
            }

            int scoreI = _Score / 3;
            float scoreF = (float)_Score / 3f;
            if (scoreF - scoreI >= 0.5f)
            {
                scoreI++;
            }
            scoreI = Mathf.Max(scoreI, 1);//最低是1
            UI_Reward.RewardUIManager.Instance.UpdateScore(scoreI, UI_Reward.ModuleType.Story, "redbomb_moudle_reward");
            EnterCard();
        }

        #endregion 下红包雨阶段

        #region 选贺卡阶段

        private void EnterCard()
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_card_enter);

            _ScoreBoardGo.SetActive(false);
            _RainPlayer.SetActive(false);
            _CardPlayer.SetActive(true);

            AppearAction();

            //RedBombUI.PlaySound(string.Concat(RedBombConstRes.BlessingAudioPath, RedBombConstRes.CardAppearAudio));
        }

        //出现
        private void AppearAction()
        {
            _CardImages = _CardsGo.transform.GetComponentsInChildren<Image>();
            _ActionSeqList = new List<Sequence>(_CardImages.Length);

            for (int i = 0; i < _CardImages.Length; i++)
            {
                Image image = _CardImages[i];
                image.enabled = true;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

                Sequence sequence = DOTween.Sequence();
                sequence.InsertCallback((i + 1) * 0.0667f, () =>
                {
                    AudioManager.Instance.StopEffectByKey(RedBombConstRes.AppearSoundEffect);
                    RedBombUI.PlaySoundEffect(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.AppearSoundEffect));
                });
                sequence.Append(image.DOFade(1f, 0.2f));
                sequence.Join(image.transform.DOScale(1.12f, 0.2f));
                sequence.Append(image.transform.DOScale(1.05f, 0.2f));
                sequence.Append(image.transform.DOScale(1f, 0.2f));
                if (i == _CardImages.Length - 1)
                {
                    sequence.AppendCallback(() => { StartCardTimer(); });
                }
                sequence.Play();
                _ActionSeqList.Add(sequence);
            }
        }

        private void StartCardTimer()
        {
            _CardCountdownGo.SetActive(true);
            _CardCountdown = _CardCountdownGo.GetComponent<CountdownFontType>();
            _CardCountdown.SetEndAction(CardTimerEnd);
            _CardCountdown.StartTimer();
        }

        private void StopCardTimer()
        {
            if (_CardCountdown)
            {
                _CardCountdown.StopTimer();
            }
        }

        private void CardTimerEnd()
        {
            ChooseCardPlay(Random.Range(0, 3));
        }

        //Button 调用
        public void ChooseCard(int cardNo)
        {
            if (!_IsCardConfirmed)
            {
                //防止统计出错
                RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_card_click);
            }

            _CardCountdownGo.SetActive(false);
            ChooseCardPlay(cardNo);
        }

        private void ChooseCardPlay(int cardNo)
        {
            if (_IsCardConfirmed)
            {
                return;
            }
            _IsCardConfirmed = true;
            RedBombUI.PlaySoundEffect(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.ClickSoundEffect));

            _ChooseCardNo = cardNo;

            if (_CardImages != null && _CardImages.Length > 0)
            {
                ClickAction();
            }
        }

        private void ClickAction()
        {
            Image image = _CardImages[_ChooseCardNo];
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.transform.DOScale(0.92f, 0.17f));
            sequence.Append(image.transform.DOScale(1f, 0.16f));
            //sequence.Append(image.transform.DOShakeScale(0.33f, 0.92f, 10));
            sequence.AppendCallback(() => { MoveAction(); });
            sequence.Play();
            _ActionSeqList.Add(sequence);
        }

        private void MoveAction()
        {
            Image image = null;
            for (int i = 0; i < _CardImages.Length; i++)
            {
                image = _CardImages[i];
                if (i == _ChooseCardNo)
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(image.transform.DOMoveX(_CardsGo.transform.position.x, 0.5f));
                    //sequence.AppendInterval(0.29f);
                    //sequence.Append(image.transform.DOScale(0.92f, 0.2f));
                    //sequence.Join(image.DOFade(0f, 0.2f));
                    sequence.AppendCallback(() =>
                    {
                        RedBombUI.PlaySoundEffect(string.Concat(RedBombConstRes.SoundEffectPath, RedBombConstRes.OpenSoundEffect));
                        ScaleAction();
                    });
                    sequence.Play();
                    _ActionSeqList.Add(sequence);
                }
                else
                {
                    image.DOFade(0, 0.5f);
                }
            }
        }

        private void ScaleAction()
        {
            {
                Image chooseImage = _CardImages[_ChooseCardNo];
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.29f);
                sequence.Append(chooseImage.transform.DOScale(0.92f, 0.21f));
                sequence.Append(chooseImage.transform.DOScale(2.18f, 0.2f));
                sequence.Join(chooseImage.DOFade(0f, 0.2f));
                sequence.Play();
                _ActionSeqList.Add(sequence);
            }

            {
                Image highImage = _BigCardHighGo.GetComponent<Image>();
                highImage.enabled = true;
                highImage.color = new Color(highImage.color.r, highImage.color.g, highImage.color.b, 0f);
                Image lowImage = _BigCardLowGo.GetComponent<Image>();
                lowImage.enabled = true;
                lowImage.color = new Color(lowImage.color.r, lowImage.color.g, lowImage.color.b, 0f);

                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.67f);
                sequence.Append(highImage.DOFade(1f, 0.25f));
                sequence.Join(lowImage.DOFade(1f, 0.25f));
                sequence.InsertCallback(0.92f, () => { EnterBlessingPlayer(); });
                sequence.AppendInterval(0.17f);
                sequence.Append(highImage.rectTransform.DOAnchorPosY(1000f, 0.5f));
                sequence.Join(lowImage.rectTransform.DOAnchorPosY(-1000f, 0.5f));
                sequence.AppendCallback(() => { _CardPlayer.SetActive(false); });
                sequence.Play();
                _ActionSeqList.Add(sequence);
            }
        }

        #endregion 选贺卡阶段

        #region 祝福卡阶段

        private void EnterBlessingPlayer()
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_blessing_enter);

            _BlessingPlayer.SetActive(true);
            _ScreenShotGo.SetActive(true);

            StartBlessingTimer();
            ComposeBlessingCard();
        }

        private void StartBlessingTimer()
        {
            _BlessingCountdown = _BlessingCountdownGo.GetComponent<CountdownFontType>();
            _BlessingCountdown.SetEndAction(() => { ExitRedBombPlayer(); });
            _BlessingCountdown.StartTimer();
        }

        private void StopBlessingTimer()
        {
            if (_BlessingCountdown)
            {
                _BlessingCountdown.StopTimer();
            }
        }

        //按顺序发放组合贺卡
        private void ComposeBlessingCard()
        {
            DealCardOrder();
            Sprite CardSp = GameResMgr.Instance.LoadResource<Sprite>(string.Concat(RedBombConstRes.BlessingImgPath, _CardsName[_CardIdx], ".png"), false);
            Sprite WordSp = GameResMgr.Instance.LoadResource<Sprite>(string.Concat(RedBombConstRes.BlessingImgPath, _WordsName[_WordIdx], ".png"), false);

            Image cardImg = _ScreenShotCardGo.GetAddComponent<Image>();
            cardImg.enabled = true;
            cardImg.sprite = CardSp;
            _FakeCardImg.sprite = CardSp;

            Image wordImg = _ScreenShotWordGo.GetAddComponent<Image>();
            wordImg.enabled = true;
            wordImg.sprite = WordSp;
            _FakeWordImg.sprite = WordSp;

            RedBombUI.PlaySound(string.Concat(RedBombConstRes.BlessingAudioPath, RedBombConstRes.CardGetAudio), () =>
            {
                RedBombUI.PlaySound(string.Concat(RedBombConstRes.BlessingAudioPath, _WordsAudio[_WordIdx]));
            });
        }

        //Butoon 调用
        public void SaveButoon()
        {
            if (_HasSaved)
            {
                return;
            }
            _HasSaved = true;
            CommonUIMgr.Instance.CloseUI(_BlessingPlayer.transform.Find("Save").gameObject);

            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_blessing_save);
            _BlessingSaveButtonIconGo.SetActive(false);
            Vector2 pos = _UICamera.WorldToScreenPoint(_BlessingSaveButtonIconGo.transform.position);
            UI_Reward.RewardUIManager.Instance.RegisterOfflineBonus(3, pos, UI_Reward.ModuleType.Story, null, "redbomb_moudle_reward");
            UI_Reward.RewardUIManager.Instance.SetSuccess();

            StopBlessingTimer();
            CreateScreenShot();

            StartCoroutine(SaveEndTimer());
        }

        private IEnumerator SaveEndTimer()
        {
            yield return new WaitForSeconds(3f);
            ExitRedBombPlayer();
        }

        //创建贺卡
        private void CreateScreenShot()
        {
            _BlessingPlayer.SetActive(false);

            StartCoroutine(CreateBlessingCardCamera(_BlessingCardFileName));
        }

        /*
        private IEnumerator CreateBlessingCard()
        {
            yield return new WaitForEndOfFrame();

            float ratio = Screen.height / 1080f;
            C_DebugHelper.LogError(ratio);

            RectTransform rectTransform = _BlessingCardGo.GetComponent<RectTransform>();
            Vector2 screen = RectTransformUtility.WorldToScreenPoint(_UICamera, _BlessingCardGo.transform.position);
            Vector2 pos = screen - new Vector2(rectTransform.rect.width * rectTransform.pivot.x, rectTransform.rect.height * rectTransform.pivot.y) * ratio;
            C_DebugHelper.LogError(pos);

            Image image = _BlessingCardGo.GetComponent<Image>();
            Vector2 realSize = image.sprite.rect.size * ratio;

            // 创建一个纹理
            Texture2D texture = new Texture2D((int)realSize.x, (int)realSize.y, TextureFormat.RGBA32, false);
            // 读取内容到纹理图片中
            //texture.wrapMode = TextureWrapMode.Clamp;
            texture.ReadPixels(new Rect(pos, realSize), 0, 0, false);
            texture.Apply();
            //Destroy(texture);

            //拉大纹理，会糊
            //Texture2D texture2 = new Texture2D(image.mainTexture.width, image.mainTexture.height, TextureFormat.RGBA32, false);
            //Color newColor = Color.clear;
            //for (int i = 0; i < texture2.height; ++i)
            //{
            //    for (int j = 0; j < texture2.width; ++j)
            //    {
            //        newColor = texture.GetPixelBilinear((float)j / (float)texture2.width, (float)i / (float)texture2.height);
            //        texture2.SetPixel(j, i, newColor);
            //    }
            //}

            //// 保存前面对纹理的修改
            //texture2.Apply();
            // 编码纹理为PNG格式
            byte[] bytes = texture.EncodeToPNG();
            // 销毁没用的图片纹理
            Destroy(texture);

            if (Application.platform == RuntimePlatform.Android)
            {
                // 这个路径会将图片保存到手机的沙盒中，这样就可以在手机上对其进行读写操作了
                File.WriteAllBytes(Application.persistentDataPath + "/test.png", bytes);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string path = "C:\\Users\\ZZW\\Desktop\\RedBomb";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(string.Concat(path, "/test.png"), bytes);
            }
        }
        */

        //截图
        private IEnumerator CreateBlessingCardCamera(string filename)
        {
            yield return new WaitForEndOfFrame();

            Image image = _ScreenShotCardGo.GetComponent<Image>();
            RenderTexture renderTexture = new RenderTexture((int)image.sprite.rect.width, (int)image.sprite.rect.height, 0);
            _ScreenShotCamera.targetTexture = renderTexture;
            _ScreenShotCamera.Render();
            RenderTexture.active = renderTexture;
            // 创建一个纹理
            Texture2D texture = new Texture2D((int)image.sprite.rect.width, (int)image.sprite.rect.height, TextureFormat.RGBA32, false);
            // 读取内容到纹理图片中
            texture.ReadPixels(image.sprite.rect, 0, 0);
            // 保存前面对纹理的修改
            texture.Apply();
            _ScreenShotCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // 编码纹理为PNG格式
            byte[] bytes = texture.EncodeToPNG();
            // 销毁没用的图片纹理
            Destroy(texture);

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //string filePath = string.Concat(Application.persistentDataPath, "/", filename);
                //File.WriteAllBytes(filePath, bytes);
                try
                {
                    string base64String = System.Convert.ToBase64String(bytes);
                    C_DebugHelper.Log("获取当前图片base64长度为---" + base64String.Length);
                    GameHelper.Instance.AddToDCIM(ref base64String);
                }
                catch (System.Exception e)
                {
                    C_DebugHelper.LogError("ImgToBase64String 转换失败:" + e.Message);
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string path = "D:\\RedBomb\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(string.Concat(path, filename), bytes);
            }
        }

        //按顺序发放祝福卡
        private void DealCardOrder()
        {
            string data = PlayerPrefs.GetString(_DataKey, null);
            if (!string.IsNullOrEmpty(data))
            {
                string[] numbers = data.Split('_');
                if (numbers.Length == 2)
                {
                    _CardIdx = int.Parse(numbers[0]);
                    _WordIdx = int.Parse(numbers[1]);
                    if (_WordIdx < _WordsName.Length - 1)
                    {
                        _WordIdx++;
                    }
                    else
                    {
                        _WordIdx = 0;
                        if (_CardIdx < _CardsName.Length - 1)
                        {
                            _CardIdx++;
                        }
                        else
                        {
                            _CardIdx = 0;
                        }
                    }
                }
                else
                {
                    C_DebugHelper.LogError(string.Concat("红包存储数据格式内容出错:", data));
                    //强制赋值
                    _CardIdx = 0;
                    _WordIdx = 0;
                }
            }
            else
            {
                C_DebugHelper.Log("红包存储数据为空");
                _CardIdx = 0;
                _WordIdx = 0;
            }
            RecordPlayerData();
            _BlessingCardFileName = string.Concat("XWK_BlessingCard", _CardIdx.ToString(), "_", _WordIdx.ToString(), ".png");
        }

        private void RecordPlayerData()
        {
            string data = string.Concat(_CardIdx, "_", _WordIdx);
            C_DebugHelper.Log(string.Concat("红包存储数据:", data));
            PlayerPrefs.SetString(_DataKey, data);
        }

        #endregion 祝福卡阶段

        //清除动作，以防被打断后报错
        private void ClearCardAction()
        {
            if (_ActionSeqList != null && _ActionSeqList.Count > 0)
            {
                for (int i = 0; i < _ActionSeqList.Count; i++)
                {
                    if (_ActionSeqList[i] != null)
                    {
                        _ActionSeqList[i].Kill();
                    }
                }
                _ActionSeqList.Clear();
            }
        }

        //返回按钮调用
        public void ExitButton()
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_game_return);
            ExitRedBombPlayer();
        }

        //祝福卡返回按钮调用
        public void ExitButtonBlessing()
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, RedBombConstStatistic.redbomb_blessing_return);

            ExitRedBombPlayer();
        }

        /// <summary>
        /// 退出玩法
        /// </summary>
        private void ExitRedBombPlayer()
        {
            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.Chick, string.Concat(RedBombConstStatistic.redbomb_game_click, _Score.ToString()));

            RedBombConstStatistic.SendDataStatistics(EnumDataStatistics.TimeEnd, RedBombConstStatistic.redbomb_game_duration);
            AudioManager.Instance.StopBgMusic();

            StopRainTimer();

            StopCardTimer();

            StopBlessingTimer();

            Destroy(gameObject);

            C_EventHandler.SendEvent(C_EnumEventChannel.Global, "MainGameEvent", YB.XWK.MainScene.GameEventEnum.GAME_EVENT_ENUM_RESUME_GAME);
        }

        protected override void OnDestroy()
        {
            StopCoroutine(SaveEndTimer());
            ClearCardAction();
            CancelInvoke("ExitRedBombPlayer");
            CancelInvoke("RedBombTimer");
            CancelInvoke("PlayWinEffect");
            CancelInvoke("RainWinFinish");
            CancelInvoke("PlayFailEffect");

            _ClickEffectPrefab = null;
            _RedBombPrefab = null;
            Resources.UnloadUnusedAssets();
            base.OnDestroy();
        }

        private static void PlaySound(string path, System.Action action = null)
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(path, BundleType);
            //C_DebugHelper.LogError(clip);
            AudioManager.Instance.PlayerSoundByClip(clip, action);
        }

        private static void PlaySoundEffect(string path, bool loop = false)
        {
            AudioClip clip = ABResMgr.Instance.LoadResource<AudioClip>(path, BundleType);
            //C_DebugHelper.LogError(clip);
            AudioManager.Instance.PlayEffectClipSound(clip, loop);
        }
    }
}