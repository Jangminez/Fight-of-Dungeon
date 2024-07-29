using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAbility : MonoBehaviour
{
    [Header("Upgrade Button")]
    [SerializeField] private Button _attackBtn;
    [SerializeField] private Button _attackSpeedBtn;
    [SerializeField] private Button _criticalBtn;
    [SerializeField] private Button _maxHpBtn;
    [SerializeField] private Button _hpGenerationBtn;
    [SerializeField] private Button _defenseBtn;
    [SerializeField] private Button _maxMpBtn;
    [SerializeField] private Button _mpGenerationBtn;

    private Player _player;

    private void Awake()
    {
        // 플레이어 찾기
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // 각 버튼 이벤트 연결
        _attackBtn.onClick.AddListener(UpAttack);
        _attackSpeedBtn.onClick.AddListener(UpAttackSpeed);
        _criticalBtn.onClick.AddListener(UpCritical);
        _maxHpBtn.onClick.AddListener(UpMaxHp);
        _hpGenerationBtn.onClick.AddListener(UpHpGeneration);
        _defenseBtn.onClick.AddListener(UpDefense);
        _maxMpBtn.onClick.AddListener(UpMaxMp);
        _mpGenerationBtn.onClick.AddListener(UpMpGeneration);
    }

    private void UpAttack()
    {
        Debug.Log("공격 업!");
    }

    private void UpAttackSpeed()
    {
        Debug.Log("공격속도 업!");
    }

    private void UpCritical()
    {
        Debug.Log("크리티컬 확률 업!");
    }

    private void UpMaxHp()
    {
        Debug.Log("최대 체력 업!");
    }

    private void UpHpGeneration()
    {
        Debug.Log("체력 재생속도 업!");
    }

    private void UpDefense()
    {
        Debug.Log("방어력 업!");
    }

    private void UpMaxMp()
    {
        Debug.Log("최대 마나 업!");
    }

    private void UpMpGeneration()
    {
        Debug.Log("마나 재생속도 업!");
    }

}
