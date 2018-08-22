﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
    public enum EnumFeatures
    {
        Exempt = -2,

        All = -1,

        Home,

        Master_Party,
            Master_AddressBook,
            Master_Contact,
            Master_NotesAppointment,
            Master_BankBook,
            Master_Bin,
        Master_Item,
           Master_ItemNotes,
        Master_Employee,
        Master_Role,
        Master_RoleFeature,
        Master_LOV,
        Master_LOVType,
        Master_PriceList,
        Master_UOMConversion,
        Master_Asset,           
           Master_AssetAttachments,
           Master_AssetAudit,
        Transaction_Dispatcher,
        Transaction_DispatcherRequestExpense,
        Transaction_ExpensesRequest,
        Transaction_SalesOrder,
            Transaction_SalesOrderItem,
            Transaction_SalesOrderAttachment,
            Transaction_SalesOrderNote,
        Transaction_PurchaseOrder,
            Transaction_PurchaseOrderItem,
            Transaction_PurchaseOrderAttachment,
            Transaction_PurchaseOrderNote,
        Transaction_Invoice,
            Transaction_InvoiceItem,
            Transaction_InvoiceExpense,
            Transaction_InvoiceAttachment,
            Transaction_InvoiceNote,
        Transaction_Scale,
          Transaction_ScaleDetails,
          Transaction_ScaleAttachment,
          Transaction_ScaleNote,
          Transaction_ScaleExpense,
        Transaction_Booking,
          Transaction_Container,
          Transaction_BookingAttachment,
          Transaction_BookingNote,
         Transaction_Settlement,
          Transaction_SettlementDetails,
        Transaction_Payment,
          Transaction_PaymentDetails,
          Transaction_PaymentAttachment,
          Transaction_PaymentNote,
          Transaction_PaymentExpense,
        Transaction_Receipt,
          Transaction_ReceiptDetails,
          Transaction_ReceiptAttachment,
          Transaction_ReceiptNote,
          Transaction_ReceiptExpense,
       Transaction_AuditLog,
       Transaction_InventoryAudit,
       Transaction_Cash,
       Reports_Report,
       Charts_Chart,
       Searches_Search, 
       Administration_QBLog,
       Administration_Cycle,
         Administration_CycleDetails
    }
}