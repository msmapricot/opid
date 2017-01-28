using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSM.Models
{
    public class ModificationRow : DataRow
    {
        public string ModificationType { get; set; }

        public string LBVDModificationReason { get; set; }
    
        public string TIDModificationReason { get; set; }
   
        public string TDLModificationReason { get; set; }
   
        public string MBVDModificationReason { get; set; }
     
        public string SDMReason { get; set; }
    }
}