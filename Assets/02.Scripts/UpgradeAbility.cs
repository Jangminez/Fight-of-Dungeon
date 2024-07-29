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
        // �÷��̾� ã��
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // �� ��ư �̺�Ʈ ����
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
        Debug.Log("���� ��!");
    }

    private void UpAttackSpeed()
    {
        Debug.Log("���ݼӵ� ��!");
    }

    private void UpCritical()
    {
        Debug.Log("ũ��Ƽ�� Ȯ�� ��!");
    }

    private void UpMaxHp()
    {
        Debug.Log("�ִ� ü�� ��!");
    }

    private void UpHpGeneration()
    {
        Debug.Log("ü�� ����ӵ� ��!");
    }

    private void UpDefense()
    {
        Debug.Log("���� ��!");
    }

    private void UpMaxMp()
    {
        Debug.Log("�ִ� ���� ��!");
    }

    private void UpMpGeneration()
    {
        Debug.Log("���� ����ӵ� ��!");
    }

}
