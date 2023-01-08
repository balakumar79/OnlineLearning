using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Admin
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] LanguageIds { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Active { get; set; }
        public List<SelectListItem> Languages { get; set; }
    }
}
