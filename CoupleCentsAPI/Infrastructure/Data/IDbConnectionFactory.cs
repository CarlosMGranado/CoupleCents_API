using System.Data;

namespace CoupleCentsAPI.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}