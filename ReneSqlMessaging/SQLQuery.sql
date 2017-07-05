--CREATE PROC usp_Register
--@username varchar(50),
--@password varchar(50)
--AS
--INSERT Accounts
--VALUES(@username, @password)

CREATE PROC usp_Login
@username varchar(50),
@password varchar(50)
AS
SELECT *
FROM Accounts
WHERE @username = Accounts.Username AND @password = Accounts.Password