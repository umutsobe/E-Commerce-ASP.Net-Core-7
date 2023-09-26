namespace e_trade_api.application;

public class GetAllOrdersByFilterResponseDTO
{
    public int TotalOrderCount { get; set; }
    public List<GetOrderByFilter>? Orders { get; set; }
}

public class GetOrderByFilter
{
    public string Id { get; set; }
    public string OrderCode { get; set; }
    public string Username { get; set; }
    public float TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Completed { get; set; }
}


//   id: string;
//   orderCode: string;
//   userName: string;
//   totalPrice: number;
//   createdDate: Date;
//   completed: boolean;
