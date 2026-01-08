CREATE TABLE Family (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
);

CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    FamilyId INT NOT NULL,
    FOREIGN KEY (FamilyId) REFERENCES Family(Id)
);

CREATE TABLE ExpenseType (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
);

CREATE TABLE Expense (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    TypeId INT NOT NULL,
    AffectsFamilyBudget BIT NOT NULL DEFAULT 0,
    WhoItAffects INT NULL,
    FOREIGN KEY (TypeId) REFERENCES ExpenseType(Id),
    FOREIGN KEY (WhoItAffects) REFERENCES User(Id)
);

CREATE TABLE Keyword (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ExpenseId INT NOT NULL,
    FOREIGN KEY (ExpenseId) REFERENCES Expense(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Users_FamilyId ON [User](FamilyId);
CREATE INDEX IX_Expenses_TypeId ON Expense(TypeId);
CREATE INDEX IX_Expenses_WhoItAffects ON Expense(WhoItAffects);
CREATE INDEX IX_Keywords_ExpenseId ON Keyword(ExpenseId);