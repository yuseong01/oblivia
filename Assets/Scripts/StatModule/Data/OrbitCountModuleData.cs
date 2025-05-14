// �̵� �ӵ� ����
using UnityEngine;

[CreateAssetMenu(menuName = "StatModules/Orbit Count")]
public class OrbitCountModuleData : StatModuleData
{
    public int OrbitCount = 0;
    public override IStatModule CreateInstance()
    {
        return new OrbitCountModule(this);
    }
}