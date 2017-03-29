(NOT EXISTS ( SELECT TOP (1) 1 FROM @tmpSpecialList ) OR temp.sellerid IN ( SELECT SellerID FROM @tmpSpecialList))
      AND temp.sellerid NOT IN (SELECT  SellerID FROM @tmpBlockList)
