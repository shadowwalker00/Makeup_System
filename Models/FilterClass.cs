using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyFace.Models
{
    public class FilterClass
    {
        public string AttachPic { get; set; }
        public string Name { get; set; }
    }

    public class FilterClassManager
    {
        public static List<FilterClass> getFilters()
        {
            var filters = new List<FilterClass>();
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter0.png", Name = "原图" });
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter1.png", Name = "灰白" });
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter2.png", Name = "曝光" });
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter4.png", Name = "暖色" });
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter5.png", Name = "冷色" });
            filters.Add(new FilterClass { AttachPic = "/Assets/Filters/filter7.png", Name = "版画" });
            return filters;
        }
    }
}
