using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject PileofGoldParent;
    [SerializeField] private Text Counter;
    private Vector3[] InitialPos;
    private Quaternion[] InitialRotation;
    void Start()
    {
        GameManager.Instance.coinEffect = this;

        InitialPos = new Vector3[PileofGoldParent.transform.childCount];
        InitialRotation = new Quaternion[PileofGoldParent.transform.childCount];

        for (int i = 0; i < PileofGoldParent.transform.childCount; i++)
        {
            InitialPos[i] = PileofGoldParent.transform.GetChild(i).position;
            InitialRotation[i] = PileofGoldParent.transform.GetChild(i).rotation;
        }
    }

    private void ResetGolds()
    {
        for (int i = 0; i < PileofGoldParent.transform.childCount; i++)
        {
            PileofGoldParent.transform.GetChild(i).position = InitialPos[i];
            PileofGoldParent.transform.GetChild(i).rotation = InitialRotation[i];
        }
    }

    public void RewardPileOfGold(int pre_Gold, int next_Gold)
    {
        ResetGolds();

        var delay = 0f;

        PileofGoldParent.SetActive(true);

        for (int i = 0; i < PileofGoldParent.transform.childCount; i++)
        {
            PileofGoldParent.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            PileofGoldParent.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(-200f, 1720f), 1f)
            .SetDelay(delay + 0.5f).SetEase(Ease.InBack);

            PileofGoldParent.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);

            PileofGoldParent.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        StartCoroutine(SetGold(pre_Gold, next_Gold));
    }

    IEnumerator SetGold(int pre_Gold, int next_Gold)
    {
        yield return new WaitForSecondsRealtime(1f);
        
        float timer = 0f;
        float duration = 2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            GameManager.Instance.Gold = (int)Mathf.Lerp(pre_Gold, next_Gold, timer / duration);
            yield return null;
        }

        GameManager.Instance.Gold = next_Gold;
    }
}
