USE MKTPLS
GO
/*
    LockPrice : 是否要锁定价格， 就是不能改价格了
*/
Create Table DBO.MPS_Seller_PreOrderItemInfo(
    TransactionNumber  int IDENTITY(1,1) NOT NULL,
    SellerId           char(6) not null,
    ItemNumber         char(25) not null,
    CountryCode        Char(3) not null,
    LockPrice          bit not null,
    SoldQuantity       decimal(10,2),  
    SoldAmount         decimal(10,2),
    BufferID           Int,
    InUser             Varchar(15) not null,
    InDate         datetime not null,
    LastEditUser       Varchar(15),
    LastEditDate       datetime
     
CONSTRAINT [PK_MPS_Seller_PreOrderItemInfo] PRIMARY KEY CLUSTERED
(
    TransactionNumber ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MPS_Seller_PreOrderItemInfo_ItemNumberAndCountryCode] ON [dbo].[MPS_Seller_PreOrderItemInfo]
(
    [ItemNumber] ASC,
    [CountryCode] ASC
)WITH(FILLFACTOR = 90) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MPS_Seller_PreOrderItemInfo_SellerId] ON [dbo].[MPS_Seller_PreOrderItemInfo]
(
    [SellerId] ASC
)WITH(FILLFACTOR = 90) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_MPS_Seller_PreOrderItemInfo_BufferID] ON [dbo].[MPS_Seller_PreOrderItemInfo]
(
    [BufferID] ASC
)WITH(FILLFACTOR = 90) ON [PRIMARY]
GO
