using UnityEngine;

namespace XWK.Common.UI_Reward
{
    internal class RewardStar : MonoBehaviour
    {
        [SerializeField]
        [Header("金币动画")]
        private Animator _CoinAnimator;

        private GameObject _AwardTrailingEffects;

        private void Awake()
        {
            _AwardTrailingEffects = Instantiate(RewardUI.Instance.StarEffectPrefab, transform);
            _AwardTrailingEffects.transform.localScale = new Vector2(1, 1);
            _AwardTrailingEffects.SetActive(false);
            _CoinAnimator.speed = 0f;
        }

        public void SetTrailingEffects(bool active)
        {
            _AwardTrailingEffects.SetActive(active);
            if (active)
            {
                _AwardTrailingEffects.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                _AwardTrailingEffects.GetComponent<ParticleSystem>().Stop();
            }
        }

        public void SetCoinAnimator(bool play, float speed = 1f, bool ishalf = false)
        {
            if (play)
            {
                if (ishalf)
                {
                    _CoinAnimator.Play("Half");
                }
                else
                {
                    _CoinAnimator.Play("Play");
                }
                _CoinAnimator.speed = speed;
            }
            else
            {
                _CoinAnimator.speed = 0f;
            }
        }
    }
}