﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public class FutureCommand : IPresenter.IMarketArea
    {
        public IPresenter.AddCommandDelegate AddCommandNotify { get; set; }
        public int IMarketAreaID { get; set; }
        public string IMarketAreaName { get; set; }
        public Market MarketContainer { get; set; }
        List<TradeType> IPresenter.IMarketArea.Type { get; set; }
        public List<Symbol> ListSymbol { get; set; }
        public List<ParameterItem> MarketAreaConfig { get; set; }
        public static int NumCheck { get; set; }               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void AddCommand(OpenTrade Command)
        {
            string comment = string.Empty;
            string mode = string.Empty;
            string content = string.Empty;            

            #region Set Property IsBuy And CommandType Send To Client
            bool IsBuy = false;
            string CommandType = "SellFuture";
            switch (Command.Type.ID)
            {
                case 11:
                    IsBuy = true;
                    CommandType = "BuyFuture";
                    break;
                case 12:
                    IsBuy = false;
                    CommandType = "SellFuture";
                    break;
                case 17:
                    IsBuy = true;
                    CommandType = "BuyStopFutureCommand";
                    break;
                case 18:
                    IsBuy = false;
                    CommandType = "SellStopFutureCommand";
                    break;
                case 19:
                    IsBuy = true;
                    CommandType = "BuyLimitFutureCommand";
                    break;
                case 20:
                    IsBuy = false;
                    CommandType = "SellLimitFutureCommand";
                    break;
            }            
            #endregion

            int Result = -1;

            Command.IsClose = false;
            if (!Command.IsReOpen)
                Command.OpenTime = DateTime.Now;

            Command.CloseTime = Command.OpenTime;            
            Command.Taxes = 0;

            if (string.IsNullOrEmpty(Command.Comment))
                Command.Comment = "[future command]";

            #region Find Price Close Of Symbol
            switch (Command.Type.ID)
            {
                case 11:
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;
                    comment = "[future buy]";
                    mode = "buy future";
                    break;
                case 12:
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                    comment = "[future sell]";
                    mode = "sell future";
                    break;     
                case 17:    //BUY STOP FUTURE COMMAND
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;
                    comment = "[buy stop future command]";
                    mode = "buy stop future";
                    break;
                case 18:    //SELL STOP FUTURE COMMAND
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                    comment = "[sell stop future command]";
                    mode = "sell stop future";
                    break;
                case 19:    //BUY LIMIT FUTURE COMMAND
                    Command.ClosePrice = Command.Symbol.TickValue.Bid;
                    comment = "[buy limit future command]";
                    mode = "buy limit future";
                    break;
                case 20:    //SELL LIMIT FUTURE COMMAND
                    Command.ClosePrice = Command.Symbol.TickValue.Ask;
                    comment = "[sell limit future command]";
                    mode = "sell limit future";
                    break;
            }
            #endregion

            #region INSERT SYSTEM LOG EVENT MAKE COMMAND
            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);              
            #endregion

            #region CHECK ISCLOSE ONLY OF FUTURE MARKET AREA
            if (Command.Symbol.isCloseOnlyFuture)
            {
                #region INSERT SYSTEM LOG 
                content = "'" + Command.Investor.Code + "': " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice +
                               " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ") unsuccessful [symbol close only]";
                comment = "[symbol close only]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                #region CHECK SYMBOL CLOSE ONLY
                if (Command.IsServer)
                {
                    string Message = "AddCommandByManager$False,AFC00001," + 0 + "," + Command.Investor.InvestorID + "," +
                           Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                               Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                               CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + 0 + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    return;
                }
                else
                {
                    string Message = "AddCommand$False,AFC00001," + 0 + "," + Command.Investor.InvestorID + "," +
                           Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                               Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                               CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + 0 + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);
                    return;
                }
                #endregion                
            }
            #endregion

            #region GET TIME EXP AND COMPARE WITH TIME OPEN COMMAND
            //Set Exp Time
            if (Command.Symbol.ParameterItems != null)
            {
                int countParameter = Command.Symbol.ParameterItems.Count;
                for (int i = 0; i < countParameter; i++)
                {
                    if (Command.Symbol.ParameterItems[i].Code == "S045")
                    {
                        if (Command.Symbol.ParameterItems[i].DateValue < DateTime.Now)
                        {
                            #region INSERT SYSTEM LOG IF MAKE FUTURE COMMAND UNCOMPLETE
                            content = "'" + Command.Investor.Code + "': " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice +
                               " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ") unsuccessful [symbol exp time]";
                            comment = "[symbol exp time]";

                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
                            #endregion

                            #region COMPARE TIME EXP WITH TIME CURRENT
                            if (Command.IsServer)
                            {
                                string Message = "AddCommandByManager$False,AFC00002," + 0 + "," + Command.Investor.InvestorID + "," +
                                       Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                           Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                           CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + 0 + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                if (Command.Investor.ClientCommandQueue == null)
                                    Command.Investor.ClientCommandQueue = new List<string>();

                                Command.Investor.ClientCommandQueue.Add(Message);

                                return;
                            }
                            else
                            {
                                string Message = "AddCommand$False,AFC00002," + 0 + "," + Command.Investor.InvestorID + "," +
                                       Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                           Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                           CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + 0 + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                if (Command.Investor.ClientCommandQueue == null)
                                    Command.Investor.ClientCommandQueue = new List<string>();

                                Command.Investor.ClientCommandQueue.Add(Message);
                                return;
                            }
                            #endregion                            
                        }
                        else
                        {
                            Command.ExpTime = Command.Symbol.ParameterItems[i].DateValue;
                        }

                        break;
                    }
                }
            }
            #endregion            

            if (Command.Type.ID == 11 || Command.Type.ID == 12)
            {
                //Calculator Profit Of Command        
                Command.CalculatorProfitCommand(Command);
                Command.Profit = Command.Symbol.ConvertCurrencyToUSD(Command.Symbol.Currency, Command.Profit, false, Command.SpreaDifferenceInOpenTrade, Command.Symbol.Digit);

                #region CALCULATION MARGIN FOR COMMAND
                //Call Function Calculator Margin Of Command            
                Command.CalculatorMarginCommand(Command);
                #endregion

                Command.Commission = Model.CalculationFormular.Instance.CalculationCommission(Command); 
            }
 
            string CommandCode = string.Empty;

            #region REND CLIENT CODE IF CLIENT CODE == EMPTY
            if (string.IsNullOrEmpty(Command.ClientCode))
            {
                string tempCode = string.Empty;
                Random ran = new Random();
                int tempRan = ran.Next(0000000, 9999999);
                Command.ClientCode = Command.Investor.InvestorID + "_" + tempRan;
                bool isOK = false;

                while (!isOK)
                {
                    if (Command.Investor.CommandList != null && Command.Investor.CommandList.Count > 0)
                    {
                        int count = Command.Investor.CommandList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Command.Investor.CommandList[i].ClientCode == Command.ClientCode)
                            {
                                Command.ClientCode = Command.Investor.InvestorID + "_" + ran.Next(0000000, 9999999);
                                break;
                            }
                        }

                        isOK = true;
                    }
                    else
                    {
                        isOK = true;
                    }
                }
            }
            #endregion            

            #region Add Command To Database And Build Command Code
            //Add Command To Database
            //                
            Result = TradingServer.Facade.FacadeAddNewOpenTrade(Command);

            Command.ID = Result;

            //Call Function Update Command Code Of Command
            CommandCode = TradingServer.Model.TradingCalculate.Instance.BuildCommandCode(Result.ToString());
            TradingServer.Facade.FacadeUpdateCommandCode(Result, CommandCode);
            #endregion

            if (Result > 0)
            {
                double spreaDifferenceInCommand = Model.CommandFramework.CommandFrameworkInstance.GetSpreadDifference(Command.Symbol.SecurityID, Command.Investor.InvestorGroupInstance.InvestorGroupID);

                #region Build Command And Add Command To CommandList In Symbol And Command List In Investor
                //Build Two Instance OpenTrade
                //One Instance For Investor 
                //One Instance For Symbol And MarketArea
                #region Build Instance OpenTrade For Investor
                Business.OpenTrade newOpenTradeInvestor = new OpenTrade();
                newOpenTradeInvestor.ID = Result;
                newOpenTradeInvestor.ClientCode = Command.ClientCode;
                newOpenTradeInvestor.ClosePrice = Command.ClosePrice;
                newOpenTradeInvestor.CloseTime = Command.CloseTime;
                newOpenTradeInvestor.CommandCode = CommandCode;
                newOpenTradeInvestor.Commission = Command.Commission;
                newOpenTradeInvestor.ExpTime = Command.ExpTime;                
                newOpenTradeInvestor.Investor = Command.Investor;
                newOpenTradeInvestor.IsClose = Command.IsClose;
                newOpenTradeInvestor.OpenPrice = Command.OpenPrice;
                newOpenTradeInvestor.OpenTime = Command.OpenTime;
                newOpenTradeInvestor.Profit = Command.Profit;
                newOpenTradeInvestor.Size = Command.Size;
                newOpenTradeInvestor.StopLoss = Command.StopLoss;
                newOpenTradeInvestor.Swap = Command.Swap;
                newOpenTradeInvestor.Symbol = Command.Symbol;
                newOpenTradeInvestor.TakeProfit = Command.TakeProfit;
                newOpenTradeInvestor.Type = new TradeType();
                newOpenTradeInvestor.Type.ID = Command.Type.ID;
                newOpenTradeInvestor.Type.Name = Command.Type.Name;                
                newOpenTradeInvestor.Margin = Command.Margin;
                newOpenTradeInvestor.IGroupSecurity = Command.IGroupSecurity;
                newOpenTradeInvestor.Commission = Command.Commission;
                newOpenTradeInvestor.IsHedged = Command.IsHedged;
                newOpenTradeInvestor.SpreaDifferenceInOpenTrade = spreaDifferenceInCommand;
                newOpenTradeInvestor.AgentCommission = Command.AgentCommission;
                #endregion

                #region Find Investor In Investor List And Add Command To Investor List
                //Find Investor In Investor List And Add Command To Investor List
                if (Business.Market.InvestorList != null)
                {
                    int countInvestor = Business.Market.InvestorList.Count;
                    for (int n = 0; n < countInvestor; n++)
                    {
                        if (Business.Market.InvestorList[n].InvestorID == newOpenTradeInvestor.Investor.InvestorID)
                        {
                            if (Business.Market.InvestorList[n].CommandList != null)
                            {
                                Business.Market.InvestorList[n].CommandList.Add(newOpenTradeInvestor);
                            }
                            else
                            {
                                Business.Market.InvestorList[n].CommandList = new List<OpenTrade>();
                                Business.Market.InvestorList[n].CommandList.Add(newOpenTradeInvestor);
                            }

                            break;
                        }
                    }
                }
                #endregion

                #region Build Instance Open Trade For Symbol
                Business.OpenTrade newOpenTradeSymbol = new OpenTrade();
                newOpenTradeSymbol.ID = Result;
                newOpenTradeSymbol.ClientCode = Command.ClientCode;
                newOpenTradeSymbol.ClosePrice = Command.ClosePrice;
                newOpenTradeSymbol.CloseTime = Command.CloseTime;
                newOpenTradeSymbol.CommandCode = CommandCode;
                newOpenTradeSymbol.Commission = Command.Commission;
                newOpenTradeSymbol.ExpTime = Command.ExpTime;                
                newOpenTradeSymbol.Investor = Command.Investor;
                newOpenTradeSymbol.IsClose = false;
                newOpenTradeSymbol.OpenPrice = Command.OpenPrice;
                newOpenTradeSymbol.OpenTime = Command.OpenTime;
                newOpenTradeSymbol.Profit = Command.Profit;
                newOpenTradeSymbol.Size = Command.Size;
                newOpenTradeSymbol.StopLoss = Command.StopLoss;
                newOpenTradeSymbol.Swap = Command.Swap;
                newOpenTradeSymbol.Symbol = Command.Symbol;
                newOpenTradeSymbol.TakeProfit = Command.TakeProfit;
                newOpenTradeSymbol.Type = new TradeType();
                newOpenTradeSymbol.Type.ID = Command.Type.ID;
                newOpenTradeSymbol.Type.Name = Command.Type.Name;                
                newOpenTradeSymbol.Margin = Command.Margin;
                newOpenTradeSymbol.IGroupSecurity = Command.IGroupSecurity;
                newOpenTradeSymbol.Commission = Command.Commission;
                newOpenTradeSymbol.IsHedged = Command.IsHedged;
                newOpenTradeSymbol.SpreaDifferenceInOpenTrade = spreaDifferenceInCommand;
                newOpenTradeSymbol.AgentCommission = Command.AgentCommission;
                #endregion

                #region Find Symbol In Market And Add Command To Market Area And List Symbol
                //Find Symbol In Market And Add Command To Market Area And List Symbol
                if (Business.Market.SymbolList != null)
                {
                    int countSymbol = Business.Market.SymbolList.Count;
                    for (int i = 0; i < countSymbol; i++)
                    {
                        if (Business.Market.SymbolList[i].SymbolID == newOpenTradeSymbol.Symbol.SymbolID)
                        {
                            if (Business.Market.SymbolList[i].CommandList != null)
                            {
                                Business.Market.SymbolList[i].CommandList.Add(newOpenTradeSymbol);
                            }
                            else
                            {
                                Business.Market.SymbolList[i].CommandList = new List<OpenTrade>();
                                Business.Market.SymbolList[i].CommandList.Add(newOpenTradeSymbol);
                            }
                            break;
                        }
                    }
                }
                #endregion

                #region Build Instance Open Trade For Command Executor
                Business.OpenTrade newOpenTradeExe = new OpenTrade();
                newOpenTradeExe.ID = Result;
                newOpenTradeExe.ClientCode = Command.ClientCode;
                newOpenTradeExe.ClosePrice = Command.ClosePrice;
                newOpenTradeExe.CloseTime = Command.CloseTime;
                newOpenTradeExe.CommandCode = CommandCode;
                newOpenTradeExe.Commission = Command.Commission;
                newOpenTradeExe.ExpTime = Command.ExpTime;                
                newOpenTradeExe.Investor = Command.Investor;
                newOpenTradeExe.IsClose = false;
                newOpenTradeExe.OpenPrice = Command.OpenPrice;
                newOpenTradeExe.OpenTime = Command.OpenTime;
                newOpenTradeExe.Profit = Command.Profit;
                newOpenTradeExe.Size = Command.Size;
                newOpenTradeExe.StopLoss = Command.StopLoss;
                newOpenTradeExe.Swap = Command.Swap;
                newOpenTradeExe.Symbol = Command.Symbol;
                newOpenTradeExe.TakeProfit = Command.TakeProfit;
                newOpenTradeExe.Type = new TradeType();
                newOpenTradeExe.Type.ID = Command.Type.ID;
                newOpenTradeExe.Type.Name = Command.Type.Name;                
                newOpenTradeExe.Margin = Command.Margin;
                newOpenTradeExe.IGroupSecurity = Command.IGroupSecurity;
                newOpenTradeExe.Commission = Command.Commission;
                newOpenTradeExe.IsHedged = Command.IsHedged;
                newOpenTradeExe.SpreaDifferenceInOpenTrade = spreaDifferenceInCommand;
                newOpenTradeExe.AgentCommission = Command.AgentCommission;
                #endregion

                Business.Market.CommandExecutor.Add(newOpenTradeExe);

                //If Client Add New Command Complete Then Add Message To Client Queue
                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                #region Map Command Server To Client
                if (Command.IsServer)
                {
                    string Message = "AddCommandByManager$True,Add New Command Complete," + Result + "," + Command.Investor.InvestorID + "," +
                       Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                           Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                           CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);
                }
                else
                {
                    string Message = "AddCommand$True,Add New Command Complete," + Result + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);
                }
                #endregion

                Command.IsHedged = Command.Symbol.IsHedged;

                //Command.Investor.UpdateCommand(Command);
                Business.Margin newMargin = new Margin();
                newMargin = Command.Symbol.CalculationTotalMargin(Command.Investor.CommandList);
                Command.Investor.Margin = newMargin.TotalMargin;
                Command.Investor.FreezeMargin = newMargin.TotalFreezeMargin;

                //Business.RequestDealer Request = new RequestDealer();
                //NOTIFY INVESTOR TO MANAGER
                TradingServer.Facade.FacadeSendNotifyManagerRequest(3, Command.Investor);

                //SEND NOTIFY TO MANAGER THEN ADD NEW ACCOUNT
                TradingServer.Facade.FacadeSendNoticeManagerRequest(1, Command);
                #endregion

                #region INSERT SYSTEM LOG IF MAKE FUTURE COMMAND COMPLETE
                content = "'" + Command.Investor.Code + "': future order #" + CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice + " commission: " + Command.Commission;
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion                
            }
            else
            {
                #region Return Error Can't Insert Command To Database For Client
                //Add Result To Client Command Queue Of Investor
                string Message = "AddCommand$False,Can't Insert Database," + Result + "," + Command.Investor.InvestorID + "," +
                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);
                #endregion
            }   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void CloseCommand(OpenTrade Command)
        {
            if (Command.IsMultiClose)
            {
                this.MultiCloseCommand(Command);
                return;
            }

            //Check Condition Close Command
            Business.OpenTrade newOpenTrade = new OpenTrade();

            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = DateTime.Now;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = true;
            newOpenTrade.IsHedged = Command.IsHedged;
            newOpenTrade.Margin = Command.Margin;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Profit = Command.Profit;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.IsServer = Command.IsServer;

            if (string.IsNullOrEmpty(Command.Comment))
                newOpenTrade.Comment = Command.Comment;

            newOpenTrade.AgentCommission = Command.AgentCommission;

            //Call Function Calculator Profit Command Close
            newOpenTrade.CalculatorProfitCommand(newOpenTrade);
            newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

            //Call Function Update Command Of Inveestor
            newOpenTrade.Investor.UpdateCommand(newOpenTrade);

            //Send Notify to Manager
            TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiCloseCommand(OpenTrade Command)
        {
            //Check Condition Close Command
            Business.OpenTrade newOpenTrade = new OpenTrade();

            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.CloseTime = DateTime.Now;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = true;
            newOpenTrade.IsMultiClose = Command.IsMultiClose;
            newOpenTrade.IsHedged = Command.IsHedged;
            newOpenTrade.Margin = Command.Margin;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Profit = Command.Profit;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Taxes = Command.Taxes;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.IsServer = Command.IsServer;
            newOpenTrade.AgentCommission = Command.AgentCommission;

            if (string.IsNullOrEmpty(Command.Comment))
            {
                newOpenTrade.Comment = "[multi close future command]";
            }
            else
            {
                newOpenTrade.Comment = Command.Comment;
            }

            //Call Function Update Command Of Inveestor
            newOpenTrade.Investor.UpdateCommand(newOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void MultiUpdateCommand(OpenTrade Command)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        public void UpdateCommand(OpenTrade Command)
        {
            string content = string.Empty;
            string comment = "[modify order]";
            string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(Command.Type.ID).ToLower();
            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), Command.Symbol.Digit);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

            #region Check Valid Take Profit And Stop Loss
            bool ResultTakeProfit = false;

            if (Command.TakeProfit != 0 || Command.StopLoss != 0)
            {
                if (Command.Type.ID == 11 || Command.Type.ID == 12)
                {
                    ResultTakeProfit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                                                                        Command.Symbol.StopLossTakeProfitLevel, Command.Symbol.Digit, int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));
                }
                else if (Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
                {
                    ResultTakeProfit = Command.Symbol.CheckLimitAndStopPendingOrder(Command.Symbol.Name, Command.Type.ID, Command.OpenPrice, Command.StopLoss,
                                            Command.TakeProfit, Command.Symbol.FreezeLevel, Command.Symbol.Digit);
                }

                if (ResultTakeProfit == false)
                {
                    #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                    string tempContent = string.Empty;
                    tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                        openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid s/l or t/p] (" + bid + "/" + ask + ")";

                    //content = content + " unsuccesful [invalid s/l or t/p]";
                    comment = "[invalid s/l or t/p]";

                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    #endregion

                    bool IsBuy = false;
                    if (Command.Type.ID == 11 || Command.Type.ID == 17 || Command.Type.ID == 19)
                        IsBuy = true;

                    String Message = "UpdateCommand$False,INVALID S/L OR T/P," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Update";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    return;
                }

                #region Call Check FreezeLevel
                //Call Check FreezeLevel
                if (Command.Type.ID == 11 || Command.Type.ID == 12)
                {
                    ResultTakeProfit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                                                    Command.Symbol.FreezeLevel, Command.Symbol.Digit, int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));
                }
                else if (Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
                {
                    ResultTakeProfit = Command.Symbol.CheckLimitAndStopPendingOrder(Command.Symbol.Name, Command.Type.ID, Command.OpenPrice, Command.StopLoss,
                                            Command.TakeProfit, Command.Symbol.FreezeLevel, Command.Symbol.Digit);
                }                

                if (ResultTakeProfit == false)
                {
                    #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                    string tempContent = string.Empty;
                    tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                        openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid freeze level] (" + bid + "/" + ask + ")";

                    //content = content + " unsuccesful [invalid freeze level]";
                    comment = "[invalid freeze level]";

                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    #endregion

                    bool IsBuy = false;
                    if (Command.Type.ID == 11 || Command.Type.ID == 17 || Command.Type.ID == 19)
                        IsBuy = true;

                    string Message = "UpdateCommand$False,INVALID FREEZE LEVEL," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Update";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                        Command.Investor.ClientCommandQueue.Add(Message);

                    return;
                }
                #endregion
            }
            #endregion

            Business.OpenTrade newOpenTrade = new OpenTrade();
            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.CloseTime = Command.CloseTime;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.Type = Command.Type;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;
            newOpenTrade.AgentCommission = Command.AgentCommission;

            if (string.IsNullOrEmpty(Command.Comment))
                newOpenTrade.Comment = Command.Comment;

            double TakeProfit = 0;
            double StopLoss = 0;
            TakeProfit = Command.TakeProfit;
            StopLoss = Command.StopLoss;

            #region Find In List Symbol And Update Take Profit, Stop Loss
            if (Business.Market.SymbolList != null)
            {
                bool Flag = false;
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Flag)
                        break;

                    if (Business.Market.SymbolList[i].CommandList != null)
                    {
                        int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                        for (int j = 0; j < countCommand; j++)
                        {
                            if (Business.Market.SymbolList[i].CommandList[j].ID == Command.ID)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].StopLoss != StopLoss ||
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit != TakeProfit)
                                {
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss = StopLoss;
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit = TakeProfit;
                                }

                                if (Business.Market.SymbolList[i].CommandList[j].Type.ID == 17 ||
                                    Business.Market.SymbolList[i].CommandList[j].Type.ID == 18 ||
                                    Business.Market.SymbolList[i].CommandList[j].Type.ID == 19 ||
                                    Business.Market.SymbolList[i].CommandList[j].Type.ID == 20)
                                {
                                    if (Business.Market.SymbolList[i].CommandList[j].OpenPrice != Command.OpenPrice)
                                    {
                                        Business.Market.SymbolList[i].CommandList[j].OpenPrice = Command.OpenPrice;
                                    }
                                }

                                #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                                content = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                    openPrice + " sl: " + stopLoss + " tp: " + takeProfit;

                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                #endregion

                                #region MAP COMMAND SEND TO CLIENT
                                string Message = "UpdateCommand$True,UPDATE COMMAND COMPLETE," + Command.ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Name + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Size + "," + false + "," +
                                    Business.Market.SymbolList[i].CommandList[j].OpenTime + "," +
                                    Business.Market.SymbolList[i].CommandList[j].OpenPrice + "," +
                                    Business.Market.SymbolList[i].CommandList[j].StopLoss + "," +
                                    Business.Market.SymbolList[i].CommandList[j].TakeProfit + "," +
                                    Business.Market.SymbolList[i].CommandList[j].ClosePrice + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Commission + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Swap + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Profit + "," + "Comment," +
                                    Business.Market.SymbolList[i].CommandList[j].ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Type.Name + "," +
                                    1 + "," + Business.Market.SymbolList[i].CommandList[j].ExpTime + "," +
                                    Business.Market.SymbolList[i].CommandList[j].ClientCode + "," +
                                    Business.Market.SymbolList[i].CommandList[j].CommandCode + "," +
                                    Business.Market.SymbolList[i].CommandList[j].IsHedged + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                    Business.Market.SymbolList[i].CommandList[j].Margin + ",Update";

                                //If Client Update Command Then Add Message To Client Message
                                if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);

                                #endregion

                                Flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region FIND IN LIST COMMAND EXECUTOR AND UPDATE TAKE PROFIT , STOP LOSS
            if (Business.Market.CommandExecutor != null)
            {
                int count = Business.Market.CommandExecutor.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.CommandExecutor[i].ID == Command.ID)
                    {
                        if (Business.Market.CommandExecutor[i].TakeProfit != Command.TakeProfit ||
                            Business.Market.CommandExecutor[i].StopLoss != Command.StopLoss)
                        {
                            Business.Market.CommandExecutor[i].TakeProfit = TakeProfit;
                            Business.Market.CommandExecutor[i].StopLoss = StopLoss;                            
                        }

                        if (Business.Market.CommandExecutor[i].Type.ID == 17 ||
                            Business.Market.CommandExecutor[i].Type.ID == 18 ||
                            Business.Market.CommandExecutor[i].Type.ID == 19 ||
                            Business.Market.CommandExecutor[i].Type.ID == 20)
                        {
                            if (Business.Market.CommandExecutor[i].OpenPrice != Command.OpenPrice)
                            {
                                Business.Market.CommandExecutor[i].OpenPrice = Command.OpenPrice;
                            }
                        }

                        break;
                    }
                }
            }
            #endregion

            Command.Investor.UpdateCommand(newOpenTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="RefSymbol"></param>
        public void SetTickValueNotify(Tick Tick, Symbol RefSymbol)
        {
            NumCheck++;
            if (NumCheck == 100)
                NumCheck = 0;

            #region Process Command List
            //Call Function Calculate Command 
            if (RefSymbol.CommandList != null && RefSymbol.CommandList.Count > 0)
            {
                int count = RefSymbol.CommandList.Count;
                //for (int i = 0; i < RefSymbol.CommandList.Count; i++)
                for (int i = count - 1; i >= 0; i--)
                {
                    if (RefSymbol.CommandList[i].IsClose == true)
                        continue;

                    #region Switch Condition Type
                    //Set Close Price For Command     
                    switch (RefSymbol.CommandList[i].Type.ID)
                    {
                        case 11: //Spot Buy Command                    
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;
                        case 12: //Spot Sell Command                     
                            double Ask = 0;
                            Ask = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = Ask;
                            break;
                        case 17:    //Buy Stop Future
                            double ask = 0;
                            ask = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = ask;
                            break;
                        case 18:    //Sell Stop Future
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;
                        case 19:    //Buy Limit Future
                            double askBuyLimit = 0;
                            askBuyLimit = (Symbol.ConvertNumberPip(RefSymbol.CommandList[i].Symbol.Digit, RefSymbol.CommandList[i].SpreaDifferenceInOpenTrade) + Tick.Ask);
                            RefSymbol.CommandList[i].ClosePrice = askBuyLimit;
                            break;
                        case 20:    //Sell Limit Future
                            RefSymbol.CommandList[i].ClosePrice = Tick.Bid;
                            break;
                    }
                    #endregion

                    //Call Function Calculator Command
                    Business.OpenTrade newOpenTrade = new OpenTrade();
                    newOpenTrade = this.CalculateCommand(RefSymbol.CommandList[i]);

                    #region COMMAND BECAUSE PENDING ORDER DON'T ACTIVE(15/07/2011)
                    //if (RefSymbol.CommandList[i].Type.ID != newOpenTrade.Type.ID)
                    //{
                    //    if (newOpenTrade.Type != null)
                    //    {
                    //        RefSymbol.CommandList[i].Type = new TradeType();
                    //        RefSymbol.CommandList[i].Type.ID = newOpenTrade.Type.ID;
                    //        RefSymbol.CommandList[i].Type.Name = newOpenTrade.Type.Name;
                    //        //RefSymbol.CommandList[i].Type = newOpenTrade.Type;
                    //    }
                    //}
                    #endregion

                    if (newOpenTrade.IsClose == true)
                        RefSymbol.CommandList[i].IsClose = true;

                    if (RefSymbol.CommandList[i].Investor.CommandList.Count > 0 && RefSymbol.CommandList[i].Investor.CommandList != null)
                    {
                        RefSymbol.CommandList[i].Investor.UpdateCommand(newOpenTrade);
                    }
                }
            }
            #endregion    

            Facade.FacadeCalculationAlert(Tick, RefSymbol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public OpenTrade CalculateCommand(OpenTrade Command)
        {
            Business.OpenTrade newOpenTrade = new OpenTrade();
            newOpenTrade.ClientCode = Command.ClientCode;
            newOpenTrade.CloseTime = Command.CloseTime;
            newOpenTrade.ExpTime = Command.ExpTime;
            newOpenTrade.ID = Command.ID;
            newOpenTrade.Investor = Command.Investor;
            newOpenTrade.IsClose = Command.IsClose;
            newOpenTrade.OpenPrice = Command.OpenPrice;
            newOpenTrade.OpenTime = Command.OpenTime;
            newOpenTrade.Size = Command.Size;
            newOpenTrade.StopLoss = Command.StopLoss;
            newOpenTrade.Swap = Command.Swap;
            newOpenTrade.Symbol = Command.Symbol;
            newOpenTrade.TakeProfit = Command.TakeProfit;
            newOpenTrade.Type = new TradeType();
            newOpenTrade.Type.ID = Command.Type.ID;
            newOpenTrade.Type.Name = Command.Type.Name;
            //newOpenTrade.Type = Command.Type;
            newOpenTrade.ClosePrice = Command.ClosePrice;
            newOpenTrade.Margin = Command.Margin;
            newOpenTrade.CommandCode = Command.CommandCode;
            newOpenTrade.Commission = Command.Commission;
            newOpenTrade.IGroupSecurity = Command.IGroupSecurity;            

            switch (newOpenTrade.Type.ID)
            {
                #region Case Buy Future Command
                case 11:
                    {
                        if (newOpenTrade.TakeProfit != 0)
                        {
                            if (Command.ClosePrice >= newOpenTrade.TakeProfit)
                            {                                
                                //call function close command         
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.TakeProfit;

                                //Calculator Profit Of Command                        
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);                                
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT TAKE PROFIT)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'" + Command.Investor.Code + "': take profit #" + Command.CommandCode + " at " + takeProfit + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[take profit]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        if (newOpenTrade.StopLoss != 0)
                        {
                            if (Command.ClosePrice <= newOpenTrade.StopLoss)
                            {
                                //call function close command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.StopLoss;

                                //Calculator Profit Of Command                        
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);                                
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT STOP LOSS)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string stoploss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'" + Command.Investor.Code + "': stop loss #" + Command.CommandCode + " at " + stoploss + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop loss]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        //Calculator Profit Of Command                        
                        //newOpenTrade.CalculatorProfitCommand(newOpenTrade);                        
                        //newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                    }
                    break;
                #endregion

                #region Case Sell Future Command
                case 12:
                    {
                        if (newOpenTrade.TakeProfit != 0)
                        {
                            if (Command.ClosePrice <= newOpenTrade.TakeProfit)
                            {
                                //call Function Close Command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.TakeProfit;

                                //Calculator Profit Of Command
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);                                
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT STOP LOSS)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string stoploss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'" + Command.Investor.Code + "': take profit #" + Command.CommandCode + " at " + stoploss + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[take profit]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        if (newOpenTrade.StopLoss != 0)
                        {
                            if (Command.ClosePrice >= newOpenTrade.StopLoss)
                            {
                                //call function close command
                                newOpenTrade.IsClose = true;
                                newOpenTrade.CloseTime = DateTime.Now;
                                newOpenTrade.ClosePrice = Command.StopLoss;

                                //Calculator Profit Of Command
                                newOpenTrade.CalculatorProfitCommand(newOpenTrade);                                
                                newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);

                                //send notify to manage if command SL/TP
                                TradingServer.Facade.FacadeSendNoticeManagerRequest(2, newOpenTrade);

                                #region INSERT SYSTEM LOG (EVENT STOP LOSS)
                                //INSERT SYSTEM LOG(EVENT TAKE PROFIT)
                                string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                string stoploss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
                                string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
                                string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

                                string content = "'" + Command.Investor.Code + "': stop loss #" + Command.CommandCode + " at " + stoploss + " (" + bid + "/" + ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[stop loss]", "", Command.Investor.Code);
                                #endregion

                                break;
                            }
                        }

                        //Calculator Profit Of Command
                        //newOpenTrade.CalculatorProfitCommand(newOpenTrade);                        
                        //newOpenTrade.Profit = newOpenTrade.Symbol.ConvertCurrencyToUSD(newOpenTrade.Symbol.Currency, newOpenTrade.Profit, false, newOpenTrade.SpreaDifferenceInOpenTrade, newOpenTrade.Symbol.Digit);
                    }
                    break;
                #endregion

                #region BUY STOP FUTURE COMMAND
                case 17:    //BUY STOP FUTURE COMMAND
                    {
                        if (newOpenTrade.ClosePrice >= newOpenTrade.OpenPrice)
                        {
                            //Make Buy Command
                            newOpenTrade.OpenTime = DateTime.Now;

                            int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 11)
                                {
                                    newOpenTrade.Type = new TradeType();
                                    newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                                    newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                                    //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                                    //SYSTEM LOG ACTIVATE PENDING ORDER 
                                    string content = string.Empty;
                                    content = "pending buy stop order #" + newOpenTrade.CommandCode + " triggered";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion                

                #region SELL STOP FUTURE COMMAND
                case 18:    //SELL STOP FUTURE COMMAND
                    {
                        if (newOpenTrade.ClosePrice <= newOpenTrade.OpenPrice)
                        {
                            //Make sell command
                            newOpenTrade.OpenTime = DateTime.Now;

                            int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 12)
                                {
                                    newOpenTrade.Type = new TradeType();
                                    newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                                    newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;

                                    //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                                    //SYSTEM LOG ACTIVATE PENDING ORDER 
                                    string content = string.Empty;
                                    content = "pending sell stop order #" + newOpenTrade.CommandCode + " triggered";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region BUY LIMIT FUTURE COMMAND
                case 19:    //BUY LIMIT FUTURE COMMAND
                    {
                        if (Command.ClosePrice <= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.OpenTime = DateTime.Now;

                            int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 11)
                                {
                                    newOpenTrade.Type = new TradeType();
                                    newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                                    newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                                    //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                                    //SYSTEM LOG ACTIVATE PENDING ORDER 
                                    string content = string.Empty;
                                    content = "pending buy limit order #" + newOpenTrade.CommandCode + " triggered";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region SELL LIMIT FUTURE COMMAND
                case 20:    //SELL LIMIT FUTURE COMMAND
                    {
                        if (newOpenTrade.ClosePrice >= newOpenTrade.OpenPrice)
                        {
                            newOpenTrade.OpenTime = DateTime.Now;

                            int count = newOpenTrade.Symbol.MarketAreaRef.Type.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (newOpenTrade.Symbol.MarketAreaRef.Type[i].ID == 12)
                                {
                                    newOpenTrade.Type = new TradeType();
                                    newOpenTrade.Type.ID = newOpenTrade.Symbol.MarketAreaRef.Type[i].ID;
                                    newOpenTrade.Type.Name = newOpenTrade.Symbol.MarketAreaRef.Type[i].Name;
                                    //newOpenTrade.Type = newOpenTrade.Symbol.MarketAreaRef.Type[i];

                                    //SYSTEM LOG ACTIVATE PENDING ORDER 
                                    string content = string.Empty;
                                    content = "pending sell limit order #" + newOpenTrade.CommandCode + " triggered";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[pending order triggered]", "", newOpenTrade.Investor.Code);

                                    break;
                                }
                            }
                        }
                    }
                    break;
                #endregion                

                default:
                    {
                        if (newOpenTrade.Type == null)
                            TradingServer.Facade.FacadeAddNewSystemLog(1, "[type command is empty]", "[check type command]", "", newOpenTrade.CommandCode);
                        else
                            TradingServer.Facade.FacadeAddNewSystemLog(1, "[type command incorect] " + newOpenTrade.Type.ID, "[check type command]", "", newOpenTrade.CommandCode);
                    }
                    break;
            }

            return newOpenTrade;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public IPresenter.CloseCommandDelegate CloseCommandNotify(OpenTrade Command)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>
        public IPresenter.SendClientCmdDelegate SendClientCmdDelegate(string Cmd)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initStatus"></param>
        /// <returns></returns>
        public IPresenter.InitServerDelegate CheckStatusInitServer(InitStatus initStatus)
        {
            return null;
        }
    }
}