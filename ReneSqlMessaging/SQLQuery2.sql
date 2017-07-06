--SELECT *
--FROM Accounts

--create PROCEDURE SendMessage
--@sender varchar(50),
--@receiver varchar(50)
--AS
--SELECT ID AS SenderID, 
--		[Message]

--FROM Accounts
--JOIN PersonalNotes
--ON Accounts.ID = PersonalNotes.SenderID
--WHERE Accounts.Username = @sender

--alter PROCEDURE usp_StoreMessage
--@sender varchar(50),
--@message varchar(MAX),
--@time varchar(50),
--@date varchar(50)

--AS
--DECLARE @senderID int
--SELECT @senderID = ID FROM Accounts WHERE Accounts.Username = @sender

--INSERT INTO PersonalNotes
--Values(@senderID, @message, @time, @date)

ALTER PROCEDURE usp_ViewAllMessages
@sender varchar(50)

AS
DECLARE @senderID int
	SELECT @senderID = ID FROM Accounts WHERE Accounts.Username = @sender
SELECT [Message], [Date], [Time]
FROM PersonalNotes
WHERE @senderID = SenderID

--SELECT *
--FROM PersonalNotes
 