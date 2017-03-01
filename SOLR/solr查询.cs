public partial class OrderSolrService
    {
        public string GenerateSolrQuery_OrderStatusList(List<OrderStatus> orderStatusList)
        {
            if (orderStatusList == null || orderStatusList.Count == 0)
            {
                throw new ArgumentNullException();
            }

            StringBuilder queryOrderStatusBuilder = new StringBuilder();
            queryOrderStatusBuilder.Append("OrderStatus:");
            queryOrderStatusBuilder.Append("(");

            for (int i = 0; orderStatusList.Count > 1 && i < orderStatusList.Count - 1; i++)
            {
                queryOrderStatusBuilder.AppendFormat("'{0}' OR ", (char)orderStatusList[i]);
            }
            queryOrderStatusBuilder.AppendFormat("'{0}'", (char)orderStatusList[orderStatusList.Count - 1]);
            queryOrderStatusBuilder.Append(")");

            return queryOrderStatusBuilder.ToString();
        }

        public IEnumerable<OrderProcessLogSolr> GetOrderProcessLogs(int orderNumber, string sellerid = null)
        {
            var solrService = SolrContainer.Resolve<OrderProcessLogSolr>();

            AbstractSolrQuery solrQuery = new SolrQuery(string.Format("SONumber:{0}", orderNumber));

            if (!string.IsNullOrWhiteSpace(sellerid))
            {
                solrQuery = solrQuery && new SolrQuery("SellerID:" + sellerid);
            }

            QueryOptions queryOptions = new QueryOptions { OrderBy = new[] { new SortOrder("EventTime", SolrNet.Order.DESC) }, };

            return solrService.Query(solrQuery, queryOptions);
        }

        public AbstractSolrQuery GetSolrQuery(OrderQueryCriteria query)
        {
            AbstractSolrQuery solrQuery = new SolrQuery("*:*");

            if (!string.IsNullOrWhiteSpace(query.CountryCode))
            {
                solrQuery = solrQuery && new SolrQuery("CountryCode:" + query.CountryCode);
            }

            #region 是否查询 Download 前的订单，主要用于权限控制

            if (query.DownloadStatus.HasValue && query.DownloadStatus.Value == OrderDownloadStatus.Downloaded)
            {
                AbstractSolrQuery tmpQuery = new SolrQuery("*:*");
                tmpQuery = tmpQuery && new SolrQuery("AcctPostDate:[* TO *]");
                //solrQuery = solrQuery && new SolrQuery("ExpectShippingDate:[* TO *]");
                tmpQuery = tmpQuery && new SolrQuery(GenerateSolrQuery_OrderStatusList(OrderStatusDic.DownloadedOrderStatusList));
                if (query.Is3PLSeller)
                {
                    AbstractSolrQuery tQuery = new SolrQuery(string.Format("ExpectShippingDate:[{0} TO *]", DateTime.Now.ToSolrDateTimeString()));
                    tmpQuery = tmpQuery || tQuery;
                }
                solrQuery = solrQuery && tmpQuery;
            }

            #endregion 是否查询 Download 前的订单，主要用于权限控制

            if (query.IsRejectReturnOrder.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format("{0}(IsRejectSO:true)"
                    , query.IsRejectReturnOrder.Value.Equals(false) ? "-" : string.Empty));
            }

            if (query.IsReplacementReturnOrder.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format("{0}(IsReplacementSO:true)"
                   , query.IsReplacementReturnOrder.Value.Equals(false) ? "-" : string.Empty));
            }

            #region Basic search condition

            if (!string.IsNullOrWhiteSpace(query.SellerID))
            {
                solrQuery = solrQuery && new SolrQuery("SellerID:" + query.SellerID);
            }

            if (query.OrderNumberList != null && query.OrderNumberList.Count > 0)
            {
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_OrderNumberList(query.OrderNumberList));
            }

            if (query.SellerMutiChannelOrderNumberList != null && query.SellerMutiChannelOrderNumberList.Count > 0)
            {
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_SellerMutiChannelOrderNumberList(query.SellerMutiChannelOrderNumberList));
                solrQuery = solrQuery && new SolrQuery("SOType:'99997'");
            }

            if (!string.IsNullOrWhiteSpace(query.InvoiceNumber))
            {
                int invoicenumber = 0;

                if (int.TryParse(query.InvoiceNumber, out invoicenumber))
                {
                    solrQuery = solrQuery && new SolrQuery("InvoiceNumber:" + invoicenumber);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.CustomerName))
            {
                solrQuery = solrQuery && new SolrQuery(query.CustomerName.ToSolrFuzzyQueryString("CustomerName"));
            }

            if (!string.IsNullOrWhiteSpace(query.SellerPartNumber))
            {
                solrQuery = solrQuery && new SolrQuery("SellerPartNumber:\"" + query.SellerPartNumber + "\"");
            }

            if (!string.IsNullOrWhiteSpace(query.NeweggItemNumber))
            {
                solrQuery = solrQuery && new SolrQuery("ItemNumber:\"" + query.NeweggItemNumber + "\"");
            }

            if (!string.IsNullOrWhiteSpace(query.CustomerPONumber))
            {
                solrQuery = solrQuery && new SolrQuery("CustomerPONumber:\"" + query.CustomerPONumber + "\"");
            }

            if (!string.IsNullOrWhiteSpace(query.CustomerPhoneNumber))
            {
                solrQuery = solrQuery && new SolrQuery("CustomerPhoneNumber:\"" + query.CustomerPhoneNumber + "\"");
            }

            if (!string.IsNullOrWhiteSpace(query.ItemTitle))
            {
                solrQuery = solrQuery && new SolrQuery(query.ItemTitle.ToSolrFuzzyQueryString("ItemShortTitle"));
            }

            if (!string.IsNullOrWhiteSpace(query.ManufacturerName))
            {
                solrQuery = solrQuery && new SolrQuery(query.ManufacturerName.ToSolrFuzzyQueryString("ItemManufacturerName"));
            }

            #endregion Basic search condition

            #region Advance search condition

            if (query.OrderDateFrom.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format(@"OrderDate:[""{0}"" TO *]", query.OrderDateFrom.Value.ToSolrDateTimeString()));
            }

            if (query.OrderDateTo.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format(@"OrderDate:[* TO ""{0}""]", query.OrderDateTo.Value.ToSolrDateTimeString()));
            }

            if (query.AutoVoidDateFrom.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format(@"AutoVoidTime:[""{0}"" TO *]", query.AutoVoidDateFrom.Value.ToSolrDateTimeString()));
            }

            if (query.AutoVoidDateTo.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(string.Format(@"AutoVoidTime:[* TO ""{0}""]", query.AutoVoidDateTo.Value.ToSolrDateTimeString()));
            }

            if (query.OrderStatusList != null && query.OrderStatusList.Count > 0)
            {
                if (query.OrderStatusList.Any(d => d == OrderStatus.WaitingForRelease))
                { 
                    solrQuery = solrQuery && new SolrQuery(string.Format(@"ExpectShippingDate:[""{0}"" TO *]", DateTime.Now.ToSolrDateTimeString()));
                }
               
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_OrderStatusList(query.OrderStatusList));
            }

            if (query.OrderSalesChannelTypeList != null && query.OrderSalesChannelTypeList.Count > 0)
            {
                foreach (var salesChannel in query.OrderSalesChannelTypeList)
                {
                    solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_OrderSalesChannel(salesChannel));
                }
            }

            if (query.OrderFulfillmentChannel.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_OrderFulfillmentChannel(query.OrderFulfillmentChannel.Value));
            }

            if (query.ShippingToCountryList != null && query.ShippingToCountryList.Count > 0)
            {
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_ShippingToCountryList(query.ShippingToCountryList));
            }
            //注意：目前还是简单的将 ShipToCountry <> USA 的订单作为 IsInternationalShipping
            if (query.IsInternationalShipping.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(GenerateSolrQuery_IsInternationalShipping(query.IsInternationalShipping.Value));
            }

            if (query.IsAutoVoidOrder.HasValue && query.IsAutoVoidOrder.Value == true)
            {
                solrQuery = solrQuery && new SolrQuery("SOType:'99999' AND IsAutoVoid:true");
            }

            if (query.IsNeweggFlashOrder.HasValue && query.IsNeweggFlashOrder.Value == true)
            {
                solrQuery = solrQuery && new SolrQuery("AddressType:'1'");
            }

            if (query.IsManualIssuedOrder.HasValue)
            {
                solrQuery = solrQuery && new SolrQuery(
                    string.Format("{0}IsManualIssued:true",
                    query.IsManualIssuedOrder.Value == true ? string.Empty : "-"
                    ));
            }

            if (query.IsPremierOrder.HasValue)
            {
                if (query.IsPremierOrder == OrderPremierType.Yes)
                {
                    solrQuery = solrQuery && new SolrQuery(string.Format("ShipViaCode:{0}", this.premierShipViaCode));
                }
                else
                {
                    solrQuery = solrQuery && new SolrQuery(string.Format("-ShipViaCode:{0}", this.premierShipViaCode));
                }
            }

            if (!string.IsNullOrWhiteSpace(query.WarehouseNumber))
            {
                solrQuery = solrQuery && new SolrQuery("ItemShipFromWarehouseNumber:\"" + query.WarehouseNumber + "\"");
            }

            #endregion Advance search condition

            return solrQuery;
        }

        public IEnumerable<OrderSolr> SearchOrders_FromMKPLSolr(OrderQueryCriteria query)
        {
            if (query == null)
            {
                query = new OrderQueryCriteria();
            }
            if (query.ExistInvalidFormatFilter)
            {
                return new List<OrderSolr>();
            }

            var solrService = SolrContainer.Resolve<OrderSolr>();

            QueryOptions queryOptions = OrderSolr.GetSolrQueryOption(query.PageInfo);
            return solrService.Query(GetSolrQuery(query), queryOptions);
        }

        public void UpdateB2BOrderCompleteFlag(int soNumber)
        {
            AbstractSolrQuery solrQuery = new SolrQuery("*:*");
            solrQuery = solrQuery && new SolrQuery("SONumber:" + soNumber);

            var solrService = SolrContainer.Resolve<OrderSolr>();

            IEnumerable<OrderSolr> _result = solrService.Query(solrQuery, null);

            if (null == _result || !_result.Any())
            {
                return;
            }

            var list = _result.ToList();
            list[0].B2BOrder_IsCompleted = false;
            solrService.Add(list[0]);
            solrService.Commit();
        }

        private string GenerateSolrQuery_IsInternationalShipping(bool isIntenrationalShipping)
        {
            List<string> domesticCountryCode = new List<string> { "USA", "USB" };

            return isIntenrationalShipping ?
                 string.Format("SOType:('99999' OR '99998') AND -{0}", GenerateSolrQuery_ShippingToCountryList(domesticCountryCode))
                 : GenerateSolrQuery_ShippingToCountryList(domesticCountryCode);
        }

        private string GenerateSolrQuery_OrderFulfillmentChannel(OrderFulfillmentChannel orderFulfillmentChannel)
        {
            return orderFulfillmentChannel == OrderFulfillmentChannel.Seller ?
                string.Format("SOType:'{0}'", OrderSolr.SOType_FufillmentBySeller_Value)
                : string.Format("SOType:('{0}' OR '{1}')", OrderSolr.SOType_FufillmentByNewegg_Value, OrderSolr.SOType_MultiChannel_Value);
        }

        private string GenerateSolrQuery_OrderNumberList(IList<int> orderNumberList)
        {
            if (orderNumberList == null || orderNumberList.Count == 0)
            {
                throw new ArgumentNullException();
            }

            StringBuilder queryShippingToCountryBuilder = new StringBuilder();
            queryShippingToCountryBuilder.Append("SONumber:");
            queryShippingToCountryBuilder.Append("(");

            for (int i = 0; orderNumberList.Count > 1 && i < orderNumberList.Count - 1; i++)
            {
                queryShippingToCountryBuilder.AppendFormat("{0} OR ", orderNumberList[i]);
            }
            queryShippingToCountryBuilder.AppendFormat("{0}", orderNumberList[orderNumberList.Count - 1]);
            queryShippingToCountryBuilder.Append(")");

            return queryShippingToCountryBuilder.ToString();
        }

        private string GenerateSolrQuery_SellerMutiChannelOrderNumberList(IList<string> sellerMultiChannelOrderNumberList)
        {
            if (sellerMultiChannelOrderNumberList == null || sellerMultiChannelOrderNumberList.Count == 0)
            {
                throw new ArgumentNullException();
            }

            StringBuilder querySellerMultiChannelOrderNumberBuilder = new StringBuilder();
            querySellerMultiChannelOrderNumberBuilder.Append("ReferenceSoNumber:");
            querySellerMultiChannelOrderNumberBuilder.Append("(");

            for (int i = 0; sellerMultiChannelOrderNumberList.Count > 1 && i < sellerMultiChannelOrderNumberList.Count - 1; i++)
            {
                querySellerMultiChannelOrderNumberBuilder.AppendFormat("{0} OR ", sellerMultiChannelOrderNumberList[i]);
            }
            querySellerMultiChannelOrderNumberBuilder.AppendFormat("{0}", sellerMultiChannelOrderNumberList[sellerMultiChannelOrderNumberList.Count - 1]);
            querySellerMultiChannelOrderNumberBuilder.Append(")");

            return querySellerMultiChannelOrderNumberBuilder.ToString();
        }

        private string GenerateSolrQuery_OrderSalesChannel(OrderSalesChannel orderSalesChannel)
        {
            switch (orderSalesChannel)
            {
                case OrderSalesChannel.MultiChannel:
                    return string.Format("SOType:'99997'");

                case OrderSalesChannel.Newegg:
                    return string.Format("SOType:('99999' OR '99998')", OrderSolr.AddressType_NeweggFlash_Value);

                default:
                    return "*:*";
            }
        }

        private string GenerateSolrQuery_ShippingToCountryList(List<string> shippingToCountryList)
        {
            if (shippingToCountryList == null || shippingToCountryList.Count == 0)
            {
                throw new ArgumentNullException();
            }

            StringBuilder queryShippingToCountryBuilder = new StringBuilder();
            queryShippingToCountryBuilder.Append("ShipToCountry:");
            queryShippingToCountryBuilder.Append("(");

            for (int i = 0; shippingToCountryList.Count > 1 && i < shippingToCountryList.Count - 1; i++)
            {
                queryShippingToCountryBuilder.AppendFormat("'{0}' OR ", shippingToCountryList[i]);
            }
            queryShippingToCountryBuilder.AppendFormat("'{0}'", shippingToCountryList[shippingToCountryList.Count - 1]);
            queryShippingToCountryBuilder.Append(")");

            return queryShippingToCountryBuilder.ToString();
        }

        #endregion

     
    }
