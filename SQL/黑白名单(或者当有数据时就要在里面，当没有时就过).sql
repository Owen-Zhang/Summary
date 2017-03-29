(NOT EXISTS ( SELECT TOP (1) 1 FROM @tmpSpecialList ) OR temp.sellerid IN ( SELECT SellerID FROM @tmpSpecialList))
      AND temp.sellerid NOT IN (SELECT  SellerID FROM @tmpBlockList)
      
      
      
INSERT  INTO @tmpSpecialList(SellerID)
SELECT  T.c.value('(./text())[1]', 'Char(6)')
FROM    @XMLSpecialList.nodes('/SpecialSellerList/SpecialSeller') AS T ( c )
