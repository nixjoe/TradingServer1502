using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TradingServer
{
    public static partial class Facade
    {
        //===================Instance==========================
        //
        #region Create Instance Class Security
        private static Business.Security security;
        private static Business.Security SecurityInstance
        {
            get
            {
                if (security == null)
                {
                    Facade.security = new Business.Security();
                }
                return security;
            }
        }
        #endregion

        #region Create Instance Class Security Config
		private static Business.Security securityConfig;
        private static Business.Security SecurityConfigInstance
        {
            get
            {
                if (securityConfig == null)
                {
                    securityConfig = new Business.Security();
                }
                return securityConfig;
            }
        }
	    #endregion

        #region Create Instance Class IGroupSecurity
		private static Business.IGroupSecurity iGroupSecurity;
        private static Business.IGroupSecurity IGroupSecurityInstance
        {
            get
            {
                if (iGroupSecurity == null)
                {
                    iGroupSecurity = new Business.IGroupSecurity();
                }
                return iGroupSecurity;
            }
        }
        #endregion

        #region Create Instance Class Symbol
        private static Business.Symbol symbol;
        private static Business.Symbol SymbolInstance
        {
            get
            {
                if (Facade.symbol == null)
                {
                    Facade.symbol = new Business.Symbol();
                }
                return Facade.symbol;
            }
        }
        #endregion

        #region Create Instance Class InvestorGroup
        private static Business.InvestorGroup investorGroup;
        private static Business.InvestorGroup InvestorGroupInstance
        {
            get
            {
                if (Facade.investorGroup == null)
                {
                    Facade.investorGroup = new Business.InvestorGroup();
                }
                return Facade.investorGroup;
            }
        }
        #endregion

        #region Create Instance Class Parameter Item
        private static Business.ParameterItem parameterItem;
        private static Business.ParameterItem ParameterItemInstance
        {
            get
            {
                if (Facade.parameterItem == null)
                {
                    Facade.parameterItem = new Business.ParameterItem();
                }
                return Facade.parameterItem;
            }
        }
	    #endregion

        #region Create Instance Class IGroupSymbol
        private static Business.IGroupSymbol iGroupSymbol;
        private static Business.IGroupSymbol IGroupSymbolInstance 
        {
            get
            {
                if (Facade.iGroupSymbol == null)
                {
                    Facade.iGroupSymbol = new Business.IGroupSymbol();
                }
                return Facade.iGroupSymbol;
            }
        }
        #endregion

        #region Create Instance Class Investor
        private static Business.Investor investor;
        private static Business.Investor InvestorInstance
        {
            get
            {
                if (Facade.investor == null)
                {
                    Facade.investor = new Business.Investor();
                }
                return Facade.investor;
            }
        }
        #endregion

        #region Create Instance Class Investor Profile
        //private static Business.InvestorProfile investorProfile;
        //private static Business.InvestorProfile InvestorProfileInstance
        //{
        //    get
        //    {
        //        if (Facade.investorProfile == null)
        //        {
        //            Facade.investorProfile = new Business.InvestorProfile();
        //        }
        //        return Facade.investorProfile;
        //    }
        //}
        #endregion
                
        #region Create Instance Class Command Type
        private static DBW.DBWCommandType dbwCommandType;
        private static DBW.DBWCommandType DBWCommandTypeInstance
        {
            get
            {
                if (Facade.dbwCommandType == null)
                {
                    Facade.dbwCommandType = new DBW.DBWCommandType();
                }

                return Facade.dbwCommandType;
            }
        }
        #endregion

        #region Create Instance Class SpotCommand
        private static Business.SpotCommand spotCommand;
        private static Business.SpotCommand SpotCommandInstance
        {
            get
            {
                if (Facade.spotCommand == null)
                {
                    Facade.spotCommand = new Business.SpotCommand();
                }

                return Facade.spotCommand;
            }
        }
        #endregion

        #region Create Instance Class InvestorStaus
        private static Business.InvestorStatus investorStatus;
        private static Business.InvestorStatus InvestorStatusInstance
        {
            get
            {
                if (Facade.investorStatus == null)
                {
                    Facade.investorStatus = new Business.InvestorStatus();
                }
                return Facade.investorStatus;
            }
        }
        #endregion

        #region Create Instance Class Online Command
        private static Business.OpenTrade openTrade;
        private static Business.OpenTrade OpenTradeInstance
        {
            get
            {
                if (Facade.openTrade == null)
                {
                    Facade.openTrade = new Business.OpenTrade();
                }
                return Facade.openTrade;
            }
        }
        #endregion

        #region Create Instance Class Trade Type
        private static Business.TradeType tradeType;
        private static Business.TradeType TradeTypeInstance
        {
            get
            {
                if (Facade.tradeType == null)
                {
                    Facade.tradeType = new Business.TradeType();
                }

                return Facade.tradeType;
            }
        }
        #endregion

        #region Create Instance Class Agent
        private static Business.Agent agent;
        private static Business.Agent AgentInstance
        {
            get
            {
                if (Facade.agent == null)
                {
                    Facade.agent = new Business.Agent();
                }
                return Facade.agent;
            }
        }
        #endregion

        #region Create Instance Class News
        private static Business.News news;
        private static Business.News NewsInstance
        {
            get
            {
                if (Facade.news == null)
                {
                    Facade.news = new Business.News();
                }
                return Facade.news;
            }
        }
        #endregion

        #region Create Instance Class Alert
        private static Business.PriceAlert alert;
        private static Business.PriceAlert AlertInstance
        {
            get
            {
                if (Facade.alert == null)
                {
                    Facade.alert = new Business.PriceAlert();
                }
                return Facade.alert;
            }
        }
        #endregion

        #region Create Instance Class AgentGroup
        private static Business.AgentGroup agentGroup;
        private static Business.AgentGroup AgentGroupInstance
        {
            get
            {
                if (Facade.agentGroup == null)
                {
                    Facade.agentGroup = new Business.AgentGroup();
                }
                return Facade.agentGroup;
            }
        }
        #endregion

        #region Create Instance Class IAgentSecurity
        private static Business.IAgentSecurity iAgentSecurity;
        private static Business.IAgentSecurity IAgentSecurityInstance
        {
            get
            {
                if (Facade.iAgentSecurity == null)
                {
                    Facade.iAgentSecurity = new Business.IAgentSecurity();
                }
                return Facade.iAgentSecurity;
            }
        }
        #endregion     

        #region Create Instance Class IAgentSymbol
        private static Business.IAgentGroup iAgentGroup;
        private static Business.IAgentGroup IAgentGroupInstance
        {
            get
            {
                if (Facade.iAgentGroup == null)
                {
                    Facade.iAgentGroup = new Business.IAgentGroup();
                }
                return Facade.iAgentGroup;
            }
        }
        #endregion

        #region Create Instance Class Time Event
        private static Business.TimeEvent TimeEvent;
        private static Business.TimeEvent TimeEventInstance
        {
            get
            {
                if (Facade.TimeEvent == null)
                {
                    Facade.TimeEvent = new Business.TimeEvent();
                }

                return Facade.TimeEvent;
            }
        }
        #endregion

        #region Create Instance Class Role
        private static Business.Role role;
        private static Business.Role RoleInstance
        {
            get
            {
                if (Facade.role == null)
                {
                    Facade.role = new Business.Role();
                }

                return Facade.role;
            }
        }
        #endregion

        #region Create Instance Class Permit
        private static Business.Permit permit;
        private static Business.Permit PermitInstance
        {
            get
            {
                if (Facade.permit == null)
                {
                    Facade.permit = new Business.Permit();
                }

                return Facade.permit;
            }
        }
        #endregion

        #region Create Instance Class Market
        private static Business.Market market;
        private static Business.Market MarketInstance
        {
            get
            {
                Facade.market = Business.Market.marketInstance;
                return Facade.market;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS DBW INVESTOR ACCOUNTLOG
        private static Business.InvestorAccountLog investorAccountLog;
        private static Business.InvestorAccountLog InvestorAccountLog
        {
            get
            {
                if (Facade.investorAccountLog == null)
                {
                    Facade.investorAccountLog = new Business.InvestorAccountLog();
                }

                return Facade.investorAccountLog;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS DBW ORDER DATA
        private static Business.OrderData orderData;
        internal static Business.OrderData OrderDataInstance
        {
            get
            {
                if (Facade.orderData == null)
                    Facade.orderData = new Business.OrderData();

                return Facade.orderData;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS SYSTEM LOG
        private static Business.SystemLog systemLog;
        internal static Business.SystemLog InstanceSystemLog
        {
            get
            {
                if (Facade.systemLog == null)
                {
                    Facade.systemLog = new Business.SystemLog();
                }

                return Facade.systemLog;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS INTERNAL MAIL
        private static Business.InternalMail internalMail;
        public static Business.InternalMail internalMailInstance
        {
            get
            {
                if (Facade.internalMail == null)
                    Facade.internalMail = new Business.InternalMail();

                return Facade.internalMail;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS VIRTUAL DEALER
        private static Business.VirtualDealer virtualDealer;
        private static Business.VirtualDealer VirtualDealerInstance
        {
            get
            {
                if (Facade.virtualDealer == null)
                    Facade.virtualDealer = new Business.VirtualDealer();

                return Facade.virtualDealer;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS IVIRTUAL DEALER
        private static Business.IVirtualDealer iVirtualDealer;
        private static Business.IVirtualDealer IVirtualDealerInstance
        {
            get
            {
                if (Facade.iVirtualDealer == null)
                    Facade.iVirtualDealer = new Business.IVirtualDealer();

                return Facade.iVirtualDealer;
            }
        }
        #endregion
        
        //========================Function ===============================
        //  
        #region Function Class Command Type
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.TradeType> FacadeGetAllCommandType()
        {
            return Facade.DBWCommandTypeInstance.GetAllTradeType();
        }
        #endregion

        #region Function Class Trade Type
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.TradeType> FacadeGetAllTradeType()
        {
            return Facade.TradeTypeInstance.GetAllTradeType();
        }

        /// <summary>
        /// FACADE GET TYPE NAME BY TYPE ID IN CLASS MARKET
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        public static string FacadeGetTypeNameByTypeID(int TypeID)
        {
            return Facade.TradeTypeInstance.GetTypeNameByTypeID(TypeID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public static string FacadeConvertTypeIDToName(int typeID)
        {
            return Facade.TradeTypeInstance.ConvertTypeIDToName(typeID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public static bool FacadeGetIsBuyByTypeID(int typeID)
        {
            return Facade.TradeTypeInstance.GetIsBuyByTypeID(typeID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountCommandType()
        {
            return Facade.TradeTypeInstance.CountTotalCommandType();
        }
        #endregion

        #region Function Class News
        public static int FacadeAddNews(string title, string body, DateTime timeAdd, string category)
        {
            return Facade.NewsInstance.AddNews(title, body, timeAdd, category);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void FacadeGetTopNews()
        {
            Facade.NewsInstance.GetTopNews();
        }
        #endregion

        #region Function Class Time Event
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int FacadeAddTimeEvent()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Function Class Market
        /// <summary>
        /// FACADE FIND OPEN TRADE IN COMMAND LIST OF CLASS MARKET
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeFindOpenTradeInCommandList(int CommandID)
        {
            return Facade.MarketInstance.FindOpenTradeInSymbolList(CommandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refCommandID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeFindOpenTradeInSymbolListByRefID(int refCommandID)
        {
            return Facade.MarketInstance.FindOpenTradeInSymbolListByRefID(refCommandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeFindOpenTradeInCommandEx(int commandID)
        {
            return Facade.MarketInstance.FindOpenTradeInCommandExe(commandID);
        }

        /// <summary>
        /// FACADE FIND OPEN TRADE IN INVESTOR LIST OF CLASS MARKET
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeFindOpenTradeInInvestorList(int CommandID)
        {
            return Facade.MarketInstance.FindOpenTradeInInvestorList(CommandID);
        }

        /// <summary>
        /// FACADE REMOVE OPEN TRADE IN COMMAND LIST OF CLASS MARKET
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        public static bool FacadeRemoveOpenTradeInCommandList(int CommandID)
        {
            return Facade.MarketInstance.FindAndRemoveOpenTradeInCommandList(CommandID);
        }

        /// <summary>
        /// FACADE REMOVE OPEN TRADE IN COMMAND EXECUTOR OF CLASS MARKET
        /// </summary>
        /// <param name="CommandID"></param>
        public static bool FacadeRemoveOpenTradeInCommandExecutor(int CommandID)
        {
            return Facade.MarketInstance.FindAndRemoveOpenTradeInCommandExecutor(CommandID);
        }

        /// <summary>
        /// FACADE GET OPEN TRADE BY OPEN TRADE ID OF CLASS MARKET IN COMMAND EXCUTOR LIST
        /// </summary>
        /// <param name="OpenTradeID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeGetOpenTradeByOpenTradeID(int OpenTradeID)
        {
            return Facade.OpenTradeInstance.GetOnlineCommandByOnlineCommandID(OpenTradeID);
        }

        /// <summary>
        /// FACADE REMOVE OPEN TRADE IN BINARY COMMAND LIST CLASS MARKET(IT WILL CALL THEN CANCEL BINARY)
        /// </summary>
        /// <param name="CommandID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeRemoveOpenTradeInBinaryCommandList(int CommandID)
        {
            return Facade.MarketInstance.FindAndRemoveOpenTradeInBinaryCommandList(CommandID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static Business.OpenTrade FacadeFillInstanceOpenTrade(int InvestorID,string Symbol,int Type)
        {
            return Facade.MarketInstance.FillInstanceToCommand(InvestorID, Symbol, Type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        public static Model.MailConfig FacadeGetMailConfig(Business.Investor objInvestor)
        {
            return Facade.MarketInstance.GetMailConfig(objInvestor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.Tick> FacadeGetTickOnline()
        {
            return Facade.MarketInstance.GetTickOnline();
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="SymbolName"></param>
        /// <returns></returns>
        public static List<Business.Tick> FacadeGetQueueTickOfSymbol(string SymbolName)
        {
            return Facade.MarketInstance.GetTickQueueBySymbolName(SymbolName);
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <returns></returns>
        public static List<Business.QuoteSymbol> FacadeGetQuoteList()
        {
            return Facade.MarketInstance.GetQuoteList();
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <returns></returns>
        public static List<Business.Symbol> FacadeGetSymbolList()
        {
            return Facade.MarketInstance.GetSymbolList();
        }

        /// <summary>
        /// FILL INSTANCE OPEN TRADE
        /// </summary>
        /// <param name="objCommand"></param>
        /// <param name="Command"></param>
        public static void FillInstanceOpenTrade(Business.RequestDealer RequestObject, Business.OpenTrade Command)
        {
            #region Find Symbol In Symbol List Command Type,Symbol
            //Find Symbol In Symbol List Command Type
            if (Business.Market.SymbolList != null)
            {
                bool FlagSymbol = false;
                int countSymbol = Business.Market.SymbolList.Count;
                for (int j = 0; j < countSymbol; j++)
                {
                    if (Business.Market.SymbolList[j].Name == RequestObject.Request.Symbol.Name)
                    {
                        if (Business.Market.SymbolList[j].MarketAreaRef.Type != null)
                        {
                            int countType = Business.Market.SymbolList[j].MarketAreaRef.Type.Count;
                            for (int n = 0; n < countType; n++)
                            {
                                if (Business.Market.SymbolList[j].MarketAreaRef.Type[n].ID == RequestObject.Request.Type.ID)
                                {
                                    Command.Type = Business.Market.SymbolList[j].MarketAreaRef.Type[n];
                                    break;
                                }
                            }
                        }

                        Command.Symbol = Business.Market.SymbolList[j];
                        FlagSymbol = true;
                        break;
                    }

                    if (FlagSymbol == false)
                    {
                        if (Business.Market.SymbolList[j].RefSymbol != null && Business.Market.SymbolList[j].RefSymbol.Count > 0)
                        {
                            Command.Symbol = Model.CommandFramework.CommandFrameworkInstance.ClientFindSymbolReference(Business.Market.SymbolList[j].RefSymbol, RequestObject.Request.Symbol.Name);

                            if (Command.Symbol != null)
                            {
                                int countType = Command.Symbol.MarketAreaRef.Type.Count;
                                for (int k = 0; k < countType; k++)
                                {
                                    if (Command.Symbol.MarketAreaRef.Type[k].ID == RequestObject.Request.Type.ID)
                                    {
                                        Command.Type = Command.Symbol.MarketAreaRef.Type[k];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Find Investor List
            //Find Investor List
            if (Business.Market.InvestorList != null)
            {
                int countInvestor = Business.Market.InvestorList.Count;
                for (int n = 0; n < countInvestor; n++)
                {
                    if (Business.Market.InvestorList[n].InvestorID == RequestObject.Request.Investor.InvestorID)
                    {
                        Command.Investor = Business.Market.InvestorList[n];
                        break;
                    }
                }
            }
            #endregion

            #region Fill IGroupSecurity
            if (Command.Investor != null)
            {
                if (Business.Market.IGroupSecurityList != null)
                {
                    int countIGroupSecurity = Business.Market.IGroupSecurityList.Count;
                    for (int i = 0; i < countIGroupSecurity; i++)
                    {
                        if (Business.Market.IGroupSecurityList[i].SecurityID == Command.Symbol.SecurityID &&
                            Business.Market.IGroupSecurityList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                        {
                            Command.IGroupSecurity = Business.Market.IGroupSecurityList[i];
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Find Spread Difference And Add To Property SpreadDifference In Symbol
            double spreadDifference = 0;
            spreadDifference = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(Command.Symbol.SecurityID, Command.Investor.InvestorGroupInstance.InvestorGroupID);
            Command.SpreaDifferenceInOpenTrade = spreadDifference;
            //if (Command.IGroupSecurity != null)
            //{
            //    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
            //    {
            //        int count = Command.IGroupSecurity.IGroupSecurityConfig.Count;
            //        for (int i = 0; i < count; i++)
            //        {
            //            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B04")
            //            {
            //                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out spreadDifference);
            //                Command.Symbol.SpreadDifference = spreadDifference;
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion
        }
      
        #endregion

        #region Function Class Calculator Facade
        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeGetFreeCalculator()
        {
            return Business.CalculatorFacade.FreeCalculator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> FacadeGetCalculatorInformation()
        {
            return Business.CalculatorFacade.GetCalculatorInfo();
        }
        #endregion

        #region FUNCTION CLASS INVESTOR ACCOUNT LOG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<Business.InvestorAccountLog> FacadeGetInvestorAccountLogByInvestorID(int InvestorID)
        {
            return Facade.InvestorAccountLog.GetInvestorAccountLogByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<Business.InvestorAccountLog> FacadeGetInvestorAccountLogWithTime(DateTime TimeStart, DateTime TimeEnd, int InvestorID)
        {
            return Facade.InvestorAccountLog.GetInvestorAccountLogWithTime(TimeStart, TimeEnd, InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public static List<Business.InvestorAccountLog> FacadeGetInvestorAccountWithComment(string Comment)
        {
            return Facade.InvestorAccountLog.GetInvestorAccountLogWithComment(Comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        public static int FacadeAddInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            return Facade.InvestorAccountLog.AddNewInvestorAccountLog(objInvestorAccountLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DealID"></param>
        /// <returns></returns>
        public static bool FacadeUpdateDealID(int ID, string DealID)
        {
            return Facade.InvestorAccountLog.UpdateDealID(ID, DealID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorAccountLogID"></param>
        /// <returns></returns>
        public static bool FacadeDeleteInvestorAccountLog(int InvestorAccountLogID)
        {
            return Facade.InvestorAccountLog.DeleteInvestorAccountLog(InvestorAccountLogID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorAccountLog"></param>
        /// <returns></returns>
        public static bool FacadeUpdateInvestorAccountLog(Business.InvestorAccountLog objInvestorAccountLog)
        {
            return Facade.InvestorAccountLog.UpdateInvestorAccountLog(objInvestorAccountLog);
        }
        #endregion

        #region YEAR EVENT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListTimeEvent"></param>
        //public static bool FacadeAddYearEvent(List<Business.TimeEvent> ListTimeEvent)
        //{
        //    return TradingServer.Facade.MarketInstance.AddYearEvent(ListTimeEvent);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListTimeEvent"></param>
        /// <returns></returns>
        //public static bool FacadeUpdateYearEvent(List<Business.TimeEvent> ListTimeEvent)
        //{
        //    return TradingServer.Facade.MarketInstance.UpdateYearEvent(ListTimeEvent);
        //}
        #endregion

        #region ORDER DATA
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        public static List<Business.OrderData> FacadeGetDataOrder(int InvestorID, int Start, int Limit)
        {
            return Facade.OrderDataInstance.GetOrderDataStartEnd(InvestorID, Start, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static Business.OrderData FacadeGetOrderDataByCode(string Code)
        {
            return Facade.OrderDataInstance.GetOrderDataByCode(Code);
        }
        #endregion

        #region FUNCTION CLASS SYSTEM LOG
        /// <summary>
        /// ADD NEW SYSTEM LOG TO MARKET
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="content"></param>
        /// <param name="comment"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool FacadeAddNewSystemLog(int typeID, string content, string comment, string ipAddress, string investorCode)
        {
            Business.SystemLog newSystemLog = new Business.SystemLog();
            newSystemLog.TypeID = typeID;
            newSystemLog.LogContent = content;
            newSystemLog.Comment = comment;
            newSystemLog.IPAddress = ipAddress;
            newSystemLog.InvestorCode = investorCode;

            //return Facade.InstanceSystemLog.Insert(typeID, content, comment, ipAddress, investorCode);
            return Facade.InstanceSystemLog.Insert(newSystemLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="content"></param>
        /// <param name="comment"></param>
        /// <param name="ipAddress"></param>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        public static bool FacadeAddNewTickLog(int typeID, string content, string comment, string ipAddress, string investorCode)
        {
            Business.SystemLog newSystemLog = new Business.SystemLog();
            newSystemLog.TypeID = typeID;
            newSystemLog.LogContent = content;
            newSystemLog.Comment = comment;
            newSystemLog.IPAddress = ipAddress;
            newSystemLog.InvestorCode = investorCode;

            Facade.InstanceSystemLog.AddNewTickLog(newSystemLog);

            return true;
        }

        /// <summary>
        /// ADD NEW SYSTEM LOG TO DATABASE
        /// </summary>
        /// <param name="log"></param>
        public static bool FacadeAddNewSystemLog(Business.SystemLog log)
        {
            return Facade.InstanceSystemLog.Insert(log.TypeID, log.LogContent, log.Comment, log.IPAddress, log.InvestorCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetSystemLogByTime(DateTime begin, DateTime end)
        {
            return Facade.InstanceSystemLog.GetByTime(begin, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetSystemLogByTimeAndType(int typeID, DateTime begin, DateTime end)
        {
            return Facade.InstanceSystemLog.GetByTimeAndTye(typeID, begin, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="typeID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetSystemLogByCodeAndTime(DateTime begin, DateTime end, int typeID, string code)
        {
            return Facade.InstanceSystemLog.GetCodeAndTime(typeID, begin, end, code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetLogLikeCode(DateTime begin, DateTime end, string code)
        {
            return Facade.InstanceSystemLog.GetLogLikeContent(code, begin, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetLogByIPAddress(DateTime begin, DateTime end, string ipAddress)
        {
            return Facade.InstanceSystemLog.GetLogByIPAddress(ipAddress, begin, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="ipAddress"></param>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public static List<Business.SystemLog> FacadeGetLogByIPAddressAndType(DateTime begin, DateTime end, string ipAddress, int typeID)
        {
            return Facade.InstanceSystemLog.GetLogByIPAddressAndType(begin, end, typeID, ipAddress);
        }
        #endregion

        #region FUNCTION CLASS VIRTUAL DEALER
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static List<Business.VirtualDealer> FacadeGetAllVirtualDealer()
        //{
        //    return Facade.VirtualDealerInstance.GetAllVirtualDealer();
        //}

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        //public static int FacadeAddNewVirtualDealer(Business.VirtualDealer objVirtualDealer)
        //{
        //    return Facade.VirtualDealerInstance.AddVirtualDealer(objVirtualDealer);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        //public static bool FacadeUpdateVirtualDealer(Business.VirtualDealer objVirtualDealer)
        //{
        //    return Facade.VirtualDealerInstance.UpdateVirtualDealer(objVirtualDealer);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        //public static bool FacadeDeleteVirtualDealer(int virtualDealerID)
        //{
        //    return Facade.VirtualDealerInstance.DeleteVirtualDealer(virtualDealerID);
        //}
        #endregion

        #region FUNCTION CLASS VIRTUAL DEALER CONFIG
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.ParameterItem> FacadeGetVirtualDealerConfig()
        {
            return Facade.ParameterItemInstance.GetAllVirtualDealerConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerConfigID"></param>
        /// <returns></returns>
        public static List<Business.ParameterItem> FacadeGetVirtualDealerConfigByID(int virtualDealerConfigID)
        {
            return Facade.ParameterItemInstance.GetVirtualDealerConfigByVirtualDealerID(virtualDealerConfigID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        public static List<Business.ParameterItem> FacadeGetVirtualConfigByVirtualDealerID(int virtualDealerID)
        {
            return Facade.ParameterItemInstance.GetVirtualDealerConfigByVirtualDealerID(virtualDealerID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        public static int FacadeAddNewVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.ParameterItemInstance.AddVirtualDealerConfig(objParameterItem);
        }


        public static bool FacadeUpdateVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            return Facade.ParameterItemInstance.UpdateVirtualDealerConfig(objParameterItem);
        }
        #endregion

        #region GET LAST BALANCE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        public static string FacadeGetLastCandles(string symbol, int timeFrame,int port)
        {
            return ProcessQuoteLibrary.Business.QuoteProcess.GetLastCandles(symbol, timeFrame, port);
        }
        #endregion

        /// <summary>
        /// DEBUG
        /// </summary>
        public static void FacadeSetIsBlock(bool IsBlock)
        {
            TradingServer.Business.CalculatorFacade.IsBlock = IsBlock;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="IsBlock"></param>
        public static void FacadeSetIsBlockBuildCandles(bool IsBlock)
        {
            ProcessQuoteLibrary.Business.QuoteProcess.IsBlock = IsBlock;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static List<Business.Tick> FacadeCheckQueueTick(int Number)
        {
            List<Business.Tick> ResultTick = new List<Business.Tick>();
            List<ProcessQuoteLibrary.Business.Tick> Result = new List<ProcessQuoteLibrary.Business.Tick>();
            Result = ProcessQuoteLibrary.Business.QuoteProcess.GetQueueCheck(Number);
            if (Result != null)
            {
                int count = Result.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.Tick newTick = new Business.Tick();
                    newTick.Ask = double.Parse(Result[i].Ask);
                    newTick.Bid = double.Parse(Result[i].Bid);
                    newTick.Status = Result[i].Status;
                    newTick.SymbolName = Result[i].Symbol;
                    newTick.TickTime = Result[i].DateTime;

                    ResultTick.Add(newTick);
                }
            }

            return ResultTick;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountQueueTick()
        {
            return ProcessQuoteLibrary.Business.QuoteProcess.GetCountQueue();            
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountCommandExecutor()
        {
            return TradingServer.Business.Market.CommandExecutor.Count;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountCommandInSymbol()
        {
            int result = 0;
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        result += Business.Market.SymbolList[i].CommandList.Count;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DEBUG
        /// </summary>
        /// <returns></returns>
        public static int FacadeCountCommandInInvestor()
        {
            int result = 0;
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                { 
                    if(Business.Market.InvestorList[i].CommandList!= null)
                    {
                        result += Business.Market.InvestorList[i].CommandList.Count;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void FacadeProcessCheckListCommand()
        {
            for (int i = 0; i < TradingServer.Business.Market.InvestorList.Count; i++)
            {
                if (TradingServer.Business.Market.InvestorList[i].CommandList != null)
                {
                    for (int j = 0; j < TradingServer.Business.Market.InvestorList[i].CommandList.Count; j++)
                    {
                        if (Business.Market.CommandExecutor != null)
                        {
                            bool flag = false;
                            for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                            {
                                if (Business.Market.InvestorList[i].CommandList[j].ID == Business.Market.CommandExecutor[n].ID)
                                {
                                    flag = true;
                                    break;
                                }                                
                            }

                            if (!flag)
                            {
                                TradingServer.Facade.FacadeAddNewSystemLog(1, "CommandID: " + Business.Market.InvestorList[i].CommandList[j].ID, "Check Command", "", "");

                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        public static void FacadeResetUserConfig(int investorID)
        {
            TradingServer.Facade.FacadeUpdateUserConfig(investorID, "");
        }
    }
}