using System;

namespace Haley.Enums
{
    public enum Alignment {
        Left,
        Right
    }

    public enum BadgeAlignment {
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

    public enum BadgeAnchor {
        Center,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum BadgeShape {
        Ellipse,
        Rectangle,
    }

    public enum BadgeType {
        Success,
        Info,
        Warning,
        Error
    }

    public enum CardMode {
        Simple,
        Flyer,
        Professional
    }

    public enum ControlBoxStyle {
        Windows,
        Mac
    }

    public enum DisplayMode {
        Mini,
        Compact,
        Full
    }

    public enum DockLocation {
        Left,
        Right
    }

    public enum NumericType {
        Integer,
        Double,
    }

    public enum PaginationMode {
        Simple,
        Extended
    }
    public enum PickerAdornerShape
    {
        None,
        Triangle,
        Rectangle,
        Circle,
    }

    public enum IconSourcePreference {
        ImageSource = 0,
        IconKind = 1
    }
}
