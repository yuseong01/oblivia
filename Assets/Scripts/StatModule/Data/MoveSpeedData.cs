// �̵� �ӵ� ����
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Move Speed")]
public class MoveSpeedData : StatModuleData
{
    public float MoveSpeed = 1;
    public override IStatModule CreateInstance()
    {
        return new MoveSpeedModule(this);
    }
}