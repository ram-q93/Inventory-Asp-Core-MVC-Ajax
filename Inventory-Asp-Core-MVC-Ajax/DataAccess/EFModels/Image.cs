using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }
}
