using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smART.Library;
using Telerik.Web.Mvc;
using smART.ViewModel;
using Omu.ValueInjecter;
using smART.Common;
using smART.MVC.Present.Helpers;
using smART.Notification;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Activities;
using smART.Integration.Email;

namespace smART.MVC.Present.Controllers.Transaction
{
    [Feature(EnumFeatures.Transaction_Container)]
    public class ContainerController : BaseGridController<ContainerLibrary, Container>
    {

        private static bool _sendMail = false;

        #region /* Constructors */

        public ContainerController()
            : base("Container", new string[] { "Booking" }, new string[] { "Booking" })
        {
        }

        #endregion

        #region /* Supporting Actions - Display Actions */

        protected override ActionResult Display(GridCommand command, string id, bool isNew)
        {
            int totalRows = 0;
            IEnumerable<Container> resultList;    // = ((IParentChildLibrary<TEntity>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize==0?20:command.PageSize, "", "Asc", IncludePredicates);

            if (isNew || id == "0")
            {
                resultList = TempEntityList;
                totalRows = TempEntityList.Count;
            }
            else
            {
                resultList = ((IParentChildLibrary<Container>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);

            }

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        protected override ActionResult Display(GridCommand command, Container entity, bool isNew = false)
        {
            if (entity.Booking != null && entity.Booking.ID != 0)
                return Display(command, entity.Booking.ID.ToString(), isNew);
            else
                return base.Display(command, entity, isNew);
        }

        #endregion

        [HttpPost]
        public virtual ActionResult GetByParentID(string id)
        {
            IEnumerable<Container> resultList = ((IParentChildLibrary<Container>)Library).GetAllByParentID(int.Parse(id));
            SelectList list = new SelectList(resultList, "ListValue", "ListText");

            return Json(list);
        }

        public string GetContainerStatus()
        {
            //[1] Open-Empty-  where Weighing ticket is not created && dispatch request with request_type='pickup' but 'drop off' record is missing in dispatch table.

            //[2] WIP -  where ticket is in que. &&  dispatch request with request_type='pickup' but 'drop off' record is missing in dispatch table.

            //[3] Open-Loaded: - where weighing ticket is created. &&  dispatch request with request_type='pickup' but 'drop off' record is missing in dispatch table.

            //[4] Shipped : Dispatch request - 'Drop off' created and closed

            //[5] Closed = Invoice_Generated_Flag = true.


            //DispatcherRequest  (RequestType,RequestStatus,Container_No,Booking_Ref_No)
            //Weighing  (Weinghing_Ticket_No);
            //Booking   (Invoice_Generated_Flag,Booking_Ref_No)
            //Container (Booking_Ref_No, Container_No)


            //if (Booking.Invoice_Generated_Flag= true)
            //  status = Closed

            //Elseif (Weighing.TicketNo(Booking_Ref_No, Container_No) = Empty  && DispatcherRequest(Container_No,Booking_Ref_No).RequestType= "pickup")
            //  status = Open-Empty

            //Elseif (Weighing.TicketNo(Booking_Ref_No, Container_No) = Que  && DispatcherRequest(Container_No,Booking_Ref_No).RequestType= "pickup")
            //  status = WIP

            //Elseif (Weighing.TicketNo(Booking_Ref_No, Container_No) != Empty  && DispatcherRequest(Container_No,Booking_Ref_No).RequestType= "pickup")
            //  status = Open-Loaded

            //Elseif (Weighing.TicketNo(Booking_Ref_No, Container_No) = Created  && DispatcherRequest(Container_No,Booking_Ref_No).RequestType= "drop off" && DispatcherRequest(Container_No,Booking_Ref_No).Status= "Closed" )
            //  status = Shipped 

            //}

            return string.Empty;

        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _OpenContainers(GridCommand command)
        {
            int totalRows = 0;
            ContainerLibrary containerLibrary = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            IEnumerable<Container> resultList = containerLibrary.GetOpenContainersWithPaging(
                                                                                              out totalRows,
                                                                                              command.Page,
                                                                                              command.PageSize,
                                                                                              command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                              command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                              new string[] { "Booking.Sales_Order_No.Party" },
                                                                                              (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors)
                                                                                            );

            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        public ActionResult _OpenBookingContainers()
        {
            int totalRows = 0;
            ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
            IEnumerable<Container> resultList = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString())
                                                                        .GetOpenBookingContainersWithPaging(
                                                                              out totalRows,
                                                                              1,
                                                                              ViewBag.PageSize,
                                                                              "",
                                                                              "Asc",
                                                                              new string[] { "Booking.Sales_Order_No.Party" }
                                                                  );

            return View("~/Views/Transaction/Booking/_OpenBookingContainers.cshtml", resultList);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _OpenBookingContainers(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
        {
            int totalRows = 0;
            IEnumerable<Container> resultList = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetOpenBookingContainersWithPaging(
                                                                                                                                out totalRows,
                                                                                                                                command.Page,
                                                                                                                                command.PageSize,
                                                                                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                                                new string[] { "Booking.Sales_Order_No.Party" },
                                                                                                                                (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult _GetContainerByBookingId(GridCommand command, string id)
        {
            int totalRows = 0;
            IEnumerable<Container> resultList;
            resultList = ((IParentChildLibrary<Container>)Library).GetAllByPagingByParentID(out totalRows, int.Parse(id.ToString()), command.Page, command.PageSize == 0 ? 20 : command.PageSize, "", "Asc", IncludePredicates);
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        [HttpPost]
        public override ActionResult _GetJSon(string id)
        {
            Container entity = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetByID(id.ToString(), new string[] { "Booking.Sales_Order_No.Party" }); //"Booking.Sales_Order_No.Party"

            return Json(entity);
        }

        public ActionResult _LoadedContainers()
        {
            int totalRows = 0;
            ViewBag.PageSize = ConfigurationHelper.GetsmARTLookupGridPageSize();
            IEnumerable<Container> resultList = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetLoadedContainersWithPaging(
                                                                                                  out totalRows,
                                                                                                  1,
                                                                                                  ViewBag.PageSize,
                                                                                                  "",
                                                                                                  "Asc",
                                                                                                  new string[] { "Booking.Sales_Order_No.Party" }
                                                                  );

            return View("~/Views/Transaction/Booking/_LoadedContainers.cshtml", resultList);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult _LoadedContainers(GridCommand command)  //Need to improve this to correct totalrows value. after filter.
        {
            int totalRows = 0;
            IEnumerable<Container> resultList = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString()).GetLoadedContainersWithPaging(
                                                                                                                                out totalRows,
                                                                                                                                command.Page,
                                                                                                                                command.PageSize,
                                                                                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].Member,
                                                                                                                                command.SortDescriptors.Count == 0 ? "" : command.SortDescriptors[0].SortDirection == System.ComponentModel.ListSortDirection.Descending ? "Desc" : "Asc",
                                                                                                                                new string[] { "Booking.Sales_Order_No.Party" },
                                                                                                                                (command.FilterDescriptors.Count == 0 ? null : command.FilterDescriptors));
            return View(new GridModel { Data = resultList, Total = totalRows });
        }

        //[HttpPost]
        //[GridAction(EnableCustomBinding = true)]
        //public override ActionResult _Insert(Container data, GridCommand command, bool isNew = false)
        //{
        //    ModelState.Clear();

        //    if (string.IsNullOrEmpty(data.Container_No))
        //    {
        //        ModelState.AddModelError("Item", "Container No. is required");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if (isNew)
        //        {
        //            TempEntityList.Add(data);
        //        }
        //        else
        //        {
        //            data = Library.Add(data);
        //        }
        //    }


        //    return Display(command, data, isNew);
        //}

        //[HttpPost]
        //[GridAction(EnableCustomBinding = true)]
        //public override ActionResult _Update(Container data, GridCommand command, bool isNew = false)
        //{
        //    string bookingId = data.Booking.ID.ToString();

        //    ModelState.Clear();

        //    if (string.IsNullOrEmpty(data.Container_No))
        //    {
        //        ModelState.AddModelError("ContainerNo", "Container No. is required");
        //    }
        //    else if (data.TransferBooking != null && data.TransferBooking.ID > 0 && data.Booking.ID != data.TransferBooking.ID && (data.Status.ToLower() == "shipped" || data.Status.ToLower() == "closed"))
        //    {
        //        ModelState.AddModelError("ContainerStatus", string.Format("Can not transfer {0} status container", data.Status));
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        if (isNew)
        //        {
        //            //TODO: Add logic to update in memory data
        //            TempEntityList.SingleOrDefault(m => m.ID == data.ID).InjectFrom(data);
        //        }
        //        else
        //        {
        //            if (data.TransferBooking != null && data.TransferBooking.ID > 0 && data.Booking.ID != data.TransferBooking.ID)
        //            {                      
        //                data.Booking = new Booking();
        //                data.Booking.ID = data.TransferBooking.ID;
        //                data.Last_Updated_Date = DateTime.Now;
        //                data.Updated_By = System.Web.HttpContext.Current.User.Identity.Name;
        //            }

        //            ContainerLibrary contLib = new ContainerLibrary(Configuration.GetsmARTDBContextConnectionString());
        //            contLib.Modify(data, new string[] { "Booking" });
        //        }
        //    }

        //    return Display(command,bookingId, isNew);
        //}

        protected override void ValidateEntity(Container entity)
        {
            ModelState.Clear();

            string bookingId = entity.Booking.ID.ToString();

            if (string.IsNullOrEmpty(entity.Container_No))
            {
                ModelState.AddModelError("ContainerNo", "Container No. is required");
            }
            else if (entity.TransferBooking != null && entity.TransferBooking.ID > 0 && entity.Booking.ID != entity.TransferBooking.ID && (entity.Status.ToLower() == "shipped" || entity.Status.ToLower() == "closed"))
            {
                ModelState.AddModelError("ContainerStatus", string.Format("Can not transfer {0} status container", entity.Status));
            }
        }

        protected override void ChildGrid_OnModifying(Container entity)
        {
            if (entity.ID > 0 && entity.TransferBooking != null && entity.TransferBooking.ID > 0 && entity.Booking.ID != entity.TransferBooking.ID)
            {
                // Update new booking Id
                int sourceBookingId = entity.Booking.ID;
                entity.Booking = new Booking();
                entity.Booking.ID = entity.TransferBooking.ID;
                entity.Last_Updated_Date = DateTime.Now;
                entity.Updated_By = System.Web.HttpContext.Current.User.Identity.Name;
                entity.TransferBooking.ID = sourceBookingId;
            }
        }

        protected override void ChildGrid_OnModified(Container entity)
        {
            if (entity.ID > 0 && entity.TransferBooking != null && entity.TransferBooking.ID > 0 && entity.Booking.ID != entity.TransferBooking.ID)
            {
                // Update dispatcher booking Id
                DispatcherRequestLibrary dispathLib = new DispatcherRequestLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                IEnumerable<DispatcherRequest> dispatcherReqs = dispathLib.GetDispatcherByBookingAndContainerNo(entity.TransferBooking.ID, entity.Container_No,

                    new string[] { "TruckingCompany", "Driver", "Booking_Ref_No", "Purchase_Order_No", "Party", "Location", "Shipper", "Sales_Order_No", "Asset", "Container" });
                DispatcherRequest dispatcherReq = dispatcherReqs.FirstOrDefault();
                if (dispatcherReq != null)
                {
                    dispatcherReq.Booking_Ref_No.ID = entity.Booking.ID;
                    dispathLib.Modify(dispatcherReq, new string[] { "TruckingCompany", "Driver", "Booking_Ref_No", "Purchase_Order_No", "Party", "Location", "Shipper", "Sales_Order_No", "Asset" });
                }
                entity.Booking.ID = entity.TransferBooking.ID;
            }
        }

        //[HttpPost]
        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult _SendEmail(string id, GridCommand command) {
        [HttpGet]
        public ActionResult SendEmail(string id)
        {
            string destinationFilePath = string.Empty;
            FilelHelper fileHelper = new FilelHelper();
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Email send failed due to Container# not found.");

                ContainerLibrary containerLib = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                Container container = containerLib.GetByID(id, new string[] { "Booking.Sales_Order_No.Party" });

                if (container == null || container.Booking == null || container.Booking.Sales_Order_No == null || container.Booking.Sales_Order_No.Party == null)
                    throw new Exception("Email send failed.");

                IEnumerable<Contact> contacts = Helpers.ContactHelper.GetEmailContactsByPartyId(container.Booking.Sales_Order_No.Party.ID);
                if (contacts.Count() <= 0)
                    throw new Exception("There is no email contact exists.");

                NotificationDefinition notDef = new NotificationDefinition();
                notDef.ToRecipients = new System.Net.Mail.MailAddressCollection();
                foreach (var item in contacts)
                {
                    notDef.ToRecipients.Add(new System.Net.Mail.MailAddress(item.Email, item.ListText));
                }

                EmployeeHelper employeeHelper = new EmployeeHelper();
                Employee employee = employeeHelper.GetEmployeeByUsername(System.Web.HttpContext.Current.User.Identity.Name);
                if (employee == null || string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Email_Password))
                    throw new Exception("Sender email and password is required.");

                ScaleLibrary scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                Scale scale = scaleLib.GetScalesByContainerId(container.ID);

                ScaleAttachmentsLibrary scaleAttachLib = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                IEnumerable<ScaleAttachments> scaleAttachments = scaleAttachLib.GetAllByParentID(scale.ID);
                if (scaleAttachments == null || scaleAttachments.Count() <= 0)
                    throw new Exception("There is no attachment found.");

                destinationFilePath = Path.Combine(Path.GetTempPath(), container.ID.ToString());
                fileHelper.CreateDirectory(destinationFilePath);
                fileHelper.DeleteFiles(destinationFilePath);
                string imageFilePath = Path.Combine(destinationFilePath, "Images");
                fileHelper.CreateDirectory(imageFilePath);

                string zipFilePath = Path.Combine(destinationFilePath, container.ID.ToString() + ".zip");

                foreach (var item in scaleAttachments)
                {
                    string sourceFilePath = fileHelper.GetFilePathByFileRefId(item.Document_RefId.ToString());
                    string imageFileFullPath = Path.Combine(imageFilePath, item.Document_Name);
                    System.IO.File.Copy(sourceFilePath, imageFileFullPath);
                }
                new smART.Common.FilelHelper().CreateZip(imageFilePath, zipFilePath);

                if (!fileHelper.FileExits(zipFilePath))
                    throw new Exception("There is no attachment found.");

                string xslPath = Path.Combine(ConfigurationHelper.GetsmARTXslPath(), "ContainerEmailBody.xslt");
                string smtpAddress = ConfigurationHelper.GetsmARTSMTPServer();

                notDef.Attachments = new List<System.Net.Mail.Attachment>();
                notDef.Attachments.Add(new System.Net.Mail.Attachment(zipFilePath));
                notDef.DeliveryType = EnumNotificationDeliveryType.Email;
                notDef.FormatType = EnumFormatType.HTML;
                notDef.Sender = new System.Net.Mail.MailAddress(employee.Email, employee.Emp_Name);
                notDef.SMTPServer = smtpAddress;
                notDef.SMTPServerCredentialID = employee.Email;
                notDef.SMTPServerCredentialPwd = employee.Email_Password;
                notDef.Subject = "Booking#/Container#: " + container.Booking.Booking_Ref_No + "/" + container.Container_No;
                NotificationHelper.StartNotificationWF(id, container.Booking.Sales_Order_No.Party.Party_Name, PartyHelper.GetOrganizationName(), employee.Emp_Name, notDef, xslPath, NotificationWFCompleted, container.Booking.Booking_Ref_No, container.Booking.Sales_Order_No.ID.ToString());
                //return Display(command, container);
                if (_sendMail == true)
                    return Json(new { Sucess = "Email send sucessfully." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Sucess = "Email send failed." }, JsonRequestBehavior.AllowGet);
                //return Json(new { Sucess = "Email send sucessfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //var script = @"ShowAlertMessage(""Send mail failed"");";
                //return JavaScript(script);
                return Json(new { Sucess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                //fileHelper.DeleteDirectory(destinationFilePath);
            }
        }

        // This method called on notification workflow completion.
        internal static void NotificationWFCompleted(WorkflowApplicationCompletedEventArgs e)
        {
            switch (e.CompletionState)
            {
                case ActivityInstanceState.Closed:
                    // If workflow complated without any error.
                    string id = Convert.ToString(e.Outputs["Result"]);
                    ContainerLibrary contLib = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
                    Container container = contLib.GetByID(id, new string[] { "Booking" });
                    container.Send_Mail = true;
                    container.Mail_Send_On = DateTime.Now;
                    contLib.Modify(container, new string[] { "Booking" });
                    _sendMail = true;
                    break;
                // If any error occurred during workflow execution, will be handled here.
                case ActivityInstanceState.Faulted:
                    _sendMail = false;
                    Exception ex = e.TerminationException;
                    ExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error, "Error occured in Notification Workflow", "PresentPolicy");
                    break;
            }
        }

        //[HttpPost]
        //[GridAction(EnableCustomBinding = true)]
        //public override ActionResult _Delete(string id, GridCommand command, string MasterID = null, bool isNew = false) {
        //  ContainerLibrary contLib = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
        //  string errorMsg;
        //  if (contLib.IsRefExits(int.Parse(id), out errorMsg)) {
        //    var script = @"alert(" + errorMsg + ");";
        //    return Json(JavaScript(script));
        //    //return Display(command, id, false);
        //  }
        //  else
        //    return base._Delete(id, command, MasterID, isNew);
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult _ValidateOnDelete(string id)
        {
            ContainerLibrary contLib = new ContainerLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
            string errorMsg;
            if (contLib.IsRefExits(int.Parse(id), out errorMsg))
                return Json(new { resultCode = "failed", message = errorMsg });
            else
                return Json(new { resultCode = "success" });
        }

        #region Deleted

        ////[GridAction(EnableCustomBinding = true)]
        ////[GridAction]
        //public ActionResult _TransferContainer(int bookingId, bool isNew = false)
        //{
        //    IEnumerable<Container> resultList = new ContainerLibrary(Configuration.GetsmARTDBContextConnectionString()).GetOpenContainers();
        //    ViewBag.isNew = isNew;
        //    ViewBag.bookingId = bookingId;
        //    return View("~/Views/Transaction/Booking/_TransferContainer.cshtml", resultList);

        //    //if (Request.IsAjaxRequest()) return PartialView("~/Views/Transaction/Booking/_TransferContainer.cshtml", resultList);
        //    //return PartialView();
        //}

        //[GridAction]
        //public ActionResult _SelectTransferContainer()
        //{
        //    IEnumerable<Container> resultList = new ContainerLibrary(Configuration.GetsmARTDBContextConnectionString()).GetOpenContainers();

        //    return View("~/Views/Transaction/Booking/_TransferContainer.cshtml", new GridModel(resultList));

        //    //if (Request.IsAjaxRequest()) return PartialView("~/Views/Transaction/Booking/_TransferContainer.cshtml", new GridModel(resultList));
        //    //return PartialView();
        //}

        ////[HttpPost]
        //public ActionResult Transfer(int? id, GridCommand command, int bookingId, bool isNew = false)
        //{

        //    if (id.HasValue)
        //    {
        //        Container entity = Library.GetByID(id.ToString());

        //        if (isNew)
        //        {
        //            TempEntityList.Add(entity);
        //            //return View("~/Views/Transaction/Booking/New.cshtml");
        //        }
        //        else
        //        {
        //            entity = Library.Add(entity);
        //            //return View("~/Views/Transaction/Booking/New.cshtml", booking);
        //        }
        //    }

        //    IEnumerable<Container> resultList = new ContainerLibrary(Configuration.GetsmARTDBContextConnectionString()).GetOpenContainers();
        //    return View(new GridModel { Data = resultList, Total = 10 });
        //    //return RedirectToAction("New", "Booking", new { id = bookingId });


        //}
        #endregion Deleted
    }
}