using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using smART.Library;
using smART.ViewModel;
using System.Transactions;
using System.IO;

namespace smART.MVC.Service.Model
{

    public class Scale : smART.MVC.Service.Model.BaseEntity
    {

        public string Scale_Ticket_No
        {
            get;
            set;
        }

        public string Ticket_Type
        {
            get;
            set;
        }

        public string Ticket_Status
        {
            get;
            set;
        }

        public string Customer_Name
        {
            get;
            set;
        }

        public string Customer_Address
        {
            get;
            set;
        }

        public string Customer_City
        {
            get;
            set;
        }

        public string Customer_State
        {
            get;
            set;
        }

        public string Customer_Zip
        {
            get;
            set;
        }

        public string Customer_DOB
        {
            get;
            set;
        }

        public string Customer_Country
        {
            get;
            set;
        }

        public string License_No
        {
            get;
            set;
        }

        public string Make
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }


        public string Vehicle_Plate_No
        {
            get;
            set;
        }

        public string Truck_No
        {
            get;
            set;
        }

        public string Customer_ACLicense_ID
        {
            get; 
            set;
        }


        public void MapServiceEntityToServerEntity(smART.ViewModel.Scale serverEntity)
        {
            base.MapServiceEntityToServerEntity(serverEntity);
            serverEntity.Scale_Ticket_No = Scale_Ticket_No;
            serverEntity.Ticket_Type = Ticket_Type;
            //serverEntity.Ticket_Status = Ticket_Status;
            //serverScale.Customer_Name = Convert.ToString(row["Customer_Name"]);
            //Customer_Address = Convert.ToString(row["Customer_Address"]);
            //Customer_City = Convert.ToString(row["Customer_City"]);
            //Customer_State = Convert.ToString(row["Customer_State"]);
            //Customer_Country = Convert.ToString(row["Customer_Country"]);
            //License_No = Convert.ToString(row["License_No"]);
            serverEntity.Make = Make;
            serverEntity.Model = Model;
            serverEntity.Color = Color;
            serverEntity.Vehicle_Plate_No = Vehicle_Plate_No;
            serverEntity.Truck_No = Truck_No;

        }
    }
}
