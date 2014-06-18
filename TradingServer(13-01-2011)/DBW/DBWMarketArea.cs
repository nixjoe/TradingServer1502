﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWMarketArea
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<IPresenter.IMarketArea> GetAllMarketArea()
        {
            List<IPresenter.IMarketArea> Result = new List<IPresenter.IMarketArea>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaTableAdapter adap = new DSTableAdapters.MarketAreaTableAdapter();
            DS.MarketAreaDataTable tbMarketArea = new DS.MarketAreaDataTable();
            TradingServer.DBW.DBWMarketAreaConfig newMarketAreaConfig = new DBWMarketAreaConfig();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbMarketArea = adap.GetData();

                if (tbMarketArea != null)
                {
                    int count = tbMarketArea.Count;
                    for (int i = 0; i < count; i++)
                    {
                        switch (tbMarketArea[i].Name)
                        {
                            case "SpotCommand":
                                {
                                    IPresenter.IMarketArea newIMarketArea = new Business.SpotCommand();
                                    newIMarketArea.IMarketAreaID = tbMarketArea[i].MartketAreaID;
                                    newIMarketArea.IMarketAreaName = tbMarketArea[i].Name;
                                    newIMarketArea.MarketAreaConfig = newMarketAreaConfig.GetMarketAreaConfigByMarketAreaID(tbMarketArea[i].MartketAreaID);
                                    
                                    Result.Add(newIMarketArea);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketAreaID"></param>
        /// <returns></returns>
        internal IPresenter.IMarketArea GetMarketAreaByID(int MarketAreaID)
        {
            IPresenter.IMarketArea Result = new Business.SpotCommand();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaTableAdapter adap = new DSTableAdapters.MarketAreaTableAdapter();
            DS.MarketAreaDataTable tbMarketArea = new DS.MarketAreaDataTable();
            TradingServer.DBW.DBWMarketAreaConfig newMarketAreaConfig = new DBWMarketAreaConfig();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbMarketArea = adap.GetMarketAreaByID(MarketAreaID);

                if (tbMarketArea != null)
                {
                    switch (tbMarketArea[0].Name)
                    {
                        case "SpotCommand":
                            {
                                Result = new Business.SpotCommand();
                                Result.IMarketAreaID = tbMarketArea[0].MartketAreaID;
                                Result.IMarketAreaName = tbMarketArea[0].Name;
                                Result.MarketAreaConfig = newMarketAreaConfig.GetMarketAreaConfigByMarketAreaID(tbMarketArea[0].MartketAreaID);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountMarketArea()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.MarketAreaTableAdapter adap = new DSTableAdapters.MarketAreaTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountMarketArea();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result.Value;
        }
    }
}