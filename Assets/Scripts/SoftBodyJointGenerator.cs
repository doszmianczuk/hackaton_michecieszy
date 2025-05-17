using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SoftBodyJointGenerator : MonoBehaviour
{
    public SoftBody softBody;
    public Rigidbody2D pointPrefab;

    public int pointCount;
    public float pointScale;

    public float jointFrequency;
    public float dampingRatio;
    
    public float centerJointFrequency;
    public float centerDampingRatio;
    
    Coroutine fixCoroutine;
    
    private void Start()
    {
        CreateBlob();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RecreateBlob();
        }
    }

    public void RecreateBlob()
    {
        foreach (Transform point in softBody.points)
        {
            Destroy(point.gameObject);
        }
        
        softBody.points.Clear();
        var collider = GetComponent<CircleCollider2D>();
        var radius = collider.radius;
        collider.radius = softBody.distanceFromCenter * 2;
        
        if (fixCoroutine != null)
        {
            StopCoroutine(fixCoroutine);
        }
        
        CreateBlob();

        fixCoroutine = StartCoroutine(RevertColliderRadius(collider, radius));
    }

    private void CreateBlob()
    {
        softBody.spriteShape.spline.Clear();
        
        float angle = 2 * Mathf.PI / pointCount;

        List<Rigidbody2D> newPoints = new();

        for (float i = 0.0f; i < 2 * Mathf.PI; i += angle)
        {
            Rigidbody2D point = Instantiate(pointPrefab, transform);

            Vector3 pointPos = new Vector3(Mathf.Cos(i), Mathf.Sin(i));
            pointPos *= softBody.distanceFromCenter;
            pointPos += transform.position;
            point.transform.position = pointPos;
            point.transform.localScale = new Vector3(pointScale, pointScale, pointScale);

            softBody.points.Add(point.transform);
            
            newPoints.Add(point);
        }
        

        
        for (int i = 0; i < newPoints.Count; ++i)
        {
            Rigidbody2D point = newPoints[i];
            int next = (i + 1) % newPoints.Count;
            int prev = (i + newPoints.Count - 1) % newPoints.Count;

            DistanceJoint2D distanceJoint = point.gameObject.AddComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = newPoints[next];

            SpringJoint2D springJoint = point.gameObject.AddComponent<SpringJoint2D>();
            springJoint.connectedBody = newPoints[next];
            springJoint.frequency = jointFrequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.breakAction = JointBreakAction2D.Ignore;

            springJoint = point.gameObject.AddComponent<SpringJoint2D>();
            springJoint.connectedBody = newPoints[prev];
            springJoint.frequency = jointFrequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.breakAction = JointBreakAction2D.Ignore;

            
            springJoint = point.gameObject.AddComponent<SpringJoint2D>();
            springJoint.connectedBody = GetComponent<Rigidbody2D>();
            springJoint.frequency = centerJointFrequency;
            springJoint.dampingRatio = centerDampingRatio;
            springJoint.breakAction = JointBreakAction2D.Ignore;
            springJoint.autoConfigureDistance = false;


            softBody.spriteShape.spline.InsertPointAt(i, point.transform.position);
            softBody.spriteShape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
    }
    
    private IEnumerator RevertColliderRadius(CircleCollider2D collider, float radius)
    {
        yield return new WaitForSeconds(0.1f);
        collider.radius = radius;
    }
}
