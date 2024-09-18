using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _pointPrefab;
    [SerializeField] private float _timeInterval;

    private List<SpriteRenderer> _path = new();
    private int _pathLength;
    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_timeInterval);
    }

    public void DrawPath(Bird target)
    {
        HideOld();
        StartCoroutine(CreatePath(target));
    }

    private IEnumerator CreatePath(Bird target)
    {
        while (target.IsCollided == false)
        {
            DrawPoint(target.transform.position);
            yield return _delay;
        }
    }

    private void DrawPoint(Vector3 position)
    {
        SpriteRenderer point = _pathLength < _path.Count? _path[_pathLength] : CreateNewPoint(position);

        point.transform.position = position;
        point.gameObject.SetActive(true);
        _pathLength++;
    }

    private SpriteRenderer CreateNewPoint(Vector3 position)
    {
        SpriteRenderer point = Instantiate(_pointPrefab, position, Quaternion.identity, transform);
        _path.Add(point);
        return point;
    }

    private void HideOld()
    {
        _path.ForEach(point => point.gameObject.SetActive(false));
        _pathLength = 0;
    }
}
