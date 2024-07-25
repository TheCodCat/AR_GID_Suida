using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class MoveMarkerAndInstanceGid : PlayerState
{
    [SerializeField] private Transform _marker;
    [SerializeField] private GID _gid;
    private Vector3 _newPos;

    [SerializeField] private ARRaycastManager _ARManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Transform _camera;
    private Vector2 _posRay;
    private void Start()
    {
        _posRay = GetScreenPos();
    }
    public override void Init()
    {
        IsThis = true;
        _camera = Camera.main.transform;
    }

    public override void Run()
    {
        if (_ARManager.Raycast(_posRay, _hits,TrackableType.Planes))
        {
            _newPos = _hits[0].pose.position;
            _marker.position = _newPos;
        }
    }

    private Vector2 GetScreenPos()
    {
        return new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void InstanceGid(InputAction.CallbackContext context)
    {
        if (context.performed & _hits.Count != 0 & IsThis)
        {
            Debug.Log("������� ��������� ����");
            _gid.transform.position = _hits[0].pose.position;
            _gid.gameObject.SetActive(true);
            _gid.transform.LookAt(new Vector3(_camera.position.x, 0, _camera.position.z));
            IsThis = false;
            StateMachin.OnNewState?.Invoke(NewState);
        }
    }

}
