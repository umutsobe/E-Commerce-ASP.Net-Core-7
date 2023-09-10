using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class TwoFactorAuthenticationReadRepository
    : ReadRepository<TwoFactorAuthentication>,
        ITwoFactorAuthenticationReadRepository
{
    public TwoFactorAuthenticationReadRepository(ETradeApiDBContext context)
        : base(context) { }
}
