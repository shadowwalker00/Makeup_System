using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyFace.Models
{
    public class WallPaper
    {
        public string AttachPic { get; set; }
        public string Name { get; set; }
    }
    public class WallPaperManager
    {
        public static List<WallPaper> getWallPapers()
        {
            var wallPapers = new List<WallPaper>();
            wallPapers.Add(new WallPaper{ AttachPic="/Assets/WallPaper/hat.png",Name="帽子"});
            wallPapers.Add(new WallPaper { AttachPic = "/Assets/WallPaper/mustache.png", Name = "八字胡" });
            wallPapers.Add(new WallPaper { AttachPic = "/Assets/WallPaper/sunglasses.png", Name = "墨镜" });
            return wallPapers;
        }
    }
}
