using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum UIDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
    TopLeft = 16,
    //TopCenter,
    TopRight = 32,
    //MiddleLeft,
    MiddleCenter = 64,
    //MiddleRight,
    BottomLeft = 128,
    //BottomCenter,
    BottomRight = 256
}

[Serializable]
public class ResizableArea
{

    public const float DefaultSide = 10;

    public static Rect GetSide(Rect _rect, UIDirection _sideDirection, float _side, float _offset = 0)
    {
        switch (_sideDirection)
        {
            case UIDirection.MiddleCenter:
                return new Rect(_rect.x + _side / 2, _rect.y + _side / 2, _rect.width - _side, _rect.height - _side);
            case UIDirection.Top:
                return new Rect(_rect.x + _side / 2, _rect.y - _side / 2 + _offset, _rect.width - _side, _side);
            case UIDirection.Bottom:
                return new Rect(_rect.x + _side / 2, _rect.y + _rect.height - _side / 2 + _offset, _rect.width - _side, _side);
            case UIDirection.Left:
                return new Rect(_rect.x - _side / 2 + _offset, _rect.y + _side / 2, _side, _rect.height - _side);
            case UIDirection.Right:
                return new Rect(_rect.x + _rect.width - _side / 2 + _offset, _rect.y + _side / 2, _side, _rect.height - _side);
            case UIDirection.TopLeft:
                return new Rect(_rect.x - _side / 2 + _offset, _rect.y - _side / 2 + _offset, _side, _side);
            case UIDirection.TopRight:
                return new Rect(_rect.x + _rect.width - _side / 2 + _offset, _rect.y - _side / 2 + _offset, _side, _side);
            case UIDirection.BottomLeft:
                return new Rect(_rect.x - _side / 2 + _offset, _rect.y + _rect.height - _side / 2 + _offset, _side, _side);
            case UIDirection.BottomRight:
                return new Rect(_rect.x + _rect.width - _side / 2 + _offset, _rect.y + _rect.height - _side / 2 + _offset, _side, _side);
        }
        return new Rect();
    }

    bool isDragging;

    UIDirection[] directions;
    UIDirection sideDirection;
    UIDirection enabledSides;
    Dictionary<UIDirection, Rect> sides;
    Dictionary<UIDirection, float> sideOffset;

    public Vector2 minSize = Vector2.zero;
    public Vector2 maxSize = Vector2.zero;
    public float side = DefaultSide;

    Dictionary<UIDirection, Rect> Sides
    {
        get
        {
            if (sides == null)
                sides = new Dictionary<UIDirection, Rect>();
            return sides;
        }
    }

    public Dictionary<UIDirection, float> SideOffset
    {
        get
        {
            if (sideOffset == null)
                sideOffset = new Dictionary<UIDirection, float>();
            return sideOffset;
        }
    }
    UIDirection[] Directions
    {
        get
        {
            if (directions == null)
            {
                Array array = Enum.GetValues(typeof(UIDirection));
                directions = new UIDirection[array.Length];
                for (int i = 0; i < directions.Length; i++)
                {
                    UIDirection direction = (UIDirection)(array.GetValue(i));
                    directions[i] = direction;
                }
            }
            return directions;
        }
    }

    public ResizableArea()
    {
        directions = Directions;
    }

    public void EnableSide(UIDirection _direction)
    {
        enabledSides |= _direction;
    }

    public void DisableSide(UIDirection _direction)
    {
        enabledSides &= ~_direction;
        if (Sides.ContainsKey(_direction))
            Sides.Remove(_direction);
    }

    public bool IsEnabled(UIDirection _direction)
    {
        return enabledSides.HasFlag(_direction);
    }

    void Reload(Rect _rect)
    {
        foreach (var direction in Directions)
        {
            if (enabledSides.HasFlag(direction))
            {
                float offset = 0;
                SideOffset.TryGetValue(direction, out offset);
                Sides[direction] = GetSide(_rect, direction, side, offset);
            }
        }
    }

    public virtual Rect OnGUI(Rect _rect)
    {
        Reload(_rect);
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.Repaint:
                if (IsEnabled(UIDirection.Top))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.Top], MouseCursor.ResizeVertical);
                if (IsEnabled(UIDirection.Bottom))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.Bottom], MouseCursor.ResizeVertical);

                if (IsEnabled(UIDirection.Left))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.Left], MouseCursor.ResizeHorizontal);
                if (IsEnabled(UIDirection.Right))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.Right], MouseCursor.ResizeHorizontal);

                if (IsEnabled(UIDirection.TopLeft))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.TopLeft], MouseCursor.ResizeUpLeft);
                if (IsEnabled(UIDirection.TopRight))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.TopRight], MouseCursor.ResizeUpRight);

                if (IsEnabled(UIDirection.BottomLeft))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.BottomLeft], MouseCursor.ResizeUpRight);
                if (IsEnabled(UIDirection.BottomRight))
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.BottomRight], MouseCursor.ResizeUpLeft);

                if (IsEnabled(UIDirection.MiddleCenter) && isDragging && sideDirection == UIDirection.MiddleCenter)
                    EditorGUIUtility.AddCursorRect(Sides[UIDirection.MiddleCenter], MouseCursor.MoveArrow);
                break;
            case EventType.MouseDown:
                foreach (var direction in Directions)
                {
                    if (IsEnabled(direction) && Sides[direction].Contains(evt.mousePosition))
                    {
                        sideDirection = direction;
                        isDragging = true;
                        break;
                    }
                }
                break;
            case EventType.MouseUp:
                isDragging = false;
                sideDirection = UIDirection.None;
                break;
            case EventType.MouseDrag:
                if (isDragging)
                {
                    switch (sideDirection)
                    {
                        case UIDirection.Top:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.y += evt.delta.y;
                                _rect.height -= evt.delta.y;
                            }
                            break;
                        case UIDirection.Bottom:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.height += evt.delta.y;
                            }
                            break;
                        case UIDirection.Left:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.x += evt.delta.x;
                                _rect.width -= evt.delta.x;
                            }
                            break;
                        case UIDirection.Right:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.width += evt.delta.x;
                            }
                            break;
                        case UIDirection.TopLeft:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.y += evt.delta.y;
                                _rect.height -= evt.delta.y;

                                _rect.x += evt.delta.x;
                                _rect.width -= evt.delta.x;
                            }
                            break;
                        case UIDirection.TopRight:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.y += evt.delta.y;
                                _rect.height -= evt.delta.y;

                                _rect.width += evt.delta.x;
                            }
                            break;
                        case UIDirection.BottomLeft:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.height += evt.delta.y;

                                _rect.x += evt.delta.x;
                                _rect.width -= evt.delta.x;
                            }
                            break;
                        case UIDirection.BottomRight:
                            if (IsEnabled(sideDirection))
                            {
                                _rect.height += evt.delta.y;

                                _rect.width += evt.delta.x;
                            }
                            break;
                        case UIDirection.MiddleCenter:
                            if (IsEnabled(sideDirection))
                                _rect.position += evt.delta;
                            break;

                    }
                    evt.Use();
                }
                break;
            default:
                break;
        }

        _rect.width = Mathf.Max(_rect.width, minSize.x);
        _rect.height = Mathf.Max(_rect.height, minSize.y);

        if (maxSize != Vector2.zero)
        {
            _rect.width = Mathf.Min(_rect.width, maxSize.x);
            _rect.height = Mathf.Min(_rect.height, maxSize.y);
        }

        return _rect;
    }
}