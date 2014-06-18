﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TradingServer
{
    public static partial class ClientFacade
    {
        #region Create Instance Class OpenTrade
        private static Business.OpenTrade openTrade;
        private static Business.OpenTrade OpenTradeInstance
        {
            get
            {
                if (ClientFacade.openTrade == null)
                {
                    ClientFacade.openTrade = new Business.OpenTrade();
                }
                return ClientFacade.openTrade;
            }
        }
        #endregion       

        //======================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static ClientBusiness.DealMessage FacadeMakeCommand(TradingServer.ClientBusiness.Command objCommand)
        {   
            ClientBusiness.DealMessage Result = new ClientBusiness.DealMessage();
            Result.isDeal = false;

            Business.OpenTrade Command = new Business.OpenTrade();

            FillInstanceOpenTrade(objCommand, Command);

            #region SET VALUE FOR COMMAND
            bool isRequest = false;
            if (Business.Market.marketInstance.MQLCommands != null)
            {
                int count = Business.Market.marketInstance.MQLCommands.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.marketInstance.MQLCommands[i].ClientCode == objCommand.ClientCode)
                    {
                        isRequest = true;
                        break;
                    }   
                }

                if(!isRequest)
                {
                    NJ4XConnectSocket.MQLCommand newMQLCommand = new NJ4XConnectSocket.MQLCommand();
                    newMQLCommand.ClientCode = objCommand.ClientCode;
                    newMQLCommand.InvestorCode = Command.Investor.Code;
                    newMQLCommand.IpAddress = objCommand.IpAddress;

                    Business.Market.marketInstance.MQLCommands.Add(newMQLCommand);
                }
            }
            Command.ClientCode = objCommand.ClientCode;
            #endregion

            #region INSERT SYSTEM LOG EVENT MAKE COMMAND
            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.OpenPrice.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.TakeProfit.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.StopLoss.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

            string mode = string.Empty;
            string content = string.Empty;
            string comment = string.Empty;
            switch (Command.Type.ID)
            {
                case 1:
                    {
                        content = "'" + Command.Investor.Code + "': instant buy " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                        comment = "[instant buy]";
                    }
                    break;
                case 2:
                    {
                        content = "'" + Command.Investor.Code + "': instant sell " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                        comment = "[instant sell]";
                    }
                    break;
                case 7:
                    {
                        content = "'" + Command.Investor.Code + "': order buy limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order buy limit]";
                    }
                    break;
                case 8:
                    {
                        content = "'" + Command.Investor.Code + "': order sell limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order sell limit]";
                    }
                    break;
                case 9:
                    {
                        content = "'" + Command.Investor.Code + "': order buy stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order buy stop]";
                    }
                    break;
                case 10:
                    {
                        content = "'" + Command.Investor.Code + "': order sell stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order sell stop]";
                    }
                    break;
                case 11:
                    {
                        content = "'" + Command.Investor.Code + "': future buy " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[future buy]";
                    }
                    break;
                case 12:
                    {
                        content = "'" + Command.Investor.Code + "': future sell " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[future sell]";
                    }
                    break;
                case 17:
                    {
                        content = "'" + Command.Investor.Code + "': order buy stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order buy stop]";
                    }
                    break;
                case 18:
                    {
                        content = "'" + Command.Investor.Code + "': order sell stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order sell stop]";
                    }
                    break;
                case 19:
                    {
                        content = "'" + Command.Investor.Code + "': order buy limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[order buy limit]";
                    }
                    break;
                case 20:
                    {
                        content = "'" + Command.Investor.Code + "': order sell limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " exp " + Command.ExpTime + " (" + bid + "/" + ask + ")";
                        comment = "[sell limit]";
                    }
                    break;
            }

            if (!isRequest)
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, objCommand.IpAddress, Command.Investor.Code);
            #endregion

            TimeSpan _timeSpan = DateTime.Now - Command.Investor.TimeDelayMakeCommand;
            if (_timeSpan.TotalSeconds > 0 & _timeSpan.TotalSeconds < 1)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "PLEASE TRY AGAIN";

                //string message = "AddCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                //            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                //            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                //            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                //            DateTime.Now;

                //Command.Investor.ClientCommandQueue.Add(message);

                //if (Command.Investor.IsOnline == false)
                //{
                //    Business.Investor.investorInstance.SendCommandToInvestorOnline(Command.Investor.InvestorID, Business.TypeLogin.ReadOnly, message);
                //}

                //#region INSERT SYSTEM LOG EVENT MAKE COMMAND
                //content = content + " unsuccessful [account read only]";
                //TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);
                //#endregion
                Command.Investor.TimeDelayMakeCommand = DateTime.Now;

                return Result;
            }

            Command.Investor.TimeDelayMakeCommand = DateTime.Now;

            #region CHECK INVESTOR ONLINE(CHECK ACCOUNT IS PRIMARY)
            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(objCommand.InvestorID, TradingServer.Business.TypeLogin.Primary, objCommand.LoginKey);
            if (!checkOnline)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "ACCOUNT READ ONLY";

                string message = "AddCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                Command.Investor.ClientCommandQueue.Add(message);

                if (Command.Investor.IsOnline == false)
                {
                    Business.Investor.investorInstance.SendCommandToInvestorOnline(Command.Investor.InvestorID, Business.TypeLogin.ReadOnly, message);
                }

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [account read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK MANUAL DEALERS OR AUTOMATIC
            string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");

            if (executionType == "manual only- no automation" ||
                executionType == "manual- but automatic if no dealer online" ||
                executionType == "automatic only")
            {
                Command.OpenPrice = double.Parse(objCommand.OpenPrice);
                Command.Size = objCommand.Size;
                Command.StopLoss = objCommand.StopLoss;
                Command.TakeProfit = objCommand.TakeProfit;
                Command.ClientCode = objCommand.ClientCode;
                Command.IsHedged = Command.Symbol.IsHedged;
                Command.MaxDev = objCommand.MaxDev;
                Command.ExpTime = objCommand.TimeExpiry;
                Command.IpAddress = objCommand.IpAddress;

                Command.Symbol.MarketAreaRef.AddCommand(Command);
                Result.Command = objCommand;
                Result.isDeal = true;
                return Result;
            }
            #endregion

            //===========================================================================================================================

            #region SET PRICE SERVER
            Command.BidServer = Command.Symbol.TickValue.Bid;
            Command.AskServer = Command.Symbol.TickValue.Ask;
            #endregion   
         
            #region CHECK RULE TRADE IN ET5 SYSTEM(UPDATE 19/09/2013). NOTE: THE SAME MT4(ANDREW BOSS)
            bool isEnable = Command.IsEnableCheckRuleTrade(Command);
            if (isEnable)
            {
                bool isValidRule = Command.CheckRuleTrader(Command, objCommand.Size);
                if (!isValidRule)
                {
                    if (Command.Investor != null)
                    {
                        //Add Result To Client Command Queue Of Investor
                        string Message = "AddCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        int investorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                        if (investorOnline > 0)
                            Command.Investor.ClientCommandQueue.Add(Message);
                    }

                    Result.isDeal = false;
                    Result.Error = "TRADE IS DISABLED";
                    Result.Command = objCommand;

                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                    string tempContent = "make command";
                    tempContent = tempContent + " unsuccessful [trade is disabled]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, "[trade is disabled]", objCommand.IpAddress, Command.Investor.Code);
                    #endregion

                    return Result;
                }
            }
            #endregion

            #region CHECK TIME TICK SERVER
            if (!Command.Symbol.CheckTimeTick())
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "AddCommand$False,MARKET ON HOLD," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    int investorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                    if (investorOnline > 0)
                        Command.Investor.ClientCommandQueue.Add(Message);
                }

                Result.isDeal = false;
                Result.Error = "MARKET ON HOLD";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                string tempContent = "make command";
                tempContent = tempContent + " unsuccessful [symbol on hold]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, "[symbol on hold]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK NULL OBJECT
            if (Command == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "[Make Command Error] Command Is Null", "[Make Command Failed]", "", "");

                return Result;
            }

            if (Command.Investor == null || Command.Symbol == null || Command.Type == null || Command.IGroupSecurity == null)
            {
                //Add Result To Client Command Queue Of Investor
                string Message = "AddCommand$False,COMMAND DON'T EXISTS," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                //int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                //if (countInvestorOnline > 0)
                    Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "TRADE IS DISABLED. PLEASE TRY AGAIN";
                Result.Command = objCommand;

                TradingServer.Facade.FacadeAddNewSystemLog(1, "symbol, investor, type or igroupsecurity empty", "[make command]", "", "");

                return Result;
            }
            #endregion            

            #region CHECK VALID IPADDRESS
            bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, objCommand.IpAddress);
            if (!checkIP)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "INVALID IP ADDRESS";

                string message = "AddCommand$False,INVALID IP ADDRESS," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                Command.Investor.ClientCommandQueue.Add(message);

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [invalid ip address]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid ip address]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK INVESTOR ONLINE
            //CHECK INVESTOR ONLINE
            bool isOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
            if (!isOnline)
            {
                //ADD INVESTOR TO LIST ONLINE IF CLIENT TIME OUT
                bool isAdd = TradingServer.Business.Investor.investorInstance.CheckOnlineInvestor(objCommand.InvestorID, objCommand.LoginKey);
            }
            #endregion

            #region CHECK INVESTOR ONLINE(CHECK ACCOUNT IS PRIMARY)
            //bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(objCommand.InvestorID, TradingServer.Business.TypeLogin.Primary, objCommand.LoginKey);
            //if (!checkOnline)
            //{
            //    Result.isDeal = false;
            //    Result.Command = objCommand;
            //    Result.Error = "ACCOUNT READ ONLY";

            //    string message = "AddCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
            //                Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
            //                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
            //                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
            //                DateTime.Now;

            //    Command.Investor.ClientCommandQueue.Add(message);

            //    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
            //    content = content + " unsuccessful [account read only]";
            //    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);
            //    #endregion

            //    return Result;
            //}
            #endregion

            Command.Investor.IpAddress = objCommand.IpAddress;

            #region CHECK SYMBOL EXISTS IN SECURITY
            bool isExists = Business.Market.marketInstance.IsExistsSymbolInSecurity(Command);
            if (!isExists)
            {
                //Add Result To Client Command Queue Of Investor
                string Message = "AddCommand$False,ACTION NOT ALLOW," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "ACTION NOT ALLOW";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [symbol don't exits in security]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol don't exits in security]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion            

            #region CHECK ACCOUNT LOGIN WITH PASSWORD READ ONLY
            if (Command.Investor.IsReadOnly)
            {
                //Add Result To Client Command Queue Of Investor
                string Message = "AddCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "TRADE IS DISABLED";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [trade is disabled]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[trade is disabled]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CONVERT TYPE.ID TO COMMAND TYPE
            string CommandType = string.Empty;
            switch (Command.Type.ID)
            {
                case 1:
                    CommandType = "Open";
                    break;
                case 2:
                    CommandType = "Open";
                    break;
                case 7:
                    CommandType = "BuyLimit";
                    break;
                case 8:
                    CommandType = "SellLimit";
                    break;
                case 9:
                    CommandType = "BuyStop";
                    break;
                case 10:
                    CommandType = "SellStop";
                    break;
                case 11:
                    CommandType = "BuyFuture";
                    break;
                case 12:
                    CommandType = "SellFuture";
                    break;
                case 17:
                    CommandType = "BuyStopFutureCommand";
                    break;
                case 18:
                    CommandType = "SellStopFutureCommand";
                    break;
                case 19:
                    CommandType = "BuyLimitFutureCommand";
                    break;
                case 20:
                    CommandType = "SellLimitFutureCommand";
                    break;
            } 
            #endregion                       

            #region CHECK TIME MARKET
            if (!Business.Market.IsOpen)
            {
                string Message = "AddCommand$False,MARKET IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," +
                            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                            Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS CLOSE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [market is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is close]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }   //END IF CHECK STATUS MARKET
            #endregion            

            #region CHECK IS TRADE OF SYMBOL
            if (!Command.Symbol.IsTrade)
            {
                string Message = "AddCommand$False,SYMBOL IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                                Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                                Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," +
                                Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                                Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "SYMBOL IS CLOSE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [symbol is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol is close]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK HOLIDAY OF SERVER
            if (Command.Symbol.IsHoliday)
            {
                string Message = "AddCommand$False,MARKET IS HOLIDAY," + Command.ID + "," + Command.Investor.InvestorID + "," +
                               Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                               Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                               Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," +
                               Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                               Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS HOLIDAY";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [market is holiday]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is holiday]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion           

            if (Command.Investor != null)
            {
                #region CHECK INVESTOR ISDISABLE
                if (Command.Investor.IsDisable)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "AddCommand$False,ACCOUNT IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "ACCOUNT IS DISABLED";
                    Result.Command = objCommand;

                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                    content = content + " unsuccessful [account is disabled]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account is disabled]", objCommand.IpAddress, Command.Investor.Code);
                    #endregion

                    return Result;
                }   //END IF CHECK ISDISABLE OF ACCOUNT
                #endregion

                #region CHECK INVESTOR READ ONLY
                if (Command.Investor.ReadOnly)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "AddCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                    if (countInvestorOnline > 0)
                        Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "TRADE IS DISABLED";
                    Result.Command = objCommand;

                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                    content = content + " unsuccessful [trade is disabled]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[trade is disabled]", objCommand.IpAddress, Command.Investor.Code);
                    #endregion

                    return Result;
                }   //END IF CHECK READ ONLY OF ACCOUNT
                #endregion
            }

            #region CHECK TICK SIZE OF SYMBOL
            bool CheckTickSize = Command.Symbol.CheckTickSizeAtOpenCommand(Command.OpenPrice, Command.Symbol.Digit, Command.Symbol.TickSize);
            if (!CheckTickSize)
            {
                string Message = "AddCommand$False,INVALID TICK SIZE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + "," +
                            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                            Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                if (countInvestorOnline > 0)
                    Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "INVALID TICK SIZE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [invalid tick size]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid tick size]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }   //END IF CHECK TICK SIZE OF SYMBOL
            #endregion

            Command.OpenPrice = double.Parse(objCommand.OpenPrice);
            Command.Size = objCommand.Size;
            Command.StopLoss = objCommand.StopLoss;
            Command.TakeProfit = objCommand.TakeProfit;
            Command.ClientCode = objCommand.ClientCode;
            Command.IsHedged = Command.Symbol.IsHedged;
            Command.MaxDev = objCommand.MaxDev;
            Command.ExpTime = objCommand.TimeExpiry;

            #region CHECK OPEN PRICE OF PENDING ORDER
            if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10 ||
                Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
            {
                bool ResultCheckOpenPrice = false;
                ResultCheckOpenPrice = Command.Symbol.CheckOpenPricePendingOrder(Command.Symbol.Name, Command.Type.ID, Command.OpenPrice, Command.Symbol.LimitLevel,
                                            Command.Symbol.StopLevel, Command.Symbol.Digit, int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));
                if (ResultCheckOpenPrice == false)
                {
                    string Message = "AddCommand$False,INVALID OPEN PRICE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                    if (countInvestorOnline > 0)
                        Command.Investor.ClientCommandQueue.Add(Message);

                    Result.Error = "INVALID OPEN PRICE";
                    Result.Command = objCommand;
                    Result.isDeal = false;

                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                    content = content + " unsuccessful [invalid open price]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                    #endregion

                    return Result;
                }   //END IF CHECK S/L AND T/P OF COMMAND
            }   //END IF CHECK COMMAND IS PENDING ORDER
            #endregion

            #region Get Setting IsTrade In Group
            bool IsTradeGroup = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G01")
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                        IsTradeGroup = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion
            
            if (IsTradeGroup)
            {
                if (Command.Symbol == null || Command.Investor == null || Command.Type == null || Command.IGroupSecurity == null)
                {
                    #region Check Investor != null Then Return Error To Client
                    if (Command.Investor != null)
                    {
                        //Add Result To Client Command Queue Of Investor
                        string Message = "AddCommand$False,TRADE IS DISABLED," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        Command.Investor.ClientCommandQueue.Add(Message);

                        Result.isDeal = false;
                        Result.Error = "Can't Find Symbol Investor Or Type Command";
                        Result.Command = objCommand;

                        #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                        content = content + " unsuccessful [can't find symbol investor or type command]";
                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[can't find symbol investor or type command]", objCommand.IpAddress, Command.Investor.Code);
                        #endregion

                        return Result;
                    }
                    #endregion                    
                }   //END IF CHECK SYMBOL, INVESTOR , TYPE, IGROUPSECURITY IS NULL
                else
                {
                    #region Set Property IsBuy And CommandType Send To Client
                    bool IsBuy = false;
                    if (Command.Type.ID == 1 || Command.Type.ID == 7 || Command.Type.ID == 9 || Command.Type.ID == 11 ||
                        Command.Type.ID == 17 || Command.Type.ID == 19)
                        IsBuy = true;
                    #endregion                    

                    #region Check Lots Minimum And Maximum In IGroupSecurity And Check IsTrade In IGroupSecurity
                    bool IsTrade = false;
                    double Minimum = -1;
                    double Maximum = -1;
                    double Step = -1;
                    bool ResultCheckStepLots = false;

                    #region Get Config IGroupSecurity
                    if (Command.IGroupSecurity.IGroupSecurityConfig != null)
                    {
                        int countIGroupSecurityConfig = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                        for (int i = 0; i < countIGroupSecurityConfig; i++)
                        {
                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B01")
                            {
                                if (Command.IGroupSecurity.IGroupSecurityConfig[i].BoolValue == 1)
                                    IsTrade = true;
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B11")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Minimum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B12")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Maximum);
                            }

                            if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B13")
                            {
                                double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Step);
                            }
                        }
                    }
                    #endregion

                    if (IsTrade == true)
                    {
                        ResultCheckStepLots = Command.IGroupSecurity.CheckStepLots(Minimum, Maximum, Step, Command.Size);

                        #region If Check Step Lots False Return Client
                        if (ResultCheckStepLots == false)
                        {
                            string Message = "AddCommand$False,WRONG VOLUME MINIMUM : " + Minimum + " LOT " + "MAXIMUM " + Maximum + " LOT," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                            if (Command.Investor.ClientCommandQueue == null)
                                Command.Investor.ClientCommandQueue = new List<string>();

                            int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                            if (countInvestorOnline > 0)
                                Command.Investor.ClientCommandQueue.Add(Message);

                            Result.isDeal = false;
                            Result.Error = "WRONG VOLUME";
                            Result.Command = objCommand;

                            #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                            content = content + " unsuccessful [wrong volume minimum]";
                            TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[wrong volume minimum]", objCommand.IpAddress, Command.Investor.Code);
                            #endregion

                            return Result;
                        }   //END IF CHECK STEP LOTS OF SYMBOL
                        #endregion
                    }   //END IF CHECK IS TRADE OF SYMBOL
                    else
                    {
                        string Message = "AddCommand$False,SYMBOL CANNOT BE TRADED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                        if (countInvestorOnline > 0)
                            Command.Investor.ClientCommandQueue.Add(Message);

                        Result.isDeal = false;
                        Result.Error = "SYMBOL CANNOT BE TRADED";
                        Result.Command = objCommand;

                        #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                        content = content + " unsuccessful [symbol cannot be trade]";
                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol cannot be traded]", objCommand.IpAddress, Command.Investor.Code);
                        #endregion

                        return Result;
                    }   //END ELSE CHECK IS TRADE OF SYMBOL                    
                    #endregion

                    #region Check Status Trade Of Symbol(Full Access,Close Only, No)
                    if (Command.Symbol.Trade.ToUpper() != "FULL ACCESS")
                    {
                        string Message = "AddCommand$False,ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                        if (countInvestorOnline > 0)
                            Command.Investor.ClientCommandQueue.Add(Message);
                     
                        Result.isDeal = false;
                        Result.Error = "ACTION NOT ALLOWED";
                        Result.Command = objCommand;

                        #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                        content = content + " unsuccessful [symbol close only]";
                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol close only]", objCommand.IpAddress, Command.Investor.Code);
                        #endregion

                        return Result;
                    }   //END IF CHECK STATUS OF SYMBOL(FULL ACCESS, CLOSE ONLY, NO TRADE)
                    #endregion

                    #region Check Stop Loss And Take Profit
                    if (Command.StopLoss > 0 || Command.TakeProfit > 0)
                    {
                        if (Command.Type.ID == 1 || Command.Type.ID == 2 || Command.Type.ID == 11 || Command.Type.ID == 12)
                        {
                            #region Check Limit And Stop Of Open Trade
                            bool ResultCheckLimit = false;

                            ResultCheckLimit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                                                                Command.Symbol.StopLossTakeProfitLevel, Command.Symbol.Digit, 
                                                                int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));

                            if (ResultCheckLimit == false)
                            {
                                string Message = "AddCommand$False,MARKET PRICE TOO CLOSE ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                if (Command.Investor.ClientCommandQueue == null)
                                    Command.Investor.ClientCommandQueue = new List<string>();

                                int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                                if (countInvestorOnline > 0)
                                    Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Error = "MARKET PRICE TOO CLOSE ACTION NOT ALLOWED";
                                Result.Command = objCommand;

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid limit and stop]";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid limit and stop]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion

                                return Result;
                            }
                            #endregion
                        }   //END IF CHECK COMMAND IS SPOT COMMAND
                        else if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10 || 
                            Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20 )
                        {
                            #region Check Limit And Stop Of Pending Order
                            bool ResultCheckLimit = false;
                            ResultCheckLimit = Command.Symbol.CheckLimitAndStopPendingOrder(Command.Symbol.Name, Command.Type.ID, Command.OpenPrice,
                                Command.StopLoss, Command.TakeProfit, Command.Symbol.StopLossTakeProfitLevel, Command.Symbol.Digit);

                            if (ResultCheckLimit == false)
                            {
                                string Message = "AddCommand$False,MARKET PRICE TOO CLOSE ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                if (Command.Investor.ClientCommandQueue == null)
                                    Command.Investor.ClientCommandQueue = new List<string>();

                                int countInvestorOnline = Command.Investor.CountInvestorOnline(Command.Investor.InvestorID);
                                if (countInvestorOnline > 0)
                                    Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Error = "MARKET PRICE TOO CLOSE ACTION NOT ALLOWED";
                                Result.Command = objCommand;

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid limit and stop]";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid limit and stop]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion

                                return Result;
                            }
                            #endregion
                        }   //END IF CHECK COMMAND IS PENDING ORDER
                    }   //END IF CHECK STOP LOSS AND TAKE PROFIT
                    #endregion

                    #region Check Setting IsLong
                    switch (Command.Type.ID)
                    {
                        #region Case Sell
                        case 2:
                            {
                                if (Command.Symbol.LongOnly == true)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion

                        #region Case Sell Limit
                        case 8:
                            {
                                if (Command.Symbol.LongOnly == true)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion

                        #region Case Sell Stop
                        case 10:
                            {
                                if (Command.Symbol.LongOnly == true)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion

                        #region SELL FUTURE
                        case 12:
                            {
                                if (Command.Symbol.LongOnly)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion
                        
                        #region SELL STOP FUTURE
                        case 18:
                            {
                                if (Command.Symbol.LongOnly)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion  

                        #region SELL LIMIT FUTURE
                        case 20:
                            {
                                if (Command.Symbol.LongOnly)
                                {
                                    string Message = "AddCommand$False,ONLY LONG POSITION ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                    Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                        Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                        CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                    if (Command.Investor.ClientCommandQueue == null)
                                        Command.Investor.ClientCommandQueue = new List<string>();

                                    Command.Investor.ClientCommandQueue.Add(Message);

                                    Result.isDeal = false;
                                    Result.Error = "ONLY LONG POSITION ALLOWED";
                                    Result.Command = objCommand;

                                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                    content = content + " unsuccessful [only long position]";
                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[only long position]", objCommand.IpAddress, Command.Investor.Code);
                                    #endregion

                                    return Result;
                                }
                            }
                            break;
                        #endregion  
                    }
                    #endregion

                    #region Find Price Close Of Symbol
                    switch (Command.Type.ID)
                    {
                        case 1:
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;
                            break;
                        case 2:
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;
                            break;

                        #region BUY LIMIT SPOT
                        case 7: //BUY LIMIT
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;
                            if (Command.OpenPrice > Command.Symbol.TickValue.Ask)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion                        

                        #region SELL LIMIT SPOT
                        case 8: //SELL LIMIT
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;
                            if (Command.OpenPrice < Command.Symbol.TickValue.Bid)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " < " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion                       

                        #region BUY STOP SPOT
                        case 9: //BUY STOP
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;
                            if (Command.OpenPrice < Command.Symbol.TickValue.Ask)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " < " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion                        

                        #region SELL STOP SPOT
                        case 10:    //SELL STOP
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;

                            if (Command.OpenPrice > Command.Symbol.TickValue.Bid)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion
                        
                        case 11:
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;
                            break;
                        case 12:
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;
                            break;

                        #region BUY STOP FUTURE
                        case 17:    //BUY STOP
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;

                            if (Command.OpenPrice < Command.Symbol.TickValue.Ask)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion

                        #region SELL STOP FUTURE
                        case 18:    //SELL STOP
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;

                            if (Command.OpenPrice > Command.Symbol.TickValue.Bid)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion

                        #region BUY LIMIT FUTURE
                        case 19:    //BUY LIMIT
                            Command.ClosePrice = Command.Symbol.TickValue.Bid;
                            if (Command.OpenPrice > Command.Symbol.TickValue.Ask)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion

                        #region SELL LIMIT FUTURE
                        case 20:    //SELL LIMIT
                            Command.ClosePrice = Command.Symbol.TickValue.Ask;
                            if (Command.OpenPrice < Command.Symbol.TickValue.Bid)
                            {
                                string Message = "AddCommand$False,INVALID OPEN PRICE," + Result + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0" + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                                Command.Investor.ClientCommandQueue.Add(Message);

                                Result.isDeal = false;
                                Result.Command = objCommand;
                                Result.Error = "INVALID OPEN PRICE";

                                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                                content = content + " unsuccessful [invalid open price] (" + Command.OpenPrice + " > " + Command.Symbol.TickValue.Ask + ")";
                                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid open price]", objCommand.IpAddress, Command.Investor.Code);
                                #endregion
                            }
                            break;
                        #endregion                        
                    }
                    #endregion

                    #region CHECK VALID ACCOUNT OF INVESTOR
                    bool checkValidAccount = false;
                    //Call Function Check Account Of Investor
                    checkValidAccount = Command.CheckValidAccountInvestor(Command);
                    
                    if (!checkValidAccount)
                    {
                        #region Reurn Error Invalid Account For Client
                        //Add Result To Client Command Queue Of Investor
                        string Message = "AddCommand$False,NOT ENOUGH MONEY," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                    Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                    CommandType + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        Command.Investor.ClientCommandQueue.Add(Message);
                        #endregion

                        Result.isDeal = false;
                        Result.Command = objCommand;
                        Result.Error = "NOT ENOUGH MONEY";

                        #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                        content = content + " unsuccessful [not enough money]";
                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[not enough money]", objCommand.IpAddress, Command.Investor.Code);
                        #endregion

                        return Result;
                    }
                    #endregion                   
                    
                    if (Command.Type.ID == 1 || Command.Type.ID == 2 || Command.Type.ID == 11 || Command.Type.ID == 12)
                    {
                        //Call Dealer
                        Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                        newRequestDealer.InvestorID = Command.Investor.InvestorID;
                        newRequestDealer.MaxDev = Command.MaxDev;
                        newRequestDealer.Name = "Open";
                        newRequestDealer.Request = Command;
                        newRequestDealer.TimeClientRequest = DateTime.Now;

                        TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);
                    }
                    else
                    {
                        //Name: OpenPending
                        //Call Dealer
                        //Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                        //newRequestDealer.InvestorID = Command.Investor.InvestorID;
                        //newRequestDealer.MaxDev = Command.MaxDev;
                        //newRequestDealer.Name = "OpenPending";
                        //newRequestDealer.Request = Command;
                        //newRequestDealer.TimeClientRequest = DateTime.Now;

                        //TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);

                        Command.Symbol.MarketAreaRef.AddCommand(Command);
                    }

                    Result.Command = objCommand;
                    Result.isDeal = true;
                }       
            }
            else
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "AddCommand$False,ACTION NOT ALLOWED," + Result + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                    content = content + " unsuccessful [trade is disable]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[trade is disable]", objCommand.IpAddress, Command.Investor.Code);
                    #endregion
                }
            }
            
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static bool FacadeRequestCommand(Business.RequestDealer Command)
        {
            return Facade.FacadeSendRequestToDealer(Command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static bool FacadeClientCancelRequest(Business.RequestDealer Command)
        {
            return Facade.FacadeClientCancelRequest(Command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.RequestDealer> FacadeGetAllRequestDealer()
        {
            return Facade.FacadeGetAllRequestDealer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.RequestDealer> FacadeGetAllRequestCompareDealer()
        {
            return Facade.FacadeGetAllRequestCompareDealer();
        }

        /// <summary>
        /// CLOSE SPOT COMMAND BY MANAGER
        /// </summary>
        /// <param name="CommandID"></param>
        public static bool FacadeCloseSpotCommandByManager(int CommandID,double Size,double ClosePrices)
        {
            bool Result = false;
            Business.OpenTrade Command = new Business.OpenTrade();

            #region Find In Symbol List And Remove Command
            Command = TradingServer.Facade.FacadeFindOpenTradeInCommandList(CommandID);
            #endregion

            if (Command == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "some info empty(command)", "[close command by manager]", "", "");
                return false;
            }

            if (Command.Investor == null || Command.Type == null || Command.Symbol == null || Command.IGroupSecurity == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "some info empty(investor,type,symbol,igroupsecurity)", "[close command by manager]", "", "");
            }

            //CHECK IF COMMAND.SIZE < SIZE THEN RETURN FALSE
            if (Command.Size < Size)
                return false;

            //SET NEW SIZE AND CLOSE PRICES
            double tempSize = 0;
            tempSize = Command.Size - Size;

            Command.IsServer = true;
            Command.Size = Size;
            Command.ClosePrice = ClosePrices;

            if (Command.Symbol != null && Command.Investor != null && Command.Type != null && Command.IGroupSecurity != null)
            {
                Command.Symbol.MarketAreaRef.CloseCommand(Command);
                Result = true;
            }
            else
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "some one info empty", "[close command by manager]", "", "");
            }

            return Result;
        }

        /// <summary>
        /// CLOSE SPOT COMMAND
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static ClientBusiness.DealMessage FacadeCloseSpotCommand(TradingServer.ClientBusiness.Command objCommand)
        {   
            ClientBusiness.DealMessage Result = new ClientBusiness.DealMessage();
                           
            Result.isDeal = false;

            Business.OpenTrade Command = new Business.OpenTrade();

            #region Find In Symbol List And Remove Command
            Command = TradingServer.Facade.FacadeFindOpenTradeInCommandList(objCommand.CommandID);

            if (Command.Investor == null || Command.Type == null || Command.Symbol == null)
                return null;

            Command.Investor.IpAddress = objCommand.IpAddress;

            bool isRequest = false;
            if (Business.Market.marketInstance.MQLCommands != null)
            {
                int count = Business.Market.marketInstance.MQLCommands.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.marketInstance.MQLCommands[i].ClientCode == objCommand.ClientCode)
                    {
                        isRequest = true;
                        break;
                    }
                }

                if (!isRequest)
                {
                    NJ4XConnectSocket.MQLCommand newMQLCommand = new NJ4XConnectSocket.MQLCommand();
                    newMQLCommand.ClientCode = objCommand.ClientCode;
                    newMQLCommand.InvestorCode = Command.Investor.Code;
                    newMQLCommand.IpAddress = objCommand.IpAddress;

                    Business.Market.marketInstance.MQLCommands.Add(newMQLCommand);
                }
            }

            #region CHECK VOLUME < 0
            //if (objCommand.Size < 0)
            //{
            //    //Add Result To Client Command Queue Of Investor
            //    string Message = "CloseCommand$False,INVALID VOLUME.," + Command.ID + "," + Command.Investor.InvestorID + "," +
            //            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
            //            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
            //            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
            //            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
            //            Command.Margin + ",Open," + DateTime.Now;

            //    if (Command.Investor.ClientCommandQueue == null)
            //        Command.Investor.ClientCommandQueue = new List<string>();

            //    Command.Investor.ClientCommandQueue.Add(Message);

            //    Result.isDeal = false;
            //    Result.Error = "INVALID VOLUME.";
            //    Result.Command = objCommand;

            //    return Result;
            //}
            #endregion

            string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
            string openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.OpenPrice.ToString(), Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.TakeProfit.ToString(), Command.Symbol.Digit);
            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.StopLoss.ToString(), Command.Symbol.Digit);
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);

            string mode = string.Empty;
            string content = string.Empty;
            string comment = string.Empty;

            #region REND CONTENT SYSTEM LOG AND INSERT SYSTEM LOG
            switch (Command.Type.ID)
            {
                case 1:
                    {
                        content = "'" + Command.Investor.Code + "': close instant order #" + Command.CommandCode + " buy " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                        comment = "[close instant buy]";
                    }
                    break;
                case 2:
                    {
                        content = "'" + Command.Investor.Code + "': close instant order #" + Command.CommandCode + " sell " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";
                        comment = "[close instant sell]";
                    }
                    break;
                case 7:
                    {
                        content = "'" + Command.Investor.Code + "': close buy limit order #" + Command.CommandCode + " buy limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[order buy limit]";
                    }
                    break;
                case 8:
                    {
                        content = "'" + Command.Investor.Code + "': close sell limit order #" + Command.CommandCode + " sell limit " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[order sell limit]";
                    }
                    break;
                case 9:
                    {
                        content = "'" + Command.Investor.Code + "': close buy stop order #" + Command.CommandCode + " buy stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[order buy stop]";
                    }
                    break;
                case 10:
                    {
                        content = "'" + Command.Investor.Code + "': close sell stop order #" + Command.CommandCode + " sell stop " + size + " " + Command.Symbol.Name + " at " + openPrice +
                            " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[order sell stop]";
                    }
                    break;
                case 11:
                    {
                        content = "'" + Command.Investor.Code + "': close future order #" + Command.CommandCode + " future buy " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[future buy]";
                    }
                    break;
                case 12:
                    {
                        content = "'" + Command.Investor.Code + "': close future order #" + Command.CommandCode + " future sell " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")" + " exp " + Command.ExpTime;
                        comment = "[future sell]";
                    }
                    break;
            }

            if (!isRequest)
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, objCommand.IpAddress, Command.Investor.Code);
            #endregion  

            #region CHECK INVESTOR ONLINE(CHECK ACCOUNT IS PRIMARY)
            bool _checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(objCommand.InvestorID, TradingServer.Business.TypeLogin.Primary, objCommand.LoginKey);
            if (!_checkOnline)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "ACCOUNT READ ONLY";

                string message = "AddCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                Command.Investor.ClientCommandQueue.Add(message);

                if (Command.Investor.IsOnline == false)
                {
                    Business.Investor.investorInstance.SendCommandToInvestorOnline(Command.Investor.InvestorID, Business.TypeLogin.ReadOnly, message);
                }

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [account read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK MANUAL DEALERS OR AUTOMATIC
            string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");

            if (executionType == "manual only- no automation" ||
                executionType == "manual- but automatic if no dealer online" ||
                executionType == "automatic only")
            {   
                Command.ClosePrice = objCommand.ClosePrice;
                if (objCommand.Size > 0)
                    Command.Size = objCommand.Size;
                
                Command.Symbol.MarketAreaRef.CloseCommand(Command);
                Result.Command = objCommand;
                Result.isDeal = true;
                return Result;
            }
            #endregion


            //==============================================================================================================================================================

            if (Command == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "command is empty" + objCommand.CommandID, "Check Null Close Command", "", "");
                return Result;
            }

            if (Command.Investor == null || Command.Symbol == null || Command.Type == null || Command.IGroupSecurity == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "symbol investor or type command empty" + objCommand.CommandID, "Check Null Close Command", "", "");
                return null;
            }

            #region CHECK SYMBOL ON HOLD
            if (!Command.Symbol.CheckTimeTick())
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "CloseCommand$False,MARKET ON HOLD," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                            Command.Margin + ",Open," + DateTime.Now;

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);
                }

                Result.isDeal = false;
                Result.Error = "MARKET ON HOLD";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                string tempContent = "close command";
                tempContent = tempContent + " unsuccessful [symbol on hold]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, "[symbol on hold]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            Command.Investor.IpAddress = objCommand.IpAddress;
            #endregion

            #region CALCULATION LOTS OF COMMAND(08/07/2011)
            if (objCommand.Size > 0)
            {
                if (objCommand.Size > Command.Size)
                {
                    //Business.Market.SendNotifyToClient("CMD2343344", 3, Command.Investor.InvestorID);

                    if (Command.Investor != null)
                    {
                        //Add Result To Client Command Queue Of Investor
                        string Message = "CloseCommand$False,INVALID LOTS," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                                Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                                Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                                Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                                Command.Margin + ",Open," + DateTime.Now;

                        if (Command.Investor.ClientCommandQueue == null)
                            Command.Investor.ClientCommandQueue = new List<string>();

                        Command.Investor.ClientCommandQueue.Add(Message);
                    }

                    Result.isDeal = false;
                    Result.Command = objCommand;
                    Result.Error = "INVALID LOTS";

                    return Result;
                }
                else
                {
                    Command.Size = objCommand.Size;
                }
            }
            #endregion

            #region CHECK IPADDRESS
            bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(Command.Investor.InvestorID, objCommand.IpAddress);
            if (!checkIP)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "INVALID IP ADDRESS";

                #region INSERT LOG
                content = content + " unsuccessful [invalid ip address]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid ip address]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                string message = "CloseCommand$False,INVALID IP ADDRESS," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                Command.Investor.ClientCommandQueue.Add(message);

                return Result;
            }
            #endregion

            bool isOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(objCommand.InvestorID, objCommand.LoginKey);
            if (!isOnline)
            {
                //ADD INVESTOR TO LIST ONLINE IF CLIENT TIMEOUT
                TradingServer.Business.Investor.investorInstance.CheckOnlineInvestor(objCommand.InvestorID, objCommand.LoginKey);
            }

            #region CHECK INVESTOR ONLINE(CHECK READ ONLY)
            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(Command.Investor.InvestorID, TradingServer.Business.TypeLogin.Primary, objCommand.LoginKey);
            if (!checkOnline)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "ACCOUNT READ ONLY";

                #region INSERT LOG
                content = content + " unsuccessful [account read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                string message = "CloseCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                Command.Investor.ClientCommandQueue.Add(message);

                return Result;
            }
            #endregion

            #region CHECK INVESTOR LOGIN WITH PASSWORD READ ONLY
            if (Command.Investor.IsReadOnly)
            {
                string Message = "CloseCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "TRADE IS DISABLED";
                Result.Command = objCommand;

                content = content + " unsuccessful [login password read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[login password read only]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK SYMBOL EXISTS IN SECURITY
            bool isExists = Business.Market.marketInstance.IsExistsSymbolInSecurity(Command);
            if (!isExists)
            {
                //Add Result To Client Command Queue Of Investor
                string Message = "AddCommand$False,ACTION NOT ALLOW," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                            Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                            "Open" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "ACTION NOT ALLOW";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [symbol don't exits in security]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol don't exits in security]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion            

            #region CHECK TIME MARKET
            if (!Business.Market.IsOpen)
            {
                string Message = "CloseCommand$False,MARKET IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                            Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS CLOSE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [market is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is close]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK IS TRADE OF SYMBOL
            if (!Command.Symbol.IsTrade)
            {
                string Message = "CloseCommand$False,SYMBOL IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                                Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                                Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                                Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                                Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "SYMBOL IS CLOSE";
                Result.Command = objCommand;

                content = content + " unsuccessful [symbol is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol is close]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK HOLIDAY OF SERVER
            if (Command.Symbol.IsHoliday)
            {
                string Message = "CloseCommand$False,MARKET IS HOLIDAY," + Command.ID + "," + Command.Investor.InvestorID + "," +
                               Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                               Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                               Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," + 1 + "," +
                               Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                               Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS HOLIDAY";
                Result.Command = objCommand;

                content = content + " unsuccessful [market is holiday]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is holiday]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion           

            #region Get Setting IsTrade In Group
            bool IsTradeGroup = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G01")
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                        IsTradeGroup = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            #region CHECK IN TRADE IN GROUP
            if (!IsTradeGroup)
            {
                string Message = "CloseCommand$False,GROUP IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                    ",Close," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "GROUP IS DISABLED";
                Result.Command = objCommand;

                content = content + " unsuccessful [group is disable]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[group is disable]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion            

            if (Command.Investor != null)
            {
                #region CHECK ISDISABLE OF INVESTOR
                if (Command.Investor.IsDisable)
                {
                    string Message = "CloseCommand$False,ACCOUNT IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + 
                                ",Close," + DateTime.Now;

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "ACCOUNT IS DISABLED";
                    Result.Command = objCommand;

                    content = content + " unsuccessful [account is disable]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account is disable]", objCommand.IpAddress, Command.Investor.Code);

                    return Result;
                }
                #endregion

                #region CHECK READ ONLY OF INVESTOR
                if (Command.Investor.ReadOnly)
                {
                    string Message = "CloseCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + 
                            ",Close," + DateTime.Now;

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "TRADE IS DISABLED";
                    Result.Command = objCommand;

                    content = content + " unsuccessful [account read only]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);

                    return Result;
                }

                #endregion
            }

            #region CHECK MIN MAX AND STEP LOTS	 
            double Minimum = -1;
            double Maximum = -1;
            double Step = -1;
            bool ResultCheckStepLots = false;

            #region Get Config IGroupSecurity
            if (Command.IGroupSecurity.IGroupSecurityConfig != null)
            {
                int countIGroupSecurityConfig = Command.IGroupSecurity.IGroupSecurityConfig.Count;
                for (int i = 0; i < countIGroupSecurityConfig; i++)
                {                    
                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B11")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Minimum);
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B12")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Maximum);
                    }

                    if (Command.IGroupSecurity.IGroupSecurityConfig[i].Code == "B13")
                    {
                        double.TryParse(Command.IGroupSecurity.IGroupSecurityConfig[i].NumValue, out Step);
                    }
                }
            }
            #endregion

            ResultCheckStepLots = Command.IGroupSecurity.CheckStepLots(Minimum, Maximum, Step, Command.Size);

            #region If Check Step Lots False Return Client
            if (ResultCheckStepLots == false)
            {
                string Message = "CloseCommand$False,WRONG VOLUME MINIMUM : " + Minimum + " LOT " + "MAXIMUM " + Maximum + " LOT," + Command.ID + "," +
                                    Command.Investor.InvestorID + "," + Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," +
                                    Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," +
                                    Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID +
                                    "," + Command.Margin + ",Close," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "WRONG VOLUME";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [wrong volume minimum]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[wrong volume minimum]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }   //END IF CHECK STEP LOTS OF SYMBOL
            #endregion
            #endregion

            //SET ISSERVER = FALSE
            Command.IsServer = false;

            //Set Close Price Command
            Command.ClosePrice = objCommand.ClosePrice;

            //set close time
            Command.CloseTime = DateTime.Now;

            //set ipaddress to investor 
            Command.Investor.IpAddress = objCommand.IpAddress;

            #region Check Status Trade Of Symbol(Full Access,Close Only, No)
            if (Command.Symbol.Trade == "No")
            {
                bool IsBuy = false;
                if (Command.Type.ID == 1 || Command.Type.ID == 7 || Command.Type.ID == 9 || Command.Type.ID == 11)
                    IsBuy = true;

                string Message = "CloseCommand$False,ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + 
                        Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + 
                        Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," + 
                        Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + 
                        ",Close," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "ACTION NOT ALLOWED";

                content = content + " unsuccessful [status trade no]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[status trade no]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            if (Command.Symbol != null && Command.Investor != null && Command.Type != null && Command.IGroupSecurity != null)
            {
                if (Command.Type.ID == 1 || Command.Type.ID == 2 || Command.Type.ID == 11 || Command.Type.ID == 12)
                {
                    //Call Dealer
                    Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                    newRequestDealer.InvestorID = Command.Investor.InvestorID;
                    newRequestDealer.MaxDev = Command.MaxDev;
                    newRequestDealer.Name = "Close";
                    newRequestDealer.Request = Command;
                    newRequestDealer.TimeClientRequest = DateTime.Now;

                    TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);
                }
                else
                {
                    //Call Dealer
                    //Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                    //newRequestDealer.InvestorID = Command.Investor.InvestorID;
                    //newRequestDealer.MaxDev = Command.MaxDev;
                    //newRequestDealer.Name = "ClosePending";
                    //newRequestDealer.Request = Command;
                    //newRequestDealer.TimeClientRequest = DateTime.Now;

                    //TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);

                    Command.Symbol.MarketAreaRef.CloseCommand(Command);
                }
                
                Result.isDeal = true;
                Result.Error = "Close Command Complete" + Command.Symbol.MarketAreaRef.IMarketAreaName;
                return Result;
            }
            else
            {
                #region COMMENT CODE(DAY: 15 MONTH : 06 YEAR 2011
                //Command = TradingServer.Facade.FacadeFindOpenTradeInInvestorList(objCommand.CommandID);

                ////Set Close Command
                //Command.ClosePrice = objCommand.ClosePrice;

                //#region Check Status Trade Of Symbol(Full Access,Close Only, No)
                //if (Command.Symbol.Trade == "No")
                //{
                //    bool IsBuy = false;
                //    if (Command.Type.ID == 1 || Command.Type.ID == 7 || Command.Type.ID == 9 || Command.Type.ID == 11)
                //        IsBuy = true;

                //    string Message = "CloseCommand$False,ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                //            Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                //                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                //                Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + 
                //                ",Close," + DateTime.Now;

                //    if (Command.Investor.ClientCommandQueue == null)
                //        Command.Investor.ClientCommandQueue = new List<string>();

                //    Command.Investor.ClientCommandQueue.Add(Message);

                //    Result.isDeal = false;
                //    Result.Error = "ACTION NOT ALLOWED";

                //    content = content + " [unsuccessful]";
                //    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[status trade no]", objCommand.IpAddress, Command.Investor.Code);

                //    return Result;
                //}
                //#endregion

                //if (Command.Symbol != null && Command.Investor != null && Command.Type != null && Command.IGroupSecurity != null)
                //{
                //    if (Command.Type.ID == 1 || Command.Type.ID == 2 || Command.Type.ID == 11 || Command.Type.ID == 12)
                //    {
                //        //Call Dealer
                //        Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                //        newRequestDealer.InvestorID = Command.Investor.InvestorID;
                //        newRequestDealer.MaxDev = Command.MaxDev;
                //        newRequestDealer.Name = "Close";
                //        newRequestDealer.Request = Command;
                //        newRequestDealer.TimeClientRequest = DateTime.Now;

                //        TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);
                //    }
                //    else
                //    {
                //        Command.Symbol.MarketAreaRef.CloseCommand(Command);
                //    }

                //    //Command.Symbol.MarketAreaRef.CloseCommand(Command);
                //    Result.isDeal = true;
                //    Result.Error = "Close Command Of Investor Complete " + Command.Symbol.MarketAreaRef.IMarketAreaName;

                //    return Result;
                //}
                //else
                //{
                //    if (Command.Investor != null)
                //    {
                //        //Add Result To Client Command Queue Of Investor
                //        string Message = "CloseCommand$False,Can't Find Symbol Investor Or Type Command," + "0" + "," + "-1" + "," +
                //                "NaN" + "," + "-1" + "," + false + "," + DateTime.Now + "," + "-1" + "," + "-1" + "," +
                //                    "-1" + "," + "-1" + "," + "-1" + "," + "-1" + "," + "-1" + "," + "Comment," + "-1" + "," +
                //                    "Open" + "," + 1 + "," + DateTime.Now + "," + "-1" + "," + "0000000," + false + "," + "0" + "," + "0" + ",Open," + DateTime.Now;

                //        if (Command.Investor.ClientCommandQueue == null)
                //            Command.Investor.ClientCommandQueue = new List<string>();

                //        Command.Investor.ClientCommandQueue.Add(Message);
                //    }

                //    Result.isDeal = false;
                //    Result.Error = "Can't Find Symbol Investor Or Type Command";

                //    return Result;
                //}
                #endregion

                content = "some one info empty(symbol, investor ,type, or igroupsecurity)";
                TradingServer.Facade.FacadeAddNewSystemLog(1, content, "[info empty]", objCommand.IpAddress, Command.Investor.Code);
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCommand"></param>
        /// <returns></returns>
        public static ClientBusiness.DealMessage FacadeMultiCloseCommand(TradingServer.ClientBusiness.Command objCommand)
        {
            ClientBusiness.DealMessage Result = new ClientBusiness.DealMessage();

            Result.isDeal = false;

            TradingServer.Business.OpenTrade Command = new Business.OpenTrade();

            FillInstanceOpenTrade(objCommand, Command);

            #region CHECK SYMBOL ON HOLD
            if (!Command.Symbol.CheckTimeTick())
            {
                if (Command.Investor != null)
                {
                    //Add Result To Client Command Queue Of Investor
                    string Message = "CloseCommand$False,MARKET ON HOLD," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                                "BuySpotCommand" + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);
                }

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                string tempContent = "multiple close command";
                tempContent = tempContent + " unsuccessful [symbol on hold]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, "[symbol on hold]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK NULL COMMAND
            if (Command == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "command is empty", "Check Null Close Command", "", "");
                return Result;
            }

            if (Command.Investor == null || Command.Symbol == null || Command.Type == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "symbol investor or type command empty", "Check Null Close Command", "", "");
                return null;
            }
            #endregion            

            Command.ClosePrice = objCommand.ClosePrice;

            #region BEGIN INSERT SYSTEM LOG
            string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Bid.ToString(), Command.Symbol.Digit);
            string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Symbol.TickValue.Ask.ToString(), Command.Symbol.Digit);
            string closePrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.ClosePrice.ToString(), Command.Symbol.Digit);

            string mode = string.Empty;
            string content = string.Empty;
            string comment = string.Empty;

            content = "'" + Command.Investor.Code + "': multiple close by for symbol " + Command.Symbol.Name + " at close price " + closePrice + " (" + bid + "/" + ask + ")";
            comment = "[multiple close]";

            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
            #endregion            

            #region CHECK IP ADDRESS
            bool checkIp = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(objCommand.InvestorID, objCommand.IpAddress);
            if (!checkIp)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "INVALID IP ADDRESS";

                string Message = "CloseCommand$False,INVALID IP ADDRESS," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                content = content + " unsuccessful [invalid ip address]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[invalid ip address]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK INVESTOR ONLINE
            bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(objCommand.InvestorID, Business.TypeLogin.Primary, objCommand.LoginKey);
            if (!checkOnline)
            {
                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "ACCOUNT READ ONLY";

                string Message = "CloseCommand$False,ACCOUNT READ ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                content = content + " unsuccessful [account read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK INVESTOR LOGIN WITH PASSWORD READ ONLY
            if (Command.Investor.IsReadOnly)
            {
                string Message = "CloseCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Close," +
                            DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "TRADE IS DISABLED";
                Result.Command = objCommand;

                content = content + " unsuccessful [login password read only]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[login password read only]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK TIME MARKET
            if (!Business.Market.IsOpen)
            {
                string Message = "CloseCommand$False,MARKET IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                            Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                            Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                            Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                            Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                            Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS CLOSE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG EVENT MAKE COMMAND
                content = content + " unsuccessful [market is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is close]", objCommand.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK IS TRADE OF SYMBOL
            if (!Command.Symbol.IsTrade)
            {
                string Message = "CloseCommand$False,SYMBOL IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," +
                                Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                                Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                                Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," +
                                Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                                Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "SYMBOL IS CLOSE";
                Result.Command = objCommand;

                content = content + " unsuccessful [symbol is close]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[symbol is close]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion

            #region CHECK HOLIDAY OF SERVER
            if (Command.Symbol.IsHoliday)
            {
                string Message = "CloseCommand$False,MARKET IS HOLIDAY," + Command.ID + "," + Command.Investor.InvestorID + "," +
                               Command.Symbol.Name + "," + Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                               Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," +
                               Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," + 1 + "," +
                               Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," +
                               Command.Margin + ",Open," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS HOLIDAY";
                Result.Command = objCommand;

                content = content + " unsuccessful [market is holiday]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[market is holiday]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion           

            #region CHECK TRADE IN GROUP
            #region Get Setting IsTrade In Group
            bool IsTradeGroup = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].InvestorGroupID == Command.Investor.InvestorGroupInstance.InvestorGroupID)
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countParameter = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countParameter; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G01")
                                {
                                    if (Business.Market.InvestorGroupList[i].ParameterItems[j].BoolValue == 1)
                                        IsTradeGroup = true;

                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            #endregion

            #region CHECK TRADE GROUP
            if (!IsTradeGroup)
            {
                string Message = "CloseCommand$False,GROUP IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                    1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                    ",Close," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "GROUP IS DISABLED";
                Result.Command = objCommand;

                content = content + " unsuccessful [group is disable]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[group is disable]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion            
            #endregion

            if (Command.Investor != null)
            {
                #region CHECK ISDISABLE OF INVESTOR
                if (Command.Investor.IsDisable)
                {
                    string Message = "CloseCommand$False,ACCOUNT IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                ",Close," + DateTime.Now;

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "ACCOUNT IS DISABLED";
                    Result.Command = objCommand;

                    content = content + " unsuccessful [account is disable]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account is disable]", objCommand.IpAddress, Command.Investor.Code);

                    return Result;
                }
                #endregion

                #region CHECK READ ONLY OF INVESTOR
                if (Command.Investor.ReadOnly)
                {
                    string Message = "CloseCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                            Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                            Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                            1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                            ",Close," + DateTime.Now;

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(Message);

                    Result.isDeal = false;
                    Result.Error = "TRADE IS DISABLED";
                    Result.Command = objCommand;

                    content = content + " unsuccessful [account read only]";
                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[account read only]", objCommand.IpAddress, Command.Investor.Code);

                    return Result;
                }

                #endregion
            }

            #region Check Status Trade Of Symbol(Full Access,Close Only, No)
            if (Command.Symbol.Trade == "No")
            {
                bool IsBuy = false;                

                string Message = "CloseCommand$False,ACTION NOT ALLOWED," + Command.ID + "," + Command.Investor.InvestorID + "," +
                        Command.Symbol.Name + "," + Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," +
                        Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," +
                        Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," +
                        Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                        ",Close," + DateTime.Now;

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "ACTION NOT ALLOWED";

                content = content + " unsuccessful [status trade no]";
                TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[status trade no]", objCommand.IpAddress, Command.Investor.Code);

                return Result;
            }
            #endregion
            
            //set ipaddress to investor 
            Command.Investor.IpAddress = objCommand.IpAddress;            
            Command.ClosePrice = objCommand.ClosePrice;
            Command.IsMultiClose = true;
            Command.CloseTime = DateTime.Now;
            
            if (Command.Type.ID != 7 && Command.Type.ID != 8 && Command.Type.ID != 9 && Command.Type.ID != 10)
            {
                //Call Dealer
                Business.RequestDealer newRequestDealer = new Business.RequestDealer();
                newRequestDealer.InvestorID = Command.Investor.InvestorID;
                newRequestDealer.MaxDev = Command.MaxDev;
                newRequestDealer.Name = "Close";
                newRequestDealer.Request = Command;
                newRequestDealer.TimeClientRequest = DateTime.Now;

                TradingServer.Facade.FacadeSendRequestToDealer(newRequestDealer);

                Result.isDeal = true;
                Result.Error = "Close Command Complete" + Command.Symbol.MarketAreaRef.IMarketAreaName;

                return Result;
            }
            else
            {
                Command.Symbol.MarketAreaRef.MultiCloseCommand(Command);

                Result.isDeal = true;
                Result.Error = "Close Command Complete" + Command.Symbol.MarketAreaRef.IMarketAreaName;

                return Result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Business.OpenTrade> FacadeGetCommandHistory()
        {
            return ClientFacade.OpenTradeInstance.GetAllCommandHistory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<TradingServer.ClientBusiness.Command> FacadeGetCommandHistoryByInvestor(int InvestorID)
        {
            List<ClientBusiness.Command> Result = new List<ClientBusiness.Command>();
            List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();
            //tempResult = ClientFacade.OpenTradeInstance.GetCommandHistoryByInvestor(InvestorID);
            tempResult = ClientFacade.OpenTradeInstance.GetCommandHistoryByInvestorInMonth(InvestorID);

            if (tempResult != null)
            {
                int count = tempResult.Count;
                for (int i = 0; i < count; i++)
                {
                    if (tempResult[i].Symbol == null || tempResult[i].Investor == null || tempResult[i].Type == null)
                        continue;

                    if (tempResult[i].Symbol != null)
                    {
                        ClientBusiness.Command newCommand = new ClientBusiness.Command();
                        newCommand.ClosePrice = tempResult[i].ClosePrice;
                        newCommand.CommandID = tempResult[i].ID;
                        if (tempResult[i].Type != null)
                        {
                            newCommand.CommandType = tempResult[i].Type.Name;
                        }
                        newCommand.Commission = tempResult[i].Commission;
                        newCommand.InvestorID = tempResult[i].Investor.InvestorID;

                        bool IsBuy = false;
                        if (tempResult[i].Type != null)
                        {
                            if (tempResult[i].Type.ID == 1 || tempResult[i].Type.ID == 3 || tempResult[i].Type.ID == 5 ||
                                tempResult[i].Type.ID == 7 || tempResult[i].Type.ID == 9 || tempResult[i].Type.ID == 11)
                                IsBuy = true;
                        }

                        newCommand.IsBuy = IsBuy;
                        newCommand.OpenPrice = tempResult[i].OpenPrice.ToString();
                        newCommand.Size = double.Parse(tempResult[i].Size.ToString());
                        newCommand.StopLoss = tempResult[i].StopLoss;
                        newCommand.Swap = tempResult[i].Swap;
                        if (tempResult[i].Symbol != null)
                        {
                            newCommand.Symbol = tempResult[i].Symbol.Name;
                        }
                        newCommand.TakeProfit = tempResult[i].TakeProfit;
                        newCommand.Time = tempResult[i].OpenTime.ToString();
                        newCommand.TimeExpiry = tempResult[i].ExpTime;
                        newCommand.Profit = Math.Round(tempResult[i].Profit, 2);
                        newCommand.ClientCode = tempResult[i].ClientCode;
                        newCommand.CommandCode = tempResult[i].CommandCode;

                        Result.Add(newCommand);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<TradingServer.ClientBusiness.Command> FacadeGetCommandHistoryWithDateTime(int investorID, DateTime startTime, DateTime endTime)
        {
            List<ClientBusiness.Command> Result = new List<ClientBusiness.Command>();
            List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();

            if (Business.Market.IsConnectMT4)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.ConvertGetCommandHistoryToString(Business.Market.InvestorList[i].Code, startTime, endTime);

                            //string cmdResult = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmd);
                            string cmdResult = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

                            if (!string.IsNullOrEmpty(cmdResult))
                            {
                                //List<BuildCommandElement5ConnectMT4.Business.OnlineTrade> listHistory = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertOnlineTradeToListString(cmdResult);
                                List<BuildCommandElement5ConnectMT4.Business.OnlineTrade> listHistory = BuildCommandElement5ConnectMT4.Mode.ReceiveCommand.Instance.ConvertHistoryToListString(cmdResult);
                                if (listHistory != null)
                                {
                                    int countHistory = listHistory.Count;
                                    for (int j = 0; j < countHistory; j++)
                                    {
                                        Business.OpenTrade newOpenTrade = new Business.OpenTrade();

                                        //newOpenTrade.ClientCode = tbCommandHistory[i].ClientCode;
                                        newOpenTrade.ClosePrice = listHistory[j].ClosePrice;
                                        newOpenTrade.CloseTime = listHistory[j].CloseTime;
                                        newOpenTrade.CommandCode = listHistory[j].CommandCode;
                                        newOpenTrade.Commission = listHistory[j].Commission;
                                        newOpenTrade.ExpTime = listHistory[j].TimeExpire;
                                        newOpenTrade.ID = listHistory[j].CommandID;
                                        //Investor

                                        #region FILL COMMAND TYPE
                                        switch (listHistory[j].CommandType)
                                        {
                                            case "0":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(1);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "1":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(2);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "2":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(7);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "3":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(8);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "4":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(9);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "5":
                                                {
                                                    Business.TradeType resultType = Business.Market.marketInstance.GetTradeType(10);
                                                    newOpenTrade.Type = resultType;
                                                }
                                                break;

                                            case "6":
                                                {
                                                    if (listHistory[j].Profit >= 0)
                                                    {
                                                        Business.TradeType resultType = new Business.TradeType();
                                                        resultType.ID = 13;
                                                        resultType.Name = "Deposit";
                                                        newOpenTrade.Type = resultType;
                                                    }
                                                    else
                                                    {
                                                        Business.TradeType resultType = new Business.TradeType();
                                                        resultType.ID = 14;

                                                        resultType.Name = "Withdraw";
                                                        newOpenTrade.Type = resultType;
                                                    }
                                                }
                                                break;

                                            case "7":
                                                {
                                                    if (listHistory[j].Profit >= 0)
                                                    {
                                                        Business.TradeType resultType = new Business.TradeType();
                                                        resultType.ID = 15;
                                                        resultType.Name = "CreditIn";
                                                        newOpenTrade.Type = resultType;
                                                    }
                                                    else
                                                    {
                                                        Business.TradeType resultType = new Business.TradeType();
                                                        resultType.ID = 16;
                                                        resultType.Name = "CreditOut";
                                                        newOpenTrade.Type = resultType;
                                                    }
                                                }
                                                break;
                                        }
                                        #endregion

                                        #region Find Investor In Investor List
                                        if (Business.Market.InvestorList != null)
                                        {
                                            int countInvestor = Business.Market.InvestorList.Count;
                                            for (int m = 0; m < countInvestor; m++)
                                            {
                                                if (Business.Market.InvestorList[m].Code.ToUpper().Trim() == listHistory[j].InvestorCode)
                                                {
                                                    newOpenTrade.Investor = Business.Market.InvestorList[m];
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion

                                        newOpenTrade.OpenPrice = listHistory[j].OpenPrice;
                                        newOpenTrade.OpenTime = listHistory[j].OpenTime;

                                        if (newOpenTrade.Type.ID == 13 || newOpenTrade.Type.ID == 14 || newOpenTrade.Type.ID == 15 || newOpenTrade.Type.ID == 16)
                                        {
                                            newOpenTrade.CloseTime = newOpenTrade.OpenTime;
                                        }

                                        newOpenTrade.Profit = listHistory[j].Profit;
                                        newOpenTrade.Size = listHistory[j].Size;
                                        newOpenTrade.StopLoss = listHistory[j].StopLoss;
                                        newOpenTrade.Swap = listHistory[j].Swap;
                                        //newOpenTrade.Taxes = listHistory[j].Taxes;
                                        newOpenTrade.Comment = listHistory[j].Comment;
                                        //newOpenTrade.AgentCommission = listHistory[j].AgentCommission;
                                        newOpenTrade.TakeProfit = listHistory[j].TakeProfit;

                                        #region Find Symbol In Symbol List
                                        if (Business.Market.SymbolList != null)
                                        {
                                            bool Flag = false;
                                            int countSymbol = Business.Market.SymbolList.Count;
                                            for (int k = 0; k < countSymbol; k++)
                                            {
                                                if (Flag == true)
                                                    break;

                                                if (Business.Market.SymbolList[k].Name.ToUpper().Trim() == listHistory[j].SymbolName.ToUpper().Trim())
                                                {
                                                    newOpenTrade.Symbol = Business.Market.SymbolList[k];

                                                    Flag = true;
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion

                                        tempResult.Add(newOpenTrade);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
            else
            {
                tempResult = ClientFacade.OpenTradeInstance.GetCommandHistoryWithTime(investorID, startTime, endTime);
            }
            

            if (tempResult != null)
            {
                int count = tempResult.Count;
                for (int i = 0; i < count; i++)
                {
                    if (tempResult[i].Investor == null || tempResult[i].Type == null)
                        continue;

                    ClientBusiness.Command newCommand = new ClientBusiness.Command();
                    newCommand.ClosePrice = tempResult[i].ClosePrice;
                    newCommand.CommandID = tempResult[i].ID;
                    if (tempResult[i].Type != null)
                    {
                        newCommand.CommandType = tempResult[i].Type.Name;
                        newCommand.TypeID = tempResult[i].Type.ID;
                    }
                    newCommand.Commission = tempResult[i].Commission;
                    newCommand.InvestorID = tempResult[i].Investor.InvestorID;

                    bool IsBuy = false;
                    if (tempResult[i].Type != null)
                    {
                        if (tempResult[i].Type.ID == 1 || tempResult[i].Type.ID == 3 || tempResult[i].Type.ID == 5 ||
                            tempResult[i].Type.ID == 7 || tempResult[i].Type.ID == 9 || tempResult[i].Type.ID == 11)
                            IsBuy = true;
                    }

                    newCommand.IsBuy = IsBuy;
                    newCommand.OpenPrice = tempResult[i].OpenPrice.ToString();
                    newCommand.Size = double.Parse(tempResult[i].Size.ToString());
                    newCommand.StopLoss = tempResult[i].StopLoss;
                    newCommand.Swap = tempResult[i].Swap;
                    if (tempResult[i].Symbol != null)
                    {
                        newCommand.Symbol = tempResult[i].Symbol.Name;
                    }
                    newCommand.TakeProfit = tempResult[i].TakeProfit;
                    newCommand.Time = tempResult[i].OpenTime.ToString();
                    newCommand.TimeExpiry = tempResult[i].ExpTime;
                    newCommand.Profit = Math.Round(tempResult[i].Profit, 2);
                    newCommand.ClientCode = tempResult[i].ClientCode;
                    newCommand.CommandCode = tempResult[i].CommandCode;
                    newCommand.CloseTime = tempResult[i].CloseTime;
                    newCommand.Comment = tempResult[i].Comment;

                    Result.Add(newCommand);
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<TradingServer.ClientBusiness.Command> FacadeGetOnlineCommandByInvestor(int InvestorID,int MarketArea)
        {
            List<ClientBusiness.Command> Result = new List<ClientBusiness.Command>();
            List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();

            try
            {
                #region GET ONLINE COMMAND
                if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                {
                    for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                        {
                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                            {
                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                {
                                    if (Business.Market.InvestorList[i].CommandList[j].Type.ID == 1 || Business.Market.InvestorList[i].CommandList[j].Type.ID == 2 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 11 || Business.Market.InvestorList[i].CommandList[j].Type.ID == 12)
                                    {
                                        tempResult.Add(Business.Market.InvestorList[i].CommandList[j]);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
                #endregion                

                #region COMMENT CODE 14/07/2011
                //if (Business.Market.CommandExecutor != null)
                //{                
                //    for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                //    {
                //        if (Business.Market.CommandExecutor[i].Investor.InvestorID == InvestorID)
                //        {
                //            if (MarketArea == 1)
                //            {
                //                if (Business.Market.CommandExecutor[i].Type.ID == 1 || Business.Market.CommandExecutor[i].Type.ID == 2 ||
                //                    Business.Market.CommandExecutor[i].Type.ID == 11 || Business.Market.CommandExecutor[i].Type.ID == 12)
                //                {
                //                    tempResult.Add(Business.Market.CommandExecutor[i]);
                //                }
                //            }
                //            else if (MarketArea == 2)
                //            {
                //                if (Business.Market.CommandExecutor[i].Type.ID == 7 ||
                //                    Business.Market.CommandExecutor[i].Type.ID == 8 ||
                //                    Business.Market.CommandExecutor[i].Type.ID == 9 ||
                //                    Business.Market.CommandExecutor[i].Type.ID == 10)
                //                {
                //                    tempResult.Add(Business.Market.CommandExecutor[i]);
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                #region MAP OBJECT OPEN TRADE TO COMMAND CLIENT
                if (tempResult != null)
                {
                    int count = tempResult.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string CommandType = string.Empty;
                        TradingServer.ClientBusiness.Command newCommand = new ClientBusiness.Command();
                        newCommand.ClosePrice = tempResult[i].ClosePrice;
                        newCommand.CommandID = tempResult[i].ID;

                        #region MAP COMMAND TYPE
                        if (tempResult[i].Type != null)
                        {
                            switch (tempResult[i].Type.ID)
                            {
                                case 1:
                                    CommandType = "Open";
                                    newCommand.IsBuy = true;
                                    break;
                                case 2:
                                    newCommand.IsBuy = false;
                                    CommandType = "Open";
                                    break;
                                case 7:
                                    CommandType = "BuyLimit";
                                    break;
                                case 8:
                                    CommandType = "SellLimit";
                                    break;
                                case 9:
                                    CommandType = "BuyStop";
                                    break;
                                case 10:
                                    CommandType = "SellStop";
                                    break;
                                case 11:
                                    CommandType = "BuyFutureCommand";
                                    newCommand.IsBuy = true;
                                    break;
                                case 12:
                                    CommandType = "SellFutureCommand";
                                    newCommand.IsBuy = false;
                                    break;
                            }
                        }
                        #endregion                        

                        newCommand.CommandType = CommandType;
                        newCommand.Commission = tempResult[i].Commission;
                        newCommand.InvestorID = tempResult[i].Investor.InvestorID;
                        newCommand.OpenPrice = tempResult[i].OpenPrice.ToString();
                        newCommand.Size = double.Parse(tempResult[i].Size.ToString());
                        newCommand.StopLoss = tempResult[i].StopLoss;
                        newCommand.Swap = tempResult[i].Swap;
                        newCommand.Symbol = tempResult[i].Symbol.Name;
                        newCommand.TakeProfit = tempResult[i].TakeProfit;
                        newCommand.Time = tempResult[i].OpenTime.ToString();
                        newCommand.TimeExpiry = tempResult[i].ExpTime;
                        newCommand.ClientCode = tempResult[i].ClientCode;
                        newCommand.CommandCode = tempResult[i].CommandCode;
                        newCommand.TypeID = tempResult[i].Type.ID;
                        newCommand.Margin = tempResult[i].Margin;
                        newCommand.FreezeMargin = tempResult[i].FreezeMargin;
                        newCommand.IsHedged = tempResult[i].Symbol.IsHedged;
                        newCommand.Profit = tempResult[i].Profit;
                        newCommand.Comment = tempResult[i].Comment;

                        Result.Add(newCommand);
                    }
                }
                #endregion            
            }
            catch (Exception ex)
            {

            }           

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static List<TradingServer.ClientBusiness.Command> FacadeGetOnlineCommandByInvestor(string code, int MarketArea)
        {
            List<ClientBusiness.Command> Result = new List<ClientBusiness.Command>();
            List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();

            try
            {
                #region GET ONLINE COMMAND
                if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                {
                    for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                    {
                        if (Business.Market.InvestorList[i].Code.ToUpper().Trim() == code.ToUpper().Trim())
                        {
                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                            {
                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                {
                                    if (Business.Market.InvestorList[i].CommandList[j].Type.ID == 1 || Business.Market.InvestorList[i].CommandList[j].Type.ID == 2 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 11 || Business.Market.InvestorList[i].CommandList[j].Type.ID == 12)
                                    {
                                        tempResult.Add(Business.Market.InvestorList[i].CommandList[j]);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
                #endregion

                #region MAP OBJECT OPEN TRADE TO COMMAND CLIENT
                if (tempResult != null)
                {
                    int count = tempResult.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string CommandType = string.Empty;
                        TradingServer.ClientBusiness.Command newCommand = new ClientBusiness.Command();
                        newCommand.ClosePrice = tempResult[i].ClosePrice;
                        newCommand.CommandID = tempResult[i].ID;

                        #region MAP COMMAND TYPE
                        if (tempResult[i].Type != null)
                        {
                            switch (tempResult[i].Type.ID)
                            {
                                case 1:
                                    CommandType = "Open";
                                    newCommand.IsBuy = true;
                                    break;
                                case 2:
                                    newCommand.IsBuy = false;
                                    CommandType = "Open";
                                    break;
                                case 7:
                                    CommandType = "BuyLimit";
                                    break;
                                case 8:
                                    CommandType = "SellLimit";
                                    break;
                                case 9:
                                    CommandType = "BuyStop";
                                    break;
                                case 10:
                                    CommandType = "SellStop";
                                    break;
                                case 11:
                                    CommandType = "BuyFutureCommand";
                                    newCommand.IsBuy = true;
                                    break;
                                case 12:
                                    CommandType = "SellFutureCommand";
                                    newCommand.IsBuy = false;
                                    break;
                            }
                        }
                        #endregion

                        newCommand.CommandType = CommandType;
                        newCommand.Commission = tempResult[i].Commission;
                        newCommand.InvestorID = tempResult[i].Investor.InvestorID;
                        newCommand.OpenPrice = tempResult[i].OpenPrice.ToString();
                        newCommand.Size = double.Parse(tempResult[i].Size.ToString());
                        newCommand.StopLoss = tempResult[i].StopLoss;
                        newCommand.Swap = tempResult[i].Swap;
                        newCommand.Symbol = tempResult[i].Symbol.Name;
                        newCommand.TakeProfit = tempResult[i].TakeProfit;
                        newCommand.Time = tempResult[i].OpenTime.ToString();
                        newCommand.TimeExpiry = tempResult[i].ExpTime;
                        newCommand.ClientCode = tempResult[i].ClientCode;
                        newCommand.CommandCode = tempResult[i].CommandCode;
                        newCommand.TypeID = tempResult[i].Type.ID;
                        newCommand.Margin = tempResult[i].Margin;
                        newCommand.FreezeMargin = tempResult[i].FreezeMargin;
                        newCommand.IsHedged = tempResult[i].Symbol.IsHedged;
                        newCommand.Profit = tempResult[i].Profit;
                        newCommand.Comment = tempResult[i].Comment;
                        newCommand.Digit = tempResult[i].Symbol.Digit;

                        Result.Add(newCommand);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {

            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        public static List<TradingServer.ClientBusiness.Command> FacadeGetPendingOrderByInvestorID(int investorID)
        {
            List<ClientBusiness.Command> Result = new List<ClientBusiness.Command>();
            List<Business.OpenTrade> tempResult = new List<Business.OpenTrade>();

            try
            {
                #region GET PENDING ORDER OF INVESTOR
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            if (Business.Market.InvestorList[i].CommandList != null)
                            {
                                int countCommand = Business.Market.InvestorList[i].CommandList.Count;
                                for (int j = 0; j < countCommand; j++)
                                {
                                    if (Business.Market.InvestorList[i].CommandList[j].Type.ID == 7 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 8 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 9 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 10 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 17 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 18 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 19 ||
                                        Business.Market.InvestorList[i].CommandList[j].Type.ID == 20)
                                    {
                                        tempResult.Add(Business.Market.InvestorList[i].CommandList[j]);

                                        //update time last connect
                                        Business.Market.InvestorList[i].UpdateLastConnect(investorID, Business.Market.InvestorList[i].LoginKey);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
                #endregion

                #region MAP OBJECT OPEN TRADE TO COMMAND CLIENT
                if (tempResult != null)
                {
                    int count = tempResult.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string CommandType = string.Empty;
                        TradingServer.ClientBusiness.Command newCommand = new ClientBusiness.Command();
                        newCommand.ClosePrice = tempResult[i].ClosePrice;
                        newCommand.CommandID = tempResult[i].ID;

                        if (tempResult[i].Type != null)
                        {
                            switch (tempResult[i].Type.ID)
                            {
                                case 1:
                                    CommandType = "Open";
                                    newCommand.IsBuy = true;
                                    break;
                                case 2:
                                    newCommand.IsBuy = false;
                                    CommandType = "Open";
                                    break;
                                case 7:
                                    CommandType = "BuyLimit";
                                    break;
                                case 8:
                                    CommandType = "SellLimit";
                                    break;
                                case 9:
                                    CommandType = "BuyStop";
                                    break;
                                case 10:
                                    CommandType = "SellStop";
                                    break;
                                case 11:
                                    CommandType = "BuyFutureCommand";
                                    newCommand.IsBuy = true;
                                    break;
                                case 12:
                                    CommandType = "SellFutureCommand";
                                    newCommand.IsBuy = false;
                                    break;
                                case 17:
                                    CommandType = "BuyStop";
                                    break;
                                case 18:
                                    CommandType = "SellStop";
                                    break;
                                case 19:
                                    CommandType = "BuyLimit";
                                    break;
                                case 20:
                                    CommandType = "SellLimit";
                                    break;
                            }
                        }

                        newCommand.CommandType = CommandType;
                        newCommand.Commission = tempResult[i].Commission;
                        newCommand.InvestorID = tempResult[i].Investor.InvestorID;
                        newCommand.OpenPrice = tempResult[i].OpenPrice.ToString();
                        newCommand.Size = double.Parse(tempResult[i].Size.ToString());
                        newCommand.StopLoss = tempResult[i].StopLoss;
                        newCommand.Swap = tempResult[i].Swap;
                        newCommand.Symbol = tempResult[i].Symbol.Name;
                        newCommand.TakeProfit = tempResult[i].TakeProfit;
                        newCommand.Time = tempResult[i].OpenTime.ToString();
                        newCommand.TimeExpiry = tempResult[i].ExpTime;
                        newCommand.ClientCode = tempResult[i].ClientCode;
                        newCommand.CommandCode = tempResult[i].CommandCode;
                        newCommand.TypeID = tempResult[i].Type.ID;
                        newCommand.Margin = tempResult[i].Margin;
                        newCommand.FreezeMargin = tempResult[i].FreezeMargin;
                        newCommand.IsHedged = tempResult[i].Symbol.IsHedged;
                        newCommand.Profit = tempResult[i].Profit;
                        newCommand.Comment = tempResult[i].Comment;

                        Result.Add(newCommand);
                    }
                }
                #endregion            
            }
            catch (Exception ex)
            {

            }            

            return Result;
        }

        /// <summary>
        /// UPDATE ONLINE COMMAND
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static TradingServer.ClientBusiness.DealMessage FacadeUpdateOnlineCommand(TradingServer.ClientBusiness.Command objCommand)
        {
            TradingServer.ClientBusiness.DealMessage Result = new ClientBusiness.DealMessage();
            Result.isDeal = false;

            Business.OpenTrade Command = new Business.OpenTrade();            

            //Find In Command List Of Symbol 
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                bool flag = false;
                for (int i = 0; i < count; i++)
                {
                    if (flag == false)
                    {
                        if (Business.Market.SymbolList[i].CommandList != null)
                        {
                            int countCommand = Business.Market.SymbolList[i].CommandList.Count;
                            for (int j = 0; j < countCommand; j++)
                            {
                                if (Business.Market.SymbolList[i].CommandList[j].ID == objCommand.CommandID)
                                {
                                    #region SET VALUE FOR COMMAND
                                    Command.Symbol = Business.Market.SymbolList[i];
                                    Command.Investor = Business.Market.SymbolList[i].CommandList[j].Investor;
                                    Command.Type = Business.Market.SymbolList[i].CommandList[j].Type;
                                    Command.Size = Business.Market.SymbolList[i].CommandList[j].Size;
                                    Command.ClosePrice = Business.Market.SymbolList[i].CommandList[j].ClosePrice;

                                    bool isPending = TradingServer.Model.TradingCalculate.Instance.CheckIsPendingPosition(Business.Market.SymbolList[i].CommandList[j].Type.ID);
                                    double tempOpenPrice = 0;
                                    if (isPending)
                                        tempOpenPrice = double.Parse(objCommand.OpenPrice);
                                    else
                                        tempOpenPrice = Business.Market.SymbolList[i].CommandList[j].OpenPrice;

                                    Command.OpenPrice = tempOpenPrice;
                                    Command.CommandCode = Business.Market.SymbolList[i].CommandList[j].CommandCode;
                                    Command.IGroupSecurity = Business.Market.SymbolList[i].CommandList[j].IGroupSecurity;
                                    #endregion     

                                    #region INSERT SYSTEM LOG BEFORE MODIFY COMMAND
                                    string content = string.Empty;
                                    string comment = "[modify order]";
                                    string mode = TradingServer.Facade.FacadeGetTypeNameByTypeID(Command.Type.ID).ToLower();

                                    string size = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Command.Size.ToString(), 2);
                                    string openPrice = string.Empty;
                                    string afterOpenPrice = string.Empty;
                                    string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.StopLoss.ToString(),
                                        Command.Symbol.Digit);
                                    string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.TakeProfit.ToString(),
                                        Command.Symbol.Digit);
                                    string bid = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Bid.ToString(),
                                        Business.Market.SymbolList[i].CommandList[j].Symbol.Digit);
                                    string ask = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].Symbol.TickValue.Ask.ToString(),
                                        Business.Market.SymbolList[i].CommandList[j].Symbol.Digit);

                                    openPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].OpenPrice.ToString(),
                                    Business.Market.SymbolList[i].Digit);

                                    if (objCommand.OpenPrice != null && !string.IsNullOrEmpty(objCommand.OpenPrice))
                                    {
                                        afterOpenPrice = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.OpenPrice.ToString(), Business.Market.SymbolList[i].Digit);
                                    }
                                    else
                                    {
                                        afterOpenPrice = openPrice;
                                    }

                                    string firstStopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].StopLoss.ToString(), Business.Market.SymbolList[i].Digit);
                                    string firstTakeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(Business.Market.SymbolList[i].CommandList[j].TakeProfit.ToString(), Business.Market.SymbolList[i].Digit);
                                    content = "'" + Command.Investor.Code + "': modify order #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " + openPrice +
                                                " sl: " + firstStopLoss + " tp: " + firstTakeProfit + " -> open price " + afterOpenPrice + " sl: " + stopLoss + " tp: " + takeProfit + " (" + bid + "/" + ask + ")";

                                    TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, objCommand.IpAddress, Command.Investor.Code);
                                    #endregion  

                                    bool isRequest = false;
                                    if (Business.Market.marketInstance.MQLCommands != null)
                                    {
                                        int countMQL = Business.Market.marketInstance.MQLCommands.Count;
                                        for (int n = 0; n < countMQL; n++)
                                        {
                                            if (Business.Market.marketInstance.MQLCommands[n].ClientCode == objCommand.ClientCode)
                                            {
                                                isRequest = true;
                                                break;
                                            }
                                        }

                                        if (!isRequest)
                                        {
                                            NJ4XConnectSocket.MQLCommand newMQLCommand = new NJ4XConnectSocket.MQLCommand();
                                            newMQLCommand.ClientCode = objCommand.ClientCode;
                                            newMQLCommand.InvestorCode = Command.Investor.Code;
                                            newMQLCommand.IpAddress = objCommand.IpAddress;

                                            Business.Market.marketInstance.MQLCommands.Add(newMQLCommand);
                                        }
                                    }

                                    #region CHECK INVESTOR ONLINE(CHECK ACCOUNT READ ONLY)
                                    bool _checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                                                                                                                        Business.TypeLogin.Primary, objCommand.LoginKey);
                                    if (!_checkOnline)
                                    {
                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "ACCOUNT READ ONLY";

                                        string Message = "UpdateCommand$False,ACCOUNT READ ONLY," + Command.ID + "," +
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

                                        #region INSERT SYSTEM LOG CHECK ACCOUNT INVESTOR TRADE IS DISABLED
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account read only] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion
                               
                                    #region CHECK IF MANUAL ONLY THEN SEND TO MT4 CLIENT(MQL OR NJ4X)
                                    string executionType = Business.Market.marketInstance.GetExecutionType(Command.IGroupSecurity, "B03");

                                    if (executionType == "manual only- no automation" ||
                                        executionType == "manual- but automatic if no dealer online" ||
                                        executionType == "automatic only")
                                    {
                                        Command.StopLoss = objCommand.StopLoss;
                                        Command.TakeProfit = objCommand.TakeProfit;
                                        Command.ID = objCommand.CommandID;
                                        Command.Comment = objCommand.Comment;

                                        Command.Symbol.MarketAreaRef.UpdateCommand(Command);

                                        Result.isDeal = true;
                                        Result.Command = objCommand;

                                        return Result;
                                    }
                                    #endregion

                                    #region CHECK SYMBOL ON HOLD
                                    if (!Business.Market.SymbolList[i].CheckTimeTick())
                                    {
                                        #region MAP COMMAND TO CLIENT
                                        if (Business.Market.SymbolList[i].CommandList[j].Investor != null)
                                        {
                                            string message = "UpdateCommand$False,MARKET ON HOLD," + Business.Market.SymbolList[i].CommandList[j].ID + "," +
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
                                            Business.Market.SymbolList[i].CommandList[j].ID + "," + "BuyFutureCommand" + "," + 1 + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ExpTime + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ClientCode + "," + "0000000," +
                                            Business.Market.SymbolList[i].CommandList[j].IsHedged + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Margin + ",Open";

                                            if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);
                                        }
                                        #endregion

                                        Result.isDeal = false;
                                        Result.Error = "MARKET ON HOLD";
                                        Result.Command = objCommand;

                                        #region INSERT SYSTEM LOG TIME SESSION OF SYMBOL FUTURE
                                        string tempContent = "update online command";
                                        tempContent += " unsucessful [symbol on hold]";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, "[symbol on hold]", Business.Market.SymbolList[i].CommandList[j].Investor.IpAddress,
                                            Business.Market.SymbolList[i].CommandList[j].Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion

                                    #region IF SYMBOL IS FUTURE THEN CHECK TIME SESSION OF FUTURE
                                    if (Business.Market.SymbolList[i].MarketAreaRef.IMarketAreaName == "FutureCommand")
                                    {
                                        if (Business.Market.SymbolList[i].isCloseOnlyFuture)
                                        { 
                                            string message = "UpdateCommand$False,SYMBOL CLOSE ONLY," + Business.Market.SymbolList[i].CommandList[j].ID + "," + 
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
                                            Business.Market.SymbolList[i].CommandList[j].ID + "," + "BuyFutureCommand" + "," + 1 + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ExpTime + "," + 
                                            Business.Market.SymbolList[i].CommandList[j].ClientCode + "," + "0000000," + 
                                            Business.Market.SymbolList[i].CommandList[j].IsHedged + "," + 
                                            Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Margin + ",BuyFutureCommand";

                                            if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(message);

                                            Result.isDeal = false;
                                            Result.Error = "SYMBOL CLOSE ONLY";
                                            Result.Command = objCommand;

                                            #region INSERT SYSTEM LOG TIME SESSION OF SYMBOL FUTURE
                                            string tempContent = string.Empty;
                                            tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                                openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [symbol is close] (" + bid + "/" + ask + ")";

                                            TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                            #endregion

                                            return Result;
                                        }
                                    }
                                    #endregion

                                    #region CHECK IP ADDRESS
                                    bool checkIP = TradingServer.Business.ValidIPAddress.Instance.ValidIpAddress(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID, 
                                                                                                                    objCommand.IpAddress);
                                    if (!checkIP)
                                    {
                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "INVALID IP ADDRESS";

                                        string Message = "UpdateCommand$False,INVALID IP ADDRESS," + Command.ID + "," +
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

                                        #region INSERT SYSTEM LOG CHECK ACCOUNT INVESTOR TRADE IS DISABLED
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid ip address] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion

                                    bool isOnline = TradingServer.Business.Investor.investorInstance.CheckInvestorOnline(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                        Business.Market.SymbolList[i].CommandList[j].Investor.LoginKey);
                                    if (!isOnline)
                                    {
                                        //ADD CLIENT TO LIST ONLINE IF CLIENT TIMEOUT
                                        TradingServer.Business.Investor.investorInstance.CheckOnlineInvestor(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID,
                                            Business.Market.SymbolList[i].CommandList[j].Investor.LoginKey);
                                    }

                                    #region CHECK INVESTOR ONLINE(CHECK ACCOUNT READ ONLY)
                                    bool checkOnline = TradingServer.Business.Investor.investorInstance.CheckPrimaryInvestorOnline(Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID, 
                                                                                                                                        Business.TypeLogin.Primary, objCommand.LoginKey);
                                    if (!checkOnline)
                                    {
                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "ACCOUNT READ ONLY";

                                        string Message = "UpdateCommand$False,ACCOUNT READ ONLY," + Command.ID + "," +
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

                                        #region INSERT SYSTEM LOG CHECK ACCOUNT INVESTOR TRADE IS DISABLED
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account read only] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion

                                    #region CHECK ACCOUNT INVESTOR LOGIN WITH READ ONLY PASSWORD
                                    if (Business.Market.SymbolList[i].CommandList[j].Investor.IsReadOnly)
                                    {
                                        string Message = "UpdateCommand$False,TRADE IS DISABLED," + Command.ID + "," +
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

                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "TRADE IS DISABLED";

                                        #region INSERT SYSTEM LOG CHECK ACCOUNT INVESTOR TRADE IS DISABLED
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [trade is disabled] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion

                                    #region CHECK TIME MARKET
                                    if (!Business.Market.IsOpen)
                                    {
                                        string Message = "UpdateCommand$False,MARKET IS CLOSE," + Business.Market.SymbolList[i].CommandList[j].ID + "," + 
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
                                            Business.Market.SymbolList[i].CommandList[j].ID + "," + "Open" + "," + 1 + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ExpTime + "," + 
                                            Business.Market.SymbolList[i].CommandList[j].ClientCode + "," + "0000000," + 
                                            Business.Market.SymbolList[i].CommandList[j].IsHedged + "," + 
                                            Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Margin + ",Open";

                                        if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                        Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);

                                        Result.isDeal = false;
                                        Result.Error = "MARKET IS CLOSE";
                                        Result.Command = objCommand;

                                        #region INSERT SYSTEM LOG CHECK TIME MARKET
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [market is close] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion

                                    #region CHECK HOLIDAY OF SERVER
                                    if (Business.Market.SymbolList[i].IsHoliday)
                                    {
                                        string Message = "UpdateCommand$False,MARKET IS HOLIDAY," + Business.Market.SymbolList[i].CommandList[j].ID + "," +
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
                                            Business.Market.SymbolList[i].CommandList[j].ID + "," + "BuySpotCommand" + "," + 1 + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ExpTime + "," +
                                            Business.Market.SymbolList[i].CommandList[j].ClientCode + "," + "0000000," +
                                            Business.Market.SymbolList[i].CommandList[j].IsHedged + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Type.ID + "," +
                                            Business.Market.SymbolList[i].CommandList[j].Margin + ",Open";

                                        if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                            Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                        Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);

                                        Result.isDeal = false;
                                        Result.Error = "MARKET IS HOLIDAY";
                                        Result.Command = objCommand;

                                        #region INSERT SYSTEM LOG CHECK HOLIDAY SERVER
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [market is holiday] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion           

                                    if (Business.Market.SymbolList[i].CommandList[j].Investor.IsDisable)
                                    {
                                        #region CHECK ISDISABLE OF INVESTOR
                                        string Message = "UpdateCommand$False,ACCOUNT IS DISABLED," + Command.ID + "," +
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

                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "ACCOUNT IS DISABLED";

                                        #region INSERT SYSTEM LOG CHECK STATUS OF ACCOUNT(DISABLE)
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account is disabled] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                        #endregion                                        
                                    }

                                    #region CHECK READ ONLY OF INVESTOR
                                    if (Business.Market.SymbolList[i].CommandList[j].Investor.ReadOnly)
                                    {
                                        string Message = "UpdateCommand$False,TRADE IS DISABLED," + Command.ID + "," +
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

                                        Result.isDeal = false;
                                        Result.Command = objCommand;
                                        Result.Error = "TRADE IS DISABLED";

                                        #region INSERT SYSTEM LOG CHECK ACCOUNT READ ONLY
                                        string tempContent = string.Empty;
                                        tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                            openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account read only] (" + bid + "/" + ask + ")";

                                        TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                        #endregion

                                        return Result;
                                    }
                                    #endregion                                    

                                    #region CHECK OPEN PRICE OF PENDING ORDER
                                    if (Business.Market.SymbolList[i].CommandList[j].Type.ID == 7 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 8 ||
                                        Business.Market.SymbolList[i].CommandList[j].Type.ID == 9 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 10 ||
                                        Business.Market.SymbolList[i].CommandList[j].Type.ID == 17 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 18 ||
                                        Business.Market.SymbolList[i].CommandList[j].Type.ID == 19 || Business.Market.SymbolList[i].CommandList[j].Type.ID == 20)
                                    {
                                        bool ResultCheckOpenPrice = false;
                                        double tempOpenPrices = 0;
                                        bool parseOpenPrice = double.TryParse(objCommand.OpenPrice, out tempOpenPrices);
                                        if (Business.Market.SymbolList[i].CommandList[j].OpenPrice != tempOpenPrices)
                                        {
                                            if (parseOpenPrice)
                                            {
                                                ResultCheckOpenPrice = Business.Market.SymbolList[i].CommandList[j].Symbol.CheckOpenPricePendingOrder(Business.Market.SymbolList[i].CommandList[j].Symbol.Name,
                                                    Business.Market.SymbolList[i].CommandList[j].Type.ID, tempOpenPrices, Business.Market.SymbolList[i].CommandList[j].Symbol.LimitLevel,
                                                    Business.Market.SymbolList[i].CommandList[j].Symbol.StopLevel,
                                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Digit,
                                                    int.Parse(Business.Market.SymbolList[i].CommandList[j].SpreaDifferenceInOpenTrade.ToString()));
                                            }

                                            #region CONVERT TYPE.ID TO COMMAND TYPE
                                            string CommandType = string.Empty;
                                            switch (Business.Market.SymbolList[i].CommandList[j].Type.ID)
                                            {
                                                case 1:
                                                    CommandType = "Open";
                                                    break;
                                                case 2:
                                                    CommandType = "Open";
                                                    break;
                                                case 7:
                                                    CommandType = "BuyLimit";
                                                    break;
                                                case 8:
                                                    CommandType = "SellLimit";
                                                    break;
                                                case 9:
                                                    CommandType = "BuyStop";
                                                    break;
                                                case 10:
                                                    CommandType = "SellStop";
                                                    break;
                                                case 11:
                                                    CommandType = "BuyFuture";
                                                    break;
                                                case 12:
                                                    CommandType = "SellFuture";
                                                    break;
                                                case 17:
                                                    CommandType = "BuyStopFutureCommand";
                                                    break;
                                                case 18:
                                                    CommandType = "SellStopFutureCommand";
                                                    break;
                                                case 19:
                                                    CommandType = "BuyLimitFutureCommand";
                                                    break;
                                                case 20:
                                                    CommandType = "SellLimitFutureCommand";
                                                    break;
                                            }
                                            #endregion

                                            if (ResultCheckOpenPrice == false)
                                            {
                                                #region MAP COMMAND TO CLIENT
                                                string Message = "UpdateCommand$False,INVALID OPEN PRICE," + Business.Market.SymbolList[i].CommandList[j].ID + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].Symbol.Name + "," + Business.Market.SymbolList[i].CommandList[j].Size + "," +
                                                    false + "," + Business.Market.SymbolList[i].CommandList[j].OpenTime + "," + Business.Market.SymbolList[i].CommandList[j].OpenPrice + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].StopLoss + "," + Business.Market.SymbolList[i].CommandList[j].TakeProfit + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].ClosePrice + "," + Business.Market.SymbolList[i].CommandList[j].Commission + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].Swap + "," + Business.Market.SymbolList[i].CommandList[j].Profit + "," + "Comment," +
                                                    Business.Market.SymbolList[i].CommandList[j].ID + "," + CommandType + "," + 1 + "," + Business.Market.SymbolList[i].CommandList[j].ExpTime + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].ClientCode + "," + "0000000," + Business.Market.SymbolList[i].CommandList[j].IsHedged + "," +
                                                    Business.Market.SymbolList[i].CommandList[j].Type.ID + "," + Business.Market.SymbolList[i].CommandList[j].Margin + ",Open";

                                                if (Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue == null)
                                                    Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue = new List<string>();

                                                Business.Market.SymbolList[i].CommandList[j].Investor.ClientCommandQueue.Add(Message);

                                                Result.Error = "INVALID OPEN PRICE";
                                                Result.Command = objCommand;
                                                Result.isDeal = false;

                                                #region INSERT SYSTEM LOG CHECK OPEN PRICE PENDING ORDER FALSE
                                                string tempContent = string.Empty;
                                                tempContent = "'" + Command.Investor.Code + "': modified #" + Command.CommandCode + " " + mode + " " + size + " " + Command.Symbol.Name + " at " +
                                                    openPrice + " sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid open price] (" + bid + "/" + ask + ")";

                                                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                                                #endregion
                                                #endregion

                                                return Result;
                                            }   //END IF CHECK S/L AND T/P OF COMMAND
                                        }
                                        
                                    }   //END IF CHECK COMMAND IS PENDING ORDER
                                    #endregion
                                    
                                    flag = true;

                                    //SET CLIENT CODE
                                    Command.ClientCode = Business.Market.SymbolList[i].CommandList[j].ClientCode;
                                    Command.RefCommandID = Business.Market.SymbolList[i].CommandList[j].RefCommandID;

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Command.ID = objCommand.CommandID;
            Command.TakeProfit = objCommand.TakeProfit;
            Command.StopLoss = objCommand.StopLoss;
            Command.Comment = objCommand.Comment;            

            //CHECK IF COMMAND IS PENDING ORDER AND OPEN PRICES != 0 THEN UPDATE COMMAND THIS     
            if (Command.Type != null)
            {
                if (Command.Type.ID == 7 || Command.Type.ID == 8 || Command.Type.ID == 9 || Command.Type.ID == 10 ||
                    Command.Type.ID == 17 || Command.Type.ID == 18 || Command.Type.ID == 19 || Command.Type.ID == 20)
                {
                    double tempOpenPrice = 0;
                    double.TryParse(objCommand.OpenPrice, out tempOpenPrice);
                    if (tempOpenPrice != 0)
                    {
                        Command.OpenPrice = tempOpenPrice;
                    }
                }
            }
           
            Command.Investor.IpAddress = objCommand.IpAddress;

            if (Command != null)
            {
                if (Command.Symbol != null && Command.Investor != null && Command.Type != null)
                {
                    Command.Symbol.MarketAreaRef.UpdateCommand(Command);
                }
            }

            Result.isDeal = true;
            Result.Command = objCommand;

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCommand"></param>
        /// <returns></returns>
        public static ClientBusiness.DealMessage FacadeMultiUpdateCommand(TradingServer.ClientBusiness.Command objCommand)
        {
            ClientBusiness.DealMessage Result = new ClientBusiness.DealMessage();

            Result.isDeal = false;

            Business.OpenTrade Command = new Business.OpenTrade();

            FillInstanceOpenTrade(objCommand, Command);

            if (Command == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "some info empty(command)", "[multiple update command]", "", "");
                return Result;
            }

            if (Command.Type == null || Command.Investor == null || Command.IGroupSecurity == null || Command.Symbol == null)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "some info empty(type,investor,igroupsecurity,symbol)", "[multiple update command]", "", "");
                return Result;
            }

            #region INSERT SYSTEM LOG BEFORE MODIFY COMMAND
            string content = string.Empty;
            string comment = "[multi modify order]";           

            string stopLoss = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.StopLoss.ToString(),
                Command.Symbol.Digit);
            string takeProfit = TradingServer.Model.TradingCalculate.Instance.BuildStringWithDigit(objCommand.TakeProfit.ToString(),
                Command.Symbol.Digit);            
                        
            content = "'" + Command.Investor.Code + "': multiple modify by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit;

            TradingServer.Facade.FacadeAddNewSystemLog(5, content, comment, Command.Investor.IpAddress, Command.Investor.Code);
            #endregion  

            Command.StopLoss = objCommand.StopLoss;
            Command.TakeProfit = objCommand.TakeProfit;

            bool ResultTakeProfit = false;
            if (Command.StopLoss != 0 || Command.TakeProfit != 0)
            {
                #region Call Check FreezeLevel
                ResultTakeProfit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                                            Command.Symbol.FreezeLevel, Command.Symbol.Digit, int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));

                if (ResultTakeProfit == false)
                {
                    bool IsBuy = false;                    

                    string message = "UpdateCommand$False,INVALID FREEZE LEVEL," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + 
                                        "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + 
                                        "," + Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + 
                                        Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Update";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(message);

                    #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                    string tempContent = string.Empty;
                    tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid freeze level]";

                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    #endregion

                    Result.isDeal = false;
                    Result.Command = objCommand;
                    Result.Error = "INVALID FREEZE LEVEL";

                    return Result;
                }
                #endregion

                #region Check Valid Take Profit And Stop Loss
                ResultTakeProfit = Command.Symbol.CheckLimitAndStop(Command.Symbol.Name, Command.Type.ID, Command.StopLoss, Command.TakeProfit,
                    Command.Symbol.StopLossTakeProfitLevel, Command.Symbol.Digit, int.Parse(Command.SpreaDifferenceInOpenTrade.ToString()));

                if (ResultTakeProfit == false)
                {
                    bool IsBuy = false;
                    
                    string message = "UpdateCommand$False,INVALID S/L OR T/P," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                        Command.Size + "," + IsBuy + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + "," +
                                        1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Update";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(message);

                    #region INSERT SYSTEM LOG AFTER MODIFY COMMAND
                    string tempContent = string.Empty;
                    tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol" + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [invalid s/l or t/p]";

                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    #endregion

                    Result.isDeal = false;
                    Result.Error = "INVALID FREEZE LEVEL";
                    Result.Command = objCommand;

                    return Result;
                }
                #endregion
            }  

            #region IF SYMBOL IS FUTURE THEN CHECK TIME SESSION OF FUTURE
            if (Command.Symbol.MarketAreaRef.IMarketAreaName == "FutureCommand")
            {
                if (Command.Symbol.isCloseOnlyFuture)
                {
                    string message = "UpdateCommand$False,SYMBOL CLOSE ONLY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuyFutureCommand" + 
                    "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",BuyFutureCommand";

                    if (Command.Investor.ClientCommandQueue == null)
                        Command.Investor.ClientCommandQueue = new List<string>();

                    Command.Investor.ClientCommandQueue.Add(message);

                    Result.isDeal = false;
                    Result.Error = "SYMBOL CLOSE ONLY";
                    Result.Command = objCommand;

                    #region INSERT SYSTEM LOG TIME SESSION OF SYMBOL FUTURE
                    string tempContent = string.Empty;
                    tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [symbol is close]";

                    TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                    #endregion

                    return Result;
                }
            }
            #endregion

            #region CHECK ACCOUNT INVESTOR LOGIN WITH READ ONLY PASSWORD
            if (Command.Investor.IsReadOnly)
            {
                string Message = "UpdateCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                        Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                        Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                        Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," +
                        Command.Type.ID + "," + Command.Margin + ",Update";

                //If Client Update Command Then Add Message To Client Message
                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "TRADE IS DISABLED";

                #region INSERT SYSTEM LOG CHECK ACCOUNT INVESTOR TRADE IS DISABLED
                string tempContent = string.Empty;
                tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [trade is disabled]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK TIME MARKET
            if (!Business.Market.IsOpen)
            {
                string Message = "UpdateCommand$False,MARKET IS CLOSE," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "Open" + "," + 1 + 
                    "," + Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS CLOSE";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG CHECK TIME MARKET
                string tempContent = string.Empty;
                tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [market is close]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion

            #region CHECK HOLIDAY OF SERVER
            if (Command.Symbol.IsHoliday)
            {
                string Message = "UpdateCommand$False,MARKET IS HOLIDAY," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," + Command.Size + 
                    "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," + Command.ClosePrice + "," +
                    Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + "BuySpotCommand" + "," + 1 + "," + 
                    Command.ExpTime + "," + Command.ClientCode + "," + "0000000," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";

                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Error = "MARKET IS HOLIDAY";
                Result.Command = objCommand;

                #region INSERT SYSTEM LOG CHECK HOLIDAY SERVER
                string tempContent = string.Empty;
                tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [market is holiday]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion           

            if (Command.Investor.IsDisable)
            {
                #region CHECK ISDISABLE OF INVESTOR
                string Message = "UpdateCommand$False,ACCOUNT IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," + Command.Type.Name + 
                    "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," +
                    Command.Margin + ",Update";

                //If Client Update Command Then Add Message To Client Message
                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "ACCOUNT IS DISABLED";

                #region INSERT SYSTEM LOG CHECK STATUS OF ACCOUNT(DISABLE)
                string tempContent = string.Empty;
                tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account is disabled]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
                #endregion
            }

            #region CHECK READ ONLY OF INVESTOR
            if (Command.Investor.ReadOnly)
            {
                string Message = "UpdateCommand$False,TRADE IS DISABLED," + Command.ID + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                    Command.Size + "," + false + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                    Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + "Comment," + Command.ID + "," +
                    Command.Type.Name + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," +
                    Command.Type.ID + "," + Command.Margin + ",Update";

                //If Client Update Command Then Add Message To Client Message
                if (Command.Investor.ClientCommandQueue == null)
                    Command.Investor.ClientCommandQueue = new List<string>();

                Command.Investor.ClientCommandQueue.Add(Message);

                Result.isDeal = false;
                Result.Command = objCommand;
                Result.Error = "TRADE IS DISABLED";

                #region INSERT SYSTEM LOG CHECK ACCOUNT READ ONLY
                string tempContent = string.Empty;
                tempContent = "'" + Command.Investor.Code + "': multiple modified by for symbol " + Command.Symbol.Name + " -> sl: " + stopLoss + " tp: " + takeProfit + " unsuccesful [account read only]";

                TradingServer.Facade.FacadeAddNewSystemLog(5, tempContent, comment, Command.Investor.IpAddress, Command.Investor.Code);
                #endregion

                return Result;
            }
            #endregion                                    

            Command.ID = objCommand.CommandID;
            Command.Comment = objCommand.Comment;
            Command.Investor.IpAddress = objCommand.IpAddress;
            
            if (Command != null)
            {
                if (Command.Symbol != null && Command.Investor != null && Command.Type != null)
                {
                    Command.Symbol.MarketAreaRef.MultiUpdateCommand(Command);
                }
            }

            Result.isDeal = true;
            Result.Command = objCommand;

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static Business.Investor FacadeGetInvestorByInvestor(int InvestorID)
        {
            Business.Investor Result = new Business.Investor();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].InvestorID == InvestorID)
                    {
                        #region Map Investor Account
                        Result.Address = Business.Market.InvestorList[i].Address;
                        Result.AgentID = Business.Market.InvestorList[i].AgentID;
                        Result.Balance = Business.Market.InvestorList[i].Balance;
                        Result.City = Business.Market.InvestorList[i].City;
                        Result.Code = Business.Market.InvestorList[i].Code;
                        Result.Comment = Business.Market.InvestorList[i].Comment;
                        Result.InvestorComment = Business.Market.InvestorList[i].InvestorComment;
                        Result.Country = Business.Market.InvestorList[i].Country;
                        Result.Credit = Business.Market.InvestorList[i].Credit;
                        Result.Email = Business.Market.InvestorList[i].Email;
                        Result.Equity = Business.Market.InvestorList[i].Equity;
                        Result.FirstName = Business.Market.InvestorList[i].FirstName;
                        Result.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                        Result.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                        Result.InvestorID = Business.Market.InvestorList[i].InvestorID;
                        Result.InvestorStatusID = Business.Market.InvestorList[i].InvestorStatusID;
                        Result.IsCalculating = Business.Market.InvestorList[i].IsCalculating;
                        Result.IsDisable = Business.Market.InvestorList[i].IsDisable;
                        Result.IsOnline = Business.Market.InvestorList[i].IsOnline;
                        Result.LastConnectTime = Business.Market.InvestorList[i].LastConnectTime;
                        Result.Leverage = Business.Market.InvestorList[i].Leverage;
                        Result.Margin = Business.Market.InvestorList[i].Margin;
                        Result.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                        Result.NickName = Business.Market.InvestorList[i].NickName;
                        Result.Phone = Business.Market.InvestorList[i].Phone;
                        Result.Profit = Business.Market.InvestorList[i].Profit;
                        Result.SecondAddress = Business.Market.InvestorList[i].SecondAddress;
                        Result.State = Business.Market.InvestorList[i].State;
                        Result.StatusCode = Business.Market.InvestorList[i].StatusCode;
                        Result.TaxRate = Business.Market.InvestorList[i].TaxRate;
                        Result.FreezeMargin = Business.Market.InvestorList[i].FreezeMargin;
                        Result.ReadOnly = Business.Market.InvestorList[i].ReadOnly;
                        Result.AllowChangePwd = Business.Market.InvestorList[i].AllowChangePwd;
                        Result.IDPassport = Business.Market.InvestorList[i].IDPassport;
                        #endregion                        

                        //update time last connect
                        Business.Investor.investorInstance.UpdateLastConnect(InvestorID, Business.Market.InvestorList[i].LoginKey);
                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static Business.Investor FacadeGetInvestorByCode(string code)
        {
            Business.Investor Result = new Business.Investor();
            if (Business.Market.InvestorList != null)
            {
                int count = Business.Market.InvestorList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorList[i].Code == code)
                    {
                        #region Map Investor Account
                        Result.Address = Business.Market.InvestorList[i].Address;
                        Result.AgentID = Business.Market.InvestorList[i].AgentID;
                        Result.Balance = Business.Market.InvestorList[i].Balance;
                        Result.City = Business.Market.InvestorList[i].City;
                        Result.Code = Business.Market.InvestorList[i].Code;
                        Result.Comment = Business.Market.InvestorList[i].Comment;
                        Result.InvestorComment = Business.Market.InvestorList[i].InvestorComment;
                        Result.Country = Business.Market.InvestorList[i].Country;
                        Result.Credit = Business.Market.InvestorList[i].Credit;
                        Result.Email = Business.Market.InvestorList[i].Email;
                        Result.Equity = Business.Market.InvestorList[i].Equity;
                        Result.FirstName = Business.Market.InvestorList[i].FirstName;
                        Result.FreeMargin = Business.Market.InvestorList[i].FreeMargin;
                        Result.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;
                        Result.InvestorID = Business.Market.InvestorList[i].InvestorID;
                        Result.InvestorStatusID = Business.Market.InvestorList[i].InvestorStatusID;
                        Result.IsCalculating = Business.Market.InvestorList[i].IsCalculating;
                        Result.IsDisable = Business.Market.InvestorList[i].IsDisable;
                        Result.IsOnline = Business.Market.InvestorList[i].IsOnline;
                        Result.LastConnectTime = Business.Market.InvestorList[i].LastConnectTime;
                        Result.Leverage = Business.Market.InvestorList[i].Leverage;
                        Result.Margin = Business.Market.InvestorList[i].Margin;
                        Result.MarginLevel = Business.Market.InvestorList[i].MarginLevel;
                        Result.NickName = Business.Market.InvestorList[i].NickName;
                        Result.Phone = Business.Market.InvestorList[i].Phone;
                        Result.Profit = Business.Market.InvestorList[i].Profit;
                        Result.SecondAddress = Business.Market.InvestorList[i].SecondAddress;
                        Result.State = Business.Market.InvestorList[i].State;
                        Result.StatusCode = Business.Market.InvestorList[i].StatusCode;
                        Result.TaxRate = Business.Market.InvestorList[i].TaxRate;
                        Result.FreezeMargin = Business.Market.InvestorList[i].FreezeMargin;
                        #endregion

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> FacadeGetSizePipSetting(string Code)
        {
            Dictionary<string,int> Result=new Dictionary<string,int>();
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].ParameterItems != null)
                    {
                        int countParameter = Business.Market.SymbolList[i].ParameterItems.Count;
                        for (int j = 0; j < countParameter; j++)
                        {
                            if (Business.Market.SymbolList[i].ParameterItems[j].Code == Code)
                            {
                                int ContractSize = -1;
                                int.TryParse(Business.Market.SymbolList[i].ParameterItems[j].NumValue,out ContractSize);
                                if (Result.ContainsKey(Business.Market.SymbolList[i].Name))
                                {
                                    Result[Business.Market.SymbolList[i].Name] = ContractSize;
                                }
                                else
                                {
                                    Result.Add(Business.Market.SymbolList[i].Name, ContractSize);
                                }
                                
                                break;
                            }
                        }
                    }

                    if (Business.Market.SymbolList[i].RefSymbol != null)
                    {
                        ClientFacade.FacadeGetSizePipReference(Business.Market.SymbolList[i].RefSymbol, Result,Code);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListSymbol"></param>
        /// <returns></returns>
        private static void FacadeGetSizePipReference(List<Business.Symbol> ListSymbol,Dictionary<string,int> Result,string Code)
        {            
            if (ListSymbol != null)
            {
                int count = ListSymbol.Count;
                for (int i = 0; i < count; i++)
                {
                    if (ListSymbol[i].ParameterItems != null)
                    {
                        int countParameter = ListSymbol[i].ParameterItems.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (ListSymbol[i].ParameterItems[j].Code == Code)
                            {
                                int ContractSize = -1;
                                int.TryParse(ListSymbol[i].ParameterItems[j].NumValue, out ContractSize);
                                Result.Add(ListSymbol[i].Name, ContractSize);
                                break;
                            }
                        }
                    }

                    if (ListSymbol[i].RefSymbol != null)
                    {
                        ClientFacade.FacadeGetSizePipReference(ListSymbol[i].RefSymbol, Result,Code);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeLogin(string Code, string Pwd, int InvestorIndex, string IpAddress)
        {            
            return TradingServer.Facade.FacadeLoginServer(Code, Pwd, InvestorIndex, IpAddress);
        }

        /// <summary>
        /// CLIENTFACADE CLIENT LOGIN NO CONNECT WITH MT4
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeNewLogin(string Code, string Pwd, int InvestorIndex, string IpAddress, Socket InsSocket)
        {
            return TradingServer.Facade.FacadeNewLoginServer(Code, Pwd, InvestorIndex, IpAddress, InsSocket);
        }

        /// <summary>
        /// CLIENTFACADE CLIENT LOGIN CONNECT WITH MT4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static Business.Investor FacadeNewLogin(string code, string Pwd, string ipAddress, Socket InsSocket)
        {
            return TradingServer.Facade.FacadeNewLoginServer(code, Pwd, ipAddress, InsSocket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Pwd"></param>
        /// <param name="ipAddress"></param>
        /// <param name="InsSocket"></param>
        /// <returns></returns>
        public static Business.Investor FacadeLoginMT4(string code, string Pwd, string ipAddress)
        {
            return TradingServer.Facade.FacadeLoginMT4Server(code, Pwd, ipAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static bool FacadeLogout(int InvestorID, int InvestorIndex)
        {
            bool Result = false;
            Business.Investor newInvestor = new Business.Investor();

            if (InvestorIndex != -1 && InvestorIndex <= TradingServer.Business.Market.InvestorList.Count - 1)
            {
                #region GET INVESTOR WITH INVESTOR INDEX
                newInvestor = Business.Market.InvestorList[InvestorIndex];
                if (newInvestor != null && newInvestor.InvestorID > 0)
                {
                    if (newInvestor.InvestorID == InvestorID)
                    {
                        #region INVESTOR ID = INVESTOR ID
                        Business.Market.InvestorList[InvestorIndex].IsLogout = true;
                        Business.Market.InvestorList[InvestorIndex].IsOnline = false;

                        //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                        TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[InvestorIndex]);

                        Result = true;
                        #endregion                       
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST
                        if (TradingServer.Business.Market.InvestorList != null)
                        {
                            int count = TradingServer.Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                                {
                                    TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                                    TradingServer.Business.Market.InvestorList[i].IsOnline = false;

                                    //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                    TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);

                                    Result = true;
                                    break;
                                }
                            }
                        }
                        #endregion                        
                    }
                }
                #endregion                
            }
            else
            {
                #region FORGET INVESTOR INDEX
                if (TradingServer.Business.Market.InvestorList != null)
                {
                    int count = TradingServer.Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                        {
                            TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                            TradingServer.Business.Market.InvestorList[i].IsOnline = false;

                            //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);

                            Result = true;
                            break;
                        }
                    }
                } 
                #endregion                
            }

            #region REMOVE INVESTOR IN INVESTOR ONLINE
            if (Result)
            {
                if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
                {
                    int count = Business.Market.InvestorOnline.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].InvestorID == InvestorID)
                        {
                            Business.Market.InvestorOnline[i].IsLogout = true;
                            break;
                        }
                    }
                }
            }
            #endregion            

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static bool FacadeManualLogout(int InvestorID, int InvestorIndex, string key, string ipAddress)
        {
            bool Result = false;
            string userName = string.Empty;
            string pass = string.Empty;
            Business.Investor newInvestor = new Business.Investor();
            bool isPrimary = Business.Investor.investorInstance.CheckPrimaryInvestorOnline(InvestorID, Business.TypeLogin.Primary, key);

            if (InvestorIndex != -1 && InvestorIndex <= TradingServer.Business.Market.InvestorList.Count - 1)
            {
                #region GET INVESTOR WITH INVESTOR INDEX
                newInvestor = Business.Market.InvestorList[InvestorIndex];
                if (newInvestor != null && newInvestor.InvestorID > 0)
                {
                    if (newInvestor.InvestorID == InvestorID)
                    {
                        if (Business.Market.InvestorList[InvestorIndex].InvestorID == InvestorID)
                        {
                            if (isPrimary)
                            {
                                #region INVESTOR ID = INVESTOR ID
                                
                                Business.Market.InvestorList[InvestorIndex].IsLogout = true;
                                Business.Market.InvestorList[InvestorIndex].IsOnline = false;
                                //Business.Market.InvestorList[InvestorIndex].LoginKey = string.Empty;
                                userName = Business.Market.InvestorList[InvestorIndex].Code;
                                pass = Business.Market.InvestorList[InvestorIndex].UnZipPwd;

                                //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(2, newInvestor);
                                #endregion
                            }

                            Result = true;
                        }         
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST
                        if (TradingServer.Business.Market.InvestorList != null)
                        {                            
                            for (int i = 0; i < TradingServer.Business.Market.InvestorList.Count; i++)
                            {
                                if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                                {
                                    if (isPrimary)
                                    {
                                        TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                                        TradingServer.Business.Market.InvestorList[i].IsOnline = false;
                                        //TradingServer.Business.Market.InvestorList[i].LoginKey = string.Empty;
                                        userName = Business.Market.InvestorList[i].Code;
                                        pass = Business.Market.InvestorList[i].UnZipPwd;

                                        //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                        TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);
                                    }

                                    Result = true;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion  
            }
            else
            {
                #region FORGET INVESTOR INDEX
                if (TradingServer.Business.Market.InvestorList != null)
                {                    
                    for (int i = 0; i < TradingServer.Business.Market.InvestorList.Count; i++)
                    {
                        if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                        {
                            if (isPrimary)
                            {
                                TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                                TradingServer.Business.Market.InvestorList[i].IsOnline = false;
                                //TradingServer.Business.Market.InvestorList[i].LoginKey = string.Empty;
                                userName = Business.Market.InvestorList[i].Code;
                                pass = Business.Market.InvestorList[i].UnZipPwd;

                                //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);
                            }

                            Result = true;

                            break;
                        }
                    }
                }
                #endregion
            }

            #region REMOVE INVESTOR IN INVESTOR ONLINE
            if (Result)
            {
                if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
                {                    
                    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].InvestorID == InvestorID && Business.Market.InvestorOnline[i].LoginKey == key)
                        {
                            if (!isPrimary)
                            {
                                userName = Business.Market.InvestorOnline[i].Code;
                                pass = Business.Market.InvestorOnline[i].UnZipPwd;
                            }

                            Business.Market.InvestorOnline[i].IsLogout = true;
                            Business.Market.InvestorOnline[i].IsOnline = false;

                            break;
                        }
                    }
                }
            }
            #endregion

            #region SEND COMMAND DISCONNECT NJ4X
            if (Result)
            {
                string nj4xDisconnect = NJ4XConnectSocket.MapNJ4X.Instance.MapDisconnectNJ4X(userName, pass);

                NJ4XConnectSocket.NJ4XConnectSocketAsync.Instance.SendNJ4X(nj4xDisconnect);
            }
            #endregion
            
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        public static bool FacadeLogout(int InvestorID, int InvestorIndex, string key,string ipAddress)
        {
            bool Result = false;
            Business.Investor newInvestor = new Business.Investor();
            bool isPrimary = Business.Investor.investorInstance.CheckPrimaryInvestorOnline(InvestorID, Business.TypeLogin.Primary, key);
            //int countInvestor = Business.Investor.investorInstance.CountPrimaryInvestor(InvestorID);
            if (InvestorIndex != -1 && InvestorIndex <= TradingServer.Business.Market.InvestorList.Count - 1)
            {
                #region GET INVESTOR WITH INVESTOR INDEX
                newInvestor = Business.Market.InvestorList[InvestorIndex];
                if (newInvestor != null && newInvestor.InvestorID > 0)
                {
                    if (newInvestor.InvestorID == InvestorID)
                    {
                        if (isPrimary)
                        {
                            #region INVESTOR ID = INVESTOR ID
                            Business.Market.InvestorList[InvestorIndex].IsLogout = true;
                            Business.Market.InvestorList[InvestorIndex].IsOnline = false;

                            //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                            TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[InvestorIndex]);
                            #endregion
                        }

                        Result = true;
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST
                        if (TradingServer.Business.Market.InvestorList != null)
                        {
                            int count = TradingServer.Business.Market.InvestorList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                                {
                                    if (isPrimary)
                                    {
                                        TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                                        TradingServer.Business.Market.InvestorList[i].IsOnline = false;
                                        
                                        //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                        TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);
                                    }

                                    Result = true;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            else
            {
                #region FORGET INVESTOR INDEX
                if (TradingServer.Business.Market.InvestorList != null)
                {
                    int count = TradingServer.Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (TradingServer.Business.Market.InvestorList[i].InvestorID == InvestorID)
                        {
                            if (isPrimary)
                            {
                                TradingServer.Business.Market.InvestorList[i].IsLogout = true;
                                TradingServer.Business.Market.InvestorList[i].IsOnline = false;
                                
                                //SEND NOTIFY TO MANAGER SET INVESTOR LOGOUT
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(2, TradingServer.Business.Market.InvestorList[i]);
                            }

                            Result = true;
                            break;
                        }
                    }
                }
                #endregion
            }

            #region REMOVE INVESTOR IN INVESTOR ONLINE
            if (Result)
            {
                if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
                {
                    int count = Business.Market.InvestorOnline.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].InvestorID == InvestorID && Business.Market.InvestorOnline[i].LoginKey == key)
                        {
                            Business.Market.InvestorOnline[i].IsLogout = true;
                            break;
                        }
                    }
                }
            }
            #endregion

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Business.Investor sFacadeAddNewInvestor(int InvestorStatusID, int InvestorGroupID, string AgentID, double Balance, double Credit, string Code, string Pwd,
                                                double TaxRate, double Leverage, string Address, string Phone,
                                                string City, string Country, string Email, string ZipCode,string State,
                                                string NickName)
        {
            if (Business.Market.IsRealSearver)
                return null;

            Business.Investor objInvestor = new Business.Investor();
            objInvestor.InvestorStatusID = InvestorStatusID;
            //objInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(InvestorGroupID);                        
            
            string tempDefaultGroupName = string.Empty;
            if (Business.Market.MarketConfig != null)
            {
                int countMarketConfig = Business.Market.MarketConfig.Count;
                for (int i = 0; i < countMarketConfig; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C33")
                    {
                        tempDefaultGroupName = Business.Market.MarketConfig[i].StringValue;
                    }
                }
            }

            double tempDefaultDeposit = 0;

            bool isExists = false;
            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].Name.Trim().ToUpper() == tempDefaultGroupName.Trim().ToUpper())
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countConfig = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G24")
                                    double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].StringValue, out tempDefaultDeposit);
                            }
                        }

                        objInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                        break;
                    }
                }
            }

            objInvestor.AgentID = AgentID;

            if (tempDefaultDeposit > 0)
                objInvestor.Balance = tempDefaultDeposit;
            else
                objInvestor.Balance = Balance;

            objInvestor.Credit = Credit;

            int ranCode = -1;

            //bool CheckCode = false;
            //while (!CheckCode)
            //{
            //    Random ran = new Random();
            //    ranCode = ran.Next(0000000, 9999999);

            //    CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(ranCode.ToString());
            //}

            string rendCode = TradingServer.Model.TradingCalculate.Instance.GetNextRandomCode();

            bool CheckCode = false;
            CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(rendCode);
            
            //objInvestor.Code = ranCode.ToString();
            objInvestor.Code = rendCode;
            objInvestor.PrimaryPwd = Pwd;
            objInvestor.ReadOnlyPwd = Pwd;
            objInvestor.PhonePwd = Pwd;
            objInvestor.IsDisable = false;            
            objInvestor.TaxRate = TaxRate;

            objInvestor.Leverage = Leverage;
            objInvestor.Address = Address;
            objInvestor.Phone = Phone;
            objInvestor.City = City;
            objInvestor.Country = Country;
            objInvestor.Email = Email;
            objInvestor.ZipCode = "NaN";
            objInvestor.RegisterDay = DateTime.Now;
            objInvestor.InvestorComment = "Demo Account";
            objInvestor.State = "NaN";
            objInvestor.NickName = NickName;
            objInvestor.SendReport = true;
            objInvestor.AllowChangePwd = true;
            objInvestor.UserConfig = "";

            string primaryPwd = objInvestor.PrimaryPwd;

            string temp = TradingServer.Model.ValidateCheck.Encrypt(objInvestor.PrimaryPwd);
            objInvestor.PrimaryPwd = temp;
            objInvestor.ReadOnlyPwd = temp;
            objInvestor.PhonePwd = temp;

            double tempBalance = objInvestor.Balance;
            objInvestor.Balance = 0;

            objInvestor.InvestorID = TradingServer.Facade.FacadeAddNewInvestor(objInvestor);

            if (objInvestor.InvestorID > 0)
            {
                int resultProfileID = TradingServer.Facade.FacadeAddInvestorProfile(objInvestor);

                objInvestor.InvestorProfileID = resultProfileID;

                Business.Market.InvestorList.Add(objInvestor);                

                TradingServer.Facade.FacadeAddDeposit(objInvestor.InvestorID, tempBalance, "[add deposit create account]");
            }

            //SEND MAIL IF CREATE NEW INVESTOR COMPLETE
            if (objInvestor.InvestorID > 0)
            {
                #region SEND MAIL CREATE ACCOUNT
                Model.MailConfig newMailConfig = new Model.MailConfig();
                newMailConfig = TradingServer.Facade.FacadeGetMailConfig(objInvestor);

                //string header = "<!DOCTYPE html PUBLIC" + '"' + "-//W3C//DTD XHTML 1.0 Transitional//EN" + '"' + '"' + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd +" + '"' + ">" +
                //        "<html xmlns=" + '"' + "http://www.w3.org/1999/xhtml" + '"' + "><head><meta content=" + '"' + "text/html; charset=utf-8" + '"' + "http-equiv=" + '"' + "Content-Type" + '"' + "/>" +
                //        "<title>Confirmation Mail</title><style type=" + '"' + "text/css" + '"' + ">a{	text-decoration:none;color:#333333;	} a:hover{	color:#156a9d;text-decoration:underline;}</style> " +
                //        "<script type=" + '"' + "text/javascript" + '"' + "></script></head><body style=" + '"' + "font-family:Arial;font-size:12px;color:#333333;width:500px;margin-left:auto;margin-right:auto;" + '"' + ">" +
                //        "<div id=" + '"' + "header" + '"' + "style=" + '"' + "width:500px;height:70px;border-bottom:2px #dddddd solid;border-top:2px #dddddd solid;background-color:#f9f9f9;background-image:url('http://et5.mpf.co.id/img/mpflogo.png');background-repeat:no-repeat" + '"' + ">" +
                //        "</div><div style=" + '"' + "background-color:white;width:90%;color:#333333;margin:15px;" + '"' + "><b>Dear " +
                //        objInvestor.NickName + "</b>,<br/><br/>" +
                //        "<span style='color:#156a9d;font-weight:bold'>Your demo account has been activated !</span><br/><br/>" +
                //        "Please, keep your login credentials for future access :<br/><br/><table>";

                //string userandpass = "<tr><td style='width:100px'>Username :</td><td style='color:#156a9d;font-weight:bold'>" + objInvestor.Code + "</td></tr>" +
                //    "<tr><td style='width:100px'>Password :</td><td>" + primaryPwd + "</td></tr>";

                //string bottom = "<tr><td style='width:100px'>Access :</td><td><a href='http://et5.mpf.co.id'>http://et5.mpf.co.id</a></td></tr> " +
                //    "</table><br/><span style='color:#156a9d;font-weight:bold'>Best Regards,</span><br/>Millennium Penata Futures Customer Service<br/>" +
                //    "Contact us anytime: <a href='mailto:support@mpf.co.id'>support@mpf.co.id</a><br/><br/></div>" +
                //    "<div style='background-color:#f7f7f7;width:480px;border-top:2px #dddddd solid;border-bottom:2px #dddddd solid;padding:10px;'>" +
                //    "<span style='color:#156a9d;font-weight:bold'>PT. Millennium Penata Futures </span>is the first financial institution offering online-based  investment business in Indonesia. Through years of trial, we succeeded in finding an online system that provides data and news in realtime mode." +
                //    "</div><div style='width:490px;padding:5px;font-size:10px;text-align:right'>© Copyright 2011. PT. Millennium Penata Futures</div></body></html>";

                //string content = header + userandpass + bottom;

                StringBuilder content = new StringBuilder();
                Business.ConfirmationTemplate newTemplate = new Business.ConfirmationTemplate();
                content = newTemplate.GetConfirmation();
                content.Replace("[#LinkLogo]", "http://et5.mpf.co.id/img/mpflogo.png");
                content.Replace("[#FullName]", objInvestor.NickName);
                content.Replace("[#AccountType]", "demo account");
                content.Replace("[#UserName]", objInvestor.Code);
                content.Replace("[#Password]", primaryPwd);
                content.Replace("[#Website]", "http://et5.mpf.co.id");
                content.Replace("[#AccessLink]","http://et5.mpf.co.id");
                content.Replace("[#ServiceName]","Millennium Penata Futures Customer Service");
                content.Replace("[#MailSupport]","mailto:support@mpf.co.id");
                content.Replace("[#EmailContact]","support@mpf.co.id");
                content.Replace("[#CompanyName]","PT. Millennium Penata Futures ");
                content.Replace("[#CompanyIntroduct]","is the first financial institution offering online-based  investment business in Indonesia. Through years of trial, we succeeded in finding an online system that provides data and news in realtime mode.");
                content.Replace("[#CompanyCopyright]","PT. Millennium Penata Futures");

                TradingServer.Model.TradingCalculate.Instance.SendMail(objInvestor.Email, "CONFIRMATION EMAIL", content.ToString(), newMailConfig);
                #endregion
            }

            Business.Investor newInvestor = new Business.Investor();
            if (objInvestor.InvestorID > 0)
            {   
                //newInvestor.Code = ranCode.ToString();
                newInvestor.Code = rendCode;
                newInvestor.PrimaryPwd = primaryPwd;
                newInvestor.ReadOnlyPwd = primaryPwd;
                newInvestor.PhonePwd = primaryPwd;
                newInvestor.IsDisable = false;
                newInvestor.TaxRate = TaxRate;
                newInvestor.Leverage = Leverage;
                newInvestor.Address = Address;
                newInvestor.Phone = Phone;
                newInvestor.City = City;
                newInvestor.Country = Country;
                newInvestor.Email = Email;
                newInvestor.ZipCode = "NaN";
                newInvestor.RegisterDay = DateTime.Now;
                newInvestor.InvestorComment = "Demo Account";
                newInvestor.State = "NaN";
                newInvestor.NickName = NickName;
                newInvestor.SendReport = true;
                newInvestor.InvestorID = objInvestor.InvestorID;
            }

            //SEND NOTIFY TO MANAGER
            //TradingServer.Facade.FacadeSendNotifyManagerRequest(3, newInvestor);
            //TradingServer.Facade.FacadeAutoSendMailRegistration(newInvestor);
            return newInvestor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Business.Investor FacadeAddNewInvestor(int InvestorStatusID, int InvestorGroupID, string AgentID, double Balance, double Credit, string Code, string Pwd,
                                                double TaxRate, double Leverage, string Address, string Phone,
                                                string City, string Country, string Email, string ZipCode, string State,
                                                string NickName,string key)
        {
            Business.Investor objInvestor = new Business.Investor();
            objInvestor.InvestorStatusID = InvestorStatusID;
            //objInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(InvestorGroupID);

            string tempDefaultGroupName = string.Empty;
            
            if (Business.Market.MarketConfig != null)
            {
                int countMarketConfig = Business.Market.MarketConfig.Count;
                for (int i = 0; i < countMarketConfig; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C34")
                    {
                        string[] subValue = Business.Market.MarketConfig[i].StringValue.Split('`');

                        #region PROCESS DOMAIN CLIENT
                        string[] subKey = key.Split('/');

                        string tempKey = string.Empty;

                        if (subKey.Length > 2)
                        {
                            tempKey = subKey[2];
                        }
                        else
                        {
                            tempKey = subKey[0];
                        }

                        tempKey = tempKey.Replace("http://", string.Empty);
                        tempKey = tempKey.Replace("www.", string.Empty);
                        tempKey = tempKey.Replace("/", string.Empty);
                        tempKey = tempKey.Replace("?", string.Empty);
                        #endregion

                        #region PROCESS DOMAIN SERVER
                        string[] subDomain = subValue[0].Split('/');
                        string tempDomainServer = string.Empty;
                        if (subDomain.Length == 4)
                        {
                            tempDomainServer = subDomain[2];
                        }
                        else
                        {
                            tempDomainServer = subDomain[0];
                        }

                        tempDomainServer = tempDomainServer.Replace("http://", string.Empty);
                        tempDomainServer = tempDomainServer.Replace("www.", string.Empty);
                        tempDomainServer = tempDomainServer.Replace("/", string.Empty);
                        tempDomainServer = tempDomainServer.Replace("?", string.Empty);
                        #endregion

                        if (key.ToUpper().Trim() == tempDomainServer.ToUpper().Trim())
                        {
                            tempDefaultGroupName = subValue[1];

                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(tempDefaultGroupName))
            {
                int countMarketConfig = Business.Market.MarketConfig.Count;
                for (int i = 0; i < countMarketConfig; i++)
                {
                    if (Business.Market.MarketConfig[i].Code == "C33")
                    {
                        tempDefaultGroupName = Business.Market.MarketConfig[i].StringValue;
                        break;
                    }
                }
            }

            bool isExists = false;
            double tempDepositDefault = 0;

            if (Business.Market.InvestorGroupList != null)
            {
                int count = Business.Market.InvestorGroupList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorGroupList[i].Name.Trim().ToUpper() == tempDefaultGroupName.Trim().ToUpper())
                    {
                        if (Business.Market.InvestorGroupList[i].ParameterItems != null)
                        {
                            int countConfig = Business.Market.InvestorGroupList[i].ParameterItems.Count;
                            for (int j = 0; j < countConfig; j++)
                            {
                                if (Business.Market.InvestorGroupList[i].ParameterItems[j].Code == "G24")
                                {
                                    double.TryParse(Business.Market.InvestorGroupList[i].ParameterItems[j].StringValue, out tempDepositDefault);

                                    break;
                                }
                            }
                        }
                        objInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                        break;
                    }
                }
            }

            objInvestor.AgentID = AgentID;

            if (tempDepositDefault > 0)
                objInvestor.Balance = tempDepositDefault;
            else
                objInvestor.Balance = Balance;

            objInvestor.Credit = Credit;

            int ranCode = -1;

            //bool CheckCode = false;
            //while (!CheckCode)
            //{
            //    Random ran = new Random();
            //    ranCode = ran.Next(0000000, 9999999);

            //    CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(ranCode.ToString());
            //}

            string rendCode = TradingServer.Model.TradingCalculate.Instance.GetNextRandomCode();

            bool CheckCode = false;
            CheckCode = TradingServer.Facade.FacadeGetInvestorByCode(rendCode);

            //objInvestor.Code = ranCode.ToString();
            objInvestor.Code = rendCode;
            objInvestor.PrimaryPwd = Pwd;
            objInvestor.ReadOnlyPwd = Pwd;
            objInvestor.PhonePwd = Pwd;
            objInvestor.IsDisable = false;
            objInvestor.TaxRate = TaxRate;
            objInvestor.Leverage = Leverage;
            objInvestor.Address = Address;
            objInvestor.Phone = Phone;
            objInvestor.City = City;
            objInvestor.Country = Country;
            objInvestor.Email = Email;
            objInvestor.ZipCode = "NaN";
            objInvestor.RegisterDay = DateTime.Now;
            objInvestor.InvestorComment = "Demo Account";
            objInvestor.State = "NaN";
            objInvestor.NickName = NickName;
            objInvestor.SendReport = true;
            objInvestor.AllowChangePwd = true;
            objInvestor.UserConfig = "";

            string primaryPwd = objInvestor.PrimaryPwd;

            string temp = TradingServer.Model.ValidateCheck.Encrypt(objInvestor.PrimaryPwd);
            objInvestor.PrimaryPwd = temp;
            objInvestor.ReadOnlyPwd = temp;
            objInvestor.PhonePwd = temp;

            double tempBalance = objInvestor.Balance;
            objInvestor.Balance = 0;

            objInvestor.InvestorID = TradingServer.Facade.FacadeAddNewInvestor(objInvestor);

            if (objInvestor.InvestorID > 0)
            {
                int resultProfileID = TradingServer.Facade.FacadeAddInvestorProfile(objInvestor);

                objInvestor.InvestorProfileID = resultProfileID;

                Business.Market.InvestorList.Add(objInvestor);

                TradingServer.Facade.FacadeAddDeposit(objInvestor.InvestorID, tempBalance, "[add deposit create account]");
            }

            //SEND MAIL IF CREATE NEW INVESTOR COMPLETE
            if (objInvestor.InvestorID > 0)
            {
                #region SEND MAIL CREATE ACCOUNT
                Model.MailConfig newMailConfig = new Model.MailConfig();
                newMailConfig = TradingServer.Facade.FacadeGetMailConfig(objInvestor);

                StringBuilder content = new StringBuilder();
                Business.ConfirmationTemplate newTemplate = new Business.ConfirmationTemplate();
                content = newTemplate.GetConfirmation();
                content.Replace("[#LinkLogo]", "http://et5.mpf.co.id/img/mpflogo.png");
                content.Replace("[#FullName]", objInvestor.NickName);
                content.Replace("[#AccountType]", "demo account");
                content.Replace("[#UserName]", objInvestor.Code);
                content.Replace("[#Password]", primaryPwd);
                content.Replace("[#Website]", "http://demo.efxgm.com");
                //content.Replace("[#Website]", Business.Market.CompanyWebsite);
                content.Replace("[#AccessLink]", "http://demo.efxgm.com");
                //content.Replace("[#AccessLink]", Business.Market.AccessLink);
                content.Replace("[#ServiceName]", "EFXGM Customer Service");
                //content.Replace("[#ServiceName]", Business.Market.ServiceName);
                content.Replace("[#MailSupport]", "mailto:accounts@efxgm.com");
                //content.Replace("[#MailSupport]", Business.Market.MailSupport);
                content.Replace("[#EmailContact]", "accounts@efxgm.com");
                //content.Replace("[#EmailContact]", Business.Market.MailContact);
                //content.Replace("[#CompanyName]", "PT. Millennium Penata Futures ");
                //content.Replace("[#CompanyIntroduct]", "is the first financial institution offering online-based  investment business in Indonesia. Through years of trial, we succeeded in finding an online system that provides data and news in realtime mode.");
                content.Replace("[#CompanyCopyright]", "EFXGM");
                //content.Replace("[#CompanyCopyright]", Business.Market.CompanyCopyright);

                TradingServer.Model.TradingCalculate.Instance.SendMail(objInvestor.Email, "CONFIRMATION EMAIL", content.ToString(), newMailConfig);
                #endregion
            }

            Business.Investor newInvestor = new Business.Investor();
            if (objInvestor.InvestorID > 0)
            {
                //newInvestor.Code = ranCode.ToString();
                newInvestor.Code = rendCode;
                newInvestor.PrimaryPwd = primaryPwd;
                newInvestor.ReadOnlyPwd = primaryPwd;
                newInvestor.PhonePwd = primaryPwd;
                newInvestor.IsDisable = false;
                newInvestor.TaxRate = TaxRate;
                newInvestor.Leverage = Leverage;
                newInvestor.Address = Address;
                newInvestor.Phone = Phone;
                newInvestor.City = City;
                newInvestor.Country = Country;
                newInvestor.Email = Email;
                newInvestor.ZipCode = "NaN";
                newInvestor.RegisterDay = DateTime.Now;
                newInvestor.InvestorComment = "Demo Account";
                newInvestor.State = "NaN";
                newInvestor.NickName = NickName;
                newInvestor.SendReport = true;
                newInvestor.InvestorID = objInvestor.InvestorID;
            }

            //SEND NOTIFY TO MANAGER
            //TradingServer.Facade.FacadeSendNotifyManagerRequest(3, newInvestor);
            //TradingServer.Facade.FacadeAutoSendMailRegistration(newInvestor);
            return newInvestor;
        }

        public static Business.Investor FacadeAddNewInvestorWithMT4(int InvestorStatusID, int InvestorGroupID, string AgentID, double Balance, double Credit, string Code, string Pwd,
                                                double TaxRate, double Leverage, string Address, string Phone,
                                                string City, string Country, string Email, string ZipCode, string State,
                                                string NickName)
        {
            Business.Investor result = new Business.Investor();

            string tempDefaultGroupName = string.Empty;
            int countMarketConfig = Business.Market.MarketConfig.Count;
            for (int i = 0; i < countMarketConfig; i++)
            {
                if (Business.Market.MarketConfig[i].Code == "C33")
                {
                    tempDefaultGroupName = Business.Market.MarketConfig[i].StringValue;
                    break;
                }
            }

            string cmd = BuildCommandElement5ConnectMT4.Mode.BuildCommand.Instance.BuildCommandCreateInvestor(Balance, Leverage, Address, Phone, City, Country,
                                                                                                Email, ZipCode, State, NickName, tempDefaultGroupName,
                                                                                                Pwd);

            //string resultCmd = Business.Market.InstanceSocket.SendToMT4(Business.Market.DEFAULT_IPADDRESS, Business.Market.DEFAULT_PORT, cmd);
            string resultCmd = Element5SocketConnectMT4.Business.SocketConnect.Instance.SendSocket(cmd);

            if (!string.IsNullOrEmpty(resultCmd))
            {
                string[] subValue = resultCmd.Split('$');
                if (subValue.Length == 2)
                {
                    string[] subParameter = subValue[1].Split('{');
                    if (subParameter.Length == 4)
                    {
                        bool isSuccess = false;
                        if (int.Parse(subParameter[0]) == 1)
                            isSuccess = true;

                        if (isSuccess)
                        {
                            result.InvestorID = 1;
                            result.Code = subParameter[2];
                            result.PrimaryPwd = subParameter[3];
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="InvestorName"></param>
        /// <returns></returns>
        public static ClientBusiness.DataServer FacadeGetDataServer(int InvestorID, string InvestorName,int InvestorIndex)
        {
            ClientBusiness.DataServer Result = new ClientBusiness.DataServer();
            //Result.NumAllInvestorOnline = new int();
            //Result.NumUpdate = new ClientBusiness.ChangeCommandQueue();
            //Result.NumUpdateAnnouncement = new int();
            //Result.NumUpdateChat = new ClientBusiness.ChangeCommandQueue();
            //Result.NumUpdateNews = new int();
            //Result.NumUpdateOption = new ClientBusiness.ChangeCommandQueue();
            //Result.NumUpdatePending = new ClientBusiness.ChangeCommandQueue();
            Result.Tick = new List<Business.Tick>();
            Result.TimeServer = new DateTime();
            Result.Tick = new List<Business.Tick>();
            Result.ClientMessage = new List<string>();

            #region COMMENT CODE
            #region GET TICK
            if (Business.Market.SymbolList != null)
            {
                int count = Business.Market.SymbolList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.SymbolList[i].TickValue != null)
                    {
                        Business.Tick newTick = new Business.Tick();
                        newTick = Business.Market.SymbolList[i].TickValue;
                        Result.Tick.Add(newTick);
                    }
                    else
                    {
                        Business.Tick newTick = new Business.Tick();
                        newTick.Ask = 0;
                        newTick.Bid = 0;
                        newTick.HighAsk = 0;
                        newTick.HighInDay = 0;
                        newTick.LowAsk = 0;
                        newTick.LowInDay = 0;
                        newTick.Status = "down";
                        newTick.SymbolName = Business.Market.SymbolList[i].Name;
                        newTick.TickTime = DateTime.Now;
                        newTick.SymbolID = Business.Market.SymbolList[i].SymbolID;

                        Result.Tick.Add(newTick);
                    }
                }
            }
            #endregion

            #region Get Message Server To Client
            Business.Investor newInvestor = new Business.Investor();
            if (InvestorIndex > 0 && InvestorIndex <= Business.Market.InvestorList.Count - 1)
            {
                newInvestor = Business.Market.InvestorList[InvestorIndex];

                if (newInvestor != null && newInvestor.InvestorID > 0)
                {
                    if (newInvestor.InvestorID == InvestorID)
                    {
                        if (!Business.Market.InvestorList[InvestorIndex].IsOnline)
                            Business.Market.InvestorList[InvestorIndex].IsOnline = true;

                        Business.Market.InvestorList[InvestorIndex].LastConnectTime = DateTime.Now;
                        if (newInvestor.ClientCommandQueue != null)
                        {
                            int count = newInvestor.ClientCommandQueue.Count;
                            for (int i = 0; i < count; i++)
                            {
                                Result.ClientMessage.Add(newInvestor.ClientCommandQueue[i]);
                            }

                            Business.Market.InvestorList[InvestorIndex].ClientCommandQueue.Clear();
                        }

                        Result.InvestorIndex = InvestorIndex;
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                        if (Business.Market.InvestorList != null)
                        {
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < countInvestor; j++)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == InvestorID)
                                {
                                    if (!Business.Market.InvestorList[j].IsOnline)
                                        Business.Market.InvestorList[j].IsOnline = true;

                                    Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                                    if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                                    {
                                        int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                        for (int n = 0; n < countMessage; n++)
                                        {
                                            Result.ClientMessage.Add(Business.Market.InvestorList[j].ClientCommandQueue[n]);
                                        }
                                        Business.Market.InvestorList[j].ClientCommandQueue.Clear();
                                    }

                                    //Set Investor Index
                                    Result.InvestorIndex = j;

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].InvestorID == InvestorID)
                            {
                                if (!Business.Market.InvestorList[j].IsOnline)
                                    Business.Market.InvestorList[j].IsOnline = true;

                                Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                                if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                                {
                                    int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                    for (int n = 0; n < countMessage; n++)
                                    {
                                        Result.ClientMessage.Add(Business.Market.InvestorList[j].ClientCommandQueue[n]);
                                    }
                                    Business.Market.InvestorList[j].ClientCommandQueue.Clear();
                                }

                                Result.InvestorIndex = j;

                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                if (Business.Market.InvestorList != null)
                {
                    int countInvestor = Business.Market.InvestorList.Count;
                    for (int j = 0; j < countInvestor; j++)
                    {
                        if (Business.Market.InvestorList[j].InvestorID == InvestorID)
                        {
                            if (!Business.Market.InvestorList[j].IsOnline)
                                Business.Market.InvestorList[j].IsOnline = true;

                            Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                            if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                            {
                                int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                for (int n = 0; n < countMessage; n++)
                                {
                                    Result.ClientMessage.Add(Business.Market.InvestorList[j].ClientCommandQueue[n]);
                                }
                                Business.Market.InvestorList[j].ClientCommandQueue.Clear();
                            }

                            Result.InvestorIndex = j;

                            break;
                        }
                    }
                }
                #endregion
            }
            #endregion   
            #endregion

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="investorName"></param>
        /// <param name="investorIndex"></param>
        /// <returns></returns>
        public static string FacadeGetData(int investorID, string investorName, int investorIndex)
        {
            string result = string.Empty;            
            
            #region GET TICK
            //if (Business.Market.SymbolList != null)
            //{
            //    int count = Business.Market.SymbolList.Count;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (Business.Market.SymbolList[i].TickValue != null)
            //        {
            //            result += Business.Market.SymbolList[i].TickValue.Bid + "▼" + Business.Market.SymbolList[i].TickValue.HighInDay + "▼" +
            //                Business.Market.SymbolList[i].TickValue.LowInDay + "▼" + Business.Market.SymbolList[i].TickValue.Status + "▼" +
            //                Business.Market.SymbolList[i].TickValue.SymbolName + "▼" +
            //                Business.Market.SymbolList[i].TickValue.TickTime.Ticks + "♦";
            //        }
            //        else
            //        {
            //            result += "0▼0▼0▼down▼" + Business.Market.SymbolList[i].Name + "▼" + DateTime.Now.Ticks + "♦";
            //        }
            //    }
            //}
            #endregion

            result = ClientFacade.GetTickOnInvestorOnline(investorID);

            //Business.TypeLogin type = ClientFacade.GetTypeLogin(investorID);

            #region REMOVE CHAR
            //Remove Charater end "♦"
            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }
            //Add Character Start "◊"
            result+="◊";
            #endregion

            //if (type == Business.TypeLogin.Primary)
            //{
                #region Get Message Server To Client
                if (Business.Market.InvestorList != null)
                {
                    #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                    if (Business.Market.InvestorList != null)
                    {
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < countInvestor; j++)
                        {
                            if (Business.Market.InvestorList[j].InvestorID == investorID)
                            {
                                if (!Business.Market.InvestorList[j].IsOnline)
                                {
                                    if (Business.Market.InvestorList[j].IsLogout)
                                        return result;

                                    Business.Market.InvestorList[j].IsOnline = true;
                                }

                                Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                                if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                                {
                                    int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                    for (int n = 0; n < countMessage; n++)
                                    {
                                        try
                                        {
                                            if (Business.Market.InvestorList[j].ClientCommandQueue[0] != null)
                                            {
                                                result += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";
                                                Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                            }
                                            else
                                            {
                                                Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                            }
                                            //n--;
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                    #endregion
                }
                #endregion   
            //}                    

            #region REMOVE CHAR
            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// update new version remove backup data(2/1/2014)
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="investorName"></param>
        /// <param name="investorIndex"></param>
        /// <returns></returns>
        public static StringBuilder FacadeNewGetData(int investorID, int investorIndex, string key, int port, string keyConfirm)
        {
            StringBuilder result = new StringBuilder();
            string resultBackup = string.Empty;
            string resultCommand = string.Empty;
            List<string> tempResult = new List<string>();

            //result = ClientFacade.GetNewTickOnInvestorOnline(investorID, key, port);

            Business.TypeLogin type = ClientFacade.GetTypeLogin(investorID, key);

            bool isValidKey = false;
            if (type == Business.TypeLogin.Primary)
            {
                Business.Investor newInvestor;
                if (investorIndex >= 0 && investorIndex <= Business.Market.InvestorList.Count - 1)
                {
                    newInvestor = Business.Market.InvestorList[investorIndex];
                    if (newInvestor != null && newInvestor.InvestorID > 0 && newInvestor.InvestorID == investorID && newInvestor.LoginKey == key)
                    {
                        #region CHECK STATUS INVESTOR
                        if (!newInvestor.IsOnline)
                        {
                            if (newInvestor.IsLogout)
                                return result;

                            newInvestor.IsOnline = true;

                            newInvestor.AddInvestorToInvestorOnline(newInvestor);
                        }
                        #endregion

                        StringBuilder temp = ClientFacade.GetNewTickOnInvestorOnline(newInvestor, port);

                        #region REMOVE CHAR
                        //Remove Charater end "♦"
                        if (temp.Length > 1)
                        {
                            temp = temp.Remove(temp.Length - 1, 1);
                        }
                        result = temp;
                        //Add Character Start "◊"
                        result.Append("◊");
                        #endregion

                        newInvestor.LastConnectTime = DateTime.Now;

                        #region GET CLIENT COMMAND SERVER
                        if (newInvestor.ClientCommandQueue != null)
                        {
                            int countMessage = newInvestor.ClientCommandQueue.Count;
                            for (int n = 0; n < countMessage; n++)
                            {
                                try
                                {
                                    if (newInvestor.ClientCommandQueue[0] != null)
                                    {
                                        string _tempQueue = newInvestor.ClientCommandQueue[0];
                                        result.Append(_tempQueue);
                                        result.Append("▼");

                                        resultBackup += _tempQueue + "▼";

                                        if (_tempQueue == "OLOFF14790251")
                                        {
                                            newInvestor.ClientCommandQueue.RemoveAt(0);
                                            continue;
                                        }
                                        else
                                        {
                                            tempResult.Add(_tempQueue);
                                            newInvestor.ClientCommandQueue.RemoveAt(0);
                                        }
                                    }
                                    else
                                    {
                                        newInvestor.ClientCommandQueue.RemoveAt(0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                }
                            }
                        }
                        #endregion

                        #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                        StringBuilder _strMessage = newInvestor.AddCommandToInvestorOnline(investorID, key, tempResult);
                        result.Append(_strMessage);
                        #endregion

                        #region PROCESS DATA BACKUP
                        //StringBuilder _tempBackupData = ProcessBackupMessage(keyConfirm, resultBackup, newInvestor);
                        //result.Append(_tempBackupData);
                        #endregion

                        isValidKey = true;
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                        if (Business.Market.InvestorList != null)
                        {
                            bool flag = false;
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                            {
                                if (Business.Market.InvestorList[j].LoginKey != null)
                                {
                                    if (Business.Market.InvestorList[j].InvestorID == investorID &&
                                        Business.Market.InvestorList[j].LoginKey == key)
                                    {
                                        Business.Investor _tempInvestor = Business.Market.InvestorList[j];

                                        #region CHECK STATUS INVESTOR
                                        if (!_tempInvestor.IsOnline)
                                        {
                                            if (_tempInvestor.IsLogout)
                                                return result;

                                            _tempInvestor.IsOnline = true;

                                            _tempInvestor.AddInvestorToInvestorOnline(_tempInvestor);
                                        }
                                        #endregion

                                        StringBuilder temp = ClientFacade.GetNewTickOnInvestorOnline(_tempInvestor, port);

                                        #region REMOVE CHAR
                                        //Remove Charater end "♦"
                                        if (temp.Length > 1)
                                        {
                                            temp = temp.Remove(temp.Length - 1, 1);
                                        }
                                        result = temp;

                                        //Add Character Start "◊"
                                        result.Append("◊");
                                        #endregion

                                        _tempInvestor.LastConnectTime = DateTime.Now;
                                        _tempInvestor.InvestorIndex = j;

                                        //SEND COMMAND UPDATE INVESTOR INDEX
                                        if (_tempInvestor.ClientCommandQueue == null)
                                            _tempInvestor.ClientCommandQueue = new List<string>();

                                        string messageChangeIndex = "CII00434543$" + _tempInvestor.InvestorIndex;

                                        _tempInvestor.ClientCommandQueue.Add(messageChangeIndex);

                                        #region GET CLIENT COMMAND SERVER
                                        if (_tempInvestor.ClientCommandQueue != null)
                                        {
                                            int countMessage = _tempInvestor.ClientCommandQueue.Count;
                                            for (int n = 0; n < _tempInvestor.ClientCommandQueue.Count; n++)
                                            {
                                                try
                                                {
                                                    if (_tempInvestor.ClientCommandQueue[0] != null)
                                                    {
                                                        string _temp = _tempInvestor.ClientCommandQueue[0];
                                                        result.Append(_temp);
                                                        result.Append("▼");
                                                        resultBackup += _temp + "▼";

                                                        if (_temp == "OLOFF14790251")
                                                        {
                                                            _tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                            continue;
                                                        }

                                                        tempResult.Add(_temp);
                                                        
                                                        _tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                    }
                                                    else
                                                    {
                                                        _tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    
                                                }
                                            }
                                        }
                                        #endregion

                                        #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                                        StringBuilder _strMessage = _tempInvestor.AddCommandToInvestorOnline(investorID, key, tempResult);
                                        result.Append(_strMessage);
                                        #endregion

                                        #region PROCESS DATA BACKUP
                                        //StringBuilder _tempBackupData = ClientFacade.ProcessBackupMessage(keyConfirm, resultBackup, _tempInvestor);
                                        //result.Append(_tempBackupData);
                                        #endregion

                                        flag = true;
                                        isValidKey = true;
                                        break;
                                    }
                                }
                            }

                            if (!flag)
                            {
                                #region GET MESSAGE SERVER FROM INVESTOR ONLINE
                                //if (Business.Market.InvestorOnline != null)
                                //{
                                //    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                                //    {
                                //        if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                                //        {
                                //            if (Business.Market.InvestorOnline[i].ClientCommandQueue != null)
                                //            {
                                //                int countMessage = Business.Market.InvestorOnline[i].ClientCommandQueue.Count;
                                //                for (int j = 0; j < Business.Market.InvestorOnline[i].ClientCommandQueue.Count; j++)
                                //                {
                                //                    try
                                //                    {
                                //                        if (Business.Market.InvestorOnline[i].ClientCommandQueue[0] != null)
                                //                        {
                                //                            result.Append(Business.Market.InvestorOnline[i].ClientCommandQueue[0]);
                                //                            result.Append("▼");

                                //                            resultBackup += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";

                                //                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                //                        }
                                //                        else
                                //                        {
                                //                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                //                        }
                                //                    }
                                //                    catch (Exception ex)
                                //                    {
                                //                        TradingServer.Model.TradingCalculate.Instance.WriteLogFile("[Error - FacadeNewGetData] ->" + ex.Message, "errorET5", "ManagerAPI");
                                //                    }
                                //                }
                                //            }

                                //            #region PROCESS DATA BACKUP
                                //            if (port == 0)
                                //            {
                                //                string backupCommand = string.Empty;

                                //                if (Business.Market.InvestorOnline[i].BackupQueue != null)
                                //                {
                                //                    int countBackup = Business.Market.InvestorOnline[i].BackupQueue.Count;
                                //                    for (int n = 0; n < Business.Market.InvestorOnline[i].BackupQueue.Count; n++)
                                //                    {
                                //                        if (Business.Market.InvestorOnline[i].BackupQueue[n].Key == keyConfirm)
                                //                        {
                                //                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                //                            n--;
                                //                        }
                                //                        else
                                //                        {
                                //                            backupCommand += Business.Market.InvestorOnline[i].BackupQueue[n].CommandQueue + "▼";
                                //                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                //                            n--;
                                //                        }
                                //                    }
                                //                }

                                //                if (backupCommand.EndsWith("▼"))
                                //                    backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                //                string keyBackup = DateTime.Now.Ticks.ToString();
                                //                if (!string.IsNullOrEmpty(backupCommand)){
                                //                    result.Append("▼");
                                //                    result.Append(backupCommand);
                                //                    result.Append(">");
                                //                    result.Append(keyBackup);
                                //                }
                                //                else
                                //                {
                                //                    result.Append(">");
                                //                    result.Append(keyBackup);
                                //                }   

                                //                //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                //                Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                //                newBackupQueue.Key = keyBackup;
                                //                newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                //                Business.Market.InvestorOnline[i].BackupQueue.Add(newBackupQueue);
                                //            }
                                //            #endregion

                                //            isValidKey = true;
                                //        }
                                //    }
                                //}
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                    if (Business.Market.InvestorList != null)
                    {
                        bool flag = false;
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                        {
                            if (Business.Market.InvestorList[j].LoginKey != null)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == investorID &&
                                    Business.Market.InvestorList[j].LoginKey == key)
                                {
                                    Business.Investor tempInvestor = Business.Market.InvestorList[j];

                                    #region CHECK STATUS INVESTOR
                                    if (!tempInvestor.IsOnline)
                                    {
                                        if (tempInvestor.IsLogout)
                                            return result;

                                        tempInvestor.IsOnline = true;

                                        tempInvestor.AddInvestorToInvestorOnline(tempInvestor);
                                    }
                                    #endregion

                                    StringBuilder temp = ClientFacade.GetNewTickOnInvestorOnline(tempInvestor, port);

                                    #region REMOVE CHAR
                                    //Remove Charater end "♦"
                                    if (temp.Length > 1)
                                    {
                                        temp = temp.Remove(temp.Length - 1, 1);
                                    }
                                    result = temp;

                                    //Add Character Start "◊"
                                    result.Append("◊");
                                    #endregion

                                    tempInvestor.LastConnectTime = DateTime.Now;
                                    tempInvestor.InvestorIndex = j;

                                    //SEND COMMAND UPDATE INVESTOR INDEX
                                    if (tempInvestor.ClientCommandQueue == null)
                                        tempInvestor.ClientCommandQueue = new List<string>();

                                    string messageChangeIndex = "CII00434543$" + tempInvestor.InvestorIndex;
                                    tempInvestor.ClientCommandQueue.Add(messageChangeIndex);

                                    #region GET CLIENT COMMAND SERVER
                                    if (tempInvestor.ClientCommandQueue != null)
                                    {
                                        int countMessage = tempInvestor.ClientCommandQueue.Count;
                                        for (int n = 0; n < tempInvestor.ClientCommandQueue.Count; n++)
                                        {
                                            try
                                            {
                                                if (tempInvestor.ClientCommandQueue[0] != null)
                                                {
                                                    result.Append(tempInvestor.ClientCommandQueue[0]);
                                                    result.Append("▼");

                                                    resultBackup += tempInvestor.ClientCommandQueue[0] + "▼";

                                                    if (tempInvestor.ClientCommandQueue[0] == "OLOFF14790251")
                                                    {
                                                        tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                        continue;
                                                    }

                                                    tempResult.Add(tempInvestor.ClientCommandQueue[0]);
                                                    tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    tempInvestor.ClientCommandQueue.RemoveAt(0);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                
                                            }
                                        }
                                    }
                                    #endregion

                                    #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                                    StringBuilder _strMessage = tempInvestor.AddCommandToInvestorOnline(investorID, key, tempResult);
                                    result.Append(_strMessage);

                                    //if (Business.Market.InvestorOnline != null)
                                    //{
                                    //    for (int n = 0; n < Business.Market.InvestorOnline.Count; n++)
                                    //    {
                                    //        if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey != key)
                                    //        {
                                    //            if (tempResult != null)
                                    //            {
                                    //                int countMessage = tempResult.Count;
                                    //                for (int m = 0; m < countMessage; m++)
                                    //                {
                                    //                    Business.Market.InvestorOnline[n].ClientCommandQueue.Add(tempResult[m]);
                                    //                }
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey == key)
                                    //            {
                                    //                if (Business.Market.InvestorOnline[n].ClientCommandQueue != null)
                                    //                {
                                    //                    for (int m = 0; m < Business.Market.InvestorOnline[n].ClientCommandQueue.Count; m++)
                                    //                    {
                                    //                        try
                                    //                        {
                                    //                            if (Business.Market.InvestorOnline[n].ClientCommandQueue[0] != null)
                                    //                            {
                                    //                                result.Append(Business.Market.InvestorOnline[n].ClientCommandQueue[0]);
                                    //                                result.Append("▼");

                                    //                                Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                    //                            }
                                    //                        }
                                    //                        catch (Exception ex)
                                    //                        {
                                    //                            TradingServer.Model.TradingCalculate.Instance.WriteLogFile("[Error - FacadeNewGetData] ->" + ex.Message, "errorET5", "ManagerAPI");
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    #region PROCESS DATA BACKUP
                                    //StringBuilder _tempBackupData = ClientFacade.ProcessBackupMessage(keyConfirm, resultBackup, tempInvestor);
                                    //result.Append(_tempBackupData);

                                    //if (port == 0)
                                    //{
                                    //    string backupCommand = string.Empty;

                                    //    if (tempInvestor.BackupQueue != null)
                                    //    {
                                    //        int countBackup = tempInvestor.BackupQueue.Count;
                                    //        for (int n = 0; n < tempInvestor.BackupQueue.Count; n++)
                                    //        {
                                    //            if (tempInvestor.BackupQueue[n].Key == keyConfirm)
                                    //            {
                                    //                tempInvestor.BackupQueue.RemoveAt(n);
                                    //                n--;
                                    //            }
                                    //            else
                                    //            {
                                    //                backupCommand += tempInvestor.BackupQueue[n].CommandQueue + "▼";
                                    //                tempInvestor.BackupQueue.RemoveAt(n);
                                    //                n--;
                                    //            }
                                    //        }
                                    //    }

                                    //    if (backupCommand.EndsWith("▼"))
                                    //        backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                    //    string keyBackup = DateTime.Now.Ticks.ToString();
                                    //    if (!string.IsNullOrEmpty(backupCommand)){
                                    //        result.Append("▼");
                                    //        result.Append(backupCommand);
                                    //        result.Append(">");
                                    //        result.Append(keyBackup);
                                    //    }
                                    //    else
                                    //    {
                                    //        result.Append(">");
                                    //        result.Append(keyBackup);
                                    //    }   

                                    //    //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                    //    Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                    //    newBackupQueue.Key = keyBackup;
                                    //    newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                    //    tempInvestor.BackupQueue.Add(newBackupQueue);
                                    //}
                                    #endregion

                                    flag = true;
                                    isValidKey = true;
                                    break;
                                }
                            }
                        }

                        if (!flag)
                        {
                            #region GET MESSAGE SERVER FROM INVESTOR ONLINE
                            //if (Business.Market.InvestorOnline != null)
                            //{
                            //    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                            //    {
                            //        if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                            //        {
                            //            if (Business.Market.InvestorOnline[i].ClientCommandQueue != null)
                            //            {
                            //                int countMessage = Business.Market.InvestorOnline[i].ClientCommandQueue.Count;
                            //                for (int j = 0; j < Business.Market.InvestorOnline[i].ClientCommandQueue.Count; j++)
                            //                {
                            //                    try
                            //                    {
                            //                        if (Business.Market.InvestorOnline[i].ClientCommandQueue[0] != null)
                            //                        {
                            //                            result.Append(Business.Market.InvestorOnline[i].ClientCommandQueue[0]);
                            //                            result.Append("▼");

                            //                            resultBackup += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";

                            //                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                            //                        }
                            //                        else
                            //                        {
                            //                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                            //                        }
                            //                    }
                            //                    catch (Exception ex)
                            //                    {
                            //                        TradingServer.Model.TradingCalculate.Instance.WriteLogFile("[Error - FacadeNewGetData] ->" + ex.Message, "errorET5", "ManagerAPI");
                            //                    }
                            //                }
                            //            }

                            //            #region PROCESS DATA BACKUP
                            //            if (port == 0)
                            //            {
                            //                string backupCommand = string.Empty;

                            //                if (Business.Market.InvestorOnline[i].BackupQueue != null)
                            //                {
                            //                    int countBackup = Business.Market.InvestorOnline[i].BackupQueue.Count;
                            //                    for (int n = 0; n < Business.Market.InvestorOnline[i].BackupQueue.Count; n++)
                            //                    {
                            //                        if (Business.Market.InvestorOnline[i].BackupQueue[n].Key == keyConfirm)
                            //                        {
                            //                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                            //                            n--;
                            //                        }
                            //                        else
                            //                        {
                            //                            backupCommand += Business.Market.InvestorOnline[i].BackupQueue[n].CommandQueue + "▼";
                            //                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                            //                            n--;
                            //                        }
                            //                    }
                            //                }

                            //                if (backupCommand.EndsWith("▼"))
                            //                    backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                            //                string keyBackup = DateTime.Now.Ticks.ToString();
                            //                if (!string.IsNullOrEmpty(backupCommand)){
                            //                    result.Append("▼");
                            //                    result.Append(backupCommand);
                            //                    result.Append(">");
                            //                    result.Append(keyBackup);
                            //                }
                            //                else
                            //                {
                            //                    result.Append(">");
                            //                    result.Append(keyBackup);
                            //                }   

                            //                //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                            //                Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                            //                newBackupQueue.Key = keyBackup;
                            //                newBackupQueue.CommandQueue = resultBackup + backupCommand;
                            //                Business.Market.InvestorOnline[i].BackupQueue.Add(newBackupQueue);
                            //            }
                            //            #endregion

                            //            isValidKey = true;
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                    }
                    #endregion
                }               
            }
            else
            {
                #region ACCOUNT READ ONLY
                if (Business.Market.InvestorOnline != null)
                {
                    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                        {
                            Business.Investor _tempInvestorOnline = Business.Market.InvestorOnline[i];
                            _tempInvestorOnline.LastConnectTime = DateTime.Now;
                            _tempInvestorOnline.numTimeOut = 30;

                            StringBuilder temp = ClientFacade.GetNewTickOnInvestorOnline(_tempInvestorOnline, port);

                            #region REMOVE CHAR
                            if (temp.Length > 1)
                            {
                                temp = temp.Remove(temp.Length - 1, 1);
                            }
                            result = temp;
                            result.Append("◊");
                            #endregion

                            if (_tempInvestorOnline.ClientCommandQueue != null)
                            {
                                int countMessage = _tempInvestorOnline.ClientCommandQueue.Count;
                                for (int j = 0; j < _tempInvestorOnline.ClientCommandQueue.Count; j++)
                                {
                                    try
                                    {
                                        if (_tempInvestorOnline.ClientCommandQueue[j] == "OLOFF14790251")
                                        {
                                            _tempInvestorOnline.ClientCommandQueue.RemoveAt(j);
                                            j--;
                                            continue;
                                        }

                                        if (_tempInvestorOnline.ClientCommandQueue[0] != null)
                                        {
                                            result.Append(_tempInvestorOnline.ClientCommandQueue[0]);
                                            result.Append("▼");

                                            resultBackup += _tempInvestorOnline.ClientCommandQueue[0] + "▼";

                                            _tempInvestorOnline.ClientCommandQueue.RemoveAt(0);
                                        }
                                        else
                                        {
                                            _tempInvestorOnline.ClientCommandQueue.RemoveAt(0);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    }
                                }
                            }

                            #region PROCESS DATA BACKUP
                            //if (port == 0)
                            //{
                            //    string backupCommand = string.Empty;

                            //    if (_tempInvestorOnline.BackupQueue != null)
                            //    {
                            //        int countBackup = _tempInvestorOnline.BackupQueue.Count;
                            //        for (int n = 0; n < _tempInvestorOnline.BackupQueue.Count; n++)
                            //        {
                            //            if (_tempInvestorOnline.BackupQueue[n].Key == keyConfirm)
                            //            {
                            //                _tempInvestorOnline.BackupQueue.RemoveAt(n);
                            //                n--;
                            //            }
                            //            else
                            //            {
                            //                backupCommand += _tempInvestorOnline.BackupQueue[n].CommandQueue + "▼";
                            //                _tempInvestorOnline.BackupQueue.RemoveAt(n);
                            //                n--;
                            //            }
                            //        }
                            //    }

                            //    if (backupCommand.EndsWith("▼"))
                            //        backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                            //    string keyBackup = DateTime.Now.Ticks.ToString();

                            //    if (!string.IsNullOrEmpty(backupCommand)){
                            //        result.Append("▼");
                            //        result.Append(backupCommand);
                            //        result.Append(">");
                            //        result.Append(keyBackup);
                            //    }
                            //    else
                            //    {
                            //        result.Append(">");
                            //        result.Append(keyBackup);
                            //    }   

                            //    //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                            //    Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                            //    newBackupQueue.Key = keyBackup;
                            //    newBackupQueue.CommandQueue = resultBackup + backupCommand;
                            //    _tempInvestorOnline.BackupQueue.Add(newBackupQueue);
                            //}
                            #endregion

                            isValidKey = true;
                        }
                    }
                }
                #endregion
            }

            if (!isValidKey)
            {
                result.Append("OLOFF14790251");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="investorName"></param>
        /// <param name="investorIndex"></param>
        /// <returns></returns>
        public static string FacadeGetData(int investorID, int investorIndex, string key, int port, string keyConfirm)
        {
            string result = string.Empty;
            string resultBackup = string.Empty;
            string resultCommand = string.Empty;
            List<string> tempResult = new List<string>();
            result = ClientFacade.GetNewTickOnInvestorOnline(investorID, key, port);

            Business.TypeLogin type = ClientFacade.GetTypeLogin(investorID, key);

            #region REMOVE CHAR
            //Remove Charater end "♦"
            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }
            //Add Character Start "◊"
            result += "◊";
            #endregion

            bool isValidKey = false;
            if (type == Business.TypeLogin.Primary)
            {
                Business.Investor newInvestor = new Business.Investor();
                if (investorIndex >= 0 && investorIndex <= Business.Market.InvestorList.Count - 1)
                {
                    newInvestor = Business.Market.InvestorList[investorIndex];
                    if (newInvestor != null && newInvestor.InvestorID > 0 && newInvestor.InvestorID == investorID && newInvestor.LoginKey == key)
                    {
                        #region CHECK STATUS INVESTOR
                        if (!newInvestor.IsOnline)
                        {
                            if (newInvestor.IsLogout)
                                return string.Empty;

                            newInvestor.IsOnline = true;

                            newInvestor.AddInvestorToInvestorOnline(newInvestor);
                        }
                        #endregion

                        Business.Market.InvestorList[investorIndex].LastConnectTime = DateTime.Now;

                        #region GET CLIENT COMMAND SERVER
                        if (newInvestor.ClientCommandQueue != null)
                        {
                            int countMessage = newInvestor.ClientCommandQueue.Count;
                            for (int n = 0; n < newInvestor.ClientCommandQueue.Count; n++)
                            {
                                try
                                {
                                    if (newInvestor.ClientCommandQueue[0] != null)
                                    {
                                        result += newInvestor.ClientCommandQueue[0] + "▼";
                                        resultBackup += newInvestor.ClientCommandQueue[0] + "▼";

                                        if (newInvestor.ClientCommandQueue[0] == "OLOFF14790251")
                                        {
                                            newInvestor.ClientCommandQueue.RemoveAt(0);
                                            continue;
                                        }
                                        else
                                        {
                                            tempResult.Add(newInvestor.ClientCommandQueue[0]);
                                            newInvestor.ClientCommandQueue.RemoveAt(0);
                                        }

                                        //newInvestor.ClientCommandQueue.RemoveAt(0);
                                    }
                                    else
                                    {
                                        newInvestor.ClientCommandQueue.RemoveAt(0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string _temp = ex.Message + " - GetData";
                                    TradingServer.Facade.FacadeAddNewSystemLog(6, _temp, "[Exception]", "", newInvestor.Code);
                                }
                            }
                        }
                        #endregion

                        #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                        if (Business.Market.InvestorOnline != null)
                        {
                            for (int n = 0; n < Business.Market.InvestorOnline.Count; n++)
                            {
                                if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey != key)
                                {
                                    if (tempResult != null)
                                    {
                                        int countMessage = tempResult.Count;
                                        for (int m = 0; m < countMessage; m++)
                                        {
                                            Business.Market.InvestorOnline[n].ClientCommandQueue.Add(tempResult[m]);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey == key)
                                    {
                                        if (Business.Market.InvestorOnline[n].ClientCommandQueue != null)
                                        {
                                            for (int m = 0; m < Business.Market.InvestorOnline[n].ClientCommandQueue.Count; m++)
                                            {
                                                try
                                                {
                                                    if (Business.Market.InvestorOnline[n].ClientCommandQueue[0] != null)
                                                    {
                                                        result += Business.Market.InvestorOnline[n].ClientCommandQueue[0] + "▼";
                                                        Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                    else
                                                    {
                                                        Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region PROCESS DATA BACKUP
                        if (port == 0)
                        {
                            string backupCommand = string.Empty;

                            if (newInvestor.BackupQueue != null)
                            {
                                int countBackup = newInvestor.BackupQueue.Count;
                                for (int n = 0; n < newInvestor.BackupQueue.Count; n++)
                                {
                                    if (newInvestor.BackupQueue[n].Key == keyConfirm)
                                    {
                                        newInvestor.BackupQueue.RemoveAt(n);
                                        n--;
                                    }
                                    else
                                    {
                                        backupCommand += newInvestor.BackupQueue[n].CommandQueue + "▼";
                                        newInvestor.BackupQueue.RemoveAt(n);
                                        n--;
                                    }
                                }
                            }

                            if (backupCommand.EndsWith("▼"))
                                backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                            string keyBackup = DateTime.Now.Ticks.ToString();
                            if (!string.IsNullOrEmpty(backupCommand))
                                result += "▼" + backupCommand + ">" + keyBackup;
                            else
                                result += ">" + keyBackup;

                            //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                            Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                            newBackupQueue.Key = keyBackup;
                            newBackupQueue.CommandQueue = resultBackup + backupCommand;
                            newInvestor.BackupQueue.Add(newBackupQueue);
                        }
                        #endregion

                        isValidKey = true;
                    }
                    else
                    {
                        #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                        if (Business.Market.InvestorList != null)
                        {
                            bool flag = false;
                            int countInvestor = Business.Market.InvestorList.Count;
                            for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                            {
                                if (Business.Market.InvestorList[j].LoginKey != null)
                                {
                                    if (Business.Market.InvestorList[j].InvestorID == investorID &&
                                        Business.Market.InvestorList[j].LoginKey == key)
                                    {
                                        #region CHECK STATUS INVESTOR
                                        if (!Business.Market.InvestorList[j].IsOnline)
                                        {
                                            if (Business.Market.InvestorList[j].IsLogout)
                                                return string.Empty;

                                            Business.Market.InvestorList[j].IsOnline = true;

                                            Business.Market.InvestorList[j].AddInvestorToInvestorOnline(Business.Market.InvestorList[j]);
                                        }
                                        #endregion

                                        Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                                        Business.Market.InvestorList[j].InvestorIndex = j;

                                        //SEND COMMAND UPDATE INVESTOR INDEX
                                        if (Business.Market.InvestorList[j].ClientCommandQueue == null)
                                            Business.Market.InvestorList[j].ClientCommandQueue = new List<string>();

                                        string messageChangeIndex = "CII00434543$" + Business.Market.InvestorList[j].InvestorIndex;

                                        Business.Market.InvestorList[j].ClientCommandQueue.Add(messageChangeIndex);

                                        #region GET CLIENT COMMAND SERVER
                                        if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                                        {
                                            int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                            for (int n = 0; n < Business.Market.InvestorList[j].ClientCommandQueue.Count; n++)
                                            {
                                                try
                                                {
                                                    if (Business.Market.InvestorList[j].ClientCommandQueue[0] != null)
                                                    {
                                                        result += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";
                                                        resultBackup += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";

                                                        if (Business.Market.InvestorList[j].ClientCommandQueue[0] == "OLOFF14790251")
                                                        {
                                                            Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                            continue;
                                                        }

                                                        tempResult.Add(Business.Market.InvestorList[j].ClientCommandQueue[0]);
                                                        Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                    else
                                                    {
                                                        Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                        #endregion

                                        #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                                        if (Business.Market.InvestorOnline != null)
                                        {
                                            for (int n = 0; n < Business.Market.InvestorOnline.Count; n++)
                                            {
                                                if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey != key)
                                                {
                                                    if (tempResult != null)
                                                    {
                                                        int countMessage = tempResult.Count;
                                                        for (int m = 0; m < countMessage; m++)
                                                        {
                                                            Business.Market.InvestorOnline[n].ClientCommandQueue.Add(tempResult[m]);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey == key)
                                                    {
                                                        if (Business.Market.InvestorOnline[n].ClientCommandQueue != null)
                                                        {
                                                            for (int m = 0; m < Business.Market.InvestorOnline[n].ClientCommandQueue.Count; m++)
                                                            {
                                                                try
                                                                {
                                                                    if (Business.Market.InvestorOnline[n].ClientCommandQueue[0] != null)
                                                                    {
                                                                        result += Business.Market.InvestorOnline[n].ClientCommandQueue[0] + "▼";
                                                                        Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region PROCESS DATA BACKUP
                                        if (port == 0)
                                        {
                                            string backupCommand = string.Empty;

                                            if (Business.Market.InvestorList[j].BackupQueue != null)
                                            {
                                                int countBackup = Business.Market.InvestorList[j].BackupQueue.Count;
                                                for (int n = 0; n < Business.Market.InvestorList[j].BackupQueue.Count; n++)
                                                {
                                                    if (Business.Market.InvestorList[j].BackupQueue[n].Key == keyConfirm)
                                                    {
                                                        Business.Market.InvestorList[j].BackupQueue.RemoveAt(n);
                                                        n--;
                                                    }
                                                    else
                                                    {
                                                        backupCommand += Business.Market.InvestorList[j].BackupQueue[n].CommandQueue + "▼";
                                                        Business.Market.InvestorList[j].BackupQueue.RemoveAt(n);
                                                        n--;
                                                    }
                                                }
                                            }

                                            if (backupCommand.EndsWith("▼"))
                                                backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                            string keyBackup = DateTime.Now.Ticks.ToString();
                                            if (!string.IsNullOrEmpty(backupCommand))
                                                result += "▼" + backupCommand + ">" + keyBackup;
                                            else
                                                result += ">" + keyBackup;

                                            //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                            Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                            newBackupQueue.Key = keyBackup;
                                            newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                            Business.Market.InvestorList[j].BackupQueue.Add(newBackupQueue);
                                        }
                                        #endregion

                                        flag = true;
                                        isValidKey = true;
                                        break;
                                    }
                                }
                            }

                            if (!flag)
                            {
                                #region GET MESSAGE SERVER FROM INVESTOR ONLINE
                                if (Business.Market.InvestorOnline != null)
                                {
                                    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                                    {
                                        if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                                        {
                                            if (Business.Market.InvestorOnline[i].ClientCommandQueue != null)
                                            {
                                                int countMessage = Business.Market.InvestorOnline[i].ClientCommandQueue.Count;
                                                for (int j = 0; j < Business.Market.InvestorOnline[i].ClientCommandQueue.Count; j++)
                                                {
                                                    try
                                                    {
                                                        if (Business.Market.InvestorOnline[i].ClientCommandQueue[0] != null)
                                                        {
                                                            result += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";
                                                            resultBackup += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";

                                                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                                        }
                                                        else
                                                        {
                                                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                            #region PROCESS DATA BACKUP
                                            if (port == 0)
                                            {
                                                string backupCommand = string.Empty;

                                                if (Business.Market.InvestorOnline[i].BackupQueue != null)
                                                {
                                                    int countBackup = Business.Market.InvestorOnline[i].BackupQueue.Count;
                                                    for (int n = 0; n < Business.Market.InvestorOnline[i].BackupQueue.Count; n++)
                                                    {
                                                        if (Business.Market.InvestorOnline[i].BackupQueue[n].Key == keyConfirm)
                                                        {
                                                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                                            n--;
                                                        }
                                                        else
                                                        {
                                                            backupCommand += Business.Market.InvestorOnline[i].BackupQueue[n].CommandQueue + "▼";
                                                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                                            n--;
                                                        }
                                                    }
                                                }

                                                if (backupCommand.EndsWith("▼"))
                                                    backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                                string keyBackup = DateTime.Now.Ticks.ToString();
                                                if (!string.IsNullOrEmpty(backupCommand))
                                                    result += "▼" + backupCommand + ">" + keyBackup;
                                                else
                                                    result += ">" + keyBackup;

                                                //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                                Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                                newBackupQueue.Key = keyBackup;
                                                newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                                Business.Market.InvestorOnline[i].BackupQueue.Add(newBackupQueue);
                                            }
                                            #endregion

                                            isValidKey = true;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                    if (Business.Market.InvestorList != null)
                    {
                        bool flag = false;
                        int countInvestor = Business.Market.InvestorList.Count;
                        for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                        {
                            if (Business.Market.InvestorList[j].LoginKey != null)
                            {
                                if (Business.Market.InvestorList[j].InvestorID == investorID &&
                                    Business.Market.InvestorList[j].LoginKey == key)
                                {
                                    #region CHECK STATUS INVESTOR
                                    if (!Business.Market.InvestorList[j].IsOnline)
                                    {
                                        if (Business.Market.InvestorList[j].IsLogout)
                                            return string.Empty;

                                        Business.Market.InvestorList[j].IsOnline = true;

                                        Business.Market.InvestorList[j].AddInvestorToInvestorOnline(Business.Market.InvestorList[j]);
                                    }
                                    #endregion

                                    Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                                    Business.Market.InvestorList[j].InvestorIndex = j;

                                    //SEND COMMAND UPDATE INVESTOR INDEX
                                    if (Business.Market.InvestorList[j].ClientCommandQueue == null)
                                        Business.Market.InvestorList[j].ClientCommandQueue = new List<string>();

                                    string messageChangeIndex = "CII00434543$" + Business.Market.InvestorList[j].InvestorIndex;
                                    Business.Market.InvestorList[j].ClientCommandQueue.Add(messageChangeIndex);

                                    #region GET CLIENT COMMAND SERVER
                                    if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                                    {
                                        int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                        for (int n = 0; n < Business.Market.InvestorList[j].ClientCommandQueue.Count; n++)
                                        {
                                            try
                                            {
                                                if (Business.Market.InvestorList[j].ClientCommandQueue[0] != null)
                                                {
                                                    result += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";
                                                    resultBackup += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";

                                                    if (Business.Market.InvestorList[j].ClientCommandQueue[0] == "OLOFF14790251")
                                                    {
                                                        Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                        continue;
                                                    }

                                                    tempResult.Add(Business.Market.InvestorList[j].ClientCommandQueue[0]);
                                                    Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    #endregion

                                    #region ADD COMMAND TO INVESTOR LOGIN WITH PASSWORD READ ONLY
                                    if (Business.Market.InvestorOnline != null)
                                    {
                                        for (int n = 0; n < Business.Market.InvestorOnline.Count; n++)
                                        {
                                            if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey != key)
                                            {
                                                if (tempResult != null)
                                                {
                                                    int countMessage = tempResult.Count;
                                                    for (int m = 0; m < countMessage; m++)
                                                    {
                                                        Business.Market.InvestorOnline[n].ClientCommandQueue.Add(tempResult[m]);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Business.Market.InvestorOnline[n].InvestorID == investorID && Business.Market.InvestorOnline[n].LoginKey == key)
                                                {
                                                    if (Business.Market.InvestorOnline[n].ClientCommandQueue != null)
                                                    {
                                                        for (int m = 0; m < Business.Market.InvestorOnline[n].ClientCommandQueue.Count; m++)
                                                        {
                                                            try
                                                            {
                                                                if (Business.Market.InvestorOnline[n].ClientCommandQueue[0] != null)
                                                                {
                                                                    result += Business.Market.InvestorOnline[n].ClientCommandQueue[0] + "▼";
                                                                    Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                                }
                                                                else
                                                                {
                                                                    Business.Market.InvestorOnline[n].ClientCommandQueue.RemoveAt(0);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region PROCESS DATA BACKUP
                                    if (port == 0)
                                    {
                                        string backupCommand = string.Empty;

                                        if (Business.Market.InvestorList[j].BackupQueue != null)
                                        {
                                            int countBackup = Business.Market.InvestorList[j].BackupQueue.Count;
                                            for (int n = 0; n < Business.Market.InvestorList[j].BackupQueue.Count; n++)
                                            {
                                                if (Business.Market.InvestorList[j].BackupQueue[n].Key == keyConfirm)
                                                {
                                                    Business.Market.InvestorList[j].BackupQueue.RemoveAt(n);
                                                    n--;
                                                }
                                                else
                                                {
                                                    backupCommand += Business.Market.InvestorList[j].BackupQueue[n].CommandQueue + "▼";
                                                    Business.Market.InvestorList[j].BackupQueue.RemoveAt(n);
                                                    n--;
                                                }
                                            }
                                        }

                                        if (backupCommand.EndsWith("▼"))
                                            backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                        string keyBackup = DateTime.Now.Ticks.ToString();
                                        if (!string.IsNullOrEmpty(backupCommand))
                                            result += "▼" + backupCommand + ">" + keyBackup;
                                        else
                                            result += ">" + keyBackup;

                                        //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                        Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                        newBackupQueue.Key = keyBackup;
                                        newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                        Business.Market.InvestorList[j].BackupQueue.Add(newBackupQueue);
                                    }
                                    #endregion

                                    flag = true;
                                    isValidKey = true;
                                    break;
                                }
                            }
                        }

                        if (!flag)
                        {
                            #region GET MESSAGE SERVER FROM INVESTOR ONLINE
                            if (Business.Market.InvestorOnline != null)
                            {
                                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                                {
                                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                                    {
                                        if (Business.Market.InvestorOnline[i].ClientCommandQueue != null)
                                        {
                                            int countMessage = Business.Market.InvestorOnline[i].ClientCommandQueue.Count;
                                            for (int j = 0; j < Business.Market.InvestorOnline[i].ClientCommandQueue.Count; j++)
                                            {
                                                try
                                                {
                                                    if (Business.Market.InvestorOnline[i].ClientCommandQueue[0] != null)
                                                    {
                                                        result += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";
                                                        resultBackup += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";

                                                        Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                    else
                                                    {
                                                        Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }

                                        #region PROCESS DATA BACKUP
                                        if (port == 0)
                                        {
                                            string backupCommand = string.Empty;

                                            if (Business.Market.InvestorOnline[i].BackupQueue != null)
                                            {
                                                int countBackup = Business.Market.InvestorOnline[i].BackupQueue.Count;
                                                for (int n = 0; n < Business.Market.InvestorOnline[i].BackupQueue.Count; n++)
                                                {
                                                    if (Business.Market.InvestorOnline[i].BackupQueue[n].Key == keyConfirm)
                                                    {
                                                        Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                                        n--;
                                                    }
                                                    else
                                                    {
                                                        backupCommand += Business.Market.InvestorOnline[i].BackupQueue[n].CommandQueue + "▼";
                                                        Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                                        n--;
                                                    }
                                                }
                                            }

                                            if (backupCommand.EndsWith("▼"))
                                                backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                            string keyBackup = DateTime.Now.Ticks.ToString();
                                            if (!string.IsNullOrEmpty(backupCommand))
                                                result += "▼" + backupCommand + ">" + keyBackup;
                                            else
                                                result += ">" + keyBackup;

                                            //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                            Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                            newBackupQueue.Key = keyBackup;
                                            newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                            Business.Market.InvestorOnline[i].BackupQueue.Add(newBackupQueue);
                                        }
                                        #endregion

                                        isValidKey = true;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            else
            {
                #region ACCOUNT READ ONLY
                if (Business.Market.InvestorOnline != null)
                {
                    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                        {
                            if (Business.Market.InvestorOnline[i].ClientCommandQueue != null)
                            {
                                int countMessage = Business.Market.InvestorOnline[i].ClientCommandQueue.Count;
                                for (int j = 0; j < countMessage; j++)
                                {
                                    try
                                    {
                                        if (Business.Market.InvestorOnline[i].ClientCommandQueue[0] != null)
                                        {
                                            result += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";
                                            resultBackup += Business.Market.InvestorOnline[i].ClientCommandQueue[0] + "▼";

                                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                        }
                                        else
                                        {
                                            Business.Market.InvestorOnline[i].ClientCommandQueue.RemoveAt(0);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }

                            #region PROCESS DATA BACKUP
                            if (port == 0)
                            {
                                string backupCommand = string.Empty;

                                if (Business.Market.InvestorOnline[i].BackupQueue != null)
                                {
                                    int countBackup = Business.Market.InvestorOnline[i].BackupQueue.Count;
                                    for (int n = 0; n < Business.Market.InvestorOnline[i].BackupQueue.Count; n++)
                                    {
                                        if (Business.Market.InvestorOnline[i].BackupQueue[n].Key == keyConfirm)
                                        {
                                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                            n--;
                                        }
                                        else
                                        {
                                            backupCommand += Business.Market.InvestorOnline[i].BackupQueue[n].CommandQueue + "▼";
                                            Business.Market.InvestorOnline[i].BackupQueue.RemoveAt(n);
                                            n--;
                                        }
                                    }
                                }

                                if (backupCommand.EndsWith("▼"))
                                    backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

                                string keyBackup = DateTime.Now.Ticks.ToString();

                                if (!string.IsNullOrEmpty(backupCommand))
                                    result += "▼" + backupCommand + ">" + keyBackup;
                                else
                                    result += ">" + keyBackup;

                                //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
                                Business.BackupQueue newBackupQueue = new Business.BackupQueue();
                                newBackupQueue.Key = keyBackup;
                                newBackupQueue.CommandQueue = resultBackup + backupCommand;
                                Business.Market.InvestorOnline[i].BackupQueue.Add(newBackupQueue);
                            }
                            #endregion

                            isValidKey = true;
                        }
                    }
                }
                #endregion
            }

            #region REMOVE CHAR
            //if (result.Length > 1)
            //{
            //    result = result.Remove(result.Length - 1);
            //}
            #endregion

            if (!isValidKey)
            {
                result = "OLOFF14790251";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyConfirm"></param>
        /// <param name="resultBackup"></param>
        /// <param name="newInvestor"></param>
        /// <returns></returns>
        private static StringBuilder ProcessBackupMessage(string keyConfirm, string resultBackup, Business.Investor newInvestor)
        {
            StringBuilder result = new StringBuilder();

            string backupCommand = string.Empty;

            if (newInvestor.BackupQueue != null)
            {
                int countBackup = newInvestor.BackupQueue.Count;
                for (int n = 0; n < newInvestor.BackupQueue.Count; n++)
                {
                    if (newInvestor.BackupQueue[n].Key == keyConfirm)
                    {
                        newInvestor.BackupQueue.RemoveAt(n);
                        n--;
                    }
                    else
                    {
                        backupCommand += newInvestor.BackupQueue[n].CommandQueue + "▼";
                        newInvestor.BackupQueue.RemoveAt(n);
                        n--;
                    }
                }
            }

            if (backupCommand.EndsWith("▼"))
                backupCommand = backupCommand.Remove(backupCommand.Length - 1, 1);

            string keyBackup = DateTime.Now.Ticks.ToString();
            if (!string.IsNullOrEmpty(backupCommand))
            {
                result.Append("▼");
                result.Append(backupCommand);
                result.Append(">");
                result.Append(keyBackup);
            }
            else
            {
                result.Append(">");
                result.Append(keyBackup);
            }

            //ADD COMMAND TO BACKUP QUEUE OF INVESTOR
            Business.BackupQueue newBackupQueue = new Business.BackupQueue();
            newBackupQueue.Key = keyBackup;
            newBackupQueue.CommandQueue = resultBackup + backupCommand;
            newInvestor.BackupQueue.Add(newBackupQueue);

            return result;
        }

        /// <summary>
        /// COMMENT
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="investorName"></param>
        /// <param name="investorIndex"></param>
        /// <returns></returns>
        public static string FacadeWebGetData(int investorID, string investorName, int investorIndex)
        {
            string result = string.Empty;
            
            result = ClientFacade.GetWebTickOnInvestorOnline(investorID);

            #region REMOVE CHAR
            //Remove Charater end "♦"
            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }
            //Add Character Start "◊"
            result += "◊";
            #endregion
           
            #region Get Message Server To Client
            if (Business.Market.InvestorList != null)
            {
                #region SEARCH INVESTOR IN INVESTOR LIST OF CLASS MARKET
                if (Business.Market.InvestorList != null)
                {
                    int countInvestor = Business.Market.InvestorList.Count;
                    for (int j = 0; j < countInvestor; j++)
                    {
                        if (Business.Market.InvestorList[j].InvestorID == investorID)
                        {
                            if (!Business.Market.InvestorList[j].IsOnline)
                            {
                                if (Business.Market.InvestorList[j].IsLogout)
                                    return result;

                                Business.Market.InvestorList[j].IsOnline = true;
                            }

                            Business.Market.InvestorList[j].LastConnectTime = DateTime.Now;
                            if (Business.Market.InvestorList[j].ClientCommandQueue != null)
                            {
                                int countMessage = Business.Market.InvestorList[j].ClientCommandQueue.Count;
                                for (int n = 0; n < countMessage; n++)
                                {
                                    try
                                    {
                                        if (Business.Market.InvestorList[j].ClientCommandQueue[0] != null)
                                        {
                                            result += Business.Market.InvestorList[j].ClientCommandQueue[0] + "▼";
                                            Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                        }
                                        else
                                        {
                                            Business.Market.InvestorList[j].ClientCommandQueue.RemoveAt(0);
                                        }
                                        //n--;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }

                            break;
                        }
                    }
                }
                #endregion
            }
            #endregion                     

            #region REMOVE CHAR
            if (result.Length > 1)
            {
                result = result.Remove(result.Length - 1);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        private static string GetTickOnInvestorOnline(int investorID)
        {
            string result = string.Empty;
            bool flagInvestor = false;

            #region GET TICK OF INVESTOR
            if (Business.Market.InvestorOnline != null)
            {
                int count = Business.Market.InvestorOnline.Count;
                for (int i = 0; i < count; i++)
                {
                    //if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID)
                    {
                        #region COMMENT CODE
                        Business.Market.InvestorOnline[i].numTimeOut = 30;
                        if (Business.Market.InvestorOnline[i].TickInvestor != null && Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                        {
                            #region COMMENT
                            while (Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                            {
                                try
                                {
                                    if (Business.Market.InvestorOnline[i].TickInvestor[0] != null)
                                    {
                                        result += Business.Market.InvestorOnline[i].TickInvestor[0].Bid + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].HighInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].LowInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Status + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].SymbolName + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].TickTime.Ticks + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Ask + "♦";

                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                    else
                                    {
                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            #endregion
                        }
                        #endregion

                        flagInvestor = true;
                        break;
                    }
                }
            }
            #endregion  

            if (!flagInvestor)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            Business.Investor newInvestor = new Business.Investor();
                            newInvestor.InvestorID = investorID;
                            newInvestor.LastConnectTime = DateTime.Now;
                            newInvestor.numTimeOut = 30;
                            newInvestor.TickInvestor = new List<Business.Tick>();
                            newInvestor.Code = Business.Market.InvestorList[i].Code;
                            newInvestor.IsLogout = false;
                            newInvestor.LoginKey = Model.ValidateCheck.RandomKeyLogin(8);                            
                            newInvestor.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;

                            Business.Market.InvestorOnline.Add(newInvestor);

                            //set islogout for investor list
                            Business.Market.InvestorList[i].IsLogout = false;

                            TradingServer.Facade.FacadeSendNotifyManagerRequest(1, newInvestor);

                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        private static string GetNewTickOnInvestorOnline(int investorID, string key,int port)
        {
            string result = string.Empty;
            bool flagInvestor = false;

            #region GET TICK OF INVESTOR
            if (Business.Market.InvestorOnline != null)
            {
                int count = Business.Market.InvestorOnline.Count;
                for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                {
                    if (Business.Market.InvestorOnline[i].LoginKey == null)
                    {
                        string content = "Login key of investor null: " + Business.Market.InvestorOnline[i].InvestorID + " Code: " +
                            Business.Market.InvestorOnline[i].Code;

                        TradingServer.Facade.FacadeAddNewSystemLog(5, content, "[login key null]", "", Business.Market.InvestorOnline[i].Code);
                    }

                    if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                    {
                        #region COMMENT CODE
                        Business.Market.InvestorOnline[i].numTimeOut = 30;
                        if (Business.Market.InvestorOnline[i].TickInvestor != null && 
                            Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                        {
                            #region COMMENT
                            while (Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                            {
                                try
                                {
                                    if (Business.Market.InvestorOnline[i].TickInvestor[0] != null)
                                    {
                                        string time = "";
                                        if (port == 0)
                                            time = Business.Market.InvestorOnline[i].TickInvestor[0].TickTime + "▼";
                                        else
                                            time = Business.Market.InvestorOnline[i].TickInvestor[0].TickTime.Ticks + "▼";

                                        result += Business.Market.InvestorOnline[i].TickInvestor[0].Bid + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].HighInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].LowInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Status + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].SymbolName + "▼" +
                                            //Business.Market.InvestorOnline[i].TickInvestor[0].TickTime.Ticks + "▼" +
                                            time +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Ask + "♦";

                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                    else
                                    {
                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            #endregion
                        }
                        #endregion

                        flagInvestor = true;
                        break;
                    }
                }
            }
            #endregion

            if (!flagInvestor)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID && Business.Market.InvestorList[i].LoginKey == key)
                        {
                            Business.Investor newInvestor = new Business.Investor();
                            newInvestor.InvestorID = investorID;
                            newInvestor.LastConnectTime = DateTime.Now;
                            newInvestor.numTimeOut = 30;
                            newInvestor.TickInvestor = new List<Business.Tick>();
                            newInvestor.Code = Business.Market.InvestorList[i].Code;
                            newInvestor.IsLogout = false;
                            newInvestor.LoginKey = key;
                            newInvestor.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;

                            //set islogout for investor list
                            Business.Market.InvestorList[i].IsLogout = false;

                            if (Business.Market.InvestorList[i].InvestorGroupInstance.IsEnable && !Business.Market.InvestorList[i].IsDisable)
                            {
                                Business.Market.InvestorOnline.Add(newInvestor);
                                TradingServer.Facade.FacadeSendNotifyManagerRequest(1, newInvestor);
                            }   

                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        private static StringBuilder GetNewTickOnInvestorOnline(Business.Investor investor, int port)
        {
            StringBuilder result = new StringBuilder();

            #region COMMENT CODE
            investor.numTimeOut = 30;
            if (investor.TickInvestor != null &&
                investor.TickInvestor.Count > 0)
            {
                #region COMMENT
                while (investor.TickInvestor.Count > 0)
                {
                    try
                    {
                        if (investor.TickInvestor[0] != null)
                        {
                            Business.Tick temp = investor.TickInvestor[0];
                            string time = "";
                            if (port == 0)
                                time = temp.TickTime + "▼";
                            else
                                time = temp.TickTime.Ticks + "▼";

                            result.Append(temp.Bid + "▼" +
                                temp.HighInDay + "▼" +
                                temp.LowInDay + "▼" +
                                temp.Status + "▼" +
                                temp.SymbolName + "▼" +
                                time +
                                temp.Ask + "♦");

                            investor.TickInvestor.RemoveAt(0);
                        }
                        else
                        {
                            investor.TickInvestor.RemoveAt(0);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                #endregion
            }
            #endregion

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        private static Business.TypeLogin GetTypeLogin(int investorID,string key)
        {
            Business.TypeLogin result = new Business.TypeLogin();
            if (Business.Market.InvestorOnline != null)
            {
                int count = Business.Market.InvestorOnline.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID
                        && Business.Market.InvestorOnline[i].LoginKey == key)
                    {
                        result = Business.Market.InvestorOnline[i].LoginType;

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        //private static Business.TypeLogin GetTypeLogin(int investorID, string loginKey)
        //{
        //    Business.TypeLogin result = new Business.TypeLogin();
        //    if (Business.Market.InvestorOnline != null)
        //    {
        //        int count = Business.Market.InvestorOnline.Count;
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (Business.Market.InvestorOnline[i].SessionInstance != null)
        //            {
        //                if (Business.Market.InvestorOnline[i].InvestorID == investorID &&
        //                    Business.Market.InvestorOnline[i].LoginKey == loginKey)
        //                {
        //                    result = Business.Market.InvestorOnline[i].LoginType;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        private static string GetWebTickOnInvestorOnline(int investorID)
        {
            string result = string.Empty;
            bool flagInvestor = false;

            #region GET TICK OF INVESTOR
            if (Business.Market.InvestorOnline != null)
            {
                int count = Business.Market.InvestorOnline.Count;
                for (int i = 0; i < count; i++)
                {
                    //if (Business.Market.InvestorOnline[i].InvestorID == investorID && Business.Market.InvestorOnline[i].LoginKey == key)
                    if (Business.Market.InvestorOnline[i].InvestorID == investorID)
                    {
                        #region COMMENT CODE
                        Business.Market.InvestorOnline[i].numTimeOut = 30;
                        if (Business.Market.InvestorOnline[i].TickInvestor != null && Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                        {
                            #region COMMENT
                            while (Business.Market.InvestorOnline[i].TickInvestor.Count > 0)
                            {
                                try
                                {
                                    if (Business.Market.InvestorOnline[i].TickInvestor[0] != null)
                                    {
                                        result += Business.Market.InvestorOnline[i].TickInvestor[0].Bid + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].HighInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].LowInDay + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Status + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].SymbolName + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].TickTime + "▼" +
                                            Business.Market.InvestorOnline[i].TickInvestor[0].Ask + "♦";

                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                    else
                                    {
                                        Business.Market.InvestorOnline[i].TickInvestor.RemoveAt(0);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            #endregion
                        }
                        #endregion

                        flagInvestor = true;
                        break;
                    }
                }
            }
            #endregion

            if (!flagInvestor)
            {
                if (Business.Market.InvestorList != null)
                {
                    int count = Business.Market.InvestorList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Business.Market.InvestorList[i].InvestorID == investorID)
                        {
                            Business.Investor newInvestor = new Business.Investor();
                            newInvestor.InvestorID = investorID;
                            newInvestor.LastConnectTime = DateTime.Now;
                            newInvestor.numTimeOut = 30;
                            newInvestor.TickInvestor = new List<Business.Tick>();
                            newInvestor.Code = Business.Market.InvestorList[i].Code;
                            newInvestor.IsLogout = false;
                            newInvestor.LoginKey = Model.ValidateCheck.RandomKeyLogin(8);
                            newInvestor.InvestorGroupInstance = Business.Market.InvestorList[i].InvestorGroupInstance;

                            Business.Market.InvestorOnline.Add(newInvestor);

                            //set islogout for investor list
                            Business.Market.InvestorList[i].IsLogout = false;

                            TradingServer.Facade.FacadeSendNotifyManagerRequest(1, newInvestor);

                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long FacadeGetTimeZoneServer()
        {
            long result = 0;
            //TimeSpan spanTimeServer = TimeZoneInfo.Local.BaseUtcOffset;
            //result = spanTimeServer.Ticks;
            result = DateTime.Now.Ticks;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeFrame"></param>
        /// <param name="Symbol"></param>
        /// <returns></returns>
        public static string FacadeGetCandleOnline(int TimeFrame, string Symbol,int port)
        {
            return ProcessQuoteLibrary.Business.QuoteProcess.GetCandlesOnline(TimeFrame, Symbol, port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="Symbol"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string FacadeGetCandleOnlineFromDB(int timeFrame,string Symbol,int port)
        {
            return ProcessQuoteLibrary.Business.QuoteProcess.GetCandlesOnlineFromDB(timeFrame, Symbol, port);
        }

        #region GET INTERNAL MAIL TO INVESTOR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorCode"></param>
        /// <returns></returns>
        public static List<TradingServer.Business.InternalMail> FacadeGetTopInternalMailToInvestor(string investorCode)
        {
            return TradingServer.Facade.FacadeGetTopInternalMailToInvestor(investorCode);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static List<Business.ParameterItem> FacadeGetMarketConfig()
        {
            return Business.Market.MarketConfig;
        }
    }
}
