using Assets.Scripts.C_Framework;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XWK.Common.UI_Reward
{
    internal class RewardStarTrail
    {
        protected List<RectTransform> _StarsList = null;

        ////加分回调(分数增加时间和动画播放时间不一致)
        //protected Action UpdateScoreCallback = null;
        //播放完回调
        protected Action PlayEndCallback = null;

        protected int Score = 0;

        public RewardStarTrail(List<RectTransform> starsList, Action callback, int score)
        {
            _StarsList = starsList;
            PlayEndCallback = callback;
            Score = score;
        }

        public virtual void Play()
        {
            //子类实现
            C_DebugHelper.LogError("RewardStarTrail 子类未实现 Play 方法");
        }

        //protected void UpdateScore()
        //{
        //    if (UpdateScoreCallback != null)
        //    {
        //        Action action = UpdateScoreCallback;
        //        action();
        //        UpdateScoreCallback = null;
        //    }
        //}

        protected void PlayEnd()
        {
            if (PlayEndCallback != null)
            {
                Action action = PlayEndCallback;
                action();
                PlayEndCallback = null;
            }
        }

        public virtual void SetStartPos(Vector2 pos)
        {
            //子类实现
        }
    }

    internal class RewardStarTrailNormal : RewardStarTrail
    {
        //动作记录，用于退出或打断时的清理
        private List<Sequence> _EnterSeqList = null;

        private List<Sequence> _PerformanceSeqList = null;

        public RewardStarTrailNormal(List<RectTransform> starsList, Action callback, int score) : base(starsList, callback, score)
        {
            _EnterSeqList = new List<Sequence>();
            _PerformanceSeqList = new List<Sequence>();
        }

        ~RewardStarTrailNormal()
        {
            ClearAction();
        }

        public override void Play()
        {
            StarEnterScene();
        }

        private void StarEnterScene()
        {
            //TODO  改用unity自动布局组件
            //星星入场位置选择
            bool isOdd = _StarsList.Count % 2 == 0 ? false : true;
            for (int i = 0; i < _StarsList.Count; i++)
            {
                Vector2 pos = Vector2.zero;
                float index = 0.0f;
                if (isOdd)
                {
                    if (_StarsList.Count == 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = (float)i / (float)(_StarsList.Count - 1) - 0.5f;//奇数星星有偶数个间隔
                    }
                }
                else
                {
                    index = ((float)i + 0.5f) / (float)_StarsList.Count - 0.5f;//偶数个间隔再加使其居中的偏移
                }
                pos = RewardUI.Instance.EnterPos.anchoredPosition + new Vector2(index * 600.0f, -Mathf.Abs(index) * 200.0f);

                RewardStar starCom = _StarsList[i].GetAddComponent<RewardStar>();

                Sequence sequence = DOTween.Sequence();
                sequence.InsertCallback(0f, () =>
                {
                    AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_086.ogg");
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(true);
                    }
                });
                sequence.AppendInterval(Mathf.Min(i * 0.1f, 0.02f));
                sequence.Append(_StarsList[i].DOAnchorPos(pos + new Vector2(0, 50.0f), 0.85f));
                sequence.Append(_StarsList[i].DOAnchorPos(pos, 0.15f));
                sequence.AppendCallback(() =>
                {
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(false);
                    }
                });
                if (i == _StarsList.Count - 1)
                {
                    sequence.OnComplete(Performance);
                }
                sequence.Play();
                _EnterSeqList.Add(sequence);
            }
        }

        private void Performance()
        {
            //TODO 贝塞尔曲线

            Vector2 endPos;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(RewardUI.Instance.Camera, RewardUI.Instance.EndPos.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RewardUI.Instance.Canvas, screenPos, RewardUI.Instance.Camera, out endPos);

            for (int i = 0; i < _StarsList.Count; i++)
            {
                RewardStar starCom = _StarsList[i].GetAddComponent<RewardStar>();
                Sequence sequence = DOTween.Sequence();
                float delay = 0f;
                delay += (float)i / (float)_StarsList.Count * 0.5f;
                sequence.AppendInterval(delay);
                sequence.InsertCallback(delay, () =>
                {
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(true, 1.5f, true);//打开星星动画和特效
                        starCom.SetTrailingEffects(true);
                    }
                });
                delay += 0.5f;
                sequence.Append(_StarsList[i].DOAnchorPos(_StarsList[i].anchoredPosition + new Vector2(0, -100.0f), 0.5f));//往下掉
                delay += 0.5f;
                sequence.Append(_StarsList[i].DOAnchorPos(endPos, 0.5f));//往上飞
                sequence.InsertCallback(delay, () =>
                {
                    AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_084.ogg");
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(false);//关闭星星动画
                    }
                });
                delay += 0.15f;
                sequence.Append(_StarsList[i].DOScale(0.02f, 0.15f));//飞到位置变小

                //最后一个星星
                if (i == _StarsList.Count - 1)
                {
                    GameObject scoreicon_t = RewardUI.Instance.Score.Find("scoreicon_t").gameObject;

                    sequence.InsertCallback(delay, () =>
                    {
                        RewardUI.Instance.UpdateScore(Score);//TODO
                        if (RewardUI.Instance.Score)
                        {
                            if (scoreicon_t)
                            {
                                //C_DebugHelper.LogError("555+" + delay);   //此处delay跟外部数值不同   //TODO  C#闭包
                                scoreicon_t.transform.SetAsLastSibling();
                            }
                        }
                    });

                    if (scoreicon_t)
                    {
                        string colorName = "_TintColor";
                        Color color = Color.white;
                        Material material = scoreicon_t.GetComponent<Image>().material;
                        sequence.InsertCallback(delay, () =>
                        {
                            if (material.HasProperty(colorName))//激活物体和获取shader属性需要隔一帧
                            {
                                color = material.GetColor(colorName);
                                material.SetColor(colorName, new Color(color.r, color.g, color.b, 0f));
                            }
                            else
                            {
                                C_DebugHelper.LogError("着色器有错");
                            }
                        });

                        delay += 0.1f;
                        sequence.Append(material.DOColor(new Color(color.r, color.g, color.b, 1f), colorName, 0.1f));
                        sequence.Join(scoreicon_t.GetComponent<RectTransform>().DOScale(1.25f, 0.1f));

                        delay += 0.1f;
                        sequence.Append(material.DOColor(new Color(color.r, color.g, color.b, 0f), colorName, 0.1f));
                        sequence.Join(scoreicon_t.GetComponent<RectTransform>().DOScale(1f, 0.1f));

                        sequence.InsertCallback(delay, () =>
                        {
                            scoreicon_t.transform.SetAsFirstSibling();
                        });
                    }
                }
                sequence.AppendInterval(1.0f);//等一秒特效
                if (i == _StarsList.Count - 1)
                {
                    sequence.OnComplete(PlayEnd);
                }
                sequence.Play();
                _PerformanceSeqList.Add(sequence);
            }
        }

        private void ClearAction()
        {
            if (_EnterSeqList != null && _EnterSeqList.Count > 0)
            {
                int count = _EnterSeqList.Count;
                for (int i = 0; i < count; i++)
                {
                    _EnterSeqList[i].Kill();
                }
                _EnterSeqList.Clear();
                _EnterSeqList = null;
            }
            if (_PerformanceSeqList != null && _PerformanceSeqList.Count > 0)
            {
                int count = _PerformanceSeqList.Count;
                for (int i = 0; i < count; i++)
                {
                    _PerformanceSeqList[i].Kill();
                }
                _PerformanceSeqList.Clear();
                _PerformanceSeqList = null;
            }
        }
    }

    internal class RewardStarTrailOfflineBonus : RewardStarTrail
    {
        //动作记录，用于退出或打断时的清理
        private List<Sequence> _PerformanceSeqList = null;

        private Vector2 _StartPos = Vector2.zero;

        public RewardStarTrailOfflineBonus(List<RectTransform> starsList, Action callback, int score) : base(starsList, callback, score)
        {
            _PerformanceSeqList = new List<Sequence>();
            if (starsList.Count > 1)
            {
                C_DebugHelper.LogError("星星数量大于1，错误");
            }
        }

        ~RewardStarTrailOfflineBonus()
        {
            ClearAction();
        }

        public override void Play()
        {
            Performance();
        }

        public override void SetStartPos(Vector2 pos)
        {
            _StartPos = pos;
        }

        private void Performance()
        {
            //TODO 贝塞尔曲线

            Vector2 startPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RewardUI.Instance.Canvas, _StartPos, RewardUI.Instance.Camera, out startPos);

            Vector2 endPos;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(RewardUI.Instance.Camera, RewardUI.Instance.EndPos.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RewardUI.Instance.Canvas, screenPos, RewardUI.Instance.Camera, out endPos);

            for (int i = 0; i < _StarsList.Count; i++)
            {
                _StarsList[i].anchoredPosition = startPos;

                RewardStar starCom = _StarsList[i].GetAddComponent<RewardStar>();
                float delay = 0f;
                delay += (float)i / (float)_StarsList.Count * 0.5f;
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(delay);
                sequence.InsertCallback(delay, () =>
                {
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(true, 1.5f, true);//打开星星动画和特效
                        starCom.SetTrailingEffects(true);
                    }
                });
                delay += 0.5f;
                sequence.Append(_StarsList[i].DOAnchorPos(_StarsList[i].anchoredPosition + new Vector2(0, -100.0f), 0.5f));//往下掉
                delay += 0.5f;
                sequence.Append(_StarsList[i].DOAnchorPos(endPos, 0.5f));//往上飞
                sequence.InsertCallback(delay, () =>
                {
                    AudioManager.Instance.PlayEffectAutoClose("public/sound_effect/public_xwkyx_084.ogg");
                    if (starCom)
                    {
                        starCom.SetCoinAnimator(false);//关闭星星动画
                    }
                });
                delay += 0.15f;
                sequence.Append(_StarsList[i].DOScale(0.02f, 0.15f));//飞到位置变小

                //最后一个星星
                if (i == _StarsList.Count - 1)
                {
                    GameObject scoreicon_t = RewardUI.Instance.Score.Find("scoreicon_t").gameObject;

                    sequence.InsertCallback(delay, () =>
                    {
                        RewardUI.Instance.UpdateScore(Score);//TODO
                        if (RewardUI.Instance.Score)
                        {
                            if (scoreicon_t)
                            {
                                //C_DebugHelper.LogError("555+" + delay);   //此处delay跟外部数值不同   //TODO  C#闭包
                                scoreicon_t.transform.SetAsLastSibling();
                            }
                        }
                    });

                    if (scoreicon_t)
                    {
                        string colorName = "_TintColor";
                        Color color = Color.white;
                        Material material = scoreicon_t.GetComponent<Image>().material;
                        sequence.InsertCallback(delay, () =>
                        {
                            if (material.HasProperty(colorName))//激活物体和获取shader属性需要隔一帧
                            {
                                color = material.GetColor(colorName);
                                material.SetColor(colorName, new Color(color.r, color.g, color.b, 0f));
                            }
                            else
                            {
                                C_DebugHelper.LogError("着色器有错");
                            }
                        });

                        delay += 0.1f;
                        sequence.Append(material.DOColor(new Color(color.r, color.g, color.b, 1f), colorName, 0.1f));
                        sequence.Join(scoreicon_t.GetComponent<RectTransform>().DOScale(1.25f, 0.1f));

                        delay += 0.1f;
                        sequence.Append(material.DOColor(new Color(color.r, color.g, color.b, 0f), colorName, 0.1f));
                        sequence.Join(scoreicon_t.GetComponent<RectTransform>().DOScale(1f, 0.1f));

                        sequence.InsertCallback(delay, () =>
                        {
                            scoreicon_t.transform.SetAsFirstSibling();
                        });
                    }
                }
                sequence.AppendInterval(1.0f);//等一秒特效
                if (i == _StarsList.Count - 1)
                {
                    sequence.OnComplete(PlayEnd);
                }
                sequence.Play();
                _PerformanceSeqList.Add(sequence);
            }
        }

        private void ClearAction()
        {
            if (_PerformanceSeqList != null && _PerformanceSeqList.Count > 0)
            {
                int count = _PerformanceSeqList.Count;
                for (int i = 0; i < count; i++)
                {
                    _PerformanceSeqList[i].Kill();
                }
                _PerformanceSeqList.Clear();
                _PerformanceSeqList = null;
            }
        }
    }
}