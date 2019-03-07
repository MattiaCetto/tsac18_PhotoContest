 using System;
using System.Collections.Generic;
using System.Text;

namespace tsac18_CettolinPhotoContest.data.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        
        public int NVotes { get; set; }
        public int SumVotes { get; set; }
        public string Id_User { get; set; }
        public int Rate { get; set; }
        public string Id_User2 { get; set; }
        public bool Voted { get; set; }
    }
}
