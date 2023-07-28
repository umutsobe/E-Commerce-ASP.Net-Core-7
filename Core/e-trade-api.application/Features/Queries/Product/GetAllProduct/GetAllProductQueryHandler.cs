using System.Threading.Tasks;
using MediatR;
using e_trade_api.application;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    readonly IProductReadRepository _productReadRepository;

    public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }


    public Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository
            .GetAll(false)
            .Skip(request.Page * request.Size)
            .Take(request.Size)
            .Select(
                p =>
                    new
                    {
                        p.Id,
                        p.Name,
                        p.Stock,
                        p.Price,
                        p.CreatedDate,
                        p.UpdatedDate
                    }
            )
            .ToList();

            GetAllProductQueryResponse response= new (){
                TotalCount = totalCount,
                Products= products
            };

            return Task.FromResult(response);
    }
}
