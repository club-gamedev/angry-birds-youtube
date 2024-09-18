using System;
using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private const int StripEndIndex = 1;

    [SerializeField] private CameraFollower _follower;
    [SerializeField] private Bird _birdPrefab;
    [SerializeField] private float _reloadTime;
    [SerializeField] private LineRenderer _lineRendererRight;
    [SerializeField] private LineRenderer _lineRendererLeft;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _idlePosition;
    [SerializeField] private float _maxLength;
    [SerializeField] private float _birdPositionOffset;
    [SerializeField] private float _force;

    [SerializeField] private PathDrawer _pathDrawer;

    private Vector3 _currentPosition;
    private bool _isDragging;
    private Bird _bird;

    private void Start()
    {
        StartCoroutine(Reload());
    }

    private void Update()
    {
        if (_isDragging == false)
            return;

        _currentPosition = UpdateCurrentPosition();
        UpdateStrips(_currentPosition);
        MoveBird(_currentPosition);
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        Shoot();
        ResetStrips();
    }

    private void Shoot()
    {
        _bird.ActivatePhysics();

        Vector3 force = (_center.position - _currentPosition) * _force;
        _bird.Push(force);

        _pathDrawer.DrawPath(_bird);
        _follower.SetTarget(_bird.transform);

        _bird = null;

        StartCoroutine(Reload(_reloadTime));
    }

    private IEnumerator Reload(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        _bird = Instantiate(_birdPrefab);

        _bird.DeactivatePhysics();

        ResetStrips();
        MoveBird(_currentPosition);

        _follower.SetTarget(transform);
    }

    private Vector3 UpdateCurrentPosition()
    {
        _currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentPosition.z = transform.position.z;
        return _center.position + Vector3.ClampMagnitude(_currentPosition - _center.position, _maxLength);
    }

    private void MoveBird(Vector3 position)
    {
        Vector3 direction = position - _center.position;
        _bird.transform.position = position + direction.normalized * _birdPositionOffset;
        _bird.transform.right = -direction.normalized;
    }

    private void UpdateStrips(Vector3 position)
    {
        _lineRendererRight.SetPosition(StripEndIndex, position);
        _lineRendererLeft.SetPosition(StripEndIndex, position);
    }

    private void ResetStrips()
    {
        _currentPosition = _idlePosition.position;
        UpdateStrips(_currentPosition);
    }
}
