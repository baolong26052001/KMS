using Microsoft.AspNetCore.Http;
using System;
using System.Windows;

namespace Insurance
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        public class TslideHeader
        {
            public int Id { get; set; }
            public string? Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool? IsActive { get; set; }
            public int? TimeNext { get; set; }
            public DateTime? DateModified { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class TslideDetail
        {
            public int Id { get; set; }
            public string? Description { get; set; }
            public string? TypeContent { get; set; }
            public string? ContentUrl { get; set; }
            public IFormFile? File { get; set; }
            public int? SlideHeaderId { get; set; }
            public DateTime? DateModified { get; set; }
            public DateTime? DateCreated { get; set; }
            public bool? IsActive { get; set; }
        }


    }
}
