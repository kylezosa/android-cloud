using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PADTest : MonoBehaviour
{
    [SerializeField] private List<AssetReference> m_assetRefList = new List<AssetReference>();

    private void Start()
    {
        for (int i = 0; i < m_assetRefList.Count; i++)
        {

        }
    }
}
