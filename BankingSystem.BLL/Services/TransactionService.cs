using BankingSystem.DAL.Repositorie;
using BankingSystem.Entities.Models;
using Microsoft.Identity.Client;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IGenericRepository<Account> _accountRepo;
        private readonly IGenericRepository<Transaction> _transactionRepo;

        public TransactionService(
            IGenericRepository<Account> accountRepo,
            IGenericRepository<Transaction> transactionRepo)
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
        }

        public async Task DepositAsync(int accountId, decimal amount)
        {
            var account = await _accountRepo.GetByIdAsync(accountId);

            if (account == null)
                throw new Exception("Account not found");

            account.Balance += amount;

            _accountRepo.Update(account);

            await _transactionRepo.InsertAsync(new Transaction
            {
                ToAccountId = accountId,
                Amount = amount,
                TransactionType = "Deposit",
                TransferType = "Deposit",   // 🔥 ADD THIS
                Status = "Success",
                CreatedDate = DateTime.Now
            });
            await _accountRepo.SaveAsync();
        }

        public async Task WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _accountRepo.GetByIdAsync(accountId);

            if (account == null)
                throw new Exception("Account not found");

            if (account.Balance < amount)
                throw new Exception("Insufficient balance");

            // Savings minimum balance rule
            if (account.AccountType == "Savings" && (account.Balance - amount) < 1000)
                throw new Exception("Minimum balance of 1000 must be maintained");

            account.Balance -= amount;

            _accountRepo.Update(account);

            await _transactionRepo.InsertAsync(new Transaction
            {
                FromAccountId = accountId,
                Amount = amount,
                TransactionType = "Withdraw",
                TransferType = "Withdraw",   // 🔥 ADD THIS
                Status = "Success",
                CreatedDate = DateTime.Now
            });

            await _accountRepo.SaveAsync();
        }
        public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId)
                throw new Exception("Cannot transfer to same account");

            var fromAccount = await _accountRepo.GetByIdAsync(fromAccountId);
            var toAccount = await _accountRepo.GetByIdAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new Exception("Invalid account");

            if (fromAccount.Balance < amount)
                throw new Exception("Insufficient balance");

            // Savings minimum balance rule
            if (fromAccount.AccountType == "Savings" &&
                (fromAccount.Balance - amount) < 1000)
                throw new Exception("Minimum balance of 1000 must be maintained");

            // Deduct & Add
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            _accountRepo.Update(fromAccount);
            _accountRepo.Update(toAccount);

            // Transaction entry – Debit
            await _transactionRepo.InsertAsync(new Transaction
            {
                FromAccountId = fromAccountId,
                Amount = amount,
                TransactionType = "Withdraw",
                TransferType = "Withdraw",   // 🔥 ADD THIS
                Status = "Success",
                CreatedDate = DateTime.Now
            });

            // Transaction entry – Credit
            await _transactionRepo.InsertAsync(new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                TransactionType = "Transfer-Credit",
                TransferType = "Internal",   // 🔥 ADD THIS
                Status = "Success",
                CreatedDate = DateTime.Now
            });
            await _accountRepo.SaveAsync();
        }
        public async Task<IEnumerable<Transaction>> GetAccountStatementAsync(int accountId)
        {
            var transactions = await _transactionRepo.GetAllAsync();

            return transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .OrderByDescending(t => t.CreatedDate)
                .ToList();
        }

        public async Task<int> ProcessTransferAsync(
     int fromAccountId,
     int? toAccountId,
     decimal amount,
     string transferType,
     string externalAccountNumber,
     string bankName)
        {
            if (transferType == "internal" && fromAccountId == toAccountId)
                throw new Exception("Cannot transfer to the same account");

            // Load sender account with customer
            var accounts = await _accountRepo.GetAllIncludingAsync(a => a.Customer);

            var fromAccount = accounts.FirstOrDefault(a => a.AccountId == fromAccountId);

            if (fromAccount == null)
                throw new Exception("Sender account not found");

            if (fromAccount.Balance < amount)
                throw new Exception("Insufficient Balance");

            string senderName = fromAccount.Customer?.FullName ?? "Unknown Sender";
            string receiverName = "";

            Account toAccount = null;

            if (transferType == "internal" && toAccountId.HasValue)
            {
                toAccount = accounts.FirstOrDefault(a => a.AccountId == toAccountId.Value);

                if (toAccount == null)
                    throw new Exception("Receiver account not found");

                receiverName = toAccount.Customer?.FullName ?? "Unknown Receiver";

                // Credit receiver
                toAccount.Balance += amount;
                _accountRepo.Update(toAccount);
            }
            else
            {
                receiverName = externalAccountNumber + " (" + bankName + ")";
            }

            // Debit sender
            fromAccount.Balance -= amount;
            _accountRepo.Update(fromAccount);

            await _accountRepo.SaveAsync();

            var transaction = new Transaction
            {
                TxnNumber = "TXN" + DateTime.Now.Ticks,
                FromAccountId = fromAccountId,
                ToAccountId = transferType == "internal" ? toAccountId : null,
                Amount = amount,
                TransactionType = transferType == "external"
                                    ? "ExternalTransfer"
                                    : "InternalTransfer",
                TransferType = transferType,
                Status = "Success",
                CreatedDate = DateTime.Now,
                SenderName = senderName,
                ReceiverName = receiverName,
                ExternalAccountNumber = transferType == "external" ? externalAccountNumber : null,
                ExternalBankName = transferType == "external" ? bankName : null
            };

            await _transactionRepo.InsertAsync(transaction);
            await _transactionRepo.SaveAsync();

            return transaction.TransactionId;
        }
    }
       
}