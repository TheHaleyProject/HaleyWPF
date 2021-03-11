using Haley.Utils;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace Haley.Models
{
    public class ImageRequest
    {
        public ImageSource InputImage { get; set; }
        public SolidColorBrush RequestBrush { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is ImageRequest newreq)
            {
                if (newreq.InputImage == this.InputImage && newreq.RequestBrush == this.RequestBrush)
                {
                    return true;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            //Reference : https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + InputImage.GetHashCode();
                hash = hash * 23 + RequestBrush.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return InputImage.ToString() + RequestBrush.ToString();
        }

        public ImageRequest(ImageSource input, SolidColorBrush brush) { InputImage = input; RequestBrush = brush; }
    }
}
