﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
namespace TradingServer.Business
{
    public partial class Agent
    {
        //internal static System.Threading.Thread threadOne;
        bool isDisable;
        string code;
        internal Agent()
        {
            this.AgentMail = new List<InternalMail>();
            this.AlertQueue = new List<PriceAlert>();
            IsVirtualDealer = false;
            IsDealer = false;
            IsEditDealer = false;
            IsBusy = false;
            IsAdmin = false;
            IsReports = false;
            IsDownloadStatements = false;
            IsInternalMail = false;
            IsSendNews = false;
            IsConections = false;
            IsConfigServerPlugins = false;
            IsSuperviseTrades = false;
            IsAccountant = false;
            IsRiskManager = false;
            IsJournals = false;
            IsMarketWatch = false;
            IsPersonalDetails = false;
            IsAutomaticServerReports = false;
            ProcessQuoteLibrary.Business.QuoteProcess.QuoteInstance.BuildCandle = ArchiveCandlesOffline;
        }

        public int AgentID { get; set; }
        public int AgentGroupID { get; set; }
        public string Name { get; set; }
        public int InvestorID { get; set; }
        public string Comment { get; set; }
        public string KeyActive { get; set; }
        public bool IsDisable 
        {
            get
            {
                if (this.IsVirtualDealer)
                {
                    return !this.VirtualDealer.IsEnable;
                }
                else
                {
                    return this.isDisable;
                }
            }
            set
            {
                this.isDisable = value;
            }
        }

        public List<InternalMail> AgentMail = new List<InternalMail>();
        public List<IAgentSecurity> IAgentSecurity = new List<IAgentSecurity>();
        public string GroupCondition
        {
            get;
            set;
        }
        public List<IAgentGroup> IAgentGroup = new List<IAgentGroup>();
        public List<Business.PriceAlert> AlertQueue = new List<PriceAlert>();
        public DateTime TimeSync { get; set; }
        public bool IsBusy { get; set; }
        public bool IsOnline { get; set; }
        public string Code 
        {
            get
            {
                if (this.IsVirtualDealer)
                {
                    return this.VirtualDealer.Name;
                }else
                {
                    return this.code;
                }
            }
            set
            {
                this.code = value;
            }
        }
        public int NumRequest { get; set; }
        public bool IsIpFilter { get; set; }
        public string IpForm { get; set; }
        public string IpTo { get; set; }
        public string Ip { get; set; }
        public string Pwd { get; set; }
        public List<Business.RequestDealer> VirtualRequestDealers = new List<RequestDealer>();
        public Business.VirtualDealer VirtualDealer = new VirtualDealer();
        public bool IsVirtualDealer { get; set; }
        public bool IsDealer { get; set; }
        public bool IsEditDealer { get; set; }
        public bool IsManager { get; set; }        
        public bool IsAdmin { get; set; }
        public bool IsReports { get; set; }
        public bool IsDownloadStatements { get; set; }
        public bool IsInternalMail { get; set; }
        public bool IsSendNews { get; set; }
        public bool IsConections { get; set; }
        public bool IsConfigServerPlugins { get; set; }
        public bool IsSuperviseTrades { get; set; }
        public bool IsAccountant { get; set; }
        public bool IsRiskManager { get; set; }
        public bool IsJournals { get; set; }
        public bool IsMarketWatch { get; set; }
        public bool IsPersonalDetails { get; set; }
        public bool IsAutomaticServerReports  { get; set; }

        #region PARAMETER GET HISTORY REPORT AND GET HISTORY INVESTOR(CREATE DATE : DATE :01- MONTH: 06: YEAR : 2011)
        public List<Business.OpenTrade> ListHistoryInvestor { get; set; }
        public List<Business.OpenTrade> ListHistoryReport { get; set; }
        #endregion

        #region PARAMETER GET SYSTEM LOG OF AGENT(CREATE DATE : DATE : 18- MONTH 07 - YEAR : 2011)
        public List<Business.SystemLog> ListSystemLog { get; set; }
        #endregion

        #region Create Instance Class DBWAgent
        private static DBW.DBWAgent dbwAgent;
        private static DBW.DBWAgent DBWAgentInstance
        {
            get
            {
                if (Agent.dbwAgent == null)
                {
                    Agent.dbwAgent = new DBW.DBWAgent();
                }
                return Agent.dbwAgent;
            }
        }
        #endregion

        internal List<int> MakeListIAgentGroupManager(string condition)
        {
            List<int> result = new List<int>();
            List<string> listGroupStringResult = new List<string>();
            List<string> listGroupString = new List<string>();
            for (int i = Business.Market.InvestorGroupList.Count - 1; i >= 0; i--)
            {
                listGroupString.Add(Business.Market.InvestorGroupList[i].Name);
            }
            if (!string.IsNullOrEmpty(condition))
            {
                listGroupStringResult = this.Build(condition, listGroupString);
            }
            int count = listGroupStringResult.Count;
            for (int j = count - 1; j >= 0; j--)
            {
                for (int i = Business.Market.InvestorGroupList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.InvestorGroupList[i].Name == listGroupStringResult[j])
                    {
                        result.Add(Business.Market.InvestorGroupList[i].InvestorGroupID);
                        break;
                    }
                }
            }
            return result;
        }
        internal List<int> MakeListSymbolIDManager(string condition)
        {
            List<int> result = new List<int>();
            List<string> listSymbolStringResult = new List<string>();
            List<string> listSymbolString = new List<string>();
            for (int i = Business.Market.SymbolList.Count - 1; i >= 0; i--)
            {
                listSymbolString.Add(Business.Market.SymbolList[i].Name);
            }
            if (!string.IsNullOrEmpty(condition))
            {
                listSymbolStringResult = this.Build(condition, listSymbolString);
            }
            int count = listSymbolStringResult.Count;
            for (int j = count - 1; j >= 0; j--)
            {
                for (int i = Business.Market.SymbolList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.SymbolList[i].Name == listSymbolStringResult[j])
                    {
                        result.Add(Business.Market.SymbolList[i].SymbolID);
                        break;
                    }
                }
            }
            return result;
        }
        internal List<string> Build(string a, List<string> gtVao)
        {
            List<string> result = new List<string>();
            List<string> tmpResult = new List<string>();
            List<string> tmpStrB = new List<string>();
            List<string> value = new List<string>();
            List<string> cancel = new List<string>();
            List<string> tmpCan = new List<string>();
            List<string> ok = new List<string>();
            List<string> tmpOk = new List<string>();
            List<string> ketqua = new List<string>();
            List<string> tmpKetQua = new List<string>();
            List<string> b = new List<string>();
            string pattern = ",";
            string pattern2 = "*";
            string pattern3 = "(([0-9a-zA-Z_]*[^0-9a-zA-Z_]*)*)";
            Regex myRegex = new Regex(pattern);
            string[] KQ = myRegex.Split(a);
            int demVao = gtVao.Count;
            for (int i = 0; i < demVao; i++)
            {
                b.Add(gtVao[i]);
            }
            for (int i = 0; i < KQ.Length; i++)
            {
                if (KQ[i] != "")
                {
                    tmpKetQua.Add(KQ[i]);
                }
            }
            for (int i = 0; i < tmpKetQua.Count; i++)
            {
                if (tmpKetQua[i] == "!*")
                {
                    return result;
                }
            }
            for (int i = 0; i < tmpKetQua.Count; i++)
            {
                if (Regex.Matches(tmpKetQua[i], "!").Count < 2)//nhan nhung dk co 1 or 0 dau !
                {
                    if (tmpKetQua[i].StartsWith("!"))
                    {
                        tmpCan.Add(tmpKetQua[i].Replace("!", ""));
                    }
                    else
                    {
                        if (Regex.Matches(tmpKetQua[i], "!").Count > 0)
                        {
                            tmpKetQua.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            tmpOk.Add(tmpKetQua[i]);
                        }
                    }
                }
                else//loai nhung dk lon hon or = 2 dau !
                {
                    tmpKetQua.RemoveAt(i);
                    i--;
                }
            }//end for
            cancel = ChecksDK(tmpCan);
            ok = ChecksDK(tmpOk);
            for (int i = 0; i < ok.Count; i++)
            {
                if (pattern2 == ok[i])
                {
                    tmpStrB = b;
                    break;
                }
            }//end for
            if (tmpStrB != b)//Khong co * trong list nhan
            {
                #region chi nhan vai gia tri
                for (int i = 0; i < ok.Count; i++)
                {
                    for (int j = 0; j < cancel.Count; j++)
                    {
                        if (ok[i] == cancel[j])
                        {
                            ok.RemoveAt(i);
                            cancel.RemoveAt(j);
                            j--;
                        }
                    }//end for
                }//end for
                for (int i = 0; i < ok.Count; i++)
                {
                    ok[i] = ok[i].Replace(pattern2, pattern3);
                }
                for (int i = 0; i < ok.Count; i++)
                {
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (Regex.IsMatch(b[j], @"^" + ok[i] + "$"))//@"^ va $ dung de kiem tra phai dung' dinh dang dua ra
                        {
                            result.Add(b[j]);
                            b.RemoveAt(j);
                            j--;
                        }
                    }//end for2
                }//end for1
                int demC = cancel.Count;
                for (int i = 0; i < demC; i++)
                {
                    cancel[i] = cancel[i].Replace(pattern2, pattern3);
                }
                for (int i = 0; i < cancel.Count; i++)
                {
                    for (int j = 0; j < result.Count; j++)
                    {
                        if (Regex.IsMatch(result[j], @"^" + cancel[i] + "$"))
                        {
                            result.RemoveAt(j);
                            j--;
                        }
                    }
                }//end for
                return result;
                #endregion
            }
            else
            {
                #region nhan tat ca gia tri
                for (int i = 0; i < cancel.Count; i++)
                {
                    cancel[i] = cancel[i].Replace(pattern2, pattern3);
                }
                for (int i = 0; i < cancel.Count; i++)
                {
                    for (int j = 0; j < tmpStrB.Count; j++)
                    {
                        if (Regex.IsMatch(tmpStrB[j], @"^" + cancel[i] + "$"))
                        {
                            tmpStrB.RemoveAt(j);
                            j--;
                        }//end if
                    }//end for
                }//end for
                return tmpStrB;
                #endregion
            }//end else
        }//end func

        internal List<string> ChecksDK(List<string> gtB)
        {
            string par1 = "**";
            string par3 = "*";
            string my1 = "[*]";
            Regex myR1 = new Regex(@"[" + par3 + "]*");
            for (int i = 0; i < gtB.Count; i++)//loai bo dieu kien nao bat dau hoac ket thuc bang ** hoac !!
            {
                if (gtB[i].StartsWith(par1) || gtB[i].EndsWith(par1))
                {
                    gtB.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gtB.Count; i++)//loai nhung dieu kien nao o giua co *
            {
                MatchCollection mc = Regex.Matches(gtB[i], my1);
                if (mc.Count >= 3)
                {
                    gtB.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gtB.Count; i++)
            {
                int a = gtB[i].Length - 1;
                if (gtB[i].IndexOf(par3) > 0 && gtB[i].IndexOf(par3) < a || gtB[i].LastIndexOf(par3) > 0 && gtB[i].LastIndexOf(par3) < a)
                {
                    gtB.RemoveAt(i);
                    i--;
                }
            }//end for
            return gtB;
        }//end func

        internal void LoginVirtualDealer(Business.VirtualDealer virtualDealer)
        {
            this.Ip = "127.0.0.1";
            this.AgentID = -1;
            this.TimeSync = new DateTime(2999, 1, 1);
            this.IsBusy = false;
            this.IsOnline = true;
            this.IsDealer = true;
            this.IsVirtualDealer = true;
            this.VirtualDealer = virtualDealer;
            Market.AgentList.Add(this);
            Thread thread = new Thread(new ThreadStart(this.IniVirturalDealer));
            thread.Start();
        }        

        internal void IniVirturalDealer()
        {
            while (true)
            {
                for (int i = this.VirtualRequestDealers.Count - 1; i >= 0; i--)
                {
                    switch (this.VirtualRequestDealers[i].Name.ToLower())
                    {
                        case "open":
                            {
                                this.StartOpenTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                        case "close":
                            {
                                this.StartCloseTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                        //system process not to this
                        case "update":
                            {
                                this.StartUpdateTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                        //system process not to this
                        case "updatepending":
                            {
                                this.StartUpdateTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                        //system process not to this
                        case "closepending":
                            {
                                this.StartDeleteTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                        //system process not to this
                        case "openpending":
                            {
                                this.StartOpenPendingTheThread(this.VirtualRequestDealers[i]);
                                break;
                            }
                    }                    
                    this.VirtualRequestDealers.RemoveAt(i);
                }
                Thread.Sleep(100);
            }
        }

        public Thread StartOpenTheThread(RequestDealer param1)
        {
            var t = new Thread(() => this.VirtualDealerAutoOpen(param1));
            t.Start();
            return t;
        }

        public Thread StartCloseTheThread(RequestDealer param1)
        {
            var t = new Thread(() => this.VirtualDealerAutoClose(param1));
            t.Start();
            return t;
        }

        public Thread StartUpdateTheThread(RequestDealer param1)
        {
            var t = new Thread(() => this.VirtualDealerAutoUpdate(param1));
            t.Start();
            return t;
        }

        public Thread StartDeleteTheThread(RequestDealer param1)
        {
            var t = new Thread(() => this.VirtualDealerAutoDelete(param1));
            t.Start();
            return t;
        }

        public Thread StartOpenPendingTheThread(RequestDealer param1)
        {
            var t = new Thread(() => this.VirtualDealerAutoOpenPending(param1));
            t.Start();
            return t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Agent ManagerLogin(string Code, string Pwd,string KeyActive, string ipAddress)
        {
            Business.Agent agent = new Agent();            
            Business.Investor investor = new Investor();
            investor = investor.LoginAgent(Code, Pwd);
            if (investor != null)
            {
                agent = agent.GetAgentByInvestorID(investor.InvestorID);
                if (agent != null)
                {
                    bool checkIp = agent.CheckValidRangeIpAddress(ipAddress);
                    if (checkIp == false)
                    {
                        string content = "'" + Code + "': manager blocked by IP" ;
                        string comment = "[login manager]";
                        TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                        return null;
                    }
                    for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
                    {
                        if (Business.Market.AgentList[i].AgentID == agent.AgentID)
                        {
                            if (!Business.Market.AgentList[i].IsDisable)
                            {
                                Business.Market.AgentList[i].TimeSync = DateTime.Now;
                                Business.Market.AgentList[i].Ip = ipAddress;
                                Business.Market.AgentList[i].IsBusy = true;
                                Business.Market.AgentList[i].IsOnline = true;
                                this.AddNoticeManager(Business.Market.AgentList[i].AgentID, KeyActive, Business.Market.AgentList[i]);
                                this.GetNoticeDealer(Business.Market.AgentList[i].AgentID, KeyActive);
                                this.SendNoticeManagerOnline(Business.Market.AgentList[i]);
                                return Business.Market.AgentList[i];
                            }
                            else return null;
                        }
                    }
                    agent.Code = Code;
                    agent.TimeSync = DateTime.Now;
                    agent.Ip = ipAddress;
                    agent.IsBusy = true;
                    agent.IsOnline = true;
                    agent.IAgentGroup = new List<Business.IAgentGroup>();
                    agent.IAgentGroup = Facade.FacadeGetIAgentGroupByAgentID(agent.AgentID);
                    agent.IAgentSecurity = new List<Business.IAgentSecurity>();
                    agent.IAgentSecurity = Facade.FacadeGetIAgentSecurityByAgentID(agent.AgentID);
                    List<Business.Permit> listPermit = new List<Business.Permit>();
                    listPermit = Facade.FacadeGetPermitByAgentID(agent.AgentID);
                    for (int i = listPermit.Count - 1; i >= 0; i--)
                    {
                        #region Enable Role
                        switch (listPermit[i].Role.Code)
                        {
                            case "R01":
                                agent.IsManager = true;
                                break;
                            case "R02":
                                agent.IsSuperviseTrades = true;
                                break;
                            case "R03":
                                agent.IsAdmin = true;
                                break;
                            case "R04":
                                agent.IsAccountant = true;
                                break;
                            case "R05":
                                agent.IsReports = true;
                                break;
                            case "R06":
                                agent.IsRiskManager = true;
                                break;
                            case "R07":
                                agent.IsInternalMail = true;
                                break;
                            case "R08":
                                agent.IsJournals = true;
                                break;
                            case "R09":
                                agent.IsSendNews = true;
                                break;
                            case "R10":
                                agent.IsMarketWatch = true;
                                break;
                            case "R11":
                                agent.IsConections = true;
                                break;
                            case "R12":
                                agent.IsPersonalDetails = true;
                                break;
                            case "R13":
                                agent.IsConfigServerPlugins = true;
                                break;
                            case "R14":
                                agent.IsAutomaticServerReports = true;
                                break;
                            case "R15":
                                agent.IsDealer = true;
                                break;
                            case "R16":
                                agent.IsEditDealer = true;
                                break;
                            case "R17":
                                agent.IsDownloadStatements = true;
                                break;
                        }

                        #endregion
                    }
                    if (!agent.IsDisable)
                    {
                        agent.AlertQueue = new List<PriceAlert>();
                        agent = this.GetAlertByAgent(agent);
                        Business.Market.AgentList.Add(agent);
                        this.AddNoticeManager(agent.AgentID, KeyActive, agent);
                        this.GetNoticeDealer(agent.AgentID, KeyActive);
                        this.SendNoticeManagerOnline(agent);
                    }
                    else return null;
                }
            }
            return agent;
        }

        internal bool CheckValidRangeIpAddress(string currentIp)
        {
            if (this.IsIpFilter == false) return true;
            AddressFamily addressFamily;
            byte[] lowerBytes;
            byte[] upperBytes;

            IPAddress from, to, current;
            IPAddress.TryParse(this.IpForm, out from);
            IPAddress.TryParse(this.IpTo, out to);
            IPAddress.TryParse(currentIp, out current);
            addressFamily = from.AddressFamily;
            lowerBytes = from.GetAddressBytes();
            upperBytes = to.GetAddressBytes();

            if (current.AddressFamily != addressFamily)
            {
                return false;
            }

            byte[] addressBytes = current.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }

        internal bool ManagerChangePass(int investorID, string code, string oldPass, string newPass)
        {
            bool result = false;
            Business.Investor investor = new Investor();
            investor = investor.LoginAgent(code, oldPass);
            if (investor != null)
            {
                result = TradingServer.Facade.FacadeUpdatePasswordByCode(code, newPass);
            }
            return result;
        }


        internal void AddNoticeManager(int AgentID, string KeyActive,Agent Manager)
        {
            if (NoticeManager.ContainsKey(AgentID))
            {
                Agent agent = NoticeManager[AgentID];
                if (agent != null)
                {
                    agent.KeyActive = KeyActive;
                }
            }
            else
            {
                Manager.KeyActive = KeyActive;
                NoticeManager.Add(AgentID, Manager);
            }
        }

        internal bool CheckPermitSendMail(string code,int groupInvestorID)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    if (Market.AgentList[i].IsInternalMail)
                    {
                        for (int j = 0; j < Market.AgentList[i].IAgentGroup.Count; j++)
                        {
                            if (Market.AgentList[i].IAgentGroup[j].InvestorGroupID == groupInvestorID)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal Business.Agent AdminLogin(string Code, string Pwd,string ipAddress)
        {
            Business.Agent agent = new Agent();
            Business.Investor investor = new Investor();
            investor = investor.LoginAgent(Code, Pwd);
            if (investor != null)
            {
                agent = agent.GetAgentByInvestorID(investor.InvestorID);
                if (agent != null)
                {
                    bool checkIp = agent.CheckValidRangeIpAddress(ipAddress);
                    if (checkIp == false)
                    {
                        string content = "'" + Code + "': admin blocked by IP";
                        string comment = "[login admin]";
                        TradingServer.Facade.FacadeAddNewSystemLog(4, content, comment, ipAddress, code);
                        return null;
                    }
                    for (int i = Business.Market.AdminList.Count - 1; i >= 0; i--)
                    {
                        if (Business.Market.AdminList[i].AgentID == agent.AgentID)
                        {
                            if (Business.Market.AdminList[i].IsAdmin && !Business.Market.AdminList[i].IsDisable)
                            {
                                Business.Market.AdminList[i].TimeSync = DateTime.Now;
                                Business.Market.AdminList[i].Ip = ipAddress;
                                return Business.Market.AdminList[i];
                            }
                            else return null;
                        }
                    }
                    agent.Code = Code;
                    agent.TimeSync = DateTime.Now;
                    agent.Ip = ipAddress;
                    agent.IsOnline = true;
                    agent.IAgentGroup = new List<Business.IAgentGroup>();
                    agent.IAgentGroup = Facade.FacadeGetIAgentGroupByAgentID(agent.AgentID);
                    agent.IAgentSecurity = new List<Business.IAgentSecurity>();
                    agent.IAgentSecurity = Facade.FacadeGetIAgentSecurityByAgentID(agent.AgentID);
                    List<Business.Permit> listPermit = new List<Business.Permit>();
                    listPermit = Facade.FacadeGetPermitByAgentID(agent.AgentID);
                    for (int i = listPermit.Count - 1; i >= 0; i--)
                    {
                        #region Enable Role
                        switch (listPermit[i].Role.Code)
                        {
                            case "R01":
                                agent.IsManager = true;
                                break;
                            case "R02":
                                agent.IsSuperviseTrades = true;
                                break;
                            case "R03":
                                agent.IsAdmin = true;
                                break;
                            case "R04":
                                agent.IsAccountant = true;
                                break;
                            case "R05":
                                agent.IsReports = true;
                                break;
                            case "R06":
                                agent.IsRiskManager = true;
                                break;
                            case "R07":
                                agent.IsInternalMail = true;
                                break;
                            case "R08":
                                agent.IsJournals = true;
                                break;
                            case "R09":
                                agent.IsSendNews = true;
                                break;
                            case "R10":
                                agent.IsMarketWatch = true;
                                break;
                            case "R11":
                                agent.IsConections = true;
                                break;
                            case "R12":
                                agent.IsPersonalDetails = true;
                                break;
                            case "R13":
                                agent.IsConfigServerPlugins = true;
                                break;
                            case "R14":
                                agent.IsAutomaticServerReports = true;
                                break;
                            case "R15":
                                agent.IsDealer = true;
                                break;
                            case "R16":
                                agent.IsEditDealer = true;
                                break;
                            case "R17":
                                agent.IsDownloadStatements = true;
                                break;
                        }

                        #endregion
                    }
                    if (agent.IsAdmin && !agent.IsDisable)
                    {
                        agent.AlertQueue = new List<PriceAlert>();
                        agent = this.GetAlertByAgent(agent);
                        Business.Market.AdminList.Add(agent);
                    }
                    else return null;
                }
            }
            return agent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        internal Business.Agent GetAlertByAgent(Business.Agent agent)
        {
            for (int i = 0; i < Business.Market.SymbolList.Count; i++)
            {
                if (Business.Market.SymbolList[i].AlertQueue != null)
                {
                    for (int j = 0; j < Business.Market.SymbolList[i].AlertQueue.Count; j++)
                    {
                        if (agent.InvestorID == Business.Market.SymbolList[i].AlertQueue[j].InvestorID)
                        {
                            agent.AlertQueue.Add(Business.Market.SymbolList[i].AlertQueue[j]);
                        }
                    }
                }
                else
                {
                    Business.Market.SymbolList[i].AlertQueue = new List<PriceAlert>();
                }
                
            }
            return agent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal bool DealerLogin(int AgentID, string ipAddress)
        {
            for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Business.Market.AgentList[i].AgentID == AgentID)
                {
                    Business.Market.AgentList[i].TimeSync = DateTime.Now;
                    if (Business.Market.AgentList[i].IsDealer)
                    {
                        Business.Market.AgentList[i].IsBusy = false;
                        if (!NoticeDealer.ContainsKey(AgentID))
                        {
                            NoticeDealer.Add(AgentID, "");
                        }
                        this.SendNoticeManagerOnline(Business.Market.AgentList[i]);
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal bool ManagerLogout(int AgentID)
        {
            bool result = false;
            for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Business.Market.AgentList[i].AgentID == AgentID)
                {
                    Business.Market.AgentList[i].IsOnline = false;
                    this.SendNoticeManagerOnline(Business.Market.AgentList[i]);
                    Business.Market.AgentList.RemoveAt(i);
                    this.ChangeDealerProcessRequest(AgentID);
                    NoticeManager.Remove(AgentID);
                    NoticeDealer.Remove(AgentID);

                    return true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal bool DealerLogout(int AgentID)
        {
            for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Business.Market.AgentList[i].AgentID == AgentID)
                {
                    Business.Market.AgentList[i].TimeSync = DateTime.Now;
                    Business.Market.AgentList[i].IsBusy = true;                  
                    this.ChangeDealerProcessRequest(AgentID);
                    this.SendNoticeManagerOnline(Business.Market.AgentList[i]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal bool AdminLogout(int AgentID)
        {
            bool result = false;
            for (int i = Business.Market.AdminList.Count - 1; i >= 0; i--)
            {
                if (Business.Market.AdminList[i].AgentID == AgentID)
                {
                    Business.Market.AdminList.RemoveAt(i);
                    return true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.Agent> GetAllAgent()
        {
            List<Agent> result = new List<Agent>();
            result = Agent.DBWAgentInstance.GetAllAgent();
            int count = result.Count;
            for (int i = 0; i < count; i++)
            {
                result[i].IsOnline = this.FindAgentOnline(result[i].AgentID);
            }
            return result;
        }      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal bool FindAgentOnline(int agentID)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].AgentID == agentID)
                {
                    return true;
                }
            }
            return false;
        }

        internal Agent FindAgentOnlineByID(int agentID)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].AgentID == agentID)
                {
                    return Market.AgentList[i];
                }
            }
            return null;
        }
        
        internal bool CheckPermitAddMoney(string code)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    if (Market.AgentList[i].IsAccountant) return true;
                    else return false;
                }
            }
            return false;
        }

        public string GetIPAddressServer()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }

        internal bool CheckIpAdmin(string code, string ip)
        {
            string ipServer = this.GetIPAddressServer();
            if (ip == ipServer || ip == "::1" || ip == "")
            {
                return true;
            }

            for (int i = Market.AdminList.Count - 1; i >= 0; i--)
            {
                if (Market.AdminList[i].Ip == ip && Market.AdminList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        internal bool CheckIpManager(string code, string ip)
        {
            string ipServer = this.GetIPAddressServer();
            if (ip == ipServer || ip == "::1" || ip == "")
            {
                return true;
            }

            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].Ip == ip && Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        internal bool CheckIpManagerAndAdmin(string code, string ip)
        {
            string ipServer = this.GetIPAddressServer();
            if (ip == ipServer || ip == "::1" || ip == "")
            {
                return true;
            }

            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].Ip == ip && Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }

            for (int i = Market.AdminList.Count - 1; i >= 0; i--)
            {
                if (Market.AdminList[i].Ip == ip && Market.AdminList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        internal bool CheckPermitTickManager(string code)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsMarketWatch == true && Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        internal bool CheckPermitCommandManagerAndAdmin(string code)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsEditDealer == true || Market.AgentList[i].IsDealer == true)
                {
                    return true;
                }
            }
            for (int i = Market.AdminList.Count - 1; i >= 0; i--)
            {
                if (Market.AdminList[i].IsEditDealer == true || Market.AdminList[i].IsDealer == true)
                {
                    return true;
                }
            }
            return false;
        }        

        internal bool CheckPermitAccountManagerAndAdmin(string code)
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsManager == true && Market.AgentList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }

            for (int i = Market.AdminList.Count - 1; i >= 0; i--)
            {
                if (Market.AdminList[i].IsManager == true && Market.AdminList[i].Code.ToLower() == code.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        internal List<Agent> CheckCodeAgentSendMail(int investorGroupID)
        {
            List<Agent> agents = new List<Agent>();
            List<IAgentGroup> iAgentGroup = new List<Business.IAgentGroup>();
            iAgentGroup = Facade.FacadeGetIAgentGroupByInvestorGroupID(investorGroupID);
            List<int> agentIDs = new List<int>();
            List<int> investorIDs = new List<int>();
            if (iAgentGroup != null)
            {
                for (int i = 0; i < iAgentGroup.Count; i++)
                {
                    List<Permit> permits = new List<Permit>();
                    permits = Facade.FacadeGetPermitByAgentID(iAgentGroup[i].AgentID);
                    if (permits != null)
                    {
                        for (int j = 0; j < permits.Count; j++)
                        {
                            if (permits[j].Role.RoleID == 4)
                            {
                                agentIDs.Add(permits[j].AgentID);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < agentIDs.Count; i++)
            {
                Agent ag = new Agent();
                ag = Facade.FacadeGetAgentByAgentID(agentIDs[i]);
                if(ag != null)
                {
                    agents.Add(ag);
                    investorIDs.Add(ag.InvestorID);
                }                
            }
            if (agents.Count == 0)
            {
                List<Agent> agentsSecond = new List<Agent>();
                agentsSecond = Facade.FacadeGetAllAgent();
                if (agentsSecond != null)
                {
                    if (agentsSecond.Count != 0)
                    {
                        agents.Add(agentsSecond[0]);
                        investorIDs.Add(agentsSecond[0].InvestorID);
                    }
                }
            }
            Dictionary<int,string> codes = new Dictionary<int,string>();
            codes = Facade.FacadeGetCodeByInvestorListID(investorIDs);
            if (codes != null)
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    if (codes.ContainsKey(agents[i].InvestorID))
                    {
                        agents[i].Code = codes[agents[i].InvestorID];
                    }
                    else
                    {
                        agents[i].Code = "";
                    }
                }
            }            
            return agents;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal Business.Agent GetAgentByAgentID(int AgentID)
        {
            return Agent.DBWAgentInstance.GetAgentByAgentID(AgentID);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal Business.Agent GetAgentByInvestorID(int InvestorID)
        {
            return Agent.DBWAgentInstance.GetAgentByInvestorID(InvestorID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal int CreateNewAgent(Agent agent)
        {
            return Agent.DBWAgentInstance.AddNewAgent(agent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        internal int DeleteAgentByID(int AgentID)
        {
            return Agent.DBWAgentInstance.DeleteAgentByID(AgentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentID"></param>
        /// <param name="AgentGroupID"></param>
        /// <param name="Name"></param>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool UpdateAgent(Agent agent)
        {
            bool result = false;
            result = Agent.DBWAgentInstance.UpdateAgent(agent);
            if (result)
            {
                for (int i = Business.Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.AgentList[i].AgentID == agent.AgentID)
                    {
                        Business.Market.AgentList[i].AgentGroupID = agent.AgentGroupID;
                        Business.Market.AgentList[i].Name = agent.Name;
                        Business.Market.AgentList[i].InvestorID = agent.InvestorID;
                        Business.Market.AgentList[i].Comment = agent.Comment;
                        Business.Market.AgentList[i].IsIpFilter = agent.IsIpFilter;
                        Business.Market.AgentList[i].IpForm = agent.IpForm;
                        Business.Market.AgentList[i].IpTo = agent.IpTo;
                        if (Business.Market.AgentList[i].GroupCondition != agent.GroupCondition)
                        {
                            Business.Market.AgentList[i].GroupCondition = agent.GroupCondition;                            
                            Business.Market.AgentList[i].IAgentGroup = TradingServer.Facade.FacadeGetIAgentGroupByAgentID(agent.AgentID);
                            TradingServer.Facade.FacadeSendNoticeManagerChangeAgent(2, agent.AgentID);
                        }                        
                        if (IsDisable)
                        {
                            Business.Market.AgentList.RemoveAt(i);
                        }
                        break;
                    }
                }
                for (int i = Business.Market.AdminList.Count - 1; i >= 0; i--)
                {
                    if (Business.Market.AdminList[i].AgentID == agent.AgentID)
                    {
                        Business.Market.AdminList[i].AgentGroupID = agent.AgentGroupID;
                        Business.Market.AdminList[i].Name = agent.Name;
                        Business.Market.AdminList[i].InvestorID = agent.InvestorID;
                        Business.Market.AdminList[i].Comment = agent.Comment;
                        Business.Market.AdminList[i].IsIpFilter = agent.IsIpFilter;
                        Business.Market.AdminList[i].IpForm = agent.IpForm;
                        Business.Market.AdminList[i].IpTo = agent.IpTo;
                        if (Business.Market.AdminList[i].GroupCondition != agent.GroupCondition)
                        {
                            Business.Market.AdminList[i].GroupCondition = agent.GroupCondition;
                            Business.Market.AdminList[i].IAgentGroup = TradingServer.Facade.FacadeGetIAgentGroupByAgentID(agent.AgentID);
                        }
                        if (IsDisable)
                        {
                            Business.Market.AdminList.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AgentGroupID"></param>
        /// <returns></returns>
        internal bool DeleteAgentByAgentGroupID(int AgentGroupID)
        {
            return Agent.DBWAgentInstance.DeleteAgentByAgentGroupID(AgentGroupID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        internal bool CheckAgentExist(int agentID)
        {
            return Agent.DBWAgentInstance.CheckAgentExist(agentID);
        }
    }
}
