using UnityEngine;

public class LinePool : MonoBehaviour
{
    [SerializeField] private int maxSize = 200;

    public class LineSegment
    {
        public Vector2 CurPosition;
        public Vector2 PreviousPosition;
        public bool Active;

        public LineSegment(Vector2 pos)
        {
            CurPosition = pos;
            PreviousPosition = pos;
        }
    }

    private LineSegment[] _segArr;

    void Awake()
    {
        _segArr = new LineSegment[maxSize];
        for (int i = 0; i < maxSize; i++)
        {
            _segArr[i] = new LineSegment(Vector2.zero);
            _segArr[i].Active = false;
        }
    }

    public LineSegment GetSegment(Vector2 startPos)
    {
        LineSegment segment = null;
        for (int i = 0; i < maxSize; i++)
        {
            
            if (_segArr[i].Active == false)
            {
                segment = _segArr[i];
                break;
            }
        }
        if (segment == null)
        {
            return null;
        }
        segment.CurPosition = startPos;
        segment.PreviousPosition = startPos;
        segment.Active = true;
        return segment;
    }

    public void ReturnSegment(LineSegment segment)
    {
        if (segment.Active)
        {
            segment.Active = false;
            segment.CurPosition = Vector2.zero;
            segment.PreviousPosition = Vector2.zero;
        }
    }

    public void ReturnAllSegments()
    {
        for (int i = 0; i < _segArr.Length; i++)
        {
            _segArr[i].Active = false;
            _segArr[i].CurPosition = Vector2.zero;
            _segArr[i].PreviousPosition = Vector2.zero;
        }
    }
}
