using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSystemController : MonoBehaviour
{
    [Tooltip("Unit System View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private UnitSystemView _View;

    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }
}
