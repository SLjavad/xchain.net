﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.thorchain.Models;

namespace Xchain.net.xchain.thorchain
{
    public interface IThorchianClient
    {
        void SetClientUrl(ClientUrl clientUrl);
        NodeUrl GetClientUrl();
        void SetExplorerUrl(ExplorerUrl explorerUrl);
        string GetExplorerNodeUrl(string node);
        CosmosSdkClient GetCosmosClient();
        Task<string> Deposit(DepositParam @params);

    }
}
