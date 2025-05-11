// ���� ��Ÿ�(�ν� ����)
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Attack Range")]
public class AttackRangeModuleData : StatModuleData
{
    public float Range = 1;
    public override IStatModule CreateInstance()
    {
        return new AttackRangeModule(this);
    }
}