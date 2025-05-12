using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Xml.Linq;
public abstract class UI_Base : MonoBehaviour
{
    public int canvasLayer;
    Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();
    public abstract void Init();
    private void Start()
    {
        Init();
    }
    // ���ϴ� ������Ʈ�� ��ųʸ��� ���
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //Ÿ�� �̸� �迭
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[name.Length];
        this.objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // ������Ʈ ã��
            objects[i] = FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
                Debug.Log($"Failed to bind{names[i]}");
        }
    }
    // ���ϴ� ������Ʈ�� ��ųʸ����� ����
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] obj = null;
        if (objects.TryGetValue(typeof(T), out obj) == false)
            return null;

        return obj[idx] as T;
    }


    // �ش� �̸��� ���õ� ������Ʈ T �� ���� ������Ʈ�� ã�� �Լ�
    public static T FindChild<T>(GameObject go, string name = null, bool recuresive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recuresive == false) // �ڽ��� �ڽı��� �����ΰ�
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
