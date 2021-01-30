using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    #region PersianCultureInfo ...
    public class PersianCultureInfo : CultureInfo
    {
        public PersianCultureInfo()
            : base("fa-IR", true)
        {
            base.DateTimeFormat.MonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            DateTimeFormat.MonthGenitiveNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            DateTimeFormat.AbbreviatedMonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            DateTimeFormat.AbbreviatedMonthGenitiveNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
            DateTimeFormat.AbbreviatedDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
            DateTimeFormat.ShortestDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
            DateTimeFormat.DayNames = new string[] { "یکشنبه", "دوشنبه", "ﺳﻪشنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
            DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;
            DateTimeFormat.AMDesignator = "ق.ظ";
            DateTimeFormat.PMDesignator = "ب.ظ";
            DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTimeFormat.LongDatePattern = "(dddd) dd  MMMM yyyy";
            DateTimeFormat.ShortTimePattern = "HH:mm tt";
            DateTimeFormat.LongTimePattern = "HH:mm:ss tt";
            DateTimeFormat.FullDateTimePattern = "HH:mm:ss tt dd (dddd) MMMM yyyy  ";
          
        }
    }
    #endregion

    public static class RequestCultureExtension
    {
        public static void ConfigCultureRequest(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(new PersianCultureInfo());
            });
        }
        public static void UseWeboRequestLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { new PersianCultureInfo() };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new PersianCultureInfo()),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

        }
    }

}