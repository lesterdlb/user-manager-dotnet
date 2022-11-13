using UserManager.Application.Common.Interfaces.Services;

namespace UserManager.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}