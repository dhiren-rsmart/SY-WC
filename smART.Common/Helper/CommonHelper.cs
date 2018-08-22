using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
   public  class CommonHelper
    {
       public static string GetFileNameByDocType(int docType)
       {
           switch (docType)
           {
               case (int)EnumAttachmentRefType.Customer:
                   return "Customer-Image.jpeg";
               case (int)EnumAttachmentRefType.Thumbprint1:
                   return "Thumb-Image1.jpg";
               case (int)EnumAttachmentRefType.Thumbprint2:
                   return "Thumb-Image2.jpg";
               case (int)EnumAttachmentRefType.Signature:
                   return "1BPPSignature.bmp";
               case (int)EnumAttachmentRefType.DriverLicense:
               case (int)EnumAttachmentRefType.License:
                   return "DriverLicense.jpg";
               case (int)EnumAttachmentRefType.Vehicle:
                   return "Vehicle.jpg";
               case (int)EnumAttachmentRefType.CashCard:
                   return "CashCard.jpg";
               default:
                   return "";
           }
       }
    }
}
