

select transfer_id, account_from AS 'from',u.username , account_to AS 'to', ur.username, amount
	from transfers t
	JOIN accounts a on a.account_id = t.account_from
	JOIN users u on u.user_id = t.account_from
	JOIN users ur on ur.user_id = t.account_to


select * from transfers

select * from accounts

select * from transfer_types

select * from transfer_statuses


INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) Values (2, 2, 1, 2, 100)

Select @@IDENTITY as Id

UPDATE accounts set balance = balance - (select amount from transfers where @@IDENTITY = i) where account_id = (select account_from from transfers where @@IDENTITY = i)

UPDATE accounts set balance = balance + (select amount from transfers where transfer_id = i) where account_id = (select account_to from transfers where transfer_id = i)
	
	

select * from accounts

select * from users


Begin Transaction
INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) Values (2, 2, @fromId, @toId, @amount)
UPDATE accounts set balance = balance - (select amount from transfers where account_from = @fromId AND account_to = @toId AND  amount = @amount) where account_id = @fromId
UPDATE accounts set balance = balance + (select amount from transfers where account_to = @fromId AND account_to = @toId AND amount = @amount) where account_id = toId
COMMIT Transaction
			