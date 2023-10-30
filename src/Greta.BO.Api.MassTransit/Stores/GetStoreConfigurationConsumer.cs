using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.Api.EventContracts;
using Greta.BO.Api.EventContracts.Store;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using Greta.BO.BusinessLogic.Options;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.MassTransit.Stores
{
    public class GetStoreConfigurationConsumer: BaseConsumer<GetStoreConfigurationRequestContract>
    {
        private readonly IMediator _mediator;

        public GetStoreConfigurationConsumer(
            ILogger<GetStoreConfigurationConsumer> logger,
            IMediator mediator,
            IOptions<MainOption> options
        )
            :base(logger, options)
        {
            _mediator = mediator;
        }

        public override async Task Execute(ConsumeContext<GetStoreConfigurationRequestContract> context)
        {
            var response = await _mediator.Send(new StoreGetByGuidQuery(context.Message.StoreGuid));
            if (response != null)
            {
                var store = response.Data;
                _logger.LogInformation("Store data sended");
            
                await context.RespondAsync<GetStoreConfigurationResponseContract>(new
                {
                    Name = store.Name,
                    Phone = store.Phone,
                    Address = store.Address,
                    Slogan = store.Slogan,
                    Zip = store.Zip,
                    
                    DrawerTraking = (int)store.DrawerTraking,
                    Language = store.Language,
                    Currency = store.Currency,
                    CashDiscount = store.CashDiscount,
                    CashDiscountValue = store.CashDiscountValue,
                    ClientTransparency = store.ClientTransparency,
                    AcceptChecksExactAmount = store.AcceptChecksExactAmount, 
                    CreditCardNeedSignature = store.CreditCardNeedSignature,
                    CreditCardNeedSignatureAmount = store.CreditCardNeedSignatureAmount,
                    DebitCardCashBack = store.DebitCardCashBack,
                    DebitCardCashBackMaxAmount = store.DebitCardCashBackMaxAmount,
                    SnapEBTCAshCashBack = store.SnapEBTCAshCashBack,
                    SnapEBTCAshCashBackMaxAmount = store.SnapEBTCAshCashBackMaxAmount,
                    MinimumAgeRequired = store.MinimumAgeRequired,
                    DisplayChangeDueAfterTender = store.DisplayChangeDueAfterTender,
                    DisplayLaneClosed = store.DisplayLaneClosed,
                    UseCustomer = store.UseCustomer,
                    
                    UseTaxOverride = store.UseTaxOverride,
                    UseNoSale = store.UseNoSale,
                    UsePaidOut = store.UsePaidOut,
                    UseReturn = store.UseReturn,
                    UseDiscount = store.UseDiscount,
                    UseZeroScale = store.UseZeroScale,
                    UseBottleRefund = store.UseBottleRefund,
                    UseGiftCards = store.UseGiftCards,
                    UseExchange  = store.UseExchange,
                    
                    UseReprintReceipt  = store.UseReprintReceipt,
                    UseEBTCheckBalance = store.UseEBTCheckBalance,
                    UseRemoveserviceFee = store.UseRemoveserviceFee,
                    
                    DefaulBottleDeposit = store.DefaulBottleDeposit,
                    PrintReceiptOptional = store.PrintReceiptOptional,
                    AutoLogOffCachiers = store.AutoLogOffCachiers,
                    AutoEndDate = store.AutoEndDate,
                    AutoCloseAllCachiers = store.AutoCloseAllCachiers,
                    Theme = (int)store.Theme,
                    GiftCardType = (int)store.GiftCardType,
                    OpenTime = store.OpenTime,
                    ClosedTime = store.ClosedTime
                });
            }
            else
                await context.RespondAsync<FailResponseContract>(new
                {
                    errorMessages = new List<string>() { $"Error obtain store with guid {context.Message.StoreGuid.ToString()}." },
                    Timestamp = DateTime.Now,
                });
        }
    }
}