using UnityEngine;

public class TurretView : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _muzzle;

    public Transform Pivot => _pivot;
    public Transform Muzzle => _muzzle;
}
