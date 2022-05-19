CREATE DATABASE DigitalDistributionService

USE DigitalDistributionService

CREATE TABLE [SecurityActionType] (
	SecurityActionTypeId			TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[ActionType]					NVARCHAR(32)		NOT NULL UNIQUE
)

CREATE TABLE Country (
	CountryId						TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(64)		NOT NULL UNIQUE,
	CurrencyCode					CHAR(3)				NOT NULL
)

CREATE TABLE [Account] (
	AccountId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Login]							VARCHAR(64)			NOT NULL UNIQUE,
	[Password]						VARCHAR(256)		NOT NULL,
	[Salt]							VARCHAR(256)		NOT NULL,
	Email							VARCHAR(128)		NOT NULL UNIQUE,
	Phone							CHAR(13)			NULL,
	CountryId						TINYINT				NOT NULL FOREIGN KEY REFERENCES Country(CountryId),
	Balance							DECIMAL(10,2)		NOT NULL DEFAULT 0 CHECK(Balance >= 0),
	FirstName						NVARCHAR(64)		NULL,
	MiddleName						NVARCHAR(64)		NULL,
	LastName						NVARCHAR(64)		NULL,
	BirthDate						DATE				NOT NULL,
	Username						NVARCHAR(64)		NOT NULL,
	Active							BIT					NOT NULL DEFAULT 1
)

CREATE TABLE AuthorizationRequest(
	AuthorizationRequestId			INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	ConfirmationCode				CHAR(8)				NOT NULL,
	ExpirationDate					DATETIMEOFFSET(3)	NOT NULL DEFAULT DATEADD(mi, 5, SYSDATETIME())
)

CREATE TABLE [SecurityActionAudit] (
	SecurityActionAuditId			INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	SecurityActionTypeId			TINYINT				NOT NULL FOREIGN KEY REFERENCES SecurityActionType(SecurityActionTypeId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME()
)

CREATE TABLE [GuestAudit] (
	GuestAuditId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Ipv4							VARCHAR(15)			NOT NULL,
	Device							NVARCHAR(128)		NOT NULL,
	WebClient						NVARCHAR(128)		NOT NULL,
	CountryId						TINYINT				NOT NULL FOREIGN KEY REFERENCES Country(CountryId),
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME()
)

CREATE TABLE [Role] (
	RoleId							TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Role]							NVARCHAR(16)		NOT NULL UNIQUE
);
		
CREATE TABLE AccountRole (
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	RoleId							TINYINT				NOT NULL FOREIGN KEY REFERENCES Role(RoleId),
	CONSTRAINT PK_AccountRole PRIMARY KEY (AccountId, RoleId)
)


CREATE TABLE GiftCard (
	GiftCardId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Code							CHAR(25)			NOT NULL UNIQUE,
	Currency						CHAR(3)				NOT NULL,
	Amount							DECIMAL(10, 2)		NOT NULL CHECK(Amount >= 0),
	AccountId						INT					NULL	 FOREIGN KEY REFERENCES Account(AccountId)								
)

CREATE TABLE PaymentMethod(
	PaymentMethodId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ExternalPaymentGateway			NVARCHAR(256)		NOT NULL,
	ExternalPaymentMethodToken		NVARCHAR(256)		NOT NULL
)

CREATE TABLE Developer (
	DeveloperId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(128)		NOT NULL UNIQUE,
	CompanyEmail					VARCHAR(128)		NOT NULL UNIQUE,
)

CREATE TABLE Publisher (
	PublisherId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(128)		NOT NULL UNIQUE,
	CompanyEmail					VARCHAR(128)		NOT NULL UNIQUE,
	BillingNumber					NVARCHAR(20)		NOT NULL,
)

CREATE TABLE Franchise(
	FranchiseId						INT					NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name]							NVARCHAR(32)		NOT NULL UNIQUE,
)

CREATE TABLE DeveloperAccount (
	DeveloperId						INT					NOT NULL FOREIGN KEY REFERENCES Developer(DeveloperId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	Active							BIT					NOT NULL DEFAULT 0,
	CONSTRAINT PK_DeveloperAccount PRIMARY KEY (DeveloperId, AccountId)
)

CREATE TABLE SystemRequirementCategory (
	SystemRequirementCategoryId		TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Category						NVARCHAR(16)		NOT NULL UNIQUE
)

CREATE TABLE SystemRequirement(
	SystemRequirementId				INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	SystemRequirementCategoryId		TINYINT				NOT NULL FOREIGN KEY REFERENCES SystemRequirementCategory(SystemRequirementCategoryId),
	Requirement						NVARCHAR(64)		NOT NULL UNIQUE,			
)

CREATE TABLE Tag(
	TagId							TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(16)		NOT NULL UNIQUE
)

CREATE TABLE Genre(
	GenreId							TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(16)		NOT NULL UNIQUE
)

CREATE TABLE MediaContent(
	MediaContentId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	Link							VARCHAR(256)		NOT NULL UNIQUE	
)


CREATE TABLE ContentRating(
	ContentRatingId					INT					NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MediaContentId					INT					NOT NULL FOREIGN KEY REFERENCES MediaContent(MediaContentId), 
	[Label]							NVARCHAR(32)		NOT NULL,
	[Description]					NVARCHAR(128)		NOT NULL,
	AgeRestriction					SMALLINT			NOT NULL UNIQUE CHECK(AgeRestriction >= 0 AND AgeRestriction <= 21) 
)


CREATE TABLE Product(
	ProductId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ParentProductId					INT					NULL	 FOREIGN KEY REFERENCES Product(ProductId),
	DeveloperId						INT					NOT NULL FOREIGN KEY REFERENCES Developer(DeveloperId),
	PublisherId						INT					NOT NULL FOREIGN KEY REFERENCES Publisher(PublisherId),
	FranchiseId						INT					NOT NULL FOREIGN KEY REFERENCES Franchise(FranchiseId),
	ContentRatingId					INT					NOT NULL FOREIGN KEY REFERENCES ContentRating(ContentRatingId),
	Title							NVARCHAR(128)		NOT NULL UNIQUE,
	ShortDescription				NVARCHAR(256)		NOT NULL,
	LongDescription					NVARCHAR(2048)		NOT NULL,
	DiscountPercentage				TINYINT				NOT NULL DEFAULT 0 CHECK (DiscountPercentage >= 0 AND DiscountPercentage <= 100),
	AverageRating					DECIMAL(1,1)		NULL	 DEFAULT NULL,
	ReleaseDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	IsEarlyAccess					BIT					NOT NULL DEFAULT 1,
	IsRemoved						BIT					NOT NULL DEFAULT 0
)

CREATE TABLE ProductAnnouncement(
	ProductAnnouncementId			INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	[Title]							NVARCHAR(128)		NOT NULL,
	Description						NVARCHAR(2048)		NOT NULL,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
)

CREATE TABLE Review(
	ReviewId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	IsRecommended					BIT					NOT NULL,
	IsGiftedProduct					BIT					NOT NULL DEFAULT 0,
	Commentary						NVARCHAR(512)		NOT NULL,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	UpdatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	CONSTRAINT UNQ_ReviewProductIdAccountId UNIQUE (ProductId, AccountId)
)

CREATE TABLE ReviewComment(
	ReviewCommentId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ReviewId						INT					NOT NULL FOREIGN KEY REFERENCES Review(ReviewId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	Commentary						NVARCHAR(128)		NOT NULL,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
)

CREATE TABLE ReviewRating(
	ReviewId						INT					NOT NULL FOREIGN KEY REFERENCES Review(ReviewId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	IsUseful						BIT					NOT NULL
	CONSTRAINT PK_ReviewRating PRIMARY KEY (ReviewId, AccountId)
)

CREATE TABLE ReviewCommentRating(
	ReviewCommentId					INT					NOT NULL FOREIGN KEY REFERENCES ReviewComment(ReviewCommentId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	IsUseful						BIT					NOT NULL
	CONSTRAINT PK_ReviewCommentRating PRIMARY KEY (ReviewCommentId, AccountId)
)

CREATE TABLE ProductMediaContent(
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	MediaContentId					INT					NOT NULL FOREIGN KEY REFERENCES MediaContent(MediaContentId),
	CONSTRAINT PK_ProductMediaContent PRIMARY KEY (ProductId, MediaContentId)
)

CREATE TABLE ProductGenre(
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	GenreId							TINYINT				NOT NULL FOREIGN KEY REFERENCES Genre(GenreId),
	CONSTRAINT PK_ProductGenre PRIMARY KEY (ProductId, GenreId)
)

CREATE TABLE ProductTag(
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	TagId							TINYINT				NOT NULL FOREIGN KEY REFERENCES Tag(TagId),
	CONSTRAINT PK_ProductTag PRIMARY KEY (ProductId, TagId)
)

CREATE TABLE ProductSystemRequirement(
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	SystemRequirementId				INT					NOT NULL FOREIGN KEY REFERENCES SystemRequirement(SystemRequirementId),
	CONSTRAINT PK_ProductSystemRequirement PRIMARY KEY (ProductId, SystemRequirementId)
)

CREATE TABLE ProductRegionalPrice(
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	CountryId						TINYINT				NOT NULL FOREIGN KEY REFERENCES Country(CountryId),
	Price							DECIMAL(10,2)		NOT NULL INDEX IDX_ProductRegionalPrice CHECK(Price >= 0),
	CONSTRAINT PK_ProductRegionalPrice PRIMARY KEY (ProductId, CountryId)
)

CREATE TABLE AccountPaymentMethod (
	AccountPaymentMethodId			INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	PaymentMethodId					INT					NOT NULL FOREIGN KEY REFERENCES PaymentMethod(PaymentMethodId),
)

CREATE TABLE TransactionStatus (
	TransactionStatusId				INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Status]						NVARCHAR(16)		NOT NULL UNIQUE				
)

CREATE TABLE Wishlist (
	Wishlist						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	CONSTRAINT UNQ_WishlistProductIdAccountId UNIQUE (ProductId, AccountId)				
)

CREATE TABLE Cart(
	CartId							INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	CONSTRAINT UNQ_CartProductIdAccountId UNIQUE (ProductId, AccountId)
)

CREATE TABLE [Transaction] (
	TransactionId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NULL	 FOREIGN KEY REFERENCES Product(ProductId),
	BuyerAccountId					INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	OwnerAccountId					INT					NULL	 FOREIGN KEY REFERENCES Account(AccountId),
	AccountPaymentMethodId			INT					NOT NULL FOREIGN KEY REFERENCES AccountPaymentMethod(AccountPaymentMethodId),
	CurrencyCode					CHAR(3)				NOT NULL,
	Amount							DECIMAL(10, 2)		NOT NULL,
	TransactionStatusId				INT					NOT NULL DEFAULT 1,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	UpdatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	CONSTRAINT UNQ_ProductTransactionProductIdBuyerIdOwnerId	UNIQUE(ProductId,BuyerAccountId,OwnerAccountId)
)

CREATE TABLE AccountProduct(
	AccountProductId				INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	AccountId						INT					NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
	TransactionId					INT					NOT NULL FOREIGN KEY REFERENCES [Transaction](TransactionId),
	OwnershipDate					DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
)

CREATE TABLE RefundStatus(
	RefundStatusId					TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							VARCHAR(16)			NOT NULL UNIQUE				
)

CREATE TABLE ProductRefundRequest(
	RefundRequestId					INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	AccountProductId				INT					NOT NULL FOREIGN KEY REFERENCES AccountProduct(AccountProductId),
	RefundStatusId					TINYINT				NOT NULL FOREIGN KEY REFERENCES RefundStatus(RefundStatusId),
	Reason							NVARCHAR(512)		NOT NULL,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	UpdatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
)

CREATE TABLE Bundle(
	BundleId						INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(128)		NOT NULL UNIQUE,
	DiscountPercentage				TINYINT				NOT NULL DEFAULT 0 CHECK (DiscountPercentage >= 0 AND DiscountPercentage <= 100),
	--If 1 then discount applies to sum of products without accounting for current discount for each product
	IsIndependentDiscount			BIT					NOT NULL DEFAULT 1
)

CREATE TABLE BundleProduct (
	BundleId						INT					NOT NULL FOREIGN KEY REFERENCES Bundle(BundleId),
	ProductId						INT					NOT NULL FOREIGN KEY REFERENCES Product(ProductId),
	CONSTRAINT PK_BundleProduct PRIMARY KEY (BundleId, ProductId)
)

CREATE TABLE [Application](
	ApplicationId					TINYINT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name]							NVARCHAR(128)		NOT NULL UNIQUE,
	[Path]							NVARCHAR(256)		NOT NULL UNIQUE	
)

CREATE TABLE ExceptionLogAudit(
	ExceptionLogAuditId				INT					NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ApplicationId					TINYINT				NOT NULL FOREIGN KEY REFERENCES [Application](ApplicationId),
	[Message]						NVARCHAR(128)		NULL	 DEFAULT NULL,
	Exception						NVARCHAR(2048)		NOT NULL,
	CreatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	CreatedBy						NVARCHAR(256)		NOT NULL DEFAULT SUSER_SNAME(),
	UpdatedDate						DATETIMEOFFSET(3)	NOT NULL DEFAULT SYSDATETIME(),
	UpdatedBy						NVARCHAR(256)		NOT NULL DEFAULT SUSER_SNAME(),
)
GO

--TRIGGERS
CREATE TRIGGER tr_AfterInsertAccount ON Account AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT i.AccountId, 1
		FROM inserted i

	INSERT INTO AccountRole (AccountId, RoleId)
		SELECT i.AccountId, 2
		FROM inserted i;
END
GO

CREATE TRIGGER tr_AfterUpdateAccount ON Account AFTER UPDATE AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT i.AccountId, 2
		FROM inserted i;
END
GO

CREATE TRIGGER tr_AfterInsertAuthorizationRequest ON AuthorizationRequest AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT i.AccountId, 3
		FROM inserted i;
END
GO

CREATE TRIGGER tr_AfterInsertAccountProduct ON AccountProduct AFTER INSERT AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT i.AccountId, 4
		FROM inserted i;
END
GO

CREATE TRIGGER tr_AfterUpdateProductRefundRequest ON ProductRefundRequest AFTER UPDATE AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT ap.AccountId, 5
		FROM inserted i
		JOIN AccountProduct ap ON ap.AccountProductId = i.AccountProductId
		WHERE i.RefundStatusId = 4
END
GO

CREATE TRIGGER tr_AfterUpdateGiftCard ON GiftCard AFTER UPDATE AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SecurityActionAudit (AccountId, SecurityActionTypeId)
		SELECT i.AccountId, 6
		FROM inserted i
		WHERE i.AccountId IS NOT NULL
END
GO

--SPROCS

CREATE PROCEDURE usp_GetBundleValue(@bundleId INT, @accountId INT) AS
BEGIN	
	SELECT CASE b.IsIndependentDiscount WHEN 1 THEN SUM(prp.Price) ELSE SUM(prp.Price * (100 - p.DiscountPercentage)/100) END * ((100 - B.DiscountPercentage) / 100.0)
	FROM Bundle b
	JOIN BundleProduct bp ON bp.BundleId = b.BundleId
	JOIN Product p ON p.ProductId = bp.ProductId
	JOIN Account a ON a.AccountId = @accountId
	JOIN ProductRegionalPrice prp ON prp.ProductId = bp.ProductId AND prp.CountryId = a.CountryId
	GROUP BY p.ProductId
END
GO

CREATE PROCEDURE usp_RemoveAuditRecords(@table nvarchar(128), @afterDate DATE) AS
BEGIN
	
	DECLARE @sql NVARCHAR(MAX) = 'DELETE FROM ' + @table + ' WHERE CreatedDate < ' + CONVERT(nvarchar,@afterDate)
	EXEC sp_executesql @sql	
END
GO


CREATE PROCEDURE usp_GetBestReviews(@productId INT) AS
BEGIN
	SELECT TOP 10	r.ReviewId,
					r.ProductId,
					r.AccountId,
					r.Commentary,
					r.CreatedDate,
					r.IsGiftedProduct,
					r.IsRecommended
	FROM Review r
	JOIN ReviewRating rr ON r.ReviewId = rr.ReviewId
	JOIN Product p ON p.ProductId = 1 AND p.ProductId = r.ProductId
	GROUP BY r.ReviewId
	ORDER BY AVG(rr.IsUseful)
END
GO


CREATE PROCEDURE usp_GetRecentSecurityActions(@accountId INT, @latestDate DATE) AS
BEGIN
	SELECT *
	FROM SecurityActionAudit
	WHERE AccountId = @accountId AND CreatedDate >- @latestDate
END
GO

CREATE PROCEDURE usp_RemoveExpiredAuthRequests AS 
BEGIN
	DELETE 
	FROM AuthorizationRequest
	WHERE ExpirationDate < SYSDATETIME();
END
GO

CREATE PROCEDURE usp_UpdateAverageRatings AS
BEGIN
	WITH cte_ratings AS
	(
		SELECT r.ProductId, CASE WHEN COUNT(r.ProductId) > 0 THEN AVG(r.IsRecommended) * 10 ELSE NULL END as Rating
		FROM Review r
		GROUP BY r.ProductId
	)
	UPDATE p
	SET p.AverageRating = cte.Rating
	FROM Product p
	JOIN cte_ratings cte ON p.ProductId = cte.ProductId
END
GO

CREATE PROCEDURE usp_ActivateGiftCard(@accountId INT, @code CHAR(25)) AS
BEGIN
	DECLARE @giftCardId INT = 0
	DECLARE @cardCurrency CHAR(3) = '000'
	DECLARE @accountCurrency CHAR(3) = '000'

	SELECT  @giftCardId = GiftCardId,
			@cardCurrency = Currency
	FROM GiftCard
	WHERE Code = @code

	SELECT @accountCurrency = c.CurrencyCode
	FROM Account a
	JOIN Country c ON a.CountryId = c.CountryId

	IF (@giftCardId = 0)
		RAISERROR ('Invalid gift card',-1,-1, 'usp_ActivateGiftCard');  

	IF (@cardCurrency != @accountCurrency)
		RAISERROR ('Gift card does not belong to your region',-1,-1, 'usp_ActivateGiftCard');  

	UPDATE a
	SET Balance = Balance + gc.Amount
	FROM Account a
	JOIN GiftCard gc ON gc.Code = @code
	WHERE a.AccountId = @accountId

	DELETE 
	FROM GiftCard
	WHERE GiftCardId = @giftCardId
END 
GO




