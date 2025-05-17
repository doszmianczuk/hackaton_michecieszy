using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class SoftBody : MonoBehaviour
{
    #region Constants
    private const float splineOffset = 0.5f;
    #endregion

    #region Fields
    [SerializeField]
    public SpriteShapeController spriteShape;
    [SerializeField]
    public List<Transform> points;
    
    public float distanceFromCenter;
    
    public SoftBodyJointGenerator jointGenerator;

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        UpdateVertices();
    }
    
    void Update()
    {
        UpdateVertices();
    }
    #endregion

    #region privateMethods
    private void UpdateVertices()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 _vertex = points[i].localPosition;
            Vector2 _towardsCenter = (Vector2.zero - _vertex).normalized;
            float _colliderRadius = points[i].GetComponent<CircleCollider2D>().radius;
            try
            {
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * _colliderRadius));
            }
            catch
            {
                Debug.LogError("Error: " + i + " " + points[i].name);
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_colliderRadius + splineOffset)));
            }

            Vector2 _lt = spriteShape.spline.GetLeftTangent(i);

            Vector2 _Newrt = Vector2.Perpendicular(_towardsCenter) * _lt.magnitude;
            Vector2 _Newlt = Vector2.zero - (_Newrt);

            spriteShape.spline.SetLeftTangent(i, _Newrt);
            spriteShape.spline.SetRightTangent(i, _Newlt);
        }
    }

    #endregion
    
}
