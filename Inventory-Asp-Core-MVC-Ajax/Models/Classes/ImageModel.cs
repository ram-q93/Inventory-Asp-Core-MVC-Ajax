using System;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ImageModel
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public string Base64StringData
        {
            get { 
                var result = Convert.ToBase64String(Data);
                Data = null;
                return result;
            }
            private set { }
        }
    }
}
