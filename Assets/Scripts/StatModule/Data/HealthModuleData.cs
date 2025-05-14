// ���� ü�� ����
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Health")]
public class HealthModuleData : StatModuleData
{
    public float Health = 1;
    public override IStatModule CreateInstance()
    {
        return new HealthModule(this);
    }
}