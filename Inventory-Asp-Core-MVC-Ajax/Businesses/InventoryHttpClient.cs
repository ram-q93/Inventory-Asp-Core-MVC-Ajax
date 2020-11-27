using Helper.Library.Enums;
using Helper.Library.HttpClients;
using Helper.Library.Models;
using Helper.Library.Services;
using System;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses
{
    public class InventoryHttpClient : BaseHttpClient
    {
        protected override string Host => "https://picsum.photos";

        private readonly ISerializer serializer;
        private readonly ILogger logger;

        public InventoryHttpClient(
            ISerializer serializer,
            ILogger logger,
            JsonSerializerType jsonSerializerType = JsonSerializerType.PascalCase)
            : base(serializer, jsonSerializerType)
        {
            this.serializer = serializer;
            this.logger = logger;
        }

        public async Task<Result<byte[]>> SendHttpRequestToGetImageByteArray()
        {
            try
            {
                var Uri = "/500";
                logger.Info($"Request URl : {Host + Uri}");
                var response = await GetAsync(Uri, null, Host);
                logger.Info("Response : " + serializer.SerializeToJson(response));
                var byteArrayResult = response.Content.ReadAsByteArrayAsync().Result;
                return Result<byte[]>.Successful(byteArrayResult);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return Result<byte[]>.Failed(Error.None());
            }
        }
    }
}
