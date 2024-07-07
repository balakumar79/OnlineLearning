using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Learning.Entities.Extension
{
    public static class EnumToSelectListItemExtension
    {
        public static IList<SelectListItem> EnumToSelectList<T>(this T enumType,string selectedValue=null)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            if (typeof(T).IsEnum)
            {

                var items = (Enum.GetValues(typeof(T)).Cast<int>().Select(s => new SelectListItem
                {
                    Text = Enum.GetName(typeof(T), s),
                    Value = s.ToString(),
                    Selected = string.IsNullOrEmpty(selectedValue) ? s.ToString() == enumType.ToString() : s.ToString() == selectedValue
                }));

                return items.ToList();
            }
            else
                throw new ArgumentException(nameof(enumType));
        }
    }
}
