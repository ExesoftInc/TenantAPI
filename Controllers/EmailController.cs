using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TenantAPI.Properties;

namespace TenantAPI.Controllers
{
    internal class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        internal EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        internal bool SendRegistrationEmail(string toEmail, string callbackUrl)
        {
            try
            {
                string message = Resources.RegistrationConfirmationEmail.Replace("callbackUrl", HtmlEncoder.Default.Encode(callbackUrl));
                _emailSender.SendEmailAsync(toEmail, "Email Confirmation", message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal bool ResetPasswordEmail(string toEmail, string callbackUrl)
        {
            try
            {
                string message = Resources.ResetPassword.Replace("callbackUrl", HtmlEncoder.Default.Encode(callbackUrl));
                _emailSender.SendEmailAsync(toEmail, "Reset Password", message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //internal bool SendOrderEmail(string toEmail, IOrder orderInfo, List<AssignedLicense> licenses)
        //{
        //    try
        //    {
        //        OrderEmail(toEmail, orderInfo, licenses);
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
            
        //}

        //private void OrderEmail(string toEmail, IOrder orderInfo, List<AssignedLicense> licenses)
        //{
        //    var builder = new StringBuilder();

        //    if (orderInfo.Items.Count() > 1)
        //    {
        //        builder.Append(Resources.MultipleOrderEmail);
        //        StringBuilder sLicense = new StringBuilder();

        //        foreach (var item in orderInfo.Items)
        //        {
        //            sLicense.AppendLine(@"<row><b>Software Product:" + item.ProductName + @"</b></row>");
        //            var matchItem = _entities.AssignedLicenses.Where(x => x.OrderItemId == item.ItemId);
        //            sLicense.AppendLine(@"<ul>");
        //            foreach (var assignedLicense in matchItem)
        //            {
        //                sLicense.AppendLine(@"<li>License Key: " + assignedLicense.LicenseKey + @"</li>");
        //            }
        //            sLicense.AppendLine(@"</ul>");
        //            sLicense.AppendLine(@"<row>");
        //            sLicense.AppendLine(@"<p>You can download the product from the following link: </p>");
        //            sLicense.AppendLine(@"<p><a href=" + @"https://instantwebapi.com/assets/downloads/" + item.ProductKey + "_Setup.msi" + @">Download " + item.ProductName + @"</a></p>");
        //            sLicense.AppendLine(@"</row>");
        //            sLicense.AppendLine(@"<p></p>");
        //        }

        //        builder.Replace("multipleLicenseKey", sLicense.ToString());
        //    }
        //    else if (orderInfo.Items[0].Quantity > 1)
        //    {
        //        builder.Append(Resources.MultipleOrderEmail);
        //        var item = orderInfo.Items[0];
        //        var matchItem = _entities.AssignedLicenses.Where(x => x.OrderItemId == item.ItemId);
        //        StringBuilder sLicense = new StringBuilder();
        //        sLicense.AppendLine(@"<row><b>Software Product:" + item.ProductName + @"</b></row>");
        //        sLicense.AppendLine(@"<ul>");
        //        foreach (var assignedLicense in matchItem)
        //        {
        //            sLicense.AppendLine(@"<li>License Key: " + assignedLicense.LicenseKey + @"</li>");
        //        }
        //        sLicense.AppendLine(@" </ul>");
        //        sLicense.AppendLine(@"<row>");
        //        sLicense.AppendLine(@"<p>You can download the product from the following link: </p>");
        //        sLicense.AppendLine(@"<p><a href=" + @"https://instantwebapi.com/assets/downloads/" + item.ProductKey + "_Setup.msi" + @">Download " + item.ProductName + @"</a></p>");
        //        sLicense.AppendLine(@"</row>");
        //        sLicense.AppendLine(@"<p></p>");
        //        builder.Replace("multipleLicenseKey", sLicense.ToString());
        //    }
        //    else
        //    {

        //        builder.Append(Resources.OrderEmail);
        //        builder.Replace("productKey", licenses[0].OrderItem.ProductKey);
        //    }

        //    builder.Replace("orderId", orderInfo.OrderId);
        //    builder.Replace("userName", orderInfo.UserName);
        //    builder.Replace("productName", orderInfo.Items[0].ProductName);
        //    builder.Replace("licenseKey", licenses[0].LicenseKey.ToString());
        //    builder.Replace("userEmail", orderInfo.UserEmail);

        //    _emailSender.SendEmailAsync(toEmail, string.Format(Environment.NewLine + "Order Id: '{0}' for '{1}'", orderInfo.OrderId, orderInfo.UserName), builder.ToString());
        //}

    }
}
