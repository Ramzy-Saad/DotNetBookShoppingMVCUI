USE [BookShoppingMVCUI]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_GetTopNSellingBooksByDate]
    @startDate DATETIME,
    @endDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH UnitSold AS
    (
        SELECT
            od.BookId,
            SUM(od.Quantity) AS TotalUnitSold
        FROM [Order] o
        INNER JOIN OrderDetail od
            ON o.Id = od.OrderId
        WHERE o.IsPaid = 1
          AND o.IsDeleted = 0
          AND o.CreateDate >= @startDate
          AND o.CreateDate < DATEADD(DAY, 1, @endDate)
        GROUP BY od.BookId
    )

    SELECT TOP (5)
        b.[Name],
        b.AuthorName,
        b.[Image],
        us.TotalUnitSold
    FROM UnitSold us
    INNER JOIN Book b
        ON b.Id = us.BookId
    ORDER BY us.TotalUnitSold DESC;
END
GO