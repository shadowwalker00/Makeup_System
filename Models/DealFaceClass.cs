using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyFace.Models
{
    public class DealFaceClass
    {
        public string AttachPic { get; set; }
        public string Name { get; set; }
    }
    public class DealFaceClassManager
    {
        public static List<DealFaceClass> getDealFaceClasses()
        {
            var dealFaces = new List<DealFaceClass>();
            dealFaces.Add(new DealFaceClass { AttachPic = "/Assets/Tool/brush.png", Name = "美白" });
            dealFaces.Add(new DealFaceClass { AttachPic = "/Assets/Tool/eyes.png",  Name = "亮眼" });
            return dealFaces;
        }
    }
}
