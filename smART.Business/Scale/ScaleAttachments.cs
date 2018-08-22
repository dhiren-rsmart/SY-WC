using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using smART.Common;

namespace smART.Business.Rules
{
    public class ScaleAttachments
    {
        public void Deleted(smART.ViewModel.ScaleAttachments businessEntity, smART.Model.ScaleAttachments modelEntity, smART.Model.smARTDBContext dbContext)
        {
            if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.Customer || businessEntity.Ref_Type == (int)EnumAttachmentRefType.Thumbprint1 || businessEntity.Ref_Type == (int)EnumAttachmentRefType.Thumbprint2)
            {
                smART.Model.ScaleAttachments attachment = dbContext.T_Scale_Attachments.Include("Parent.Party_ID").FirstOrDefault(m => m.ID == modelEntity.ID);

                if (attachment.Parent.Party_ID != null)
                {
                    smART.Model.Party party = dbContext.M_Party.FirstOrDefault(m => m.ID == attachment.Parent.Party_ID.ID);
                    if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.Customer)
                        party.PhotoRefId = "";
                    else if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.Thumbprint1)
                        party.ThumbImage1RefId = "";
                    else if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.Thumbprint2)
                        party.ThumbImage2RefId = "";
                    else if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.DriverLicense)
                        party.LicenseImageRefId = "";
                    else if (businessEntity.Ref_Type == (int)EnumAttachmentRefType.Signature)
                        party.SignatureImageRefId = "";

                    dbContext.SaveChanges();

                }
            }

        }
    }
}
