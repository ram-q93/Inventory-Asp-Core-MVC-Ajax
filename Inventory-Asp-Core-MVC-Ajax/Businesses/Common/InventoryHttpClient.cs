using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Enums;
using AspNetCore.Lib.HttpClients;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using System;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    [TypeLifeTime(TypeLifetime.Singleton)]
    public class InventoryHttpClient : BaseHttpClient
    {
        protected override string Host => "https://picsum.photos";

        private readonly ISerializerService serializer;
        private readonly ILogger logger;

        public InventoryHttpClient(
            ISerializerService serializer,
            ILogger logger,
            JsonSerializerType jsonSerializerType = JsonSerializerType.PascalCase)
            : base(serializer, jsonSerializerType)
        {
            this.serializer = serializer;
            this.logger = logger;
        }

        public async Task<Result<Image>> SendHttpRequestToGetImageByteArray()
        {
            try
            {
                var Uri = "/500";
                logger.Info($"Request URl : {Host + Uri}");
                var response = await GetAsync(Uri, null, Host);
                var byteArrayResult = response.Content.ReadAsByteArrayAsync().Result;
                logger.Info("Response : ------------->  Arrived");

                return Result<Image>.Successful(new Image()
                {
                    Data = byteArrayResult
                });
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return Result<Image>.Failed(Error.None());
            }
        }
    }
}
