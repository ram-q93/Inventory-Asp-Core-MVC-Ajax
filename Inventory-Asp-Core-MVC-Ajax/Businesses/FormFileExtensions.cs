using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;
        public const int ImageMaximumBytes = 5000000;
        public const int ImageMaximumWidth = 100;
        public const int ImageMaximumHight = 100;

        public static Result IsValidImage(this IFormFile postedFile)
        {
            if(postedFile == null)
                return Result.Successful();

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return Result.Failed(Error.WithData(4009, new[] { "Error in ContentType" }));
            }

            //-------------------------------------------
            //  Check the image extension  (jpg, png, jpeg, gif...)
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return Result.Failed(Error.WithData(4009, new[] { "Error in image extension  (jpg, png, jpeg...)" }));
            }

            //-------------------------------------------
            //  Check the image Aspect ratio
            //-------------------------------------------
            using (var image = Image.FromStream(postedFile.OpenReadStream()))
            {
                if (image.Width > ImageMaximumWidth || image.Height > ImageMaximumHight)
                    return Result.Failed(Error.WithData(4009, new[] { "Error in image aspect ratio" }));
            }


            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return Result.Failed(Error.WithData(4009, new[] { "Error in image" }));

                }
                //------------------------------------------
                //check whether the image size exceeding the limit
                //------------------------------------------ 
                if (postedFile.Length > ImageMaximumBytes)
                {
                    return Result.Failed(Error.WithData(4009, new[] { "Error :  image size exceeding the limit" }));
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return Result.Failed(Error.WithData(4009, new[] { "Error" }));
                }
            }
            catch (Exception)
            {
                return Result.Failed(Error.WithData(4009, new[] { "" })); ;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {

                return Result.Failed(Error.WithData(4009, new[] { "Error " }));
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return Result.Successful();
        }
    }
}
