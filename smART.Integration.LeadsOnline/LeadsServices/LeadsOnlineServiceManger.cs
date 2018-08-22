using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smART.ViewModel;
using smART.Common;
using System.Transactions;
using smART.Integration.LeadsOnline.LeadsOnlineWCF;

namespace smART.Integration.LeadsOnline
{
    public class LeadsOnlineServiceManger : ILeadsOnlineService
    {
        LoginInfo _loginInfo;
        ScrapContract _ticketScrapWS;
        RSmartTicketService _rsmartService;

        public LeadsOnlineServiceManger()
        {
            Initialize();
        }

        private void Initialize()
        {
          _ticketScrapWS = new ScrapContract();
            _ticketScrapWS.Url = ConfigurationHelper.GetLeadsOnlineServiceUrl();
            _rsmartService = new RSmartTicketService();
        }

        public bool PostTickets()
        {
            try
            {
                string errorMsg=string.Empty;
                int errorCode;
                if (!CheckLogin(out errorMsg))
                {
                    throw new Exception(errorMsg);
                }
                IEnumerable<Scale> scales = _rsmartService.GetPendingTickets();
                try
                {
                    foreach (var scale in scales)
                    {
                        IEnumerable<ScaleDetails> scaleDetails = _rsmartService.GetTicketItemsByTicketId(scale.ID);
                        IEnumerable<ScaleAttachments> scaleAttachments = _rsmartService.GetTicketAttachmentsByTicketId(scale.ID);
                        AddressBook partyPrimaryAddress = _rsmartService.GetAddressByPartyId(scale.Party_ID.ID);

                        Ticket t = new Ticket();
                        t = _rsmartService.MapTicketFromRsmartTicket(scale, scaleDetails, scaleAttachments, partyPrimaryAddress, t);


                        // Start transaction.
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                        {
                            IsolationLevel = IsolationLevel.Serializable
                        }))
                        {
                            string statusRemarks = string.Empty;
                            EnumPostingStatus status = EnumPostingStatus.Failed;
                            string action = "add";


                            SubmitTransaction(t, out errorMsg, out errorCode);

                            if (errorCode == 13)
                            {
                                action = "update";
                                TicketKey ticketKey = new TicketKey();
                                ticketKey.ticketnumber = scale.Scale_Ticket_No;
                                ticketKey.ticketDateTime = scale.Created_Date.ToString();
                                UpdateTransaction(ticketKey, t, out errorMsg, out errorCode);
                            }

                            if (errorCode == 0)
                            {
                                status = EnumPostingStatus.Succeed;
                                _rsmartService.UpdateTicketStatus(scale);
                                statusRemarks = string.Format("Successfully {0} ticket# {1}", action, scale.Scale_Ticket_No);
                            }
                            else
                            {
                                statusRemarks = string.Format("Failed to {0} ticket# {1} due to '{2}'", action, scale.Scale_Ticket_No, errorMsg);
                            }

                            _rsmartService.AddLeadLog(scale.Scale_Ticket_No,status ,statusRemarks);
                            scope.Complete();
                        }
                    }
                    TextFileLogger.Log(string.Format("{0}{1} ticket(s) posted on leads at {2}.", System .Environment .NewLine , scales.Count(), DateTime.Now.ToString()));
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, errorMsg);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return true;
        }

        public bool CheckLogin(out string msg)
        {
            // Check login
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;    
            Response resp = _ticketScrapWS.CheckLogin(GetLoginInfo());
            msg = resp.errorResponse;
            return resp.errorCode == 0;
        }

        public bool SubmitTransaction(Ticket t, out string msg, out int errorCode)
        {
            Response resp = _ticketScrapWS.SubmitTransaction(GetLoginInfo(), t);
            msg = resp.errorResponse;
            errorCode = resp.errorCode;
            return errorCode == 0;
        }

        public bool UpdateTransaction(TicketKey oldTicketKey, Ticket t, out string msg, out int errorCode)
        {
            Response resp = _ticketScrapWS.UpdateTransaction(GetLoginInfo(), oldTicketKey, t);
            msg = resp.errorResponse;
            errorCode = resp.errorCode;
            return errorCode == 0;
        }

        public LoginInfo GetLoginInfo()
        {
            // Login Info
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.storeId = ConfigurationHelper.GetLeadsOnlineStoreId();
            loginInfo.userName = ConfigurationHelper.GetLeadsOnlineServiceUser();
            loginInfo.password = ConfigurationHelper.GetLeadsOnlineServiceUserPwd();
            return loginInfo;            
        }
    }
}
