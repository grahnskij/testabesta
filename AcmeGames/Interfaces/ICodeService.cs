﻿using AcmeGames.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeGames.Interfaces
{
    public interface ICodeService
    {
        bool RedeemCode(CodeRedeemViewModel vm);
    }
}
