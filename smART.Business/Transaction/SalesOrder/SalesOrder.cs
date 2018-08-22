using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Business.Rules
{
    public class SalesOrder
    {
        public void Adding(smART.ViewModel.SalesOrder businessEntity, smART.Model.SalesOrder modelEntity, smART.Model.smARTDBContext dbContext, out bool cancel)
        {
            if (string.IsNullOrWhiteSpace(modelEntity.Order_Status)) modelEntity.Order_Status = "Open";
            cancel = false;
        }
    }
}
