AbstractSolrQuery solrQuery = new SolrQuery("*:*");
solrQuery = solrQuery && new SolrQuery("CountryCode:" + query.CountryCode);

tmpQuery = tmpQuery && new SolrQuery("AcctPostDate:[* TO *]");
AbstractSolrQuery tQuery = new SolrQuery(string.Format("ExpectShippingDate:[{0} TO *]", DateTime.Now.ToSolrDateTimeString()));
tmpQuery = tmpQuery || tQuery;
solrQuery = solrQuery && tmpQuery

solrQuery = solrQuery && new SolrQuery(string.Format("{0}(IsReplacementSO:true)" , query.IsReplacementReturnOrder.Value.Equals(false) ? "-" : string.Empty));

solrQuery = solrQuery && new SolrQuery("SOType:'99997'");
solrQuery = solrQuery && new SolrQuery("SellerPartNumber:\"" + query.SellerPartNumber + "\"");
solrQuery = solrQuery && new SolrQuery(string.Format(@"OrderDate:[""{0}"" TO *]", query.OrderDateFrom.Value.ToSolrDateTimeString()));

solrQuery = solrQuery && new SolrQuery("SOType:'99999' AND IsAutoVoid:true");
string.Format("SOType:('99999' OR '99998') AND -{0}", GenerateSolrQuery_ShippingToCountryList(domesticCountryCode))


StringBuilder queryShippingToCountryBuilder = new StringBuilder();
queryShippingToCountryBuilder.Append("SONumber:");
queryShippingToCountryBuilder.Append("(");

for (int i = 0; orderNumberList.Count > 1 && i < orderNumberList.Count - 1; i++)
{
    queryShippingToCountryBuilder.AppendFormat("{0} OR ", orderNumberList[i]);
}
queryShippingToCountryBuilder.AppendFormat("{0}", orderNumberList[orderNumberList.Count - 1]);
queryShippingToCountryBuilder.Append(")");
