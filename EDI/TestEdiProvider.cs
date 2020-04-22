using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FreightTrust.EDI;

namespace FreightTrust.Modules.EdiTender
{
    public class TestEdiProvider : IEdiProvider
    {
        public string Name { get; set; }

        public IEnumerable<string> Get()
        {
            foreach (var file in Directory.GetFiles("/Users/micahosborne/seal/BlockArray.Seal.Tests/EdiIncoming/",
                "*.txt"))
            {
                yield return File.ReadAllText(file);
            }
        }

        public async Task Send(Company.Company f, Company.Company to, IEdiMessage message)
        {
        }
    }
}