using System.ComponentModel.DataAnnotations.Schema;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class TwoFactorAuthentication : BaseEntity
{
    [NotMapped] //updated date olmayacak
    public override DateTime UpdatedDate
    {
        get => base.UpdatedDate;
        set => base.UpdatedDate = value;
    }

    public AppUser User { get; set; }
    public string UserId { get; set; }
    public string Code { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }
}
