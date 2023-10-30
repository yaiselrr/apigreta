using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    /// <summary>
    /// Gift card service
    /// </summary>
    public interface IGiftCardService : IGenericBaseService<GiftCard>
    {
        /// <summary>
        /// Get gift card by card number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<GiftCard> GetByCardNumber(string number);

        /// <summary>
        /// Process gift cards
        /// </summary>
        /// <param name="giftcards"></param>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        Task ProcessGc(List<GiftCard> giftcards, string connectionString);

        /// <summary>
        /// Add transaction to gift card
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<long> AddTransaction(GiftCard obj, GiftCardTransaction transaction);
    }

    /// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IGiftCardService" />
    public class GiftCardService : BaseService<IGiftCardRepository, GiftCard>, IGiftCardService
    {
        /// <inheritdoc />
        public GiftCardService(IGiftCardRepository repository,
            ILogger<GiftCardService> logger)
            : base(repository, logger)
        {
        }

        /// <inheritdoc />
        public async Task<long> AddTransaction(GiftCard obj, GiftCardTransaction transaction)
        {
            return await _repository.TransactionAsync(async _ =>
            {
                await _repository.UpdateAsync(obj.Id, obj);
                var gt = await _repository.CreateAsync(transaction);
                return gt.Id;
            });
        }

        /// <inheritdoc />
        public async Task<GiftCard> GetByCardNumber(string number)
        {
            return await _repository.GetEntity<GiftCard>()
                .Where(x => x.Number == number)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task ProcessGc(List<GiftCard> giftcards, string connectionString)
        {
            var b = new DbContextOptionsBuilder()
                .UseNpgsql(connectionString, sqlopt =>
                {
                    sqlopt.UseAdminDatabase("defaultdb");
                    sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                        null);
                });

            using (var context = new SqlServerContext(b.Options))
            {
                try
                {
                    foreach (var s in giftcards)
                    {
                        var gc = await context.Set<GiftCard>()
                            .Where(x => x.StoreId == s.StoreId && x.Number == s.Number)
                            .FirstOrDefaultAsync();
                        if (gc == null)
                        {
                            await context.Set<GiftCard>().AddAsync(s);
                            _logger.LogInformation("Gift Card xxxxxxxx{Number} created with {Amount}", s.Number[8..],
                                s.Amount.ToString("C2"));
                        }
                        else
                        {
                            gc.Amount += s.Amount;
                            gc.Balance += s.Balance;
                            context.Set<GiftCard>().Update(gc);
                            _logger.LogInformation("Gift Card xxxxxxxx{Number} update with {Amount}", s.Number[8..],
                                s.Amount.ToString("C2"));
                        }
                    }

                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"ProcessGC error.");
                }
            }
        }
    }
}