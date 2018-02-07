using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPIDEntities
{
    public class UnresolvedCheck
    {
        public int Id { get; set; }
        public int RecordID { get; set; }
        public int InterviewRecordID { get; set; }
        public string Name { get; set; }
        public int Num { get; set; }
        public System.DateTime Date { get; set; }
        public string Service { get; set; }
        public string Disposition { get; set; }
        public bool Matched { get; set; }
    }
}
