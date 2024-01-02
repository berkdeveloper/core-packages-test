using Core.Domain.Core.Constants;
using Core.Domain.Filters;
using Core.Domain.Filters.Abstractions;

namespace Core.Domain.Factories;

public class FilterStrategyFactory<TAggregate> where TAggregate : class
{
    private readonly Dictionary<string, Lazy<IFilterStrategy<TAggregate>>> _strategies = new();
    private readonly object _lockObject = new();

    public IFilterStrategy<TAggregate> GetFilterStrategy(string key)
    {
        key = key.ToLowerInvariant();

        lock (_lockObject)
        {
            if (_strategies.TryGetValue(key, out var lazyStrategy))
                return lazyStrategy.Value;

            Lazy<IFilterStrategy<TAggregate>> strategy = new(() => CreateStrategy(key)); // Strateji oluşturulur

            if (strategy is not null)
                _strategies[key] = strategy;

            return strategy.Value;
        }
    }

    private IFilterStrategy<TAggregate> CreateStrategy(string key)
    {
        // Belirtilen anahtar için uygun strateji oluşturulur
        if (key == StrategyTypeConstants.EnableTrackingFilter)
            return new EnableTrackingFilter<TAggregate>();
        else if (key == StrategyTypeConstants.DeletedFilter)
            return new DeletedFilter<TAggregate>();

        return null;
    }
}
