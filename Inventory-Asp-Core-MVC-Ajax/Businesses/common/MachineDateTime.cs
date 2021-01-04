using System;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    public class MachineDateTime : IMachineDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}
