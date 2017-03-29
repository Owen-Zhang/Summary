/*-----------BEGIN:FOR DEBUG--------------------
        declare @InfoXml xml = N'
                  <PreOrderItem>
                    <TransactionNumber>1</TransactionNumber>
                    <SellerId>BGDX</SellerId>
                    <ItemNumber>9SIBGDX03E0794</ItemNumber>
                    <CountryCode>USA</CountryCode>
                    <LockPrice>1</LockPrice>
                    <SoldQuantity>0</SoldQuantity>
                    <SoldAmount>0</SoldAmount>
                    <BufferID>12545456</BufferID>
                    <IsCreate>1</IsCreate>
                    <InUser>Test</InUser>
                    <LastEditUser></LastEditUser>
                  </PreOrderItem>'
        -----------------------------------------------*/
        
                SELECT 
            T.c.value('(./TransactionNumber/text())[1]','int') AS TransactionNumber,
			      T.c.value('(./SellerId/text())[1]','char(6)') AS SellerId,
			      T.c.value('(./ItemNumber/text())[1]','char(25)') AS ItemNumber,
			      T.c.value('(./CountryCode/text())[1]','Char(3)') AS CountryCode,
			      T.c.value('(./LockPrice/text())[1]','bit') AS LockPrice,
			      T.c.value('(./SoldQuantity/text())[1]','decimal(10,2)') AS SoldQuantity,
			      T.c.value('(./SoldAmount/text())[1]','decimal(10,2)') AS SoldAmount,
			      T.c.value('(./BufferID/text())[1]','Int') AS BufferID,
            T.c.value('(./IsCreate/text())[1]','Int') AS IsCreate,
			      T.c.value('(./InUser/text())[1]','Varchar(50)') AS InUser,
			      T.c.value('(./LastEditUser/text())[1]','Varchar(50)') AS LastEditUser
			FROM @InfoXml.nodes('PreOrderItem') AS T(c)
