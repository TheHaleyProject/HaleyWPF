using System;

namespace Haley.Enums
{
    public enum PaginationMode
    {
        Simple,
        Extended
    }

    public enum CardMode
    {
        Simple,
        Flyer,
        Professional
    }

    public enum BadgeShape
    {
        Ellipse,
        Rectangle,
    }

    public enum BadgeType
    {
        Success,
        Info,
        Warning,
        Error
    }

    public enum BadgeAlignment
    {
        TopRight,
        TopLeft,
        BottomLeft,
        BottomRight,
        TopCenter,
        BottomCenter,
        LeftCenter,
        RightCenter,
        Center
    }

    public enum BadgeAnchor
    {
        Center,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum PickerAdornerShape
    {
        None,
        Triangle,
        Rectangle,
        Circle,
    }
}
