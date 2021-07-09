using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJetWallet.Sdk.Postgres;

namespace Service.FrontendKeyValue.Postgres.DesignTime
{
    public class ContextFactory : MyDesignTimeContextFactory<MyContext>
    {
        public ContextFactory() : base(options => new MyContext(options))
        {
        }
    }
}
