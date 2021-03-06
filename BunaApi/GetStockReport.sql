USE [CyroTechManager]
GO
/****** Object:  StoredProcedure [dbo].[GetCovidResults]    Script Date: 2021/08/06 10:41:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].GetStockReport
(
    @PageSize INT,
    @PageNumber INT,
    @OrderBy varchar(100) ,
	@OrderByDirection varchar(10),
	@PartId INT,
	@WorkOrderId INT,
	@From DATETIME, 
	@To DATETIME
	 
)
AS
BEGIN

--FOR TESTING ------------------------------
--DECLARE @PageSize INT = 1000
--DECLARE @PageNumber INT = 1
--DECLARE @OrderBy varchar(250) = 'StockTakeDate'
--DECLARE @OrderByDirection varchar(10) = 'DESC'
--DECLARE @PartId int --= 2

--DECLARE @From DATETIME = '2012-07-01' 
--DECLARE @To DATETIME = '2419-07-30'
--------------------------------------------
SET @OrderBy = COALESCE(@OrderBy,'CreatedDate') --Default order by column
SET @OrderByDirection = COALESCE(@OrderByDirection,'DESC') --Default order direction
DECLARE @FromDate DATETIME 
DECLARE @ToDate DATETIME 
SET @FromDate = COALESCE(@From,GETDATE()) 
SET @ToDate = COALESCE(@To,DATEADD(day,1,@FromDate)) 
;

 WITH StockResultsPaged AS
  (
    SELECT st.Quantity as CountQuantity
	,(st.Quantity - wop.OutQuantity) as AvailableQuantity
	,st.Location,st.StockTakeDate
	,(SELECT Fullname from [User] WHERE UserID = st.CreatedByID) as CreatedBy
	,p.Partid,p.Code,p.Description,p.Model,p.SerialNo
	,(SELECT DataDescription FROM StpData WHERE StpDataID = StpUnitOfMeasureID) as UnitOfMeasure,p.Price
	
	,ROW_NUMBER() OVER 
      (ORDER BY 
			   
				--ASC
				CASE WHEN @OrderByDirection = 'ASC'  THEN  --Varchar COLUMNS
					CASE @OrderBy 
					WHEN 'Location' THEN Location 
					WHEN 'Code' THEN Code
					WHEN 'Description' THEN Description
					WHEN 'Model' THEN Model
					WHEN 'SerialNo' THEN SerialNo
										
					END  
				END ASC,
				--DESC
				CASE WHEN @OrderByDirection = 'DESC'  THEN  --Varchar COLUMNS
					CASE @OrderBy 
					WHEN 'Location' THEN Location 
					WHEN 'Code' THEN Code
					WHEN 'Description' THEN Description
					WHEN 'Model' THEN Model
					WHEN 'SerialNo' THEN SerialNo
					
					END  
				END DESC,
				--ASC
				CASE WHEN @OrderByDirection = 'ASC'  THEN  --Date COLUMNS
					CASE @OrderBy 
					WHEN 'StockTakeDate' THEN StockTakeDate 
					
					END  
				END ASC,
				--DESC
				CASE WHEN @OrderByDirection = 'DESC'  THEN  --Date COLUMNS
					CASE @OrderBy 
					WHEN 'StockTakeDate' THEN StockTakeDate 
					
					END  
				END DESC,
				--ASC
				CASE WHEN @OrderByDirection = 'ASC'  THEN  --INT COLUMNS
					CASE @OrderBy 
					WHEN 'CountQuantity' THEN Quantity
				--	WHEN 'AvailableQuantity' THEN AvailableQuantity
					END  
				END ASC,
				--DESC
				CASE WHEN @OrderByDirection = 'DESC'  THEN  --INT COLUMNS
					CASE @OrderBy 
					WHEN 'CountQuantity' THEN Quantity
				--	WHEN 'AvailableQuantity' THEN AvailableQuantity
					
					END  
				END DESC
				
	  ) AS RowNumber
	FROM StockTake st WITH(NOLOCK)
	INNER JOIN Part p ON p.PartID = st.PartID
	OUTER APPLY (SELECT SUM(Quantity) as OutQuantity FROM WorkOrderPart w WITH(NOLOCK) WHERE w.PartID = st.PartID AND ReturnDate is null) wop
	WHERE
	(COALESCE(@PartID,'') = '' OR (st.[PartID] = @PartID))
	AND
	(st.StockTakeDate BETWEEN @FromDate AND @ToDate)
	
  )
  SELECT *,(SELECT count(*) FROM StockResultsPaged) as TotalRows FROM StockResultsPaged
  WHERE
   RowNumber BETWEEN (@PageNumber - 1) * @PageSize + 1 
   AND @PageNumber * @PageSize
   
END
--select * from stocktake
--select * from workorderpart
--select * from part