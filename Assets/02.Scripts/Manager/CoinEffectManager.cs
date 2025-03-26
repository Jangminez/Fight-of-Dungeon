using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinEffectManager : MonoBehaviour
{
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite diaIcon;
    [SerializeField] private GameObject PileofCoinParent;
    private Vector3[] InitialPos;
    private Quaternion[] InitialRotation;
    private Vector2 coinAnchorPos;
    void Start()
    {
        GameManager.Instance.coinEffect = this;

        InitialPos = new Vector3[PileofCoinParent.transform.childCount];
        InitialRotation = new Quaternion[PileofCoinParent.transform.childCount];

        for (int i = 0; i < PileofCoinParent.transform.childCount; i++)
        {
            InitialPos[i] = PileofCoinParent.transform.GetChild(i).position;
            InitialRotation[i] = PileofCoinParent.transform.GetChild(i).rotation;
        }
    }

    private void ResetCoins()
    {
        for (int i = 0; i < PileofCoinParent.transform.childCount; i++)
        {
            PileofCoinParent.transform.GetChild(i).position = InitialPos[i];
            PileofCoinParent.transform.GetChild(i).rotation = InitialRotation[i];
        }
    }

    /// <summary>
    /// 코인 지급 시 이펙트를 위한 함수
    /// </summary>
    /// <param name="pre_Coin"></param>
    /// <param name="next_Coin"></param>
    /// <param name="coinType"> 0 = Gold, 1 = Dia</param>
    public void RewardPileOfCoin(int pre_Coin, int next_Coin, int coinType)
    {
        ResetCoins();

        var delay = 0f;
        Sprite coinSprite = null;

        PileofCoinParent.SetActive(true);

        switch (coinType)
        {
            case 0:
                coinSprite = goldIcon;
                coinAnchorPos = new Vector2(-200f, 1720f);
                break;

            case 1:
                coinSprite = diaIcon;
                coinAnchorPos = new Vector2(110f, 1720f);
                break;
        }

        for (int i = 0; i < PileofCoinParent.transform.childCount; i++)
        {
            PileofCoinParent.transform.GetChild(i).GetComponent<Image>().sprite = coinSprite;

            PileofCoinParent.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            PileofCoinParent.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(coinAnchorPos, 1f)
            .SetDelay(delay + 0.5f).SetEase(Ease.InBack);

            PileofCoinParent.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);

            PileofCoinParent.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        StartCoroutine(SetCoin(pre_Coin, next_Coin, coinType));
    }

    IEnumerator SetCoin(int pre_Coin, int next_Coin, int coinType)
    {
        yield return new WaitForSecondsRealtime(1f);

        float timer = 0f;
        float duration = 2f;

        switch (coinType)
        {
            case 0:
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    GameManager.Instance.Gold = (int)Mathf.Lerp(pre_Coin, next_Coin, timer / duration);
                    yield return null;
                }

                GameManager.Instance.Gold = next_Coin;
                break;

            case 1:
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    GameManager.Instance.Dia = (int)Mathf.Lerp(pre_Coin, next_Coin, timer / duration);
                    yield return null;
                }

                GameManager.Instance.Dia = next_Coin;
                break;
        }
    }

    public IEnumerator SetExp(float pre_Exp, float next_Exp)
    {
        float timer = 0f;
        float duration = 2f;

        while (timer < duration)
                {
                    timer += Time.deltaTime;
                    GameManager.Instance.Exp = (int)Mathf.Lerp(pre_Exp, next_Exp, timer / duration);
                    yield return null;
                }

                GameManager.Instance.Exp = next_Exp;
    }
}
