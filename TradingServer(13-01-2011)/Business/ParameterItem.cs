﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TradingServer.Business
{
    public partial class ParameterItem
    {
        #region Property Class Parameter Item
        public int ParameterItemID { get; set; }
        public int SecondParameterID { get; set; }
        public int BoolValue { get; set; }
        public string Code { get; set; }
        public List<Business.ParameterItem> CollectionValue { get; set; }
        public DateTime DateValue { get; set; }
        public string Name { get; set; }
        public string NumValue { get; set; }
        public string StringValue { get; set; }
        #endregion        

        #region Create Instance Class DBWInvestorGroupConfig
        private static DBW.DBWInvestorGroupConfig dbwInvestorGroupConfig;
        private static DBW.DBWInvestorGroupConfig DBWInvestorGroupConfigInstance
        {
            get
            {
                if (ParameterItem.dbwInvestorGroupConfig == null)
                {
                    ParameterItem.dbwInvestorGroupConfig = new DBW.DBWInvestorGroupConfig();
                }

                return ParameterItem.dbwInvestorGroupConfig;
            }
        }
        #endregion

        #region Create Instance Class DBWTradingConfig
        private static DBW.DBWTradingConfig dbwTradingConfig;
        private static DBW.DBWTradingConfig DBWTradingConfigInstance
        {
            get
            {
                if (ParameterItem.dbwTradingConfig == null)
                {
                    ParameterItem.dbwTradingConfig = new DBW.DBWTradingConfig();
                }
                return ParameterItem.dbwTradingConfig;
            }
        }
        #endregion

        #region Create Instance Class IGroupSecurityConfig
        private static DBW.DBWIGroupSecurityConfig dbwIGroupSecurityConfig;
        private static DBW.DBWIGroupSecurityConfig DBWIGroupSecurityConfig
        {
            get
            {
                if (ParameterItem.dbwIGroupSecurityConfig == null)
                {
                    ParameterItem.dbwIGroupSecurityConfig = new DBW.DBWIGroupSecurityConfig();
                }
                return ParameterItem.dbwIGroupSecurityConfig;
            }
        }
        #endregion

        #region Create Instance Class IGroupSymbolConfig
        private static DBW.DBWIGroupSymbolConfig dbwIGroupSymbolConfig;
        private static DBW.DBWIGroupSymbolConfig DBWIGroupSymbolConfig
        {
            get
            {
                if (ParameterItem.dbwIGroupSymbolConfig == null)
                {
                    ParameterItem.dbwIGroupSymbolConfig = new DBW.DBWIGroupSymbolConfig();
                }

                return ParameterItem.dbwIGroupSymbolConfig;
            }
        }
        #endregion        

        #region Create Instance Class MarketConfig
        private static DBW.DBWMarketConfig dbwMarketConfig;
        private static DBW.DBWMarketConfig DBWMarketConfig
        {
            get
            {
                if (ParameterItem.dbwMarketConfig == null)
                {
                    ParameterItem.dbwMarketConfig = new DBW.DBWMarketConfig();
                }

                return ParameterItem.dbwMarketConfig;
            }
        }
        #endregion

        #region CREATE INSTANCE CLASS DBW VIRTUAL DEALER CONFIG
        private static DBW.DBWVirtualDealerConfig dbwVirtualDealerConfig;
        private static DBW.DBWVirtualDealerConfig VirtualDealerConfigInstance
        {
            get
            {
                if (ParameterItem.dbwVirtualDealerConfig == null)
                    ParameterItem.dbwVirtualDealerConfig = new DBW.DBWVirtualDealerConfig();

                return ParameterItem.dbwVirtualDealerConfig;
            }
        }
        #endregion

        //==================================================================================
        #region FUNCTION VIRTUAL DEALER CONFIG
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetAllVirtualDealerConfig()
        {
            return ParameterItem.VirtualDealerConfigInstance.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal List<Business.ParameterItem> GetVirtualDealerConfigByVirtualDealerID(int virtualDealerID)
        {
            return ParameterItem.VirtualDealerConfigInstance.GetVirtualDealerConfigByVirtualDealerID(virtualDealerID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal int AddVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            return ParameterItem.VirtualDealerConfigInstance.AddNewVirtualDealerConfig(objParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objParameterItem"></param>
        /// <returns></returns>
        internal bool UpdateVirtualDealerConfig(Business.ParameterItem objParameterItem)
        {
            return ParameterItem.VirtualDealerConfigInstance.UpdateVirtualDealerConfig(objParameterItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal bool DeleteVirtualDealerConfing(int virtualDealerID)
        {
            return ParameterItem.VirtualDealerConfigInstance.DeleteVirtualDealerConfig(virtualDealerID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal bool DeleteVirtualConfigByVirtualDealerID(int virtualDealerID)
        {
            return ParameterItem.VirtualDealerConfigInstance.DeleteVirtualConfigByVirtualDealearID(virtualDealerID);
        }
        #endregion
    }
}