using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BlockArray.Core;
using BlockArray.Core.Data;
using BlockArray.ServiceModel;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Sdk.Demo;
using FreightTrust.EDI;
using FreightTrust.EDI.AS2;
using FreightTrust.Modules.SamsaraConfig;
using Microsoft.Extensions.DependencyInjection;
using Selfsigned;

namespace FreightTrust.Modules.EdiTender
{
    public interface IEdiProvider
    {
        string Name { get; set; }
        IEnumerable<string> Get();
        Task Send(Company.Company from, Company.Company to, IEdiMessage message);
    }

    public interface ITradingPartnerSender
    {
        Task Send(string senderCompanyId,TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner, string subject,
            EdiMessage message, string referenceId);

        Task Send(string senderCompanyId,string channelId, string partnerId, string subject, EdiMessage message, string referenceId);
        Task Send(string senderCompanyId,string partnerId, string subject, EdiMessage message, string referenceId);
    }

    
    
    public class TradingPartnerSender : ITradingPartnerSender
    {
        private readonly BaseRepository<TradingPartnerMessage.TradingPartnerMessage> _messageRepo;
        private readonly BaseRepository<TradingChannel.TradingChannel> _channelRepo;
        private readonly BaseRepository<TradingPartner.TradingPartner> _partnerRepo;
        private readonly BaseRepository<Company.Company> _companyRepo;

        public TradingPartnerSender(BaseRepository<TradingPartnerMessage.TradingPartnerMessage> messageRepo,
            BaseRepository<TradingChannel.TradingChannel> channelRepo,
            BaseRepository<TradingPartner.TradingPartner> partnerRepo,
            BaseRepository<Company.Company> companyRepo
        )
        {
            _messageRepo = messageRepo;
            _channelRepo = channelRepo;
            _partnerRepo = partnerRepo;
            _companyRepo = companyRepo;
        }

        public async Task Send(string senderCompanyId, TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner,
            string subject, EdiMessage message, string referenceId)
        {
            var sender = await _companyRepo.Find(senderCompanyId);
            
            partner.CurrentControlNumber++;
            await _partnerRepo.Save(partner);
            await _messageRepo.Save(new TradingPartnerMessage.TradingPartnerMessage()
            {
                Type = TradingPartnerMessageType.Outgoing,
                Status = TradingPartnerMessageStatus.Queued,
                Content = await EdiUtils.CreateTransaction(message, 
                    partner.CurrentControlNumber.ToString().PadLeft(9,'0'), 
                    sender.IsaId, partner.IsaId, partner.RequestAck,partner.TestMode ? "T":"P"),
                ReferenceId = referenceId,
                ContentType = "EDI",
                Subject = subject,
                TradingChannelId = channel.Id,
                TradingPartnerId = partner.Id
            });
        }

        public async Task Send(string senderCompanyId,string partnerId, string subject, EdiMessage message, string referenceId)
        {
            var partner = await _partnerRepo.Find(partnerId);
            var channel = await _channelRepo.Find(partner.ChannelId);

            await Send(senderCompanyId, channel, partner, subject, message, referenceId);
        }

        public async Task Send(string senderCompanyId, string channelId, string partnerId, string subject,EdiMessage message, string referenceId)
        {
            await Send(senderCompanyId, await _channelRepo.Find(channelId), await _partnerRepo.Find(partnerId), subject, message, referenceId);
        }
        
        
    }


//    public interface ITransactionProvider
//    {
//        string Name { get; }
//    }
}