using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Haley.Models
{ 
    public class PaginationData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public PaginationData() { }
    }
}
