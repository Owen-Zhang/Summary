//Antlr3.StringTemplate generate html/or template content

/*st file content*/
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css" media="screen">
        .*:empty {
            display: none;
        }
    </style>
</head>
<body>
    $Orders:{Order|
    <h1 style="text-align: center; page-break-before: always">Order Packing List</h1>
    <hr style="background-color: black; border: 0; height: 5px;" />
    <table style="width: 100%; margin-bottom: 20px;">
        <tr>
            <td style="width: 50%; vertical-align: top; padding-right: 10px">
                <table style="width: 100%; border: 1px solid gray; border-collapse: collapse;">
                    <tr>
                        <td style="font-size: larger; padding: 5px 0px 0px 5px">Bill To:</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToCompanyName$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToName$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToAddress1$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToAddress2$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToCity$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 0px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderBillToInformation.BillToCountryCode$</td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; vertical-align: top; padding-left: 10px">
                <table style="width: 100%; border: 1px solid gray; border-collapse: collapse;">
                    <tr>
                        <td style="font-size: larger; padding: 5px 0px 0px 5px">Ship To:</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToCompanyName$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToName$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToAddress1$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToAddress2$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 2px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToCity$</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; padding: 0px 0px 0px 5px; word-wrap: break-word; word-break: break-all">$Order.OrderShipToInformation.ShipToCountryCode$</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table style="width: 100%; margin-bottom: 20px; border: 1px solid gray; border-collapse: collapse;">
        <tr>
            <th style="width: 15%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Order Number</th>
            <th style="width: 15%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Order Date</th>
            <th style="width: 35%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Seller</th>
            <th style="width: 35%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Shipping Method</th>
        </tr>
        <tr>
            <td style="text-align: center; font-weight: bold; padding: 10px 0px 10px 0px; border: 1px solid gray; border-collapse: collapse">$Order.OrderNumber$</td>
            <td style="text-align: center; font-weight: bold; padding: 10px 0px 10px 0px; border: 1px solid gray; border-collapse: collapse">$Order.OrderDate$</td>
            <td style="text-align: center; font-weight: bold; padding: 10px 0px 10px 0px; border: 1px solid gray; border-collapse: collapse; word-wrap: break-word; word-break: break-all">$Order.SellerName$</td>
            <td style="text-align: center; font-weight: bold; padding: 10px 0px 10px 0px; border: 1px solid gray; border-collapse: collapse; word-wrap: break-word; word-break: break-all">$Order.ShipService$</td>
        </tr>
    </table>

    <table style="width: 100%; margin-bottom: 20px; border: 1px solid gray; border-collapse: collapse;">
        <tr>
            <th style="width: 20%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Newegg Item Number</th>
            <th style="width: 70%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Item Description</th>
            <th style="width: 10%; border: 1px solid gray; font-weight: 100; background-color: rgb(233, 233, 233);">Qty Ordered</th>
        </tr>
        $Order.OrderItemList:{Item|
                <tr>
                    <td style="font-weight: bold; text-align: center; padding: 5px 0px 5px 0px; border: 1px solid gray; border-collapse: collapse;">$if(Item.IsNeweggFlashItem)$
                        <img src="../Images/neweggFlashFlag.png" />
                        $endif$
                        <label>$Item.NeweggItemNumber$</label>
                    </td>
                    <td style="padding: 5px 0px 5px 10px; border: 1px solid gray; border-collapse: collapse;">
                        <div style="font-weight: bold; padding: 0px 0px 2px 0px; word-wrap: break-word; word-break: break-all">$Item.ItemTitle$</div>
                        $Item.ItemGroupProperties:{Property|
                        <div style="padding: 0px 0px 2px 0px; word-wrap: break-word; word-break: break-all">
                            <label>$Property.PropertyName$:</label>
                            <label style="font-weight: bold">$Property.ValueName$</label>
                        </div>
                        }$
                        <div style="padding: 0px 0px 2px 0px; word-wrap: break-word; word-break: break-all">
                            <label>Manufacturer Part#:</label>
                            <label style="font-weight: bold">$Item.ManufacturerPartNumber$</label>
                        </div>
                        <div style="word-wrap: break-word; word-break: break-all">
                            <label>Seller Part#:</label>
                            <label style="font-weight: bold">$Item.SellerPartNumber$</label>
                        </div>
                    </td>
                    <td style="text-align: center; padding: 5px 0px 5px 0px; border: 1px solid gray; border-collapse: collapse; font-weight: bold">$Item.OrderedQuantity$</td>
                </tr>
        }$
    </table>

    <div style="background-color: rgb(233, 233, 233); border: 1px solid gray; padding: 10px">
        <p>For questions or issues regarding your order, including returns, please contact the seller directly. You can locate the seller information by visiting <span style="color: cornflowerblue">$Website$</span>, logging into your account and viewing order details.</p>
        <p>Have something to say about a Marketplace seller? Log in to My Account to leave feedback and a rating.</p>
    </div>
    }$
</body>
</html>

--------------------------------------------------------------------------------------------------------------------------

/*cs file content*/
           var group = new StringTemplateGroup("myGroup", TemplatePath);
            var template = group.GetInstanceOf(OrderPackingListTemplateName);
            template.SetAttribute("Orders", orders.Select(order => new
            {
                OrderBillToInformation = order.OrderBillToInformation == null ? null : new
                {
                    BillToCompanyName = order.OrderBillToInformation.BillToCompanyName,
                    BillToName = order.OrderBillToInformation.BillToName,
                    BillToAddress1 = order.OrderBillToInformation.BillToAddress1,
                    BillToAddress2 = order.OrderBillToInformation.BillToAddress2,
                    BillToCity = string.IsNullOrWhiteSpace(order.OrderBillToInformation.BillToCity) ?
                                    string.Empty :
                                    string.Format("{0}, {1} {2}", order.OrderBillToInformation.BillToCity, order.OrderBillToInformation.BillToState, order.OrderBillToInformation.BillToZipcode),
                    BillToCountryCode = string.Compare(order.OrderBillToInformation.BillToCountryCode, "USB", true) == 0 ?
                                              "USA" :
                                              order.OrderBillToInformation.BillToCountryCode
                },
                OrderShipToInformation = order.OrderShipToInformation == null ? null : new
                {
                    ShipToCompanyName = order.OrderShipToInformation.ShipToCompanyName,
                    ShipToName = order.OrderShipToInformation.ShipToName,
                    ShipToAddress1 = order.OrderShipToInformation.ShipToAddress1,
                    ShipToAddress2 = order.OrderShipToInformation.ShipToAddress2,
                    ShipToCity = string.IsNullOrWhiteSpace(order.OrderShipToInformation.ShipToCity) ?
                                        string.Empty :
                                        string.Format("{0}, {1} {2}", order.OrderShipToInformation.ShipToCity, order.OrderShipToInformation.ShipToState, order.OrderShipToInformation.ShipToZipcode),
                    ShipToCountryCode = string.Compare(order.OrderShipToInformation.ShipToCountryCode, "USB", true) == 0 ?
                                              "USA" :
                                              order.OrderShipToInformation.ShipToCountryCode
                },
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate.ToString("MM/dd/yyyy"),
                SellerName = order.SellerName,
                ShipService = order.ShipService,
                OrderItemList = order.OrderItemList == null ? null : order.OrderItemList.Select(i => new
                {
                    IsNeweggFlash = i.IsNeweggFlashItem,
                    NeweggItemNumber = i.NeweggItemNumber,
                    ItemTitle = i.ItemTitle,
                    ManufacturerPartNumber = i.ManufacturerPartNumber,
                    SellerPartNumber = i.SellerPartNumber,
                    OrderedQuantity = i.OrderedQuantity,
                    ItemGroupProperties = i.ItemGroupProperties == null ? null : i.ItemGroupProperties.Select(p => new
                    {
                        PropertyName = p.PropertyName,
                        ValueName = p.ValueName
                    })
                })
            }));
            template.SetAttribute("Website", website);
            var html = template.ToString();
