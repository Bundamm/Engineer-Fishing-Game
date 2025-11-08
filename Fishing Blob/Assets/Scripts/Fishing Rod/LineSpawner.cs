using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    [SerializeField] private Caster caster;
    [SerializeField] private GameObject floater;
    
    [Header("Fishing Line")]
    [SerializeField] private LinePool linePool;
    [SerializeField] private int startNumOfLineSegments = 20;
    [SerializeField] private float segmentLength = 0.1f;
    private LineRenderer _lineRenderer;
    private List<LinePool.LineSegment> _lineSegments;
    private bool _lineActive;
    
    
    [Header("Fishing Line Physics")]
    [SerializeField] private Vector2 gravity = new Vector2(0, -3f);
    [SerializeField] private float dampingFactor = 0.2f;

    private Vector2 _lineStartPoint;
    private Vector2 _lineEndPoint;

    
    void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = startNumOfLineSegments;
        
    }

    void Update()
    {
        if (_lineActive)
        {
            VerletSim();
            Constraints();
            CheckIfAddSegment();
            CheckIfRemoveSegment();
            DrawLine();
        }
    }

    private void CheckIfRemoveSegment()
    {
        if (_lineSegments.Count < 2)
        {
            return;
        }
        
        var firstSegment = _lineSegments[0];
        var secondSegment = _lineSegments[1];
        if (secondSegment.Active)
        {
            Vector2 casterV2 = caster.transform.position;
            float distanceToFirst = (casterV2 - firstSegment.CurPosition).magnitude;
            if (distanceToFirst < segmentLength * 0.5f && _lineSegments.Count >= startNumOfLineSegments)
            {
            
                linePool.ReturnSegment(secondSegment);
                _lineSegments.Remove(secondSegment);
                _lineRenderer.positionCount -= 1;
            }
        }
        
    }

    private void CheckIfAddSegment()
    {
        if (_lineSegments.Count <= 0) return;
        var lastSegment = _lineSegments[^1];
        if (lastSegment.Active)
        {
            Vector2 floaterV2 = floater.transform.position;
            float distanceToLast = (floaterV2 - lastSegment.CurPosition).magnitude;
            if (distanceToLast > segmentLength)
            {
                // wektor kierunku do spławika od ostatniego segmentu
                var direction = new Vector2(floater.transform.position.x - lastSegment.CurPosition.x, floater.transform.position.y - lastSegment.CurPosition.y).normalized;
                // rzeczywista odlgełość bo pomnożone przez długość
                var offset = direction * segmentLength;
                // dodaję tą odległość do pozycji ostatniego segmentu i w ten sposób otrzymuję pozycję nowego segmentu
                var newSegmentPos = lastSegment.CurPosition + offset;
                var newSeg = linePool.GetSegment(newSegmentPos);
                _lineSegments.Insert(_lineSegments.Count - 1, newSeg);
                _lineRenderer.positionCount += 1;
            }
        }
        
    }
    
    public void InitLine(GameObject newFloater)
    {
        _lineRenderer.positionCount = startNumOfLineSegments;
        if (_lineSegments is { Count: > 0 })
        {
            _lineSegments.Clear();
            linePool.ReturnAllSegments();
        }
        floater = newFloater;
        _lineSegments = new List<LinePool.LineSegment>(startNumOfLineSegments);
        
        _lineStartPoint = caster.transform.position;
        _lineEndPoint = newFloater.transform.position;
        
        Vector2 directionOfLine = (_lineEndPoint - _lineStartPoint).normalized;
        
        _lineSegments.Add(linePool.GetSegment(_lineStartPoint));
        LinePool.LineSegment firstSegment = _lineSegments[0];
        firstSegment.PreviousPosition = firstSegment.CurPosition;
        
        for (int i = 1; i < startNumOfLineSegments; i++)
        {
            Vector2 pos = _lineStartPoint + directionOfLine * (segmentLength * i);
            _lineSegments.Add(linePool.GetSegment(pos));
            LinePool.LineSegment curSegment = _lineSegments[i];
            curSegment.PreviousPosition = curSegment.CurPosition;
        }
        
        LinePool.LineSegment lastSegment = _lineSegments[^1];
         lastSegment.CurPosition = floater.transform.position;
    } 
    
    

    private void VerletSim()
    {
        for (int i = 0; i < _lineSegments.Count; i++)
        {
            LinePool.LineSegment lineSegment = _lineSegments[i];
            Vector2 velocity = (lineSegment.CurPosition - lineSegment.PreviousPosition) * dampingFactor;
            
            lineSegment.PreviousPosition = lineSegment.CurPosition;
            lineSegment.CurPosition += velocity;
            lineSegment.CurPosition += gravity * Time.deltaTime;
        }
    }

    private void Constraints()
    {
        LinePool.LineSegment firstSegment = _lineSegments[0];
        firstSegment.CurPosition = caster.transform.position;
        LinePool.LineSegment lastSegment = _lineSegments[^1];
        lastSegment.CurPosition = floater.transform.position;

        for (int i = 0; i < _lineSegments.Count - 1; i++)
        {
            LinePool.LineSegment currentSegment = _lineSegments[i];
            LinePool.LineSegment nextSegment = _lineSegments[i + 1];
            
            float distance = (currentSegment.CurPosition - nextSegment.CurPosition).magnitude;
            float dif = (distance - segmentLength);
            
            Vector2 changeDir = (currentSegment.CurPosition - nextSegment.CurPosition).normalized;
            Vector2 changeVector = changeDir * dif;

            if (i != 0)
            {
                currentSegment.CurPosition -= (changeVector * 0.5f);
                nextSegment.CurPosition += (changeVector * 0.5f);
            }
            else
            {
                nextSegment.CurPosition += changeVector;
            }
        } 
    }

    private void DrawLine()
    {
        Vector3[] linePositions = new Vector3[_lineSegments.Count];
        for (int i = 0; i < _lineSegments.Count; i++)
        {
            linePositions[i] = _lineSegments[i].CurPosition;
        }
        _lineRenderer.SetPositions(linePositions);
    }

    public void SetLineActive(bool active)
    {
        _lineActive = active;
    }

    public void DeleteLine()
    {
        _lineActive = false;
        _lineSegments.Clear();
        linePool.ReturnAllSegments();
        _lineRenderer.positionCount = 0;
        floater = null;
    }
}


